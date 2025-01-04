using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GamePadDPad = Blade.MG.Input.GamePad.GamePadDPad;

namespace Blade.MG.Input
{
    public class GamePadInput
    {
        internal GamePadState GamePadState;
        internal GamePadState LastGamePadState;

        public PlayerIndex PlayerIndex { get; private set; }

        public GamePadDPad DPad { get; private set; }


        public GamePadInput(PlayerIndex playerIndex)
        {
            this.PlayerIndex = PlayerIndex;

            DPad = new GamePadDPad(this);
        }


        internal void UpdateState(GamePadState gamePadState)
        {
            LastGamePadState = GamePadState;
            GamePadState = gamePadState;
        }


        /// <summary>Packet Number</summary>
        public int PacketNumber => GamePadState.PacketNumber;

        /// <summary>True if the Game Pad is Connected</summary>
        public bool IsConnected => GamePadState.IsConnected;


        /// <summary>Left Thumb Stick Position</summary>
        public Vector2 ThumbStickLeft => GamePadState.ThumbSticks.Left;

        /// <summary>Right Thumb Stick Position</summary>
        public Vector2 ThumbStickRight => GamePadState.ThumbSticks.Right;


        /// <summary>Left Trigger Position</summary>
        public float TriggerLeft => GamePadState.Triggers.Left;

        /// <summary>Right Trigger Position</summary>
        public float TriggerRight => GamePadState.Triggers.Right;


        /// <summary>True if the button is Down</summary>
        public bool IsButtonDown(Buttons button) => GamePadState.IsButtonDown(button);

        /// <summary>True if the button is Up</summary>
        public bool IsButtonUp(Buttons button) => GamePadState.IsButtonUp(button);

        /// <summary>True if the button has been pressed since the previous state</summary>
        public bool IsButtonPressed(Buttons button) => GamePadState.IsButtonDown(button) && LastGamePadState.IsButtonUp(button);

        /// <summary>True if the button has been released since the previous state</summary>
        public bool IsButtonReleased(Buttons button) => GamePadState.IsButtonUp(button) && LastGamePadState.IsButtonDown(button);


    }
}
