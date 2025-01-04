using Microsoft.Xna.Framework.Input.Touch;
using XnaInput = Microsoft.Xna.Framework.Input;

namespace Blade.MG.Input
{
    public class TouchInput
    {
        public TouchCollection TouchState;
        public TouchCollection LastTouchState;

        public TouchPanelCapabilities TouchPanelCapabilities { get; private set; }

        public TouchInput()
        {
            // Only enable a few gestures by default. User can opt into others
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold | GestureType.HorizontalDrag | GestureType.VerticalDrag;
            TouchPanel.EnableMouseGestures = false;
            TouchPanel.EnableMouseTouchPoint = false;

            TouchPanelCapabilities = TouchPanel.GetCapabilities();
        }


        internal void UpdateState()
        {
            // Do we need to track this ourselves or just rely on the 'Get Gesture' feature ?
            LastTouchState = TouchState;
            TouchState = XnaInput.Touch.TouchPanel.GetState();

        }

        public bool IsConnected => TouchPanelCapabilities.IsConnected;
        public bool HasPressure => TouchPanelCapabilities.HasPressure;
        public int MaximumTouchCount => TouchPanelCapabilities.MaximumTouchCount;

        public bool TryGetGesture(out GestureSample gesture)
        {
            if (!TouchPanel.IsGestureAvailable)
            {
                gesture = new GestureSample();

                return false;
            }

            gesture = TouchPanel.ReadGesture();

            return true;
        }

        //public int NumPoints => TouchState.Count;

    }
}
