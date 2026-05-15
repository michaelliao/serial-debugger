# Serial Debugger

A lightweight Windows application for debugging serial port communication. Send and receive data over a serial port in either text or hex format, with timestamps and persistent settings.

![img](app.png)

## Features

- Open/close any available COM port with one click
- Configurable baud rate, data bits, parity, stop bits, encoding, and newline
- Dual input modes — type plain text or paste raw hex (with or without spaces); the two views stay in sync
- Dual display panes showing sent (`>>`) and received (`<<`) data side-by-side in both text and hex, timestamped to the millisecond
- Auto-break grouping: bursts arriving within the configured interval are concatenated for readability
- Non-blocking send/receive on background threads with thread-safe UI updates
- Live `SentCount` / `ReceivedCount` status
- Settings auto-persist to `%AppData%\SerialDebugger\settings.json` and reload on next launch
- Hot-unplug detection — closes the port and notifies you if the device is removed while open

## Requirements

- Windows 10 / 11

## Download Release

The compiled binary can be downloaded from [Github](https://github.com/michaelliao/serial-debugger/releases/latest).

## Build from Source

```powershell
git clone https://github.com/michaelliao/serial-debugger.git
cd serial-debugger
dotnet build -c Release
```

The compiled binary is placed under `bin\Release\net10.0-windows\`. This is a framework-dependent build — it needs the .NET 10 runtime installed on the target machine.

To run from source:

```powershell
dotnet run
```

## Release a Standalone Build

To produce a single self-contained folder that runs on any Windows 10/11 machine without the .NET runtime installed:

```powershell
dotnet publish -c Release -r win-x64 --self-contained -o publish
```

Output goes to `publish\`. Trimming is enabled for Release, so the bundle is around **40 MB** (versus ~110 MB untrimmed). Ship the whole folder — `SerialDebugger.exe` won't run without the sibling DLLs.

### Why is the csproj configured this way?

`SerialDebugger.csproj` enables trimming for Release with these knobs:

- `PublishTrimmed=true` + `TrimMode=full` — removes unreferenced code from every assembly.
- `_SuppressWinFormsTrimError=true` — WinForms refuses to trim by default; this acknowledges the risk.
- `CustomResourceTypesSupport=true` — keeps the binary resource deserializer (needed for the embedded `labelRefresh.Image` in `MainForm.Designer.cs`).
- `TrimmerRootAssembly` entries for `System.Resources.Extensions`, `System.Drawing`, `System.Drawing.Common` — these assemblies are referenced by *string name* from the embedded `.resources` blob, so the trimmer can't see they are needed.

## Troubleshooting

### Crash log

On any unhandled exception, the app writes a stack trace to:

```
%AppData%\SerialDebugger\crash.log
```

If `SerialDebugger.exe` exits silently or fails to launch, open that file first.

### "I changed the source code and the trimmed exe crashes"

Trimming is aggressive: any type or assembly that's only referenced *by name at runtime* (resx-embedded images, reflection, `Type.GetType(string)`, etc.) can be stripped from the published bundle. The fix loop:

1. Run the published exe; let it crash.
2. Open `%AppData%\SerialDebugger\crash.log` and look for `FileNotFoundException` (missing assembly) or `TypeLoadException` (missing type).
3. Add the missing assembly as a `TrimmerRootAssembly` in `SerialDebugger.csproj`, inside the Release-conditional `ItemGroup`:

   ```xml
   <ItemGroup Condition="'$(Configuration)' == 'Release'">
     <TrimmerRootAssembly Include="System.Resources.Extensions" />
     <TrimmerRootAssembly Include="The.Missing.Assembly" />
   </ItemGroup>
   ```

4. Republish and retest.

Common triggers when modifying the UI: dragging a new image control onto a form (resx adds a `System.Drawing.Bitmap` reference), adding a custom `TypeConverter`, calling `Encoding.GetEncoding("GBK")` without rooting `System.Text.Encoding.CodePages`, or pulling in a NuGet package that uses reflection internally.

If the crash loop becomes unmanageable, downgrade to safer trimming by editing `SerialDebugger.csproj`:

```xml
<TrimMode>partial</TrimMode>
```

That trims only assemblies marked `IsTrimmable`. The bundle grows to ~63 MB but the surface area for trim-related failures shrinks dramatically.

A debug build (`dotnet build` or `dotnet run`) does **not** trim, so it always reflects what the source code says. Use `dotnet run` to verify behaviour, then publish to verify trim-safety.

## Usage

1. Launch `SerialDebugger.exe`.
2. Pick a port from the dropdown (click **Refresh** if your device was plugged in after launch).
3. Set baud rate, data bits, parity, stop bits, encoding, and newline as needed.
4. Click **Open Port**.
5. Type into the text box, or paste a hex string like `48 65 6C 6C 6F` (whitespace is optional) into the hex box, then click **Send**.
6. Sent and received frames appear in their respective display panes with timestamps.

### Hex input rules

| Input                | Bytes                                    |
|----------------------|------------------------------------------|
| `48 65 6C 6C 6F`     | `48 65 6C 6C 6F`                         |
| `48656C6C6F`         | `48 65 6C 6C 6F`                         |
| `48656C6C6`          | `48 65 6C 6C 06` (trailing nibble = low) |
| `48656 C6C6F`        | `48 65 06 C6 C6 0F` (whitespace splits)  |

## Settings

User settings live at `%AppData%\SerialDebugger\settings.json`. They are validated on load — invalid values fall back to defaults and the file is rewritten.

| Key            | Default   | Notes                          |
|----------------|-----------|--------------------------------|
| `baudRate`     | `115200`  | 1200 – 921600                  |
| `dataBits`     | `8`       | 7 or 8                         |
| `parity`       | `None`    | None / Even / Odd / Mark       |
| `stopBits`     | `1`       | 1 / 1.5 / 2                    |
| `textEncoding` | `UTF-8`   | UTF-8 / ASCII / …              |
| `newLine`      | `LF`      | CR / LF / CRLF                 |
| `autoBreak`    | `1000`    | 0 – 10000 ms grouping interval |

## License

Released under the [GNU GPL v3](LICENSE).
