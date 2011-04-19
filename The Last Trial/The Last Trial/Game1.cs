using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    public class Game1
    {
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, ContentManager Content, Personnage[] perso, Monster[] monster, PNJ[] pnj)
        {
            Menu.Update(monster, perso, pnj, graphics, gameTime, Content);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Personnage[] perso, Monster[] monster, PNJ[] pnj, Menu menu)
        {
            Menu.Draw(menu, perso, monster, pnj, spriteBatch, graphics, true);
        }
    }
}
