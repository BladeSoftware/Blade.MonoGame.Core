using Microsoft.Xna.Framework;
using XnaInput = Microsoft.Xna.Framework.Input;


namespace Blade.MG.Input
{

    public static class InputManager
    {
        public static int MaxGamePads = 4;

        public static KeyboardInput Keyboard = new KeyboardInput();
        public static MouseInput Mouse = new MouseInput();
        public static TouchInput Touch = new TouchInput();


        private static GamePadInput[] gamePad = new GamePadInput[] { new GamePadInput(PlayerIndex.One), new GamePadInput(PlayerIndex.Two), new GamePadInput(PlayerIndex.Three), new GamePadInput(PlayerIndex.Four) };
        public static GamePadInput GamePad(int playerIndex) => gamePad[playerIndex];
        public static GamePadInput GamePad(PlayerIndex playerIndex) => gamePad[(int)playerIndex];

        public static void Update()
        {
            // Handle Keyboard Input
            Keyboard.UpdateState(XnaInput.Keyboard.GetState());

            // Handle Mouse Input
            Mouse.UpdateState(XnaInput.Mouse.GetState());

            // Handle GamePad Input
            GamePad(0).UpdateState(XnaInput.GamePad.GetState(PlayerIndex.One));
            GamePad(1).UpdateState(XnaInput.GamePad.GetState(PlayerIndex.Two));
            GamePad(2).UpdateState(XnaInput.GamePad.GetState(PlayerIndex.Three));
            GamePad(3).UpdateState(XnaInput.GamePad.GetState(PlayerIndex.Four));

            // Handle Touch Input
            Touch.UpdateState();
        }

    }
}

