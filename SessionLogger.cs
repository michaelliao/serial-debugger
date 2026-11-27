using System.Diagnostics;
using System.Text;

namespace SerialDebugger
{
    // Writes the raw received stream of one open-port session to two files under
    // the user's home directory (~/.serial-debugger), named so they sort by
    // session start time:
    //   YYYYMMDD-hhmmss-tx-plain.log  raw bytes, no decoration
    //   YYYYMMDD-hhmmss-tx-hex.log    "A1 B2 C3 ..." hex, 64 bytes per line
    // The files are created lazily on the first received byte, so a session that
    // receives no data leaves no empty logs behind.
    internal sealed class SessionLogger : IDisposable
    {
        private const int HexBytesPerLine = 64;
        private const int RetentionDays = 30;

        private readonly object _lock = new();
        private readonly string _plainPath;
        private readonly string _hexPath;
        private FileStream? _plain;
        private FileStream? _hex;
        private bool _disposed;
        private int _hexLineBytes;

        private SessionLogger(string plainPath, string hexPath)
        {
            _plainPath = plainPath;
            _hexPath = hexPath;
        }

        public static string LogDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".serial-debugger");

        // Names are timestamped at port open; the files themselves appear only
        // on the first write.
        public static SessionLogger Start()
        {
            string stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            return new SessionLogger(
                Path.Combine(LogDirectory, $"{stamp}-tx-plain.log"),
                Path.Combine(LogDirectory, $"{stamp}-tx-hex.log"));
        }

        public void Write(byte[] buffer, int count)
        {
            try
            {
                lock (_lock)
                {
                    if (_disposed) return;

                    EnsureCreated();
                    FileStream plain = _plain!;
                    FileStream hex = _hex!;

                    plain.Write(buffer, 0, count);
                    WriteHex(hex, buffer, count);
                    plain.Flush();
                    hex.Flush();
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Session log write failed, disabling: {ex.Message}");
                Dispose();
            }
        }

        private void EnsureCreated()
        {
            if (_plain != null) return;

            Directory.CreateDirectory(LogDirectory);
            _plain = new FileStream(_plainPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            _hex = new FileStream(_hexPath, FileMode.Create, FileAccess.Write, FileShare.Read);
        }

        private void WriteHex(FileStream hex, byte[] buffer, int count)
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
            hex.Write(Encoding.ASCII.GetBytes(sb.ToString()));
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                _disposed = true;
                _plain?.Dispose();
                _hex?.Dispose();
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
