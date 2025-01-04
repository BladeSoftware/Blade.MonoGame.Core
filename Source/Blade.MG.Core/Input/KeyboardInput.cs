using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Input
{
    public class KeyboardInput
    {
        public KeyboardState KeyboardState;
        public KeyboardState LastKeyboardState;

        internal void UpdateState(KeyboardState state)
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = state;
        }

        /// <summary>Returns true if the key was pressed</summary>
        public bool KeyPressed(Keys key) => (KeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));

        /// <summary>Returns true if the key was released</summary>
        public bool KeyReleased(Keys key) => (KeyboardState.IsKeyUp(key) && LastKeyboardState.IsKeyDown(key));

        /// <summary>Returns true if the key is down</summary>
        public bool IsKeyDown(Keys key) => KeyboardState.IsKeyDown(key);

        /// <summary>Returns true if the key is up</summary>
        public bool IsKeyUp(Keys key) => KeyboardState.IsKeyUp(key);

    }

}
