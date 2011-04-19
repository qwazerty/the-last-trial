using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Last_Trial
{
    class GameState : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Game1 game;
        private Launcher lauch;

        public GameState()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            lauch = new Launcher();
            game = new Game1(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            game.Update(gameTime, graphics, Content);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            game.Draw(gameTime, spriteBatch, graphics);
            base.Draw(gameTime);
        }
    }
}
