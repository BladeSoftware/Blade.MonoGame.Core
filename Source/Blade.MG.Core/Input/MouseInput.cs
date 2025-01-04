using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Input
{

    public enum MouseButton
    {
        Primary,
        Secondary,
        Middle,
        XButton1,
        XButton2
    }

    public class MouseInput : MouseInputBase
    {
        public readonly MouseInputBase PreviousMouseState = new MouseInputBase();


        internal void UpdateState(MouseState state)
        {
            PreviousMouseState.SetMouseState(MouseState);
            MouseState = state;
        }


        /// <summary>Returns True if the mouse pointer has moved</summary>
        public bool HasPointerMoved => MouseState.X != PreviousMouseState.MouseState.X || MouseState.Y != PreviousMouseState.MouseState.Y;

        /// <summary>Returns True if the mouse pointer has moved</summary>
        public bool HasScrollWheelValueChanged => MouseState.ScrollWheelValue != PreviousMouseState.ScrollWheelValue;

        /// <summary>Returns True if the mouse pointer has moved</summary>
        public bool HasHorizontalScrollWheelValueChanged => MouseState.HorizontalScrollWheelValue != HorizontalScrollWheelValue;


        /// <summary>Returns a delta for the mouse pointers movement</summary>
        public Point PositionDelta => new Point(MouseState.X - PreviousMouseState.MouseState.X, MouseState.Y - PreviousMouseState.MouseState.Y);

        /// <summary>Returns a delta for the mouse pointers Horizontal movement</summary>
        public int PositionXDelta => MouseState.X - PreviousMouseState.MouseState.X;

        /// <summary>Returns a delta for the mouse pointers Vertical movement</summary>
        public int PositionYDelta => MouseState.Y - PreviousMouseState.MouseState.Y;


        // Does a Scroll Wheel Delta make sense ? Aren't the values already Deltas since the last update ?
        /// <summary>Returns a delta for the mouse Vertical Scroll Wheel</summary>
        public int ScrollWheelValueDelta => ScrollWheelValue - PreviousMouseState.ScrollWheelValue;

        /// <summary>Returns a delta for the mouse Horizontal Scroll Wheel</summary>
        public int HorizontalScrollWheelValueDelta => HorizontalScrollWheelValue - PreviousMouseState.HorizontalScrollWheelValue;


        // Buttons

        public Button PrimaryButton => new Button(MouseState.LeftButton, PreviousMouseState.MouseState.LeftButton);
        public Button SecondaryButton => new Button(MouseState.RightButton, PreviousMouseState.MouseState.RightButton);
        public Button MiddleButton => new Button(MouseState.MiddleButton, PreviousMouseState.MouseState.MiddleButton);

        public Button XButton1 => new Button(MouseState.XButton1, PreviousMouseState.MouseState.XButton1);
        public Button XButton2 => new Button(MouseState.XButton2, PreviousMouseState.MouseState.XButton2);


    }


    /// <summary>
    /// Wrap the Mouse State and add some helper functions
    /// </summary>
    public class MouseInputBase
    {
        // Allow Re-mapping the Scroll Wheel directions
        public static bool InvertScrollWheel = false;
        public static bool InvertHorizontalScrollWheel = false;


        // The actual mouse state
        public MouseState MouseState;


        internal void SetMouseState(MouseState state)
        {
            MouseState = state;
        }


        /// <summary>Mouse pointer position</summary>
        public Point Position => MouseState.Position;

        /// <summary>Mouse pointer X value</summary>
        public int X => MouseState.X;

        /// <summary>Mouse pointer Y value</summary>
        public int Y => MouseState.Y;

        /// <summary>Mouse Vertical Scroll Wheel - distance scrolled since last update</summary>
        public int ScrollWheelValue => MouseState.ScrollWheelValue * (InvertScrollWheel ? -1 : 1);

        /// <summary>Mouse Horizontal Scroll Wheel - distance scrolled since last update</summary>
        public int HorizontalScrollWheelValue => MouseState.HorizontalScrollWheelValue * (InvertHorizontalScrollWheel ? -1 : 1);

    }

}
