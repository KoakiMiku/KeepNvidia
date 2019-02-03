using NOpenCL;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVIDIAKeeper
{
    class KeepRunning
    {
        private static readonly string name = "NvidiaKeeper";

        private static readonly int interval = 5; // Seconds
        private static readonly int size = 1024; // Bytes

        public static void Start()
        {
            try
            {
                bool isSingle = SingleInstance.IsSingle();
                if (!isSingle)
                {
                    return;
                }
                else
                {
                    Task.Run(new Action(() => KeepNvidiaRunning()));
                    Application.Run();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }

        private static void KeepNvidiaRunning()
        {
            try
            {
                Device device = GetNvidiaDevice();
                Context context = null;

                while (true)
                {
                    bool power = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online;
                    if (context == null && power)
                    {
                        context = Context.Create(device);
                    }
                    else if (context != null && !power)
                    {
                        context.Dispose();
                        context = null;
                    }
                    else if (context != null && power)
                    {
                        MemoryCopy(context, device);
                    }

                    Task.Delay(TimeSpan.FromSeconds(interval)).Wait();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }

        private static Device GetNvidiaDevice()
        {
            try
            {
                Platform[] platforms = Platform.GetPlatforms();
                if (platforms.Length == 0)
                {
                    throw new PlatformNotSupportedException("No OpenCL platform found.");
                }

                Platform platform = null;
                foreach (Platform item in platforms)
                {
                    if (item.Name.Contains("NVIDIA"))
                    {
                        platform = item;
                        break;
                    }
                }
                if (platform == null)
                {
                    throw new PlatformNotSupportedException("No NVIDIA platform found.");
                }

                Device[] devices = platform.GetDevices(DeviceType.Gpu);
                if (devices.Length == 0)
                {
                    throw new PlatformNotSupportedException("No GPU devices found.");
                }

                return devices[0];
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }

        private static void MemoryCopy(Context context, Device device)
        {
            try
            {
                using (CommandQueue commandQueue = context.CreateCommandQueue(device, CommandQueueProperties.ProfilingEnable))
                {
                    byte[] buffer = new byte[size];
                    for (int i = 0; i < size; i++)
                    {
                        buffer[i] = 0xFF;
                    }

                    using (NOpenCL.Buffer d_idata = context.CreateBuffer(MemoryFlags.ReadOnly, size),
                                          d_odata = context.CreateBuffer(MemoryFlags.WriteOnly, size))
                    {
                        unsafe
                        {
                            fixed (byte* rawData = buffer)
                            {
                                using (commandQueue.EnqueueWriteBuffer(d_idata, true, 0, size, (IntPtr)rawData)) { }
                            }
                        }
                        commandQueue.Finish();

                        using (commandQueue.EnqueueCopyBuffer(d_idata, d_odata, 0, 0, size)) { }
                        commandQueue.Finish();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }

        public static void Stop()
        {
            try
            {
                Process process = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(name);
                foreach (var item in processes)
                {
                    if (item.Id != process.Id)
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }
    }
}
