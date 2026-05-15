namespace SerialDebugger
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panelLeft = new Panel();
            buttonOpenClose = new Button();
            groupBoxSendSetting = new GroupBox();
            labelAutoBreak = new Label();
            comboBoxAutoBreakInMs = new ComboBox();
            groupBoxEncoding = new GroupBox();
            labelNewLineChar = new Label();
            comboBoxNewLineChar = new ComboBox();
            comboBoxEncoding = new ComboBox();
            labelEncoding = new Label();
            groupBoxSerialPortSetting = new GroupBox();
            labelRefresh = new Label();
            label5 = new Label();
            comboBoxStopBits = new ComboBox();
            label4 = new Label();
            comboBoxParity = new ComboBox();
            comboBoxDataBits = new ComboBox();
            label3 = new Label();
            comboBoxBaudRate = new ComboBox();
            label2 = new Label();
            comboBoxPortName = new ComboBox();
            labelPortName = new Label();
            panelSendArea = new Panel();
            tabControlInput = new TabControl();
            tabPageTextInput = new TabPage();
            textBoxTextInput = new TextBox();
            tabPageHexInput = new TabPage();
            textBoxHexInput = new TextBox();
            panelSendAction = new Panel();
            labelDataStatus = new Label();
            buttonClear = new Button();
            buttonSend = new Button();
            splitContainer1 = new SplitContainer();
            textBoxDisplayText = new TextBox();
            textBoxDisplayHex = new TextBox();
            panelLeft.SuspendLayout();
            groupBoxSendSetting.SuspendLayout();
            groupBoxEncoding.SuspendLayout();
            groupBoxSerialPortSetting.SuspendLayout();
            panelSendArea.SuspendLayout();
            tabControlInput.SuspendLayout();
            tabPageTextInput.SuspendLayout();
            tabPageHexInput.SuspendLayout();
            panelSendAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(buttonOpenClose);
            panelLeft.Controls.Add(groupBoxSendSetting);
            panelLeft.Controls.Add(groupBoxEncoding);
            panelLeft.Controls.Add(groupBoxSerialPortSetting);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(6, 6);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(320, 678);
            panelLeft.TabIndex = 0;
            // 
            // buttonOpenClose
            // 
            buttonOpenClose.Location = new Point(12, 256);
            buttonOpenClose.Name = "buttonOpenClose";
            buttonOpenClose.Size = new Size(293, 35);
            buttonOpenClose.TabIndex = 13;
            buttonOpenClose.Text = "Open Port";
            buttonOpenClose.UseVisualStyleBackColor = true;
            // 
            // groupBoxSendSetting
            // 
            groupBoxSendSetting.Controls.Add(labelAutoBreak);
            groupBoxSendSetting.Controls.Add(comboBoxAutoBreakInMs);
            groupBoxSendSetting.Location = new Point(12, 456);
            groupBoxSendSetting.Name = "groupBoxSendSetting";
            groupBoxSendSetting.Size = new Size(293, 83);
            groupBoxSendSetting.TabIndex = 12;
            groupBoxSendSetting.TabStop = false;
            groupBoxSendSetting.Text = "Send Setting";
            // 
            // labelAutoBreak
            // 
            labelAutoBreak.Location = new Point(6, 37);
            labelAutoBreak.Name = "labelAutoBreak";
            labelAutoBreak.Size = new Size(119, 25);
            labelAutoBreak.TabIndex = 24;
            labelAutoBreak.Text = "Auto Break Time:";
            labelAutoBreak.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxAutoBreakInMs
            // 
            comboBoxAutoBreakInMs.FormattingEnabled = true;
            comboBoxAutoBreakInMs.ImeMode = ImeMode.Off;
            comboBoxAutoBreakInMs.Location = new Point(131, 38);
            comboBoxAutoBreakInMs.Name = "comboBoxAutoBreakInMs";
            comboBoxAutoBreakInMs.Size = new Size(148, 25);
            comboBoxAutoBreakInMs.TabIndex = 23;
            comboBoxAutoBreakInMs.Text = "1000";
            // 
            // groupBoxEncoding
            // 
            groupBoxEncoding.Controls.Add(labelNewLineChar);
            groupBoxEncoding.Controls.Add(comboBoxNewLineChar);
            groupBoxEncoding.Controls.Add(comboBoxEncoding);
            groupBoxEncoding.Controls.Add(labelEncoding);
            groupBoxEncoding.Location = new Point(12, 309);
            groupBoxEncoding.Name = "groupBoxEncoding";
            groupBoxEncoding.Size = new Size(293, 126);
            groupBoxEncoding.TabIndex = 11;
            groupBoxEncoding.TabStop = false;
            groupBoxEncoding.Text = "Text Setting";
            // 
            // labelNewLineChar
            // 
            labelNewLineChar.Location = new Point(6, 79);
            labelNewLineChar.Name = "labelNewLineChar";
            labelNewLineChar.Size = new Size(119, 25);
            labelNewLineChar.TabIndex = 23;
            labelNewLineChar.Text = "New Line Char:";
            labelNewLineChar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxNewLineChar
            // 
            comboBoxNewLineChar.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxNewLineChar.FormattingEnabled = true;
            comboBoxNewLineChar.Location = new Point(131, 79);
            comboBoxNewLineChar.Name = "comboBoxNewLineChar";
            comboBoxNewLineChar.Size = new Size(148, 25);
            comboBoxNewLineChar.TabIndex = 22;
            // 
            // comboBoxEncoding
            // 
            comboBoxEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEncoding.FormattingEnabled = true;
            comboBoxEncoding.Location = new Point(131, 36);
            comboBoxEncoding.Name = "comboBoxEncoding";
            comboBoxEncoding.Size = new Size(148, 25);
            comboBoxEncoding.TabIndex = 21;
            // 
            // labelEncoding
            // 
            labelEncoding.Location = new Point(6, 36);
            labelEncoding.Name = "labelEncoding";
            labelEncoding.Size = new Size(119, 25);
            labelEncoding.TabIndex = 20;
            labelEncoding.Text = "Text Encoding:";
            labelEncoding.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBoxSerialPortSetting
            // 
            groupBoxSerialPortSetting.Controls.Add(labelRefresh);
            groupBoxSerialPortSetting.Controls.Add(label5);
            groupBoxSerialPortSetting.Controls.Add(comboBoxStopBits);
            groupBoxSerialPortSetting.Controls.Add(label4);
            groupBoxSerialPortSetting.Controls.Add(comboBoxParity);
            groupBoxSerialPortSetting.Controls.Add(comboBoxDataBits);
            groupBoxSerialPortSetting.Controls.Add(label3);
            groupBoxSerialPortSetting.Controls.Add(comboBoxBaudRate);
            groupBoxSerialPortSetting.Controls.Add(label2);
            groupBoxSerialPortSetting.Controls.Add(comboBoxPortName);
            groupBoxSerialPortSetting.Controls.Add(labelPortName);
            groupBoxSerialPortSetting.Location = new Point(12, 12);
            groupBoxSerialPortSetting.Name = "groupBoxSerialPortSetting";
            groupBoxSerialPortSetting.Size = new Size(293, 234);
            groupBoxSerialPortSetting.TabIndex = 10;
            groupBoxSerialPortSetting.TabStop = false;
            groupBoxSerialPortSetting.Text = "Serial Port Setting";
            // 
            // labelRefresh
            // 
            labelRefresh.Cursor = Cursors.Hand;
            labelRefresh.Image = (Image)resources.GetObject("labelRefresh.Image");
            labelRefresh.Location = new Point(261, 31);
            labelRefresh.Name = "labelRefresh";
            labelRefresh.Size = new Size(18, 19);
            labelRefresh.TabIndex = 20;
            // 
            // label5
            // 
            label5.Location = new Point(6, 193);
            label5.Name = "label5";
            label5.Size = new Size(119, 25);
            label5.TabIndex = 19;
            label5.Text = "Stop Bits:";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxStopBits
            // 
            comboBoxStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStopBits.FormattingEnabled = true;
            comboBoxStopBits.Location = new Point(131, 193);
            comboBoxStopBits.Name = "comboBoxStopBits";
            comboBoxStopBits.Size = new Size(148, 25);
            comboBoxStopBits.TabIndex = 18;
            // 
            // label4
            // 
            label4.Location = new Point(6, 153);
            label4.Name = "label4";
            label4.Size = new Size(119, 25);
            label4.TabIndex = 17;
            label4.Text = "Parity:";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxParity
            // 
            comboBoxParity.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxParity.FormattingEnabled = true;
            comboBoxParity.Location = new Point(131, 153);
            comboBoxParity.Name = "comboBoxParity";
            comboBoxParity.Size = new Size(148, 25);
            comboBoxParity.TabIndex = 16;
            // 
            // comboBoxDataBits
            // 
            comboBoxDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDataBits.FormattingEnabled = true;
            comboBoxDataBits.Location = new Point(131, 111);
            comboBoxDataBits.Name = "comboBoxDataBits";
            comboBoxDataBits.Size = new Size(148, 25);
            comboBoxDataBits.TabIndex = 15;
            // 
            // label3
            // 
            label3.Location = new Point(6, 111);
            label3.Name = "label3";
            label3.Size = new Size(119, 25);
            label3.TabIndex = 14;
            label3.Text = "Data Bits:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxBaudRate
            // 
            comboBoxBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBaudRate.FormattingEnabled = true;
            comboBoxBaudRate.Location = new Point(131, 69);
            comboBoxBaudRate.Name = "comboBoxBaudRate";
            comboBoxBaudRate.Size = new Size(148, 25);
            comboBoxBaudRate.TabIndex = 13;
            // 
            // label2
            // 
            label2.Location = new Point(6, 69);
            label2.Name = "label2";
            label2.Size = new Size(119, 25);
            label2.TabIndex = 12;
            label2.Text = "Baud Rate:";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxPortName
            // 
            comboBoxPortName.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPortName.FormattingEnabled = true;
            comboBoxPortName.Location = new Point(131, 28);
            comboBoxPortName.Name = "comboBoxPortName";
            comboBoxPortName.Size = new Size(119, 25);
            comboBoxPortName.TabIndex = 11;
            // 
            // labelPortName
            // 
            labelPortName.Location = new Point(6, 28);
            labelPortName.Name = "labelPortName";
            labelPortName.Size = new Size(119, 25);
            labelPortName.TabIndex = 10;
            labelPortName.Text = "Port Name:";
            labelPortName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelSendArea
            // 
            panelSendArea.Controls.Add(tabControlInput);
            panelSendArea.Controls.Add(panelSendAction);
            panelSendArea.Dock = DockStyle.Bottom;
            panelSendArea.Location = new Point(326, 462);
            panelSendArea.Name = "panelSendArea";
            panelSendArea.Padding = new Padding(3);
            panelSendArea.Size = new Size(939, 222);
            panelSendArea.TabIndex = 1;
            // 
            // tabControlInput
            // 
            tabControlInput.Alignment = TabAlignment.Bottom;
            tabControlInput.Controls.Add(tabPageTextInput);
            tabControlInput.Controls.Add(tabPageHexInput);
            tabControlInput.Dock = DockStyle.Fill;
            tabControlInput.Location = new Point(3, 3);
            tabControlInput.Multiline = true;
            tabControlInput.Name = "tabControlInput";
            tabControlInput.SelectedIndex = 0;
            tabControlInput.Size = new Size(933, 155);
            tabControlInput.TabIndex = 3;
            // 
            // tabPageTextInput
            // 
            tabPageTextInput.Controls.Add(textBoxTextInput);
            tabPageTextInput.Location = new Point(4, 4);
            tabPageTextInput.Name = "tabPageTextInput";
            tabPageTextInput.Padding = new Padding(3);
            tabPageTextInput.Size = new Size(925, 125);
            tabPageTextInput.TabIndex = 0;
            tabPageTextInput.Text = "Text Input";
            tabPageTextInput.UseVisualStyleBackColor = true;
            // 
            // textBoxTextInput
            // 
            textBoxTextInput.Dock = DockStyle.Fill;
            textBoxTextInput.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxTextInput.Location = new Point(3, 3);
            textBoxTextInput.Multiline = true;
            textBoxTextInput.Name = "textBoxTextInput";
            textBoxTextInput.PlaceholderText = "Input as text.";
            textBoxTextInput.ScrollBars = ScrollBars.Vertical;
            textBoxTextInput.Size = new Size(919, 119);
            textBoxTextInput.TabIndex = 0;
            // 
            // tabPageHexInput
            // 
            tabPageHexInput.Controls.Add(textBoxHexInput);
            tabPageHexInput.Location = new Point(4, 4);
            tabPageHexInput.Name = "tabPageHexInput";
            tabPageHexInput.Padding = new Padding(3);
            tabPageHexInput.Size = new Size(925, 125);
            tabPageHexInput.TabIndex = 1;
            tabPageHexInput.Text = "Hex Input";
            tabPageHexInput.UseVisualStyleBackColor = true;
            // 
            // textBoxHexInput
            // 
            textBoxHexInput.Dock = DockStyle.Fill;
            textBoxHexInput.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxHexInput.ImeMode = ImeMode.Off;
            textBoxHexInput.Location = new Point(3, 3);
            textBoxHexInput.Multiline = true;
            textBoxHexInput.Name = "textBoxHexInput";
            textBoxHexInput.PlaceholderText = "Input as hex data: A1 B2 C3";
            textBoxHexInput.ScrollBars = ScrollBars.Vertical;
            textBoxHexInput.Size = new Size(919, 119);
            textBoxHexInput.TabIndex = 0;
            // 
            // panelSendAction
            // 
            panelSendAction.Controls.Add(labelDataStatus);
            panelSendAction.Controls.Add(buttonClear);
            panelSendAction.Controls.Add(buttonSend);
            panelSendAction.Dock = DockStyle.Bottom;
            panelSendAction.Location = new Point(3, 158);
            panelSendAction.Name = "panelSendAction";
            panelSendAction.Padding = new Padding(6);
            panelSendAction.Size = new Size(933, 61);
            panelSendAction.TabIndex = 2;
            // 
            // labelDataStatus
            // 
            labelDataStatus.Dock = DockStyle.Fill;
            labelDataStatus.Location = new Point(96, 6);
            labelDataStatus.Name = "labelDataStatus";
            labelDataStatus.Size = new Size(738, 49);
            labelDataStatus.TabIndex = 4;
            labelDataStatus.Text = "Sent: 0. Received: 0";
            labelDataStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // buttonClear
            // 
            buttonClear.Dock = DockStyle.Right;
            buttonClear.Location = new Point(834, 6);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(93, 49);
            buttonClear.TabIndex = 3;
            buttonClear.Text = "Clear";
            buttonClear.UseVisualStyleBackColor = true;
            // 
            // buttonSend
            // 
            buttonSend.Dock = DockStyle.Left;
            buttonSend.Location = new Point(6, 6);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(90, 49);
            buttonSend.TabIndex = 2;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(326, 6);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(textBoxDisplayText);
            splitContainer1.Panel1.Padding = new Padding(6);
            splitContainer1.Panel1MinSize = 100;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBoxDisplayHex);
            splitContainer1.Panel2.Padding = new Padding(6);
            splitContainer1.Panel2MinSize = 100;
            splitContainer1.Size = new Size(939, 456);
            splitContainer1.SplitterDistance = 460;
            splitContainer1.TabIndex = 3;
            // 
            // textBoxDisplayText
            // 
            textBoxDisplayText.BackColor = SystemColors.Window;
            textBoxDisplayText.Dock = DockStyle.Fill;
            textBoxDisplayText.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxDisplayText.ImeMode = ImeMode.Off;
            textBoxDisplayText.Location = new Point(6, 6);
            textBoxDisplayText.Multiline = true;
            textBoxDisplayText.Name = "textBoxDisplayText";
            textBoxDisplayText.PlaceholderText = "Text sent and received.";
            textBoxDisplayText.ReadOnly = true;
            textBoxDisplayText.ScrollBars = ScrollBars.Vertical;
            textBoxDisplayText.Size = new Size(448, 444);
            textBoxDisplayText.TabIndex = 1;
            // 
            // textBoxDisplayHex
            // 
            textBoxDisplayHex.BackColor = SystemColors.Window;
            textBoxDisplayHex.Dock = DockStyle.Fill;
            textBoxDisplayHex.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxDisplayHex.ImeMode = ImeMode.Off;
            textBoxDisplayHex.Location = new Point(6, 6);
            textBoxDisplayHex.Multiline = true;
            textBoxDisplayHex.Name = "textBoxDisplayHex";
            textBoxDisplayHex.PlaceholderText = "Hex data sent and received.";
            textBoxDisplayHex.ReadOnly = true;
            textBoxDisplayHex.ScrollBars = ScrollBars.Vertical;
            textBoxDisplayHex.Size = new Size(463, 444);
            textBoxDisplayHex.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1271, 690);
            Controls.Add(splitContainer1);
            Controls.Add(panelSendArea);
            Controls.Add(panelLeft);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Padding = new Padding(6);
            Text = "Serial Debugger";
            panelLeft.ResumeLayout(false);
            groupBoxSendSetting.ResumeLayout(false);
            groupBoxEncoding.ResumeLayout(false);
            groupBoxSerialPortSetting.ResumeLayout(false);
            panelSendArea.ResumeLayout(false);
            tabControlInput.ResumeLayout(false);
            tabPageTextInput.ResumeLayout(false);
            tabPageTextInput.PerformLayout();
            tabPageHexInput.ResumeLayout(false);
            tabPageHexInput.PerformLayout();
            panelSendAction.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelLeft;
        private GroupBox groupBoxSerialPortSetting;
        private Label label5;
        private ComboBox comboBoxStopBits;
        private Label label4;
        private ComboBox comboBoxParity;
        private ComboBox comboBoxDataBits;
        private Label label3;
        private ComboBox comboBoxBaudRate;
        private Label label2;
        private ComboBox comboBoxPortName;
        private Label labelPortName;
        private Panel panelSendArea;
        private Panel panelSendAction;
        private Label labelDataStatus;
        private Button buttonClear;
        private Button buttonSend;
        private TabControl tabControlInput;
        private TabPage tabPageTextInput;
        private TabPage tabPageHexInput;
        private TextBox textBoxTextInput;
        private TextBox textBoxHexInput;
        private GroupBox groupBoxEncoding;
        private ComboBox comboBoxEncoding;
        private Label labelEncoding;
        private Label labelNewLineChar;
        private ComboBox comboBoxNewLineChar;
        private GroupBox groupBoxSendSetting;
        private Label labelAutoBreak;
        private ComboBox comboBoxAutoBreakInMs;
        private SplitContainer splitContainer1;
        private TextBox textBoxDisplayText;
        private TextBox textBoxDisplayHex;
        private Button buttonOpenClose;
        private Label labelRefresh;
    }
}
