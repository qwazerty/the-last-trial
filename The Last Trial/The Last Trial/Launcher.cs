using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    class Launcher
    {
        public Launcher()
        {
        }

        public bool Update(GameTime gameTime, ContentManager Content, ref Personnage[] perso, ref Monster[] monster, ref PNJ[] pnj, ref int nbPlayer, ref int nbMonster)
        {
            nbPlayer = Menu.Init(perso, Content, gameTime);
            if (nbPlayer == -1)
                Program.gs.Exit();
            if (nbPlayer != 0 || GameState.G_Level() != 1)
            {
                perso = new Personnage[nbPlayer];
                LoadNewMap(gameTime, Content, perso, ref monster, ref pnj, ref nbMonster);
                return true;
            }
            return false;
        }

        public void LoadNewMap(GameTime gameTime, ContentManager Content, Personnage[] perso, ref Monster[] monster, ref PNJ[] pnj, ref int nbMonster)
        {
            pnj = new PNJ[Map.InitPNJ(GameState.G_Level())];
            nbMonster = Map.Init(GameState.G_Level(), monster, pnj);
            monster = new Monster[nbMonster];
            monster = Map.LoadMonster(monster);
            Menu.Load(perso, monster, pnj, Content, gameTime);
            //Menu.LoadingScreen(spriteBatch, Content);
        }

        public void Draw(GameTime gameTime, Menu menu, Personnage[] perso, Monster[] monster, PNJ[] pnj, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Menu.Draw(menu, perso, monster, pnj, spriteBatch, graphics, false);
        }
    }
}
