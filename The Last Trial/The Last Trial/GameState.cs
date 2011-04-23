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
        private static bool newLevel;

        private static Personnage[] perso;
        private static Monster[] monster;
        private static PNJ[] pnj;

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
            LoadingMenu.Init(Content);
            Menu.Init(Content);
            state = 0;
            level = 1;
            nbPlayer = 0;
            newLevel = false;
            lauch = new Launcher();
            game = new Game1();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (state == 0)
            {
                if (newLevel)
                {
                    lauch.LoadNewMap(gameTime, Content, graphics, spriteBatch, perso, ref monster, ref pnj, ref nbMonster);
                    state = 1;
                    newLevel = false;
                }
                else if (lauch.Update(gameTime, Content, graphics, spriteBatch, ref perso, ref monster, ref pnj, ref nbPlayer, ref nbMonster))
                {
                    state = 1;
                }
            }
            else if (state == 1)
            {
                game.Update(gameTime, graphics, Content, perso, monster, pnj);
                if (Map.G_EndLevel(monster))
                {
                    level++;
                    state = 0;
                    newLevel = true;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (state == 0 && !newLevel)
            {
                lauch.Draw(spriteBatch, graphics);
            }
            else if (state == 1)
            {
                game.Draw(gameTime, spriteBatch, graphics, perso, monster, pnj);
            }
            base.Draw(gameTime);
        }
    }
}
