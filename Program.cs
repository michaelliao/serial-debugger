namespace SerialDebugger
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (_, e) => LogCrash("AppDomain.UnhandledException", e.ExceptionObject as Exception);
            Application.ThreadException +=
                (_, e) => LogCrash("Application.ThreadException", e.Exception);

            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                LogCrash("Main", ex);
                throw;
            }
        }

        private static void LogCrash(string source, Exception? ex)
        {
            if (ex == null) return;
            try
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SerialDebugger");
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, "crash.log");
                File.AppendAllText(path,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {source}{Environment.NewLine}{ex}{Environment.NewLine}{Environment.NewLine}");
            }
            catch
            {
                // last-resort: nothing we can do
            }
        }
    }
}
