using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace The_Last_Trial
{
    class Menu : Objet
    {
        private static bool pause;

        public bool G_Pause() { return pause; }
        public void S_Pause(bool p) { pause = p; }

        public static int Init(Personnage[] perso)
        {
            // TODO : THE MAIN MENU
            return 2;
        }

        public static void Load(Menu menu, Personnage[] perso, Monster[] monster, ContentManager Content, GraphicsDevice device, int nbPlayer)
        {
            Personnage.Load(perso, Content, nbPlayer);
            Monster.Load(monster, Content);
            Map.Load(device, Content);
            Son.Load(Content);
            //Son.Play(0);
            menu.Load(Content);
        }

        public static void Update(Monster[] monster, 
            Personnage[] perso, 
            GraphicsDeviceManager graphics,
            GameTime gameTime, 
            ContentManager Content,
            int nbPlayer)
        {
            
            if (pause)
            {
                pause = Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
            else
            {
                Monster.Update(monster, gameTime, perso, Content);
                Personnage.Update(perso, gameTime, monster, graphics, Content);
                Map.Update(gameTime, perso, Content, nbPlayer);
                Monster.Resu(monster);
                pause = Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
        }

        public static void Draw(Menu menu, Personnage[] perso, Monster[] monster, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            Map.DrawBack(spriteBatch);
            Map.DrawMiddle(spriteBatch);
            Monster.Draw(monster, spriteBatch);
            Personnage.Draw(perso, spriteBatch);
            Map.DrawFirst(spriteBatch);
            menu.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void Load(ContentManager Content)
        {
            //objet = Content.Load<Texture2D>("menu/1/");
        }

        private void Draw(SpriteBatch sb)
        {
            //sb.Draw(base.objet, base.position, Color.White);
        }
    }
}
