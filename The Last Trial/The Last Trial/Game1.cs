using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Last_Trial
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static int nbPlayer;
        private static int nbMonster;
        private static bool play;
        private static int level;

        // Declaration Objets
        private static Personnage[] perso;
        private static Monster[] monster;
        private static PNJ[] pnj;
        private static Menu menu;

        public static int G_Player() { return nbPlayer; }
        public static int G_Monster() { return nbMonster; }
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            play = false;
            level = 1;
        }

        protected override void Initialize()
        {
            menu = new Menu(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Map.G_EndLevel(monster))
            {
                level++;
                LoadLevel(gameTime);
            }

            if (play)
            {
                play = Menu.Update(monster, perso, pnj, graphics, gameTime, Content, this);
                if (!play)
                {
                    nbPlayer = 0;
                }
            }
            else
            {
                nbPlayer = Menu.Init(perso, Content, gameTime);
                play = nbPlayer != 0;
                if (nbPlayer == -1)
                    this.Exit();
                else if (play)
                {
                    LoadLevel(gameTime);
                    //Menu.LoadingScreen(spriteBatch, Content);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Menu.Draw(menu, perso, monster, pnj, spriteBatch, graphics, play);
            base.Draw(gameTime);
        }

        private void LoadLevel(GameTime gameTime)
        {
            pnj = new PNJ[Map.InitPNJ(level)];
            nbMonster = Map.Init(level, monster, pnj);
            perso = new Personnage[nbPlayer];
            monster = new Monster[nbMonster];
            monster = Map.LoadMonster(monster);
            Menu.Load(menu, perso, monster, pnj, Content, GraphicsDevice, gameTime, nbPlayer);
        }
    }
}
