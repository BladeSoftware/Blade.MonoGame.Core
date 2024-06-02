using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    public static class ColorHelper
    {
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
                throw new ArgumentOutOfRangeException(nameof(hexColor));
            }

            byte[] parts;

            if (hexColor.StartsWith("#"))
            {
                if (hexColor.Length == 7 || hexColor.Length == 9)
                {
                    parts = Convert.FromHexString(hexColor.Substring(1));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(hexColor));
                }
            }
            else
            {
                if (hexColor.Length == 6 || hexColor.Length == 8)
                {
                    parts = Convert.FromHexString(hexColor);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(hexColor));
                }
            }

            if (parts.Length == 3)
            {
                return new Color(parts[0], parts[1], parts[2], (byte)255);
            }
            else
            {
                return new Color(parts[0], parts[1], parts[2], parts[3]);
            }
        }

    }
}
