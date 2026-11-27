using System.Diagnostics;
using System.Text;

namespace SerialDebugger
{
    // Writes the raw received stream of one open-port session to two files under
    // the user's home directory (~/.serial-debugger), named so they sort by
    // session start time:
    //   YYYYMMDD-hhmmss-tx-plain.log  raw bytes, no decoration
    //   YYYYMMDD-hhmmss-tx-hex.log    "A1 B2 C3 ..." hex, 64 bytes per line
    internal sealed class SessionLogger : IDisposable
    {
        private const int HexBytesPerLine = 64;
        private const int RetentionDays = 30;

        private readonly object _lock = new();
        private readonly FileStream _plain;
        private readonly FileStream _hex;
        private bool _disposed;
        private int _hexLineBytes;

        private SessionLogger(FileStream plain, FileStream hex)
        {
            _plain = plain;
            _hex = hex;
        }

        public static string LogDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".serial-debugger");

        public static SessionLogger? Start()
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);
                string stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                return new SessionLogger(
                    new FileStream(
                        Path.Combine(LogDirectory, $"{stamp}-tx-plain.log"),
                        FileMode.Create, FileAccess.Write, FileShare.Read),
                    new FileStream(
                        Path.Combine(LogDirectory, $"{stamp}-tx-hex.log"),
                        FileMode.Create, FileAccess.Write, FileShare.Read));
            }
            catch (Exception ex)
            {
                Debug.Print($"Session log disabled: {ex.Message}");
                return null;
            }
        }

        public void Write(byte[] buffer, int count)
        {
            try
            {
                lock (_lock)
                {
                    if (_disposed) return;
                    _plain.Write(buffer, 0, count);
                    WriteHex(buffer, count);
                    _plain.Flush();
                    _hex.Flush();
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Session log write failed, disabling: {ex.Message}");
                Dispose();
            }
        }

        private void WriteHex(byte[] buffer, int count)
        {
            var sb = new StringBuilder(count * 3 + 2);
            for (int i = 0; i < count; i++)
            {
                if (_hexLineBytes == HexBytesPerLine)
                {
                    sb.AppendLine();
                    _hexLineBytes = 0;
                }
                if (_hexLineBytes > 0) sb.Append(' ');
                sb.Append(buffer[i].ToString("X2"));
                _hexLineBytes++;
            }
            _hex.Write(Encoding.ASCII.GetBytes(sb.ToString()));
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
                _plain.Dispose();
                _hex.Dispose();
            }
        }

        public static void CleanOldLogs()
        {
            try
            {
                if (!Directory.Exists(LogDirectory)) return;
                DateTime cutoff = DateTime.Now.AddDays(-RetentionDays);
                foreach (FileInfo file in new DirectoryInfo(LogDirectory).EnumerateFiles("*-tx-*.log"))
                {
                    try
                    {
                        if (file.LastWriteTime < cutoff) file.Delete();
                    }
                    catch (Exception ex)
                    {
                        Debug.Print($"Failed to delete old log '{file.Name}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Old log cleanup failed: {ex.Message}");
            }
        }
    }
}
