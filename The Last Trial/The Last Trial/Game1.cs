using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace The_Last_Trial
{
    public class Game1
    {
        private bool continuer = true;
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, ContentManager Content, Personnage[] perso, Monster[] monster, PNJ[] pnj)
        {
            if (continuer)
            {
                continuer = Menu.Update(monster, perso, pnj, graphics, gameTime, Content);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, ContentManager Content, Personnage[] perso, Monster[] monster, PNJ[] pnj)
        {
            if (continuer)
            {
                Menu.Draw(perso, monster, pnj, spriteBatch, graphics, gameTime);
            }
            else
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Menu.DrawGameOver(perso, monster, pnj, spriteBatch, graphics, gameTime);
                }
                else
                {
                    GameState.Restart(Content);
                }
            }
        }
    }
}
