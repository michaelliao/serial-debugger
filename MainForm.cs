using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace SerialDebugger
{
    public partial class MainForm : Form
    {
        private enum Direction { None, Sent, Received }

        private static int _uiThreadId;
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        private readonly AppSettings _settings;
        private readonly SerialPort _port = new();

        private Direction _lastDirection = Direction.None;
        private DateTime _lastTimestamp;

        private long _sentCount;
        private long _receivedCount;

        private bool _suspendSettingsSave;
        private bool _suspendInputSync;

        public MainForm()
        {
            InitializeComponent();

            _uiThreadId = Environment.CurrentManagedThreadId;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _settings = AppSettings.LoadOrCreate();

            InitializeComboItems();
            ApplySettingsToUi();
            WireEvents();
            RefreshPortList();
            UpdateStatusLabel();
        }

        // ---------- initialization ----------

        private void InitializeComboItems()
        {
            comboBoxBaudRate.Items.Clear();
            foreach (int v in AppSettings.BaudRateValues)
                comboBoxBaudRate.Items.Add(v.ToString());

            comboBoxDataBits.Items.Clear();
            comboBoxDataBits.Items.AddRange(AppSettings.DataBitsTexts);

            comboBoxParity.Items.Clear();
            comboBoxParity.Items.AddRange(AppSettings.ParityTexts);

            comboBoxStopBits.Items.Clear();
            comboBoxStopBits.Items.AddRange(AppSettings.StopBitsTexts);

            comboBoxEncoding.Items.Clear();
            comboBoxEncoding.Items.AddRange(AppSettings.EncodingTexts);

            comboBoxNewLineChar.Items.Clear();
            comboBoxNewLineChar.Items.AddRange(AppSettings.NewLineTexts);

            comboBoxAutoBreakInMs.Items.Clear();
            comboBoxAutoBreakInMs.Items.AddRange(new object[] { "0", "200", "500", "1000", "2000", "5000", "10000" });
        }

        private void ApplySettingsToUi()
        {
            _suspendSettingsSave = true;
            try
            {
                comboBoxBaudRate.SelectedItem = _settings.BaudRateConf;
                comboBoxDataBits.SelectedItem = _settings.DataBitsConf;
                comboBoxParity.SelectedItem = _settings.ParityConf;
                comboBoxStopBits.SelectedItem = _settings.StopBitsConf;
                comboBoxEncoding.SelectedItem = _settings.TextEncodingConf;
                comboBoxNewLineChar.SelectedItem = _settings.NewLineConf;
                comboBoxAutoBreakInMs.Text = _settings.AutoBreakConf.ToString();
            }
            finally
            {
                _suspendSettingsSave = false;
            }
        }

        private void WireEvents()
        {
            comboBoxPortName.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxBaudRate.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxDataBits.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxParity.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxStopBits.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxEncoding.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxNewLineChar.SelectedIndexChanged += (_, _) => OnSettingChanged();
            comboBoxAutoBreakInMs.TextChanged += (_, _) => OnSettingChanged();
            comboBoxAutoBreakInMs.Leave += (_, _) => OnAutoBreakLeave();

            buttonOpenClose.Click += (_, _) => OnOpenCloseClick();
            buttonSend.Click += async (_, _) => await OnSendAsync();
            buttonClear.Click += (_, _) => OnClear();
            labelRefresh.Click += (_, _) => RefreshPortList();

            tabControlInput.Selected += OnInputTabSelected;

            _port.DataReceived += OnPortDataReceived;
            _port.ErrorReceived += OnPortErrorReceived;

            FormClosing += (_, _) => { try { if (_port.IsOpen) _port.Close(); } catch { } };
        }

        // ---------- settings ----------

        private void OnSettingChanged()
        {
            if (_suspendSettingsSave) return;

            string? selectedPort = comboBoxPortName.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedPort))
            {
                _settings.PortNameConf = selectedPort;
            }
            _settings.BaudRateConf = comboBoxBaudRate.SelectedItem?.ToString() ?? _settings.BaudRateConf;
            _settings.DataBitsConf = comboBoxDataBits.SelectedItem?.ToString() ?? _settings.DataBitsConf;
            _settings.ParityConf = comboBoxParity.SelectedItem?.ToString() ?? _settings.ParityConf;
            _settings.StopBitsConf = comboBoxStopBits.SelectedItem?.ToString() ?? _settings.StopBitsConf;
            _settings.TextEncodingConf = comboBoxEncoding.SelectedItem?.ToString() ?? _settings.TextEncodingConf;
            _settings.NewLineConf = comboBoxNewLineChar.SelectedItem?.ToString() ?? _settings.NewLineConf;

            if (int.TryParse(comboBoxAutoBreakInMs.Text, out int br) && br >= 0 && br <= 10_000)
            {
                _settings.AutoBreakConf = br;
            }

            _settings.Save();
        }

        private void OnAutoBreakLeave()
        {
            if (!int.TryParse(comboBoxAutoBreakInMs.Text, out int br) || br < 0 || br > 10_000)
            {
                comboBoxAutoBreakInMs.Text = _settings.AutoBreakConf.ToString();
            }
        }

        // ---------- ports ----------

        private void RefreshPortList()
        {
            string? current = comboBoxPortName.SelectedItem?.ToString();
            string preferred = !string.IsNullOrEmpty(current) ? current : _settings.PortNameConf;

            _suspendSettingsSave = true;
            try
            {
                comboBoxPortName.Items.Clear();
                string[] ports = SerialPort.GetPortNames();
                Array.Sort(ports);
                comboBoxPortName.Items.AddRange(ports);

                if (!string.IsNullOrEmpty(preferred) && comboBoxPortName.Items.Contains(preferred))
                {
                    comboBoxPortName.SelectedItem = preferred;
                }
                else if (comboBoxPortName.Items.Count > 0)
                {
                    comboBoxPortName.SelectedIndex = 0;
                }
            }
            finally
            {
                _suspendSettingsSave = false;
            }
        }

        private void OnOpenCloseClick()
        {
            if (_port.IsOpen)
            {
                ClosePort();
            }
            else
            {
                TryOpenPort();
            }
        }

        private bool TryOpenPort()
        {
            if (_port.IsOpen) return true;

            string? portName = comboBoxPortName.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(portName))
            {
                ShowError("No serial port selected.");
                return false;
            }

            try
            {
                _port.PortName = portName;
                _port.BaudRate = int.Parse(_settings.BaudRateConf);
                _port.DataBits = int.Parse(_settings.DataBitsConf);
                _port.Parity = AppSettings.ParityValues[Array.IndexOf(AppSettings.ParityTexts, _settings.ParityConf)];
                _port.StopBits = AppSettings.StopBitsValues[Array.IndexOf(AppSettings.StopBitsTexts, _settings.StopBitsConf)];
                _port.ReadTimeout = 500;
                _port.WriteTimeout = 500;
                _port.Open();

                buttonOpenClose.Text = "Close Port";
                SetPortSettingsEnabled(false);
                Debug.Print($"Opened port {_port.PortName} @ {_port.BaudRate} {_port.DataBits}-{_port.Parity}-{_port.StopBits}");
                return true;
            }
            catch (Exception ex)
            {
                ShowError($"Failed to open port {portName}: {ex.Message}");
                return false;
            }
        }

        private void ClosePort()
        {
            string name = _port.PortName;
            bool wasOpen = _port.IsOpen;
            try { if (_port.IsOpen) _port.Close(); } catch { /* swallow */ }
            buttonOpenClose.Text = "Open Port";
            SetPortSettingsEnabled(true);
            if (wasOpen) Debug.Print($"Closed port {name}");
        }

        private void SetPortSettingsEnabled(bool enabled)
        {
            comboBoxPortName.Enabled = enabled;
            comboBoxBaudRate.Enabled = enabled;
            comboBoxDataBits.Enabled = enabled;
            comboBoxParity.Enabled = enabled;
            comboBoxStopBits.Enabled = enabled;
            labelRefresh.Enabled = enabled;
        }

        // ---------- send / receive ----------

        private async Task OnSendAsync()
        {
            if (!_port.IsOpen && !TryOpenPort()) return;

            byte[] payload;
            try
            {
                payload = BuildPayload();
            }
            catch (Exception ex)
            {
                ShowError($"Invalid input: {ex.Message}");
                return;
            }

            if (payload.Length == 0) return;

            buttonSend.Enabled = false;
            try
            {
                Exception? writeError = await Task.Run(() =>
                {
                    try { _port.Write(payload, 0, payload.Length); return (Exception?)null; }
                    catch (Exception ex) { return ex; }
                });

                if (writeError != null)
                {
                    ShowError($"Failed to send: {writeError.Message}");
                    ClosePort();
                    return;
                }

                Interlocked.Add(ref _sentCount, payload.Length);
                AppendData(Direction.Sent, payload);
                UpdateStatusLabel();
                Debug.Print($"Sent {payload.Length} bytes: {HexHelper.FormatHex(payload)}");
            }
            finally
            {
                buttonSend.Enabled = true;
            }
        }

        private byte[] BuildPayload()
        {
            if (tabControlInput.SelectedTab == tabPageHexInput)
            {
                return HexHelper.ParseHex(textBoxHexInput.Text);
            }
            return EncodeTextWithNewLine(textBoxTextInput.Text);
        }

        private byte[] EncodeTextWithNewLine(string text)
        {
            Encoding enc = ResolveEncoding(_settings.TextEncodingConf);
            string newline = ResolveNewLine(_settings.NewLineConf);
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return enc.GetBytes(string.Join(newline, lines));
        }

        // WinForms multiline TextBox only renders "\r\n" as a line break.
        // Promote bare \r or \n to \r\n so decoded data is visually readable.
        private static string NormalizeLineBreaksForDisplay(string text)
        {
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return string.Join("\r\n", lines);
        }

        private void OnPortDataReceived(object? sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!_port.IsOpen) return;
                int available = _port.BytesToRead;
                if (available <= 0) return;

                var buffer = new byte[available];
                int read = _port.Read(buffer, 0, available);
                if (read <= 0) return;
                if (read != buffer.Length) Array.Resize(ref buffer, read);

                Interlocked.Add(ref _receivedCount, read);
                Debug.Print($"Received {read} bytes: {HexHelper.FormatHex(buffer)}");

                BeginInvokeSafe(() =>
                {
                    AppendData(Direction.Received, buffer);
                    UpdateStatusLabel();
                });
            }
            catch (Exception ex)
            {
                BeginInvokeSafe(() =>
                {
                    ShowError($"Serial port error: {ex.Message}");
                    ClosePort();
                });
            }
        }

        private void OnPortErrorReceived(object? sender, SerialErrorReceivedEventArgs e)
        {
            BeginInvokeSafe(() =>
            {
                ShowError($"Serial port error: {e.EventType}");
                ClosePort();
            });
        }

        // ---------- input tab conversion ----------

        private void OnInputTabSelected(object? sender, TabControlEventArgs e)
        {
            if (_suspendInputSync) return;
            _suspendInputSync = true;
            try
            {
                if (e.TabPage == tabPageHexInput)
                {
                    byte[] bytes = EncodeTextWithNewLine(textBoxTextInput.Text);
                    textBoxHexInput.Text = HexHelper.FormatHex(bytes);
                }
                else if (e.TabPage == tabPageTextInput)
                {
                    try
                    {
                        byte[] bytes = HexHelper.ParseHex(textBoxHexInput.Text);
                        Encoding enc = ResolveEncoding(_settings.TextEncodingConf);
                        textBoxTextInput.Text = NormalizeLineBreaksForDisplay(enc.GetString(bytes));
                    }
                    catch (FormatException ex)
                    {
                        ShowError($"Invalid hex input: {ex.Message}");
                    }
                }
            }
            finally
            {
                _suspendInputSync = false;
            }
        }

        // ---------- display ----------

        private void AppendData(Direction dir, byte[] bytes)
        {
            DateTime now = DateTime.Now;
            bool coalesce = dir == _lastDirection &&
                            (now - _lastTimestamp).TotalMilliseconds <= _settings.AutoBreakConf;

            Encoding enc = ResolveEncoding(_settings.TextEncodingConf);
            string text = NormalizeLineBreaksForDisplay(enc.GetString(bytes));
            string hex = HexHelper.FormatHex(bytes);

            string textChunk;
            string hexChunk;

            if (coalesce)
            {
                textChunk = text;
                hexChunk = " " + hex;
            }
            else
            {
                string prefix = dir == Direction.Sent ? ">>" : "<<";
                string separator = textBoxDisplayText.TextLength > 0
                    ? Environment.NewLine + Environment.NewLine
                    : string.Empty;
                string header = $"{separator}{prefix} {now:HH:mm:ss.fff}{Environment.NewLine}";
                textChunk = header + text;
                hexChunk = header + hex;
            }

            textBoxDisplayText.AppendText(textChunk);
            textBoxDisplayHex.AppendText(hexChunk);

            _lastDirection = dir;
            _lastTimestamp = now;
        }

        private void OnClear()
        {
            textBoxDisplayText.Clear();
            textBoxDisplayHex.Clear();
            _lastDirection = Direction.None;
            Interlocked.Exchange(ref _sentCount, 0);
            Interlocked.Exchange(ref _receivedCount, 0);
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            labelDataStatus.Text =
                $"Sent: {Interlocked.Read(ref _sentCount)}. Received: {Interlocked.Read(ref _receivedCount)}";
        }

        // ---------- helpers ----------

        private static Encoding ResolveEncoding(string name)
        {
            int idx = Array.IndexOf(AppSettings.EncodingTexts, name);
            if (idx < 0) return Utf8NoBom;
            string value = AppSettings.EncodingValues[idx];

            if (string.Equals(value, "UTF8", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "UTF-8", StringComparison.OrdinalIgnoreCase))
            {
                return Utf8NoBom;
            }

            try
            {
                return Encoding.GetEncoding(value);
            }
            catch (Exception ex)
            {
                Debug.Print($"Encoding '{value}' unavailable ({ex.Message}); falling back to UTF-8 (no BOM)");
                if (Environment.CurrentManagedThreadId == _uiThreadId)
                {
                    MessageBox.Show(
                        $"Selected encoding '{name}' is not available on this system. Falling back to UTF-8.",
                        "Serial Debugger",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                return Utf8NoBom;
            }
        }

        private static string ResolveNewLine(string name)
        {
            int idx = Array.IndexOf(AppSettings.NewLineTexts, name);
            return idx < 0 ? "\n" : AppSettings.NewLineValues[idx];
        }

        private void BeginInvokeSafe(Action action)
        {
            if (!IsHandleCreated || IsDisposed) return;
            try { BeginInvoke(action); } catch { /* form going away */ }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Serial Debugger", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
