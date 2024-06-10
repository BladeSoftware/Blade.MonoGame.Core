using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class ColorHelper
    {
        /// <summary>
        /// Convert a Hex or Json Color string to a Monogame Color
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Color FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                // No color specified
                return Color.Transparent;
            }
            else if (value.StartsWith('#'))
            {
                // Hex Color (Web Format)
                return ColorHelper.FromHexColor(value);
            }
            else if (value.Contains('{'))
            {
                // {"R":0, "G":128, "B":0, "A":255} or {R:0 G:128 B:0 A:255}
                return ColorHelper.FromJsonColor(value);

            }

            throw new ArgumentException($"Invalid Color value : {value}");
        }

        /// <summary>
        /// Convert a HEX (#RRGGBB or #RRGGBBAA) Color to a Monogame Color
        /// The leading Hash is optional
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public static Color FromHexColor(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor))
            {
                throw new ArgumentOutOfRangeException("hexColor");
            }

            var hexDigits = "0123456789ABCDEFabcdef".AsSpan();

            Span<byte> array = stackalloc byte[4];

            var hexColorSpan = hexColor.AsSpan().Trim();
            if (hexColorSpan.StartsWith("#".AsSpan()))
            {
                hexColorSpan = hexColorSpan[1..];
            }

            int n;
            if (hexColorSpan.Length == 6)
            {
                n = 3;  // Only R,G,B values
                array[3] = 255; // Default A = 255
            }
            else if (hexColorSpan.Length == 8)
            {
                n = 4; // R,G,B,A values
            }
            else
            {
                throw new FormatException($"Invalid Hex Color Format : {hexColor}");
            }

            int j = 0;
            for (int i = 0; i < n; i++)
            {
                int digit1 = hexDigits.IndexOf(hexColorSpan[j]);
                if (digit1 > 15) digit1 -= 6;
                j++;

                int digit2 = hexDigits.IndexOf(hexColorSpan[j]);
                if (digit2 > 15) digit2 -= 6;
                j++;

                if (digit1 < 0 || digit2 < 0) throw new FormatException($"Invalid Hex Color Format : {hexColor}");

                array[i] = (byte)((digit1 << 4) + digit2);
            }

            return new Color(array[0], array[1], array[2], array[3]);
        }


        /// <summary>
        /// Convert a Json color string to a Monogame Color
        /// Supports: { "R":0, "G":128, "B":0, "A":255 }
        ///         : { R:0 G:128 B:0 A:255 }
        /// </summary>
        /// <param name="jsonColor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="FormatException"></exception>
        public static Color FromJsonColor(string jsonColor)
        {
            if (string.IsNullOrWhiteSpace(jsonColor))
            {
                throw new ArgumentOutOfRangeException("jsonColor");
            }

            // { "R":0, "G":128, "B":0, "A":255 } or {R:0 G:128 B:0 A:255}
            var jsonColorSpan = jsonColor.AsSpan().Trim();
            if (!jsonColorSpan.StartsWith("{".AsSpan()) && !jsonColorSpan.EndsWith("}".AsSpan()))
            {
                throw new FormatException($"Invalid Json Color Format : {jsonColor}");
            }

            // Remove the braces
            jsonColorSpan = jsonColorSpan[1..^1];

            Span<Range> parts = stackalloc Range[5];
            int numParts;

            if (jsonColorSpan.Contains(','))
            {
                numParts = jsonColorSpan.Split(parts, ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                numParts = jsonColorSpan.Split(parts, ' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            }

            int r;
            int g;
            int b;
            int a;

            if (numParts == 3 || numParts == 4)
            {
                // Read R,G,B values
                ReadOnlySpan<char> part0 = jsonColorSpan[parts[0]];
                ReadOnlySpan<char> part1 = jsonColorSpan[parts[1]];
                ReadOnlySpan<char> part2 = jsonColorSpan[parts[2]];

                part0 = part0.Slice(part0.IndexOf(':') + 1).Trim();
                part1 = part1.Slice(part1.IndexOf(':') + 1).Trim();
                part2 = part2.Slice(part2.IndexOf(':') + 1).Trim();

                r = int.Parse(part0);
                g = int.Parse(part1);
                b = int.Parse(part2);
                a = 255;
            }
            else
            {
                throw new FormatException($"Invalid Json Color Format : {jsonColor}");
            }

            if (numParts == 4)
            {
                // Read Alpha value
                ReadOnlySpan<char> part3 = jsonColorSpan[parts[3]];

                part3 = part3.Slice(part3.IndexOf(':') + 1).Trim();
                a = int.Parse(part3);
            }

            return new Color(r, g, b, a);
        }

        public static string ToHexColor(Color color)
        {
            var hexDigits = "0123456789ABCDEF";

            Span<char> strColor = stackalloc char[9];

            strColor[0] = '#';

            // Red
            int nibble1 = color.R & 0xF;
            int nibble2 = color.R >> 4;

            strColor[1] = hexDigits[nibble2];
            strColor[2] = hexDigits[nibble1];

            // Green
            nibble1 = color.G & 0xF;
            nibble2 = color.G >> 4;

            strColor[3] = hexDigits[nibble2];
            strColor[4] = hexDigits[nibble1];

            // Blue
            nibble1 = color.B & 0xF;
            nibble2 = color.B >> 4;

            strColor[5] = hexDigits[nibble2];
            strColor[6] = hexDigits[nibble1];

            // Alpha
            nibble1 = color.A & 0xF;
            nibble2 = color.A >> 4;

            strColor[7] = hexDigits[nibble2];
            strColor[8] = hexDigits[nibble1];

            return strColor.ToString();
        }

        public static string ToJsonColor(Color color)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"R\":");
            sb.Append(color.R);
            sb.Append(",\"G\":");
            sb.Append(color.G);
            sb.Append(",\"B\":");
            sb.Append(color.B);
            sb.Append(",\"A\":");
            sb.Append(color.A);
            sb.Append("}");

            return sb.ToString();
        }

    }
}
