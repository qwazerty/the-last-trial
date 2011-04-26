using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    class GameState : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static Game1 game;
        private static Launcher lauch;

        private static int nbPlayer, nbMonster, state, level;
        private static bool newLevel, applyChanges;

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
            this.graphics.PreferredBackBufferWidth = Program.width;
            this.graphics.PreferredBackBufferHeight = Program.height;
            this.graphics.IsFullScreen = Program.fullscreen;
            this.graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lauch = new Launcher();
            Son.Load(Content);
            Son.InitLoopSound(0);
            Restart(Content);
            base.Initialize();
        }

        public static void Restart(ContentManager Content)
        {
            game = new Game1();
            Son.InstanceStop();
            if (Program.musique)
            {
                Son.InitLoopSound(0);
                Son.InstancePlay();
            }
            LoadingMenu.Init(Content);
            Menu.Init(Content);
            state = 0;
            level = 1;
            nbPlayer = 0;
            newLevel = false;
            applyChanges = true;
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
                this.graphics.PreferredBackBufferHeight = 1000;
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
            if (applyChanges)
            {
                this.graphics.PreferredBackBufferWidth = Program.width;
                this.graphics.PreferredBackBufferHeight = Program.height;
                this.graphics.IsFullScreen = Program.fullscreen;
                this.graphics.ApplyChanges();
                applyChanges = false;
            }
            if (state == 0 && !newLevel)
            {
                lauch.Draw(spriteBatch, graphics);
            }
            else if (state == 1)
            {
                game.Draw(gameTime, spriteBatch, graphics, Content, perso, monster, pnj);
            }
            base.Draw(gameTime);
        }
    }
}
