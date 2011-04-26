using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;

namespace The_Last_Trial
{
    public class Menu
    {
        private static bool pause;
        private static int currentCursor;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;
        private static Objet[] menuObject = new Objet[5];

        #region Init, Update & Draw

        public static void Init(ContentManager Content)
        {
            menuFont = Content.Load<SpriteFont>("font/menufont");
            menuObject[0] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("menu/1"));
            menuObject[1] = new Objet(new Vector2(300, 350), Content.Load<Texture2D>("menu/reprendre"));
            menuObject[2] = new Objet(new Vector2(300, 475), Content.Load<Texture2D>("menu/back"));
            menuObject[3] = new Objet(new Vector2(300, 600), Content.Load<Texture2D>("menu/quit"));
            menuObject[4] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("load/gameOver"));
            currentCursor = 0;
            pause = false;
            oldState = Keyboard.GetState();

        }

        public static bool Update(Monster[] monster, Personnage[] perso, PNJ[] pnj, GraphicsDeviceManager graphics, GameTime gameTime, ContentManager Content)
        {
            if (pause)
            {
                UpdatePause(Content);
            }
            else
            {
                newState = Keyboard.GetState();
                Monster.Update(monster, gameTime, perso, Content);
                Personnage.Update(perso, gameTime, monster, graphics, Content); 
                PNJ.Update(pnj, perso, gameTime, graphics, Content);
                Monster.Resu(monster);
                if (!Map.Update(gameTime, perso, Content))
                    return false;

                pause = (newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape));
                oldState = newState;
            }
            return GameOver(perso, Content);
        }

        private static void UpdatePause(ContentManager Content)
        {
            newState = Keyboard.GetState();
            string[] cursor = new string[3];
            if (currentCursor == 0)
            {
                cursor[0] = "_";
                cursor[1] = "";
                cursor[2] = "";
            }
            else if (currentCursor == 1)
            {
                cursor[0] = "";
                cursor[1] = "_";
                cursor[2] = "";
            }
            else if (currentCursor == 2)
            {
                cursor[0] = "";
                cursor[1] = "";
                cursor[2] = "_";
            }

            menuObject[1].S_Texture(Content.Load<Texture2D>("menu/reprendre" + cursor[0]));
            menuObject[2].S_Texture(Content.Load<Texture2D>("menu/back" + cursor[1]));
            menuObject[3].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor[2]));

            if (newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
            {
                pause = false;
            }
            if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
            {
                currentCursor--;
                if (currentCursor == -1)
                    currentCursor = 2;
            }
            if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
            {
                currentCursor = (currentCursor + 1) % 3;
            }
            if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                if (currentCursor == 0)
                {
                    pause = false;
                }
                else if (currentCursor == 1)
                {
                    GameState.Restart(Content);
                }
                else if (currentCursor == 2)
                {
                    Program.gs.Exit();
                }
            }

            oldState = newState;
        }

        public static void Draw(Personnage[] perso, Monster[] monster, PNJ[] pnj, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            try 
            {
                Map.DrawBack(spriteBatch);
                Map.DrawMiddle(spriteBatch);
                if (!Map.G_FirstHide())
                {
                    Map.DrawFirst(spriteBatch);
                }
                Mob.Draw(perso, monster, pnj, spriteBatch, gameTime);
                if (Map.G_FirstHide())
                {
                    Map.DrawFirst(spriteBatch);
                }
                if (pause)
                {
                    menuObject[0].Draw(spriteBatch);
                    menuObject[1].Draw(spriteBatch);
                    menuObject[2].Draw(spriteBatch);
                    menuObject[3].Draw(spriteBatch);
                }
            }
            catch (ArgumentNullException) { }

            spriteBatch.End();
        }

        public static void DrawGameOver(Personnage[] perso, Monster[] monster, PNJ[] pnj, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            try
            {
                Map.DrawBack(spriteBatch);
                Map.DrawMiddle(spriteBatch);
                if (!Map.G_FirstHide())
                {
                    Map.DrawFirst(spriteBatch);
                }
                Mob.Draw(perso, monster, pnj, spriteBatch, gameTime);
                if (Map.G_FirstHide())
                {
                    Map.DrawFirst(spriteBatch);
                }
                if (pause)
                {
                    menuObject[0].Draw(spriteBatch);
                    menuObject[1].Draw(spriteBatch);
                    menuObject[2].Draw(spriteBatch);
                    menuObject[3].Draw(spriteBatch);
                }
            }
            catch (ArgumentNullException) { }
            menuObject[4].Draw(spriteBatch);

            spriteBatch.End();
        }

        #endregion

        private static bool GameOver(Personnage[] perso, ContentManager Content)
        {
            bool continuer = false;
            foreach (Personnage p in perso)
            {
                if (p.G_IsAlive())
                {
                    continuer = true;
                }
            }
            if (!continuer)
            {
                oldState = Keyboard.GetState();
                menuObject[0] = new Objet(new Vector2(500, 400));
                menuObject[0].S_Texture(Content.Load<Texture2D>("menu/new"));
                menuObject[1] = new Objet(new Vector2(486, 560));
                menuObject[1].S_Texture(Content.Load<Texture2D>("menu/quit"));
                menuObject[2] = new Objet(new Vector2(150, 400));
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/epee"));
            }

            return continuer;
        }
    }
}
