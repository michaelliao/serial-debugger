# SerialDebugger

SerialDebugger is a windows WinForm App for debugging serial communication. 
It provides a user-friendly interface for sending and receiving data over serial ports, 
making it easier for developers to test and debug their serial communication applications.

## Specification

### Initialize combo box's dropdown list

When the App starts, it initialize combo box's dropdown list.

For baud rate, it will list common baud rates defined as constant: 

```
const int[] BAUD_RATE_VALUES = { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600};
```

For parity, it will list text from `PARITY_TEXTS` and get value from `PARITY_VALUES` by selected index:

```
const string[] PARITY_TEXTS = { "None", "Even", "Odd", "Mark" };
const Parity[] PARITY_VALUES = { Parity.None, Parity.Even, Parity.Odd, Parity.Mark };
```

For stop bits, it will list text from `STOP_BITS_TEXTS` and get value from `STOP_BITS_VALUES` by selected index:

```
const string[] STOP_BITS_TEXTS = { "1", "1.5", "2" };
const StopBits[] STOP_BITS_VALUES = { StopBits.One, StopBits.OnePointFive, StopBits.Two };
```

### load user settings

Load the user settings from a JSON file. If the file does not exist, it will create a new one with default values. The settings include baud rate, data bits, parity, stop bits, text encoding, newline character, and auto break time.

SerialDebugger stores user settings in a JSON file located at `C:\Users\{username}\AppData\Roaming\SerialDebugger\settings.json`.

| Setting Key  | Type | Default Value | UI Control            | Description |
|--------------|------|---------------|-----------------------|-------------|
| baudRate     | str  | "115200"      | comboBoxBaudRate      | The baud rate for serial communication (e.g., 9600, 115200). |
| dataBits     | str  | "8"           | comboBoxDataBits      | The number of data bits (e.g., 7, 8). |
| parity       | str  | "None"        | comboBoxParity        | The parity setting (e.g., None, Even, Odd). |
| stopBits     | str  | "1"           | comboBoxStopBits      | The number of stop bits (e.g., 1, 2). |
| textEncoding | str  | "UTF-8"       | comboBoxEncoding      | The text encoding for sending and receiving data (e.g., UTF-8, ASCII). |
| newLine      | str  | "LF"          | comboBoxNewLineChar   | The newline character(s) to append to sent data (e.g., CR, LF, CRLF). |
| autoBreak    | int  | 1000          | comboBoxAutoBreakInMs | The break in milliseconds between sending and receiving data. 0 ~ 10,000 |

When App starts, it will load the settings from the JSON file. If the file does not exist, it will create a new one with default values. 

Settings must be validated after loading. If any setting is invalid, it will be reset to the default value and saved back to the JSON file.

Update the UI controls to reflect the loaded settings. For example, if the loaded baud rate is "115200", it will set the selected index of `comboBoxBaudRate` to the index of "115200" in the dropdown list.

When settings changed, it will save the current settings to the JSON file.

### Refresh serial port list

When app starts or user click the "Refresh" (label `labelRefresh`), it will refresh the serial port list and display the available serial ports in the combo box.

### Open and close serial port

The button `buttonOpenClose` is used to open and close the serial port. It displays "Open Port" when the serial port is closed, and displays "Close Port" when the serial port is open.

When user click the open button, it will try to open the serial port with the settings defined in the combo boxes. If failed to open serial port, show error message and return. 

If the serial port has been removed while it is open, it will show error message and close the serial port. The button will be switched to "Open Port". 

### Input

User can input text in the text input box or hex string in the hex input box. 

Hex input accepts hex string with or without spaces. For example, "48  65  6C  6C  6F" and "48656C6C6F" are both valid hex strings that represent the text "Hello".
"48656C6C6" will be converted to bytes as [0x48, 0x65, 0x6C, 0x6C 0x06]. The last single "6" will be treated as "0x06".

White space is used to FORCE separate hex bytes in such case:

```
48 65 6C 6C 6F => [0x48, 0x65, 0x6C, 0x6C, 0x6F]
48656 C6C6F => [0x48, 0x65, 0x06, 0xC6, 0xC6, 0x0F]
```

When user input text "Hello" in the text input box, and switched to hex input box, it will convert the text to hex string "48 65 6C 6C 6F" and display in the hex input box. 
When user input hex string "48 65 6C 6C 6F" in the hex input box, and switched to text input box, it will convert the hex string to text "Hello" and display in the text input box.

### Send Data

The input will be sent when user click the "Send" button. 
If the serial port is not open, try open serial port with the settings defined in the combo boxes. If failed to open serial port, show error message and return. 
Before sending, input text or input hex string will be converted to bytes according to the selected text encoding and new line character(s) defined in settings.
Sending data will be done in a background thread to avoid blocking the UI. After sending data, it will update the SentCount in the global status. 
NOTE the input text or hex string will not be cleared after sending, user can modify the input and send again. 

### Receive Data

When serial port receive data from background thread, it will update the ReceivedCount in the global status.

### Display Sent and Received Data

`textBoxDisplayText` and `textBoxDisplayHex` are used to display sent and received data in text and hex format. 
Time stamp will be added to each sent and received data. Sent data will be prefixed with ">>" and received data will be prefixed with "<<".

A typical display text format:

```
>> 09:00:20.123
Hello

<< 09:02:21.456
Bye.
```

A typical display hex format:

```
>> 09:00:20.123
48 65 6C 6C 6F

<< 09:02:21.456
42 79 65 2E
```

Sent and received data are always separated in display text and hex, which means sent data and received data will not be mixed together. This can help user easily distinguish between sent and received data.

Multiple sent data may be displayed together, and multiple received data may be displayed together. 
For example, if user send "Hello" and then send "World" in 0.5 second which is less than `autoBreak` setting:

```
>> 09:00:20.123
HelloWorld
```

Display text and hex will be updated in a thread-safe way, which means it can be updated from background thread without causing cross-thread operation exception. 

Display text and hex will be cleared when user click the "Clear" button.
