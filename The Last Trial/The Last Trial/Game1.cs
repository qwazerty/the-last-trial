using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    public class Game1
    {
        
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
        
        public Game1(ContentManager Content)
        {
            menu = new Menu(Content);
            play = false;
            level = 1;
        }

        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, ContentManager Content)
        {
            if (play)
            {
                if (Map.G_EndLevel(monster))
                {
                    level++;
                    LoadLevel(gameTime, Content);
                }
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
                    //this.Exit();
                    ;
                else if (play)
                {
                    perso = new Personnage[nbPlayer];
                    LoadLevel(gameTime, Content);
                    //Menu.LoadingScreen(spriteBatch, Content);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Menu.Draw(menu, perso, monster, pnj, spriteBatch, graphics, play);
        }

        private void LoadLevel(GameTime gameTime, ContentManager Content)
        {
            pnj = new PNJ[Map.InitPNJ(level)];
            nbMonster = Map.Init(level, monster, pnj);
            monster = new Monster[nbMonster];
            monster = Map.LoadMonster(monster);
            Menu.Load(perso, monster, pnj, Content, gameTime, nbPlayer);
        }
    }
}
