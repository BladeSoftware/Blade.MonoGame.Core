using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Animations
{
    public class AnimationDelay : AnimationBase
    {
        public Action<object> Action { get; set; }
        public object data { get; set; }
        public bool ActionExecuted { get; set; }

        public AnimationDelay(TimeSpan delay, Action<object> action, Object data = null) : base()
        {
            base.ActionStart = DateTime.Now;
            base.ActionEnd = DateTime.Now + delay;
            base.Duration = delay;
            this.Action = action;
            this.ActionExecuted = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Active && !ActionExecuted)
            {
                Action?.Invoke(data);
            }
        }

    }

}
