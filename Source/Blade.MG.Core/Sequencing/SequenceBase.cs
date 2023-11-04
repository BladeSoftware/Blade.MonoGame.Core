using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Sequencing
{
    public class SequenceBase
    {
        public SequenceBase Parent;
        public SequenceBase Child;

        public bool Finished;

        public SequenceBase Then(SequenceBase action)
        {
            this.Child = action;

            action.Parent = this;
            return action;
        }

        public SequenceBase()
        {
            Finished = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

    }
}
