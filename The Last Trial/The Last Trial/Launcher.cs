using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    class Launcher
    {

        public bool Update(GameTime gameTime, ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch sb, ref Personnage[] perso, ref Monster[] monster, ref PNJ[] pnj, ref int nbPlayer, ref int nbMonster)
        {
            nbPlayer = LoadingMenu.Update(perso, Content);
            if (nbPlayer != 0 || GameState.Level != 1)
            {
                perso = new Personnage[nbPlayer];
                LoadNewMap(gameTime, Content, graphics, sb, perso, ref monster, ref pnj, ref nbMonster);
                return true;
            }
            return false;
        }

        public void LoadNewMap(GameTime gameTime, ContentManager Content, GraphicsDeviceManager graphics, SpriteBatch sb, Personnage[] perso, ref Monster[] monster, ref PNJ[] pnj, ref int nbMonster)
        {
            pnj = new PNJ[Map.InitPNJ(GameState.Level)];
            nbMonster = Map.Init(GameState.Level, monster, pnj);
            monster = new Monster[nbMonster];
            monster = Map.LoadMonster(monster);
            LoadingMenu.Load(perso, monster, pnj, Content, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            LoadingMenu.Draw(spriteBatch, graphics);
        }
    }
}
