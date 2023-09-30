using Microsoft.Xna.Framework.Input;

namespace Blade.MG.Input.Keyboards
{
    public abstract class KeyboardBase
    {
        // Maps from a SDL key to a Tuple<Lower Case, Upper Case> e.g. 65 -> <"a","A">
        public abstract bool TryMap(Keys key, out Tuple<string, string> keyTuple);

        /// <summary>
        /// Returns a Character representation of the SDL Key or NULL if not available
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyboardState"></param>
        /// <returns></returns>
        public string GetChar(Keys key, KeyboardState keyboardState)
        {
            bool shift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
            bool capsLock = keyboardState.CapsLock;

            if (key >= Keys.A && key <= Keys.Z)
            {
                // Alpha Characters
                bool isUpper = shift ^ capsLock;
                string keyStr = key.ToString();

                return isUpper ? keyStr.ToUpper() : keyStr.ToLower();
            }
            else if (TryMap(key, out Tuple<string, string> keyTuple))
            {
                return shift ? keyTuple.Item2 : keyTuple.Item1;
            }
            else
            {
                return null;
            }

        }

    }
}
