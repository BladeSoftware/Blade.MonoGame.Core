using Microsoft.Xna.Framework;

namespace Blade.MG.Sequencing
{
    /// <summary>
    /// Delay for the required time before calling the action
    /// </summary>
    public class DelaySequence : SequenceBase
    {
        public DateTime SequenceStart;
        public DateTime ActionEnd;
        public TimeSpan Duration;



        public Action<object> Action { get; set; }
        public object data { get; set; }
        public bool ActionExecuted { get; set; }

        public DelaySequence(TimeSpan delay, Action<object> action, Object data = null) : base()
        {
            SequenceStart = DateTime.Now;
            ActionEnd = DateTime.Now + delay;
            Duration = delay;
            Action = action;
            ActionExecuted = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Finished = (DateTime.Now > ActionEnd);

            if (Finished && !ActionExecuted)
            {
                ActionExecuted = true;
                Action?.Invoke(data);
            }
        }

    }

}
