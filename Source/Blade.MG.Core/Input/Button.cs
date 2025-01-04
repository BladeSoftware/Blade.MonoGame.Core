using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Input
{
    public struct Button
    {
        private ButtonState _currentState;
        private ButtonState _previousState;

        public Button(ButtonState currentState, ButtonState previousState)
        {
            _currentState = currentState;
            _previousState = previousState;
        }

        /// <summary>True if the button is currently Up</summary>
        public bool IsUp => _currentState == ButtonState.Released;

        /// <summary>True if the button is currently held down</summary>
        public bool IsDown => _currentState == ButtonState.Pressed;


        /// <summary>True if the button has been pressed since the previous state</summary>
        public bool Pressed => _currentState == ButtonState.Pressed && _previousState == ButtonState.Released;

        /// <summary>True if the button has been released since the previous state</summary>
        public bool Released => _currentState == ButtonState.Released && _previousState == ButtonState.Pressed;
    }
}
