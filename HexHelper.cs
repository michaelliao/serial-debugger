using System.Text;

namespace SerialDebugger
{
    internal static class HexHelper
    {
        // Parses a hex string into bytes. Whitespace forces a byte boundary:
        // each whitespace-separated token is parsed independently. Within a
        // token, digits pair left-to-right; a trailing odd nibble becomes a
        // byte whose value equals that hex digit.
        //   "48 65 6C 6C 6F" -> [0x48, 0x65, 0x6C, 0x6C, 0x6F]
        //   "48656 C6C6F"    -> [0x48, 0x65, 0x06, 0xC6, 0xC6, 0x0F]
        //   "48656C6C6"      -> [0x48, 0x65, 0x6C, 0x6C, 0x06]
        public static byte[] ParseHex(string? input)
        {
            if (string.IsNullOrEmpty(input)) return Array.Empty<byte>();

            var result = new List<byte>(input.Length / 2 + 1);
            string[] tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                ParseToken(token, result);
            }
            return result.ToArray();
        }

        private static void ParseToken(string token, List<byte> result)
        {
            for (int i = 0; i < token.Length; i++)
            {
                if (!IsHexDigit(token[i]))
                {
                    throw new FormatException($"Invalid hex character: '{token[i]}'");
                }
            }

            int j = 0;
            while (j + 1 < token.Length)
            {
                result.Add((byte)((HexVal(token[j]) << 4) | HexVal(token[j + 1])));
                j += 2;
            }
            if (j < token.Length)
            {
                result.Add((byte)HexVal(token[j]));
            }
        }

        public static string FormatHex(byte[] bytes)
        {
            if (bytes.Length == 0) return string.Empty;
            var sb = new StringBuilder(bytes.Length * 3);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0) sb.Append(' ');
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static bool IsHexDigit(char c) =>
            (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');

        private static int HexVal(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            return c - 'a' + 10;
        }
    }
}
