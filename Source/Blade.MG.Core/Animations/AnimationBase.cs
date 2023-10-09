using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Animations
{
    public class AnimationBase
    {
        public DateTime ActionStart;
        public DateTime ActionEnd;
        public TimeSpan Duration;
        public bool Active;

        public AnimationBase()
        {
            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            Active = (DateTime.Now <= ActionEnd);
        }

    }
}
