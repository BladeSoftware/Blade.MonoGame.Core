using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Blade.MG.Input
{

    public class InputManager
    {
        public static InputManager Instance { get; } = new InputManager();

        public static KeyboardState KeyboardState;
        public static KeyboardState LastKeyboardState;

        public static MouseState MouseState;
        public static MouseState LastMouseState;

        public static GamePadState GamePadStatePlayer1;
        public static GamePadState LastGamePadStatePlayer1;

        public static GamePadState GamePadStatePlayer2;
        public static GamePadState LastGamePadStatePlayer2;

        private InputManager()
        {

        }

        public void Update()
        {
            // Handle Keyboard Input
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            // Handle Mouse Input
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            // Handle Game Pad input
            LastGamePadStatePlayer1 = GamePadStatePlayer1;
            GamePadStatePlayer1 = GamePad.GetState(PlayerIndex.One);

            LastGamePadStatePlayer2 = GamePadStatePlayer2;
            GamePadStatePlayer2 = GamePad.GetState(PlayerIndex.Two);
        }


        // Returns true if they key was pressed 
        public static bool KeyPressed(Keys key)
        {
            return (InputManager.LastKeyboardState.IsKeyUp(key) && InputManager.KeyboardState.IsKeyDown(key));
        }

        // Returns true if they key was released
        public static bool KeyReleased(Keys key)
        {
            return (InputManager.LastKeyboardState.IsKeyDown(key) && InputManager.KeyboardState.IsKeyUp(key));
        }

        // Returns true if they key is down
        public static bool IsKeyDown(Keys key)
        {
            return InputManager.KeyboardState.IsKeyDown(key);
        }

        // Returns true if they key is up
        public static bool IsKeyUp(Keys key)
        {
            return InputManager.KeyboardState.IsKeyUp(key);
        }

    }
}
