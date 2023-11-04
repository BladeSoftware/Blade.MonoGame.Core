using Microsoft.Xna.Framework;

namespace Blade.MG.Sequencing
{
    public class SequenceManager
    {
        private static SequenceManager Instance { get; } = new SequenceManager();

        protected List<SequenceBase> Actions { get; init; }

        public SequenceManager()
        {
            Actions = new List<SequenceBase>();
        }

        public static void Add(SequenceBase action)
        {
            var root = action;
            while (root.Parent != null)
            {
                root = root.Parent;
            }

            Instance.Actions.Add(root);
        }

        public static void Update(GameTime gameTime)
        {
            var Actions = Instance.Actions;

            int count = Actions.Count;

            for (int i = 0; i < count; i++)
            {
                var action = Actions.ElementAtOrDefault(i);
                action?.Update(gameTime);
            }

            // Loop through and remove all completed actions
            for (int i = Actions.Count - 1; i >= 0; i--)
            {
                if (Actions[i].Finished)
                {
                    // If the Action has a child action, then add the child as a top node
                    if (Actions[i].Child != null)
                    {
                        Actions.Add(Actions[i].Child);
                    }

                    // Remove the Inactive action
                    Actions.RemoveAt(i);
                }
            }
        }


    }
}
