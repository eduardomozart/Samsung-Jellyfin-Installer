using Avalonia;
using Jellyfin2Samsung.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Jellyfin2Samsung
{
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            string logFolder;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var logRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                logFolder = Path.Combine(logRoot, "Jellyfin2Samsung", "Logs");
            }
            else
            {
                // Windows/macOS: keep existing behavior
                logFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
            }

            Directory.CreateDirectory(logFolder);

            var dtg = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            var logFile = Path.Combine(logFolder, $"debug_{dtg}.log");

            Trace.Listeners.Add(new FileTraceListener(logFile));
            Trace.AutoFlush = true;

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
