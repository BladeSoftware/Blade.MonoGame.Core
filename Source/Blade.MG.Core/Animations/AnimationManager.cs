using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Animations
{
    public class AnimationManager
    {
        public static AnimationManager Instance { get; } = new AnimationManager();

        public List<AnimationBase> Actions { get; init; }

        public AnimationManager()
        {
            Actions = new List<AnimationBase>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var action in Actions)
            {
                action.Update(gameTime);
            }

            for (int i = Actions.Count - 1; i >= 0; i--)
            {
                if (!Actions[i].Active)
                {
                    Actions.RemoveAt(i);
                }
            }
        }

    }
}
