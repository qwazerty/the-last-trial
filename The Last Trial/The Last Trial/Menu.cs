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
        private static bool pause = true;
        private static bool firstPause = false;
        private static int state = 1;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;

        public Menu(ContentManager Content) 
        {
            objet = Content.Load<Texture2D>("menu/1");
            menuFont = Content.Load<SpriteFont>("menufont");
            oldState = Keyboard.GetState();
        }

        public static int Init(Personnage[] perso)
        {
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Enter))
            {
                pause = false;
                return state;
            }
            if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
            {
                if (state == 1)
                    state = 4;
                else
                    state--;
            }
            if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
            {
                if (state == 4)
                    state = 1;
                else
                    state++;
            }
            oldState = newState;
            return 0;
        }

        public static void Load(Menu menu, Personnage[] perso, Monster[] monster, PNJ pnj, ContentManager Content, GraphicsDevice device, int nbPlayer)
        {
            Mob.Load(Content);
            Personnage.Load(perso, Content, nbPlayer);
            Monster.Load(monster, Content);
            PNJ.Load(pnj, Content);
            Map.Load(device, Content);
            Son.Load(Content);
            Son.PlayLoop(0);
        }

        public static void Update(Monster[] monster, 
            Personnage[] perso, 
            PNJ pnj,
            GraphicsDeviceManager graphics,
            GameTime gameTime, 
            ContentManager Content,
            int nbPlayer,
            Game1 game)
        {
            
            if (pause)
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                    firstPause = false;
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !firstPause)
                    pause = false;

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    pause = false;

                if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                    game.Exit();
            }
            else
            {
                Monster.Update(monster, gameTime, perso, Content);
                Personnage.Update(perso, gameTime, monster, graphics, Content); 
                PNJ.Update(pnj, perso[0], gameTime, graphics, Content);
                Map.Update(gameTime, perso, Content, nbPlayer);
                Monster.Resu(monster);
                if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                    firstPause = true;
                pause = (Keyboard.GetState().IsKeyDown(Keys.Escape) && firstPause);
            }
        }

        public static void Draw(Menu menu, Personnage[] perso, Monster[] monster, PNJ pnj, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, bool play)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            if (play)
            {
                Map.DrawBack(spriteBatch);
                Map.DrawMiddle(spriteBatch);
                Mob.Draw(monster, spriteBatch);
                Monster.Draw(monster, spriteBatch);
                Personnage.Draw(perso, spriteBatch);
                PNJ.Draw(pnj, perso[0], spriteBatch);
                Map.DrawFirst(spriteBatch);
                menu.Draw(spriteBatch);
            }
            else
            {
                menu.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void Draw(SpriteBatch sb)
        {
            if (pause)
                sb.Draw(objet, position, Color.White);

            if (Game1.G_Player() == 0)
            {
                sb.DrawString(menuFont, Convert.ToString(state), new Vector2(700, 500), Color.White);
            }
        }
    }
}
