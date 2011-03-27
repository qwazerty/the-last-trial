using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;

namespace The_Last_Trial
{
    class Menu : Objet
    {
        private static bool pause, firstPause, start;
        private static int state;
        private static float tempsInit, tempsActuel, temp;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;
        private static Objet[] menuObject = new Objet[3];

        public Menu(ContentManager Content) 
        {
            objet = Content.Load<Texture2D>("menu/1");
            menuFont = Content.Load<SpriteFont>("menufont");
            oldState = Keyboard.GetState();
            menuObject[0] = new Objet(new Vector2(500, 400));
            menuObject[1] = new Objet(new Vector2(486, 560));
            menuObject[2] = new Objet(new Vector2(150, 400));
            menuObject[2].S_Texture(Content.Load<Texture2D>("menu/epee"));
            temp = 0;
            state = 0;
            firstPause = false;
            pause = true;
            start = false;
            tempsInit = 0;
        }

        // BORDEL DE MERDE !!!
        // Je vous met au defi de comprendre cette fonction ! :D
        public static int Init(Personnage[] perso, ContentManager Content, GameTime gameTime)
        {
            newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.F12))
            {
                menuObject[0].S_Texture(Content.Load<Texture2D>("menu/back"));
                menuObject[0].S_Position(new Vector2(150, 400));
                menuObject[1].S_Position(new Vector2(150, 560));
                pause = false;
                return 2;
            }
            tempsActuel = (float)gameTime.TotalRealTime.TotalSeconds;
            if (state <= 0)
            {
                if (tempsInit + 0.9 > tempsActuel)
                {
                    temp += (tempsInit + 0.9f - tempsActuel) * 4;
                }
                else if (tempsInit + 1 > tempsActuel) 
                { 
                }
                else
                {
                    if (tempsInit + 1.9 > tempsActuel)
                    {
                        temp -= (tempsInit + 1.9f - tempsActuel) * 4;
                    }
                    else if (tempsInit + 2 > tempsActuel)
                    {
                    }
                    else
                    {
                        tempsInit = tempsActuel;
                    }
                }

                string[] cursor = new string[2];
                if (state == 0)
                {
                    cursor[0] = "_";
                    cursor[1] = "";
                }
                else
                {
                    cursor[0] = "";
                    cursor[1] = "_";
                }

                menuObject[0].S_Texture(Content.Load<Texture2D>("menu/new" + cursor[0]));
                menuObject[1].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor[1]));
                menuObject[2].S_Position(new Vector2(150 + temp, 400 - 160 * state));
                if (newState.IsKeyDown(Keys.Enter))
                {
                    if (state == 0)
                    {
                        state = 43;
                        tempsInit = tempsActuel;
                    }
                    else
                    {
                        state = 42;
                    }
                }
                if ((newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) || (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)))
                {
                    if (state == -1)
                    {
                        state = 0;
                    }
                    else
                    {
                        state = -1;
                    }
                }
            }
            else
            {
                if (tempsInit + 2.5 > tempsActuel && state > 40)
                {
                    temp += (tempsActuel - tempsInit) * 10;
                    if (state != 42)
                    {
                        menuObject[2].S_Position(new Vector2(150 + temp, 400));
                    }
                    else
                    {
                        menuObject[2].S_Position(new Vector2(150 + temp, 560));
                    }
                }
                else if (tempsInit + 4.5 > tempsActuel && state > 40)
                {
                    firstPause = true;
                    if (state == 42)
                    {
                        return -1;
                    }

                    menuObject[0].S_Texture(Content.Load<Texture2D>("menu/player"));
                    menuObject[0].S_Position(new Vector2(20, 400));

                    temp -= (tempsInit + 4.5f - tempsActuel) * 10;
                    menuObject[2].S_Position(new Vector2(150 + temp, 400));
                }
                else if (tempsInit + 2.5 > tempsActuel && start)
                {
                    temp -= (tempsActuel - tempsInit) * 10;
                    if (state != 39)
                    {
                        menuObject[2].S_Position(new Vector2(820 + temp, 240));
                    }
                    else
                    {
                        menuObject[2].S_Position(new Vector2(820 + temp, 400));
                    }
                }
                else if (start)
                {
                    if (state == 39)
                    {
                        return -1;
                    }
                    else
                    {
                        menuObject[0].S_Texture(Content.Load<Texture2D>("menu/back"));
                        menuObject[0].S_Position(new Vector2(150, 400));
                        menuObject[1].S_Position(new Vector2(150, 560));
                        return state;
                    }
                }
                else
                {
                    if (state > 40)
                    {
                        state = 1;
                        temp = 0;
                        tempsInit = tempsActuel;
                    }
                    string cursor;
                    if (pause)
                    {
                        cursor = "";
                    }
                    else
                    {
                        cursor = "_";
                    }

                    menuObject[2].S_Position(new Vector2(820 + temp, 400));
                    menuObject[1].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor));

                    if (tempsInit + 0.9 > tempsActuel)
                    {
                        temp += (tempsInit + 0.9f - tempsActuel) * 4;
                    }
                    else if (tempsInit + 1 > tempsActuel) 
                    { 
                    }
                    else
                    {
                        if (tempsInit + 1.9 > tempsActuel)
                        {
                            temp -= (tempsInit + 1.9f - tempsActuel) * 4;
                        }
                        else if (tempsInit + 2 > tempsActuel)
                        {
                        }
                        else
                        {
                            tempsInit = tempsActuel;
                        }
                    }

                    if (newState.IsKeyDown(Keys.Enter))
                    {
                        if (pause == false)
                        {
                            state = 39;
                        }

                        tempsInit = tempsActuel;
                        pause = false;
                        start = true;
                    }
                    else if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                    {
                        if (state == 1)
                        {
                            state = 4;
                        }
                        else
                        {
                            state--;
                        }
                    }
                    else if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                    {
                        if (state == 4)
                        {
                            state = 1;
                        }
                        else
                        {
                            state++;
                        }
                    }
                    else if ((newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) || (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)))
                    {
                        pause = !pause;
                    }
                }
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

        public static void Update(Monster[] monster, Personnage[] perso, PNJ pnj, GraphicsDeviceManager graphics, GameTime gameTime, ContentManager Content, Game1 game)
        {
            if (pause)
            {
                UpdatePause(Content, game);
            }
            else
            {
                Monster.Update(monster, gameTime, perso, Content);
                Personnage.Update(perso, gameTime, monster, graphics, Content); 
                PNJ.Update(pnj, perso, gameTime, graphics, Content);
                Map.Update(gameTime, perso, Content);
                Monster.Resu(monster);
                if (!Keyboard.GetState().IsKeyDown(Keys.Escape))
                    firstPause = true;
                pause = (Keyboard.GetState().IsKeyDown(Keys.Escape) && firstPause);
                if (pause)
                    state = 1;
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
                Mob.Draw(perso, monster, pnj, spriteBatch);
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
            if (Game1.G_Player() == 0)
            {
                sb.Draw(objet, position, Color.White);
                sb.Draw(menuObject[0].G_Texture(), menuObject[0].G_Position(), Color.White);
                sb.Draw(menuObject[1].G_Texture(), menuObject[1].G_Position(), Color.White);
                if (firstPause)
                {
                    if (pause)
                        sb.Draw(menuObject[2].G_Texture(),
                            new Rectangle((int)menuObject[2].G_Position().X + 300, (int)menuObject[2].G_Position().Y + 100,
                                menuObject[2].G_Texture().Width, menuObject[2].G_Texture().Height),
                            null, Color.White, (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);
                    else
                        sb.Draw(menuObject[2].G_Texture(),
                            new Rectangle((int)menuObject[2].G_Position().X + 300, (int)menuObject[2].G_Position().Y + 260,
                                menuObject[2].G_Texture().Width, menuObject[2].G_Texture().Height),
                            null, Color.White, (float)Math.PI, new Vector2(0, 0), SpriteEffects.None, 0);

                    if (state < 39)
                        sb.DrawString(menuFont, Convert.ToString(state), new Vector2(747, 422), Color.Black);
                }
                else
                {
                    sb.Draw(menuObject[2].G_Texture(), menuObject[2].G_Position(), Color.White);
                }
            }
            else if (pause)
            {
                sb.Draw(objet, position, Color.White);
                sb.Draw(menuObject[0].G_Texture(), menuObject[0].G_Position(), Color.White);
                sb.Draw(menuObject[1].G_Texture(), menuObject[1].G_Position(), Color.White);
            }

        }

        private static void UpdatePause(ContentManager Content, Game1 game)
        {
            newState = Keyboard.GetState();
            string[] cursor = new string[2];
            if (state == 1)
            {
                cursor[0] = "_";
                cursor[1] = "";
            }
            else
            {
                cursor[0] = "";
                cursor[1] = "_";
            }

            menuObject[0].S_Texture(Content.Load<Texture2D>("menu/back" + cursor[0]));
            menuObject[1].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor[1]));

            if (!newState.IsKeyDown(Keys.Escape))
                firstPause = false;
            if (newState.IsKeyDown(Keys.Escape) && !firstPause)
                pause = false;

            if ((newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) || (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)))
            {
                if (state == 1)
                    state = 2;
                else
                    state = 1;
            }

            if (newState.IsKeyDown(Keys.Enter))
            {
                if (state == 1)
                    pause = false;
                else if (state == 2)
                    game.Exit();
            }
            oldState = newState;
        }
    }
}
