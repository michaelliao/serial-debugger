using System.Diagnostics;
using System.IO.Ports;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SerialDebugger
{
    internal sealed class AppSettings
    {
        public static readonly int[] BaudRateValues =
            { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };

        public static readonly string[] DataBitsTexts = { "5", "6", "7", "8" };

        public static readonly string[] ParityTexts = { "None", "Even", "Odd", "Mark" };
        public static readonly Parity[] ParityValues = { Parity.None, Parity.Even, Parity.Odd, Parity.Mark };

        public static readonly string[] StopBitsTexts = { "1", "1.5", "2" };
        public static readonly StopBits[] StopBitsValues = { StopBits.One, StopBits.OnePointFive, StopBits.Two };

        public static readonly string[] EncodingTexts = { "ASCII", "UTF-8", "UTF-16", "GBK", "BIG5", "Shift JIS" };
        public static readonly string[] EncodingValues = { "ASCII", "UTF-8", "Unicode", "GBK", "BIG5", "Shift_JIS" };

        public static readonly string[] NewLineTexts = { "CR", "LF", "CRLF" };
        public static readonly string[] NewLineValues = { "\r", "\n", "\r\n" };

        [JsonPropertyName("portName")] public string PortNameConf { get; set; } = "";
        [JsonPropertyName("baudRate")] public string BaudRateConf { get; set; } = "115200";
        [JsonPropertyName("dataBits")] public string DataBitsConf { get; set; } = "8";
        [JsonPropertyName("parity")] public string ParityConf { get; set; } = "None";
        [JsonPropertyName("stopBits")] public string StopBitsConf { get; set; } = "1";
        [JsonPropertyName("textEncoding")] public string TextEncodingConf { get; set; } = "UTF-8";
        [JsonPropertyName("newLine")] public string NewLineConf { get; set; } = "LF";
        [JsonPropertyName("autoBreak")] public int AutoBreakConf { get; set; } = 1000;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        public static string FilePath
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SerialDebugger");
                return Path.Combine(dir, "settings.json");
            }
        }

        public static AppSettings LoadOrCreate()
        {
            string path = FilePath;
            Debug.Print($"Loading settings from: {path}");
            AppSettings settings;
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOpts) ?? new AppSettings();
                }
                else
                {
                    settings = new AppSettings();
                }
            }
            catch
            {
                settings = new AppSettings();
            }

            bool changed = settings.Validate();
            if (changed || !File.Exists(path))
            {
                settings.Save();
            }
            Debug.Print($"Loaded app settings: portName={settings.PortNameConf}, baudRate={settings.BaudRateConf}, dataBits={settings.DataBitsConf}, parity={settings.ParityConf}, stopBits={settings.StopBitsConf}, textEncoding={settings.TextEncodingConf}, newLine={settings.NewLineConf}, autoBreak={settings.AutoBreakConf}");
            return settings;
        }

        // Resets any invalid field to its default. Returns true if something was changed.
        public bool Validate()
        {
            bool changed = false;
            var defaults = new AppSettings();

            if (Array.IndexOf(BaudRateValues, ParseIntOrZero(BaudRateConf)) < 0)
            {
                BaudRateConf = defaults.BaudRateConf; changed = true;
            }
            if (Array.IndexOf(DataBitsTexts, DataBitsConf) < 0)
            {
                DataBitsConf = defaults.DataBitsConf; changed = true;
            }
            if (Array.IndexOf(ParityTexts, ParityConf) < 0)
            {
                ParityConf = defaults.ParityConf; changed = true;
            }
            if (Array.IndexOf(StopBitsTexts, StopBitsConf) < 0)
            {
                StopBitsConf = defaults.StopBitsConf; changed = true;
            }
            if (Array.IndexOf(EncodingTexts, TextEncodingConf) < 0)
            {
                TextEncodingConf = defaults.TextEncodingConf; changed = true;
            }
            if (Array.IndexOf(NewLineTexts, NewLineConf) < 0)
            {
                NewLineConf = defaults.NewLineConf; changed = true;
            }
            if (AutoBreakConf < 0 || AutoBreakConf > 10_000)
            {
                AutoBreakConf = defaults.AutoBreakConf; changed = true;
            }
            return changed;
        }

        public void Save()
        {
            string path = FilePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(this, JsonOpts));
            Debug.Print($"Stored app settings to {path}: portName={PortNameConf}, baudRate={BaudRateConf}, dataBits={DataBitsConf}, parity={ParityConf}, stopBits={StopBitsConf}, textEncoding={TextEncodingConf}, newLine={NewLineConf}, autoBreak={AutoBreakConf}");
        }

        private static int ParseIntOrZero(string? s) =>
            int.TryParse(s, out int v) ? v : 0;
    }
}
