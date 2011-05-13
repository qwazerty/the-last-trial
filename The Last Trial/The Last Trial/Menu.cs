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
        private static Objet[] menuObject = new Objet[2];

        #region Init, Update & Draw

        public static void Init(ContentManager Content)
        {
            menuFont = Content.Load<SpriteFont>("font/menufont");
            menuObject[0] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("menu/1"));
            menuObject[1] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("load/gameOver"));
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

        public static void Draw(Personnage[] perso, Monster[] monster, PNJ[] pnj, SpriteBatch sb, GraphicsDeviceManager graphics, GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            sb.Begin(SpriteBlendMode.AlphaBlend);

            try 
            {
                Map.DrawBack(sb);
                Map.DrawMiddle(sb);
                if (!Map.G_FirstHide())
                {
                    Map.DrawFirst(sb);
                }
                Mob.Draw(perso, monster, pnj, sb, gameTime);
                if (Map.G_FirstHide())
                {
                    Map.DrawFirst(sb);
                }
                Map.DrawBossTrigger(sb, gameTime);
                if (pause)
                {
                    menuObject[0].Draw(sb);
                    Color color = Color.DarkKhaki;
                    if (currentCursor == 0)
                    {
                        color = Color.Gold;
                    }
                    sb.DrawString(menuFont, LoadingMenu.Local[13], new Vector2(350, 350), color);
                    if (currentCursor == 1)
                    {
                        color = Color.Gold;
                    }
                    else
                    {
                        color = Color.DarkKhaki;
                    }
                    sb.DrawString(menuFont, LoadingMenu.Local[14], new Vector2(350, 475), color);
                    if (currentCursor == 2)
                    {
                        color = Color.Gold;
                    }
                    else
                    {
                        color = Color.DarkKhaki;
                    }
                    sb.DrawString(menuFont, LoadingMenu.Local[2], new Vector2(350, 600), color);
                }
            }
            catch (ArgumentNullException) { }

            sb.End();
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
            }
            catch (ArgumentNullException) { }
            menuObject[1].Draw(spriteBatch);

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

            return continuer;
        }
    }
}
