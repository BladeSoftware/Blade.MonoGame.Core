using Microsoft.Xna.Framework.Input;
using System.Collections.Concurrent;

namespace Blade.MG.Core.Input.Keyboards
{
    public class KeyboardFrench : KeyboardBase
    {
        // Dictionary with the mapping from a SDL key to a Tuple<Lower Case, Upper Case> e.g. <"a","A">
        private static ConcurrentDictionary<Keys, Tuple<string, string>> KeyDict;

        public override bool TryMap(Keys key, out Tuple<string, string> keyTuple)
        {
            return KeyDict.TryGetValue(key, out keyTuple);
        }

        static KeyboardFrench()
        {
            KeyDict = new ConcurrentDictionary<Keys, Tuple<string, string>>();

            KeyDict.TryAdd(Keys.D1, Tuple.Create("&", "1"));        // Numeric 1
            KeyDict.TryAdd(Keys.D2, Tuple.Create("é", "2"));        // Numeric 2
            KeyDict.TryAdd(Keys.D3, Tuple.Create("\"", "3"));       // Numeric 3
            KeyDict.TryAdd(Keys.D4, Tuple.Create("'", "4"));        // Numeric 4
            KeyDict.TryAdd(Keys.D5, Tuple.Create("(", "5"));        // Numeric 5
            KeyDict.TryAdd(Keys.D6, Tuple.Create("-", "6"));        // Numeric 6
            KeyDict.TryAdd(Keys.D7, Tuple.Create("è", "7"));        // Numeric 7
            KeyDict.TryAdd(Keys.D8, Tuple.Create("_", "8"));        // Numeric 8
            KeyDict.TryAdd(Keys.D9, Tuple.Create("ç", "9"));        // Numeric 9
            KeyDict.TryAdd(Keys.D0, Tuple.Create("à", "0"));        // Numeric 0

            KeyDict.TryAdd(Keys.Space, Tuple.Create(" ", " "));     // Space

            KeyDict.TryAdd(Keys.Multiply, Tuple.Create("*", "*"));  // Multiply
            KeyDict.TryAdd(Keys.Add, Tuple.Create("+", "+"));       // Add
            KeyDict.TryAdd(Keys.Subtract, Tuple.Create("-", "-"));  // Subtract
            KeyDict.TryAdd(Keys.Decimal, Tuple.Create(".", "."));   // Decimal
            KeyDict.TryAdd(Keys.Divide, Tuple.Create("/", "/"));    // Divide

            KeyDict.TryAdd(Keys.OemSemicolon, Tuple.Create(";", ":"));     // The OEM Semicolon key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemPlus, Tuple.Create("=", "+"));          // For any country/region, the '+' key.
            KeyDict.TryAdd(Keys.OemComma, Tuple.Create(",", "<"));         // For any country/region, the ',' key.
            KeyDict.TryAdd(Keys.OemMinus, Tuple.Create(")", "°"));         // For any country/region, the '-' key.
            KeyDict.TryAdd(Keys.OemPeriod, Tuple.Create(".", ">"));        // For any country/region, the '.' key.
            KeyDict.TryAdd(Keys.OemQuestion, Tuple.Create("/", "?"));      // The OEM question mark key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemTilde, Tuple.Create("`", "~"));         // The OEM tilde key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemOpenBrackets, Tuple.Create("[", "{"));  // The OEM open bracket key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemPipe, Tuple.Create("\\", "|"));         // The OEM pipe key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemCloseBrackets, Tuple.Create("]", "}")); // The OEM close bracket key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemQuotes, Tuple.Create("'", "\""));       // The OEM singled/float quote key on a US standard keyboard.
            KeyDict.TryAdd(Keys.OemBackslash, Tuple.Create("\\", "|"));    // The OEM angle bracket or backslash key on the RT 102 key keyboard.

        }
    }
}
