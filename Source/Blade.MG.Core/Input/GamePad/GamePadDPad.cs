using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Input.GamePad
{
    public class GamePadDPad
    {
        private GamePadInput _gamePad;

        public GamePadDPad(GamePadInput gamePad)
        {
            _gamePad = gamePad;
        }

        /// <summary>D-Pad Up Direction Button</summary>
        public Button Up => new Button(_gamePad.GamePadState.DPad.Up, _gamePad.LastGamePadState.DPad.Up);

        /// <summary>D-Pad Down Direction Button</summary>
        public Button Down => new Button(_gamePad.GamePadState.DPad.Down, _gamePad.LastGamePadState.DPad.Down);

        /// <summary>D-Pad Left Direction Button</summary>
        public Button Left => new Button(_gamePad.GamePadState.DPad.Left, _gamePad.LastGamePadState.DPad.Left);

        /// <summary>D-Pad Right Direction Button</summary>
        public Button Right => new Button(_gamePad.GamePadState.DPad.Right, _gamePad.LastGamePadState.DPad.Right);

    }
}
