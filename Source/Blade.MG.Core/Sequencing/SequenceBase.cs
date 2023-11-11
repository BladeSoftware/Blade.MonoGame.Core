using Microsoft.Xna.Framework;

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
