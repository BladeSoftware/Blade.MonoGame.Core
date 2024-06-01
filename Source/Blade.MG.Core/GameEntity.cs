using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG
{

    public abstract class GameEntity : IDisposable
    {
        protected Game game;
        protected virtual GraphicsDevice graphicsDevice => game.GraphicsDevice;
        protected ContentManager Content => game?.Content;

        public HashSet<string> Tags = new HashSet<string>();

        public virtual void Initialize(Game game)
        {
            this.game = game;
        }

        public void AddTag(string tag)
        {
            if (tag != null && !Tags.Contains(tag))
            {
                Tags.Add(tag);
            }
        }

        public void AddTags(List<string> tags)
        {
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    string tag = tags[i];

                    if (tag != null && !Tags.Contains(tag))
                    {
                        Tags.Add(tag);
                    }
                }
            }

        }


        public virtual void Dispose()
        {
        }

        //public abstract void Initialize(Game game);
        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);

        public virtual void BeforDraw(Game game, GameTime gameTime)
        {
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        //public virtual void OnCollison(GameEntity gameEntity, CollisionResult2D collisionResult)
        //{
        //}

    }
}
