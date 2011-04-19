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

        private static int nbPlayer, nbMonster, state, level;

        private static Personnage[] perso;
        private static Monster[] monster;
        private static PNJ[] pnj;
        private static Menu menu;

        public static int G_Player() { return nbPlayer; }
        public static int G_Monster() { return nbMonster; }
        public static int G_State() { return state; }
        public static int G_Level() { return level; }

        public GameState()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Load();
            lauch = new Launcher();
            game = new Game1();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        private void Load()
        {
            menu = new Menu(Content);
            state = 0;
            level = 1;
            nbPlayer = 0;
        }

        protected override void Update(GameTime gameTime)
        {
            if (state == 0)
            {
                if (level != 1)
                {
                    lauch.LoadNewMap(gameTime, Content, perso, ref monster, ref pnj, ref nbMonster);
                    state = 1;
                }
                else if (lauch.Update(gameTime, Content, ref perso, ref monster, ref pnj, ref nbPlayer, ref nbMonster))
                {
                    state = 1;
                }
            }
            else if (state == 1)
            {
                if (Map.G_EndLevel(monster))
                {
                    level++;
                    state = 0;
                }
                game.Update(gameTime, graphics, Content, perso, monster, pnj);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (state == 0)
            {
                lauch.Draw(gameTime, menu, perso, monster, pnj, spriteBatch, graphics);
            }
            else if (state == 1)
            {
                game.Draw(gameTime, spriteBatch, graphics, perso, monster, pnj, menu);
            }
            base.Draw(gameTime);
        }
    }
}
