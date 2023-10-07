using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    public static class GameExtensions
    {
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Viewport Viewport(this Game game) => game.GraphicsDevice.Viewport;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Rectangle TitleSafeArea(this Game game) => game.GraphicsDevice.DisplayMode.TitleSafeArea;

        //public int DisplayWidth => GraphicsDevice.DisplayMode.Width;
        //public int DisplayHeight => GraphicsDevice.DisplayMode.Height;
    }
}

