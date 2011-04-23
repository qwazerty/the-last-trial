using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.IO;

namespace The_Last_Trial
{
    public class LoadingMenu
    {
        private static int state, currentCursor, nbPlayer;
        private static int[] setup = new int[3];
        private static float speedX;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;
        private static Objet[] menuObject = new Objet[8];
        private static Objet background;

        public static void Init(ContentManager Content)
        {
            menuObject[0] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("menu/1"));
            menuObject[1] = new Objet(new Vector2(42, 350), Content.Load<Texture2D>("menu/epee"));
            menuObject[2] = new Objet(new Vector2(500, 350));
            menuObject[3] = new Objet(new Vector2(500, 475));
            menuObject[4] = new Objet(new Vector2(500, 600));
            menuObject[5] = new Objet(new Vector2(800, 350), Content.Load<Texture2D>("menu/on"));
            menuObject[6] = new Objet(new Vector2(800, 475), Content.Load<Texture2D>("menu/on"));
            menuObject[7] = new Objet(new Vector2(800, 600), Content.Load<Texture2D>("menu/on"));
            background = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("load/1"));
            speedX = 0;
            state = 0;
            nbPlayer = 1;
            currentCursor = 0;
            menuFont = Content.Load<SpriteFont>("font/menufont");
            oldState = Keyboard.GetState();

            InitSetup();
            Son.Load(Content);
            if (setup[2] == 1)
                Son.PlayLoop(0);
        }

        private static void InitSetup()
        {
            FileStream fs;
            StreamReader sr;
            try
            {
                fs = new FileStream("setup", FileMode.Open);
                sr = new StreamReader(fs);
            }
            catch (FileNotFoundException)
            {
                fs = new FileStream("setup", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("resolution=1200x800");
                sw.WriteLine("fullscreen=off");
                sw.WriteLine("musique=on");
                sw.Close();
                fs.Close();
                setup[0] = 1;
                setup[1] = 0;
                setup[2] = 1;
                return;
            }
            string str = sr.ReadLine();
            str = str.Substring(11);
            if (str == "1000x800")
                setup[0] = 0;
            else if (str == "1200x800")
                setup[0] = 1;
            str = sr.ReadLine();
            str = str.Substring(11);
            if (str == "off")
                setup[1] = 0;
            else if (str == "on")
                setup[1] = 1;
            str = sr.ReadLine();
            str = str.Substring(8);
            if (str == "off")
                setup[2] = 0;
            else if (str == "on")
                setup[2] = 1;
            sr.Close();
            fs.Close();
        }

        public static int Update(Personnage[] perso, ContentManager Content)
        {
            /******* STATE ********\ 
             * 0 : Menu principal *
             * 1 : Choix nb perso *
             * 2 : Setup          *
             * 9 : Exit           *
            \**********************/

            newState = Keyboard.GetState();

            #region Raccourcis
            if (newState.IsKeyDown(Keys.F10))
            {
                //MENU
            }
            if (newState.IsKeyDown(Keys.F11))
            {
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/back"));
                menuObject[2].S_Position(new Vector2(150, 400));
                menuObject[3].S_Position(new Vector2(150, 560));
                return 1;
            }
            if (newState.IsKeyDown(Keys.F12))
            {
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/back"));
                menuObject[2].S_Position(new Vector2(150, 400));
                menuObject[3].S_Position(new Vector2(150, 560));
                return 2;
            }
            #endregion
            #region Menu principal
            if (state == 0)
            {
                float posX = menuObject[1].G_Position().X;
                if (posX > 100)
                    speedX -= 0.2f;
                if (posX < 100)
                    speedX += 0.2f;
                menuObject[1].S_PosX(posX + speedX/2);
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

                menuObject[1].S_PosY(350 + 125 * currentCursor);
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/new" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/config" + cursor[1]));
                menuObject[4].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor[2]));

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
                        state = 10;
                    }
                    else if (currentCursor == 1)
                    {
                        state = 20;
                    }
                    else if (currentCursor == 2)
                    {
                        state = 90;
                    }

                    speedX = 35;
                }
            }
            #endregion
            #region Nombre Perso
            else if (state == 1)
            {
                string[] cursor = new string[3];
                if (currentCursor == 0)
                {
                    cursor[0] = "_";
                    cursor[1] = "";
                    cursor[2] = "";
                }
                if (currentCursor == 1)
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

                menuObject[2].S_PosX(42);
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/player" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/back" + cursor[1]));
                menuObject[4].S_Texture(Content.Load<Texture2D>("menu/quit" + cursor[2]));

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
                if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                {
                    nbPlayer = (nbPlayer % 4) + 1;
                }
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                {
                    nbPlayer--;
                    if (nbPlayer == 0)
                        nbPlayer = 4;
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    if (currentCursor == 0)
                    {
                        state = -42;
                        return nbPlayer;
                    }
                    else if (currentCursor == 1)
                    {
                        state = 0;
                        currentCursor = 0;
                        menuObject[1].S_PosX(42);
                        menuObject[2].S_PosX(500);
                        speedX = 0;
                    }
                    else if (currentCursor == 2)
                    {
                        Program.gs.Exit();
                    }
                }
            }
            #endregion
            #region Setup
            else if (state == 2)
            {
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

                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/resolution" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/fullscreen" + cursor[1]));
                menuObject[4].S_Texture(Content.Load<Texture2D>("menu/musique" + cursor[2]));
                if (setup[0] == 0)
                    menuObject[5].S_Texture(Content.Load<Texture2D>("menu/1000x800"));
                else if (setup[0] == 1)
                    menuObject[5].S_Texture(Content.Load<Texture2D>("menu/1200x800"));
                else if (setup[0] == 2)
                    menuObject[5].S_Texture(Content.Load<Texture2D>("menu/1400x800"));
                if (setup[1] == 0)
                    menuObject[6].S_Texture(Content.Load<Texture2D>("menu/off"));
                else if (setup[1] == 1)
                    menuObject[6].S_Texture(Content.Load<Texture2D>("menu/on"));
                if (setup[2] == 0)
                    menuObject[7].S_Texture(Content.Load<Texture2D>("menu/off"));
                else if (setup[2] == 1)
                    menuObject[7].S_Texture(Content.Load<Texture2D>("menu/on"));

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
                if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                {
                    if (currentCursor == 0)
                        setup[currentCursor] = (setup[currentCursor] + 1) % 3;
                    else
                        setup[currentCursor] = (setup[currentCursor] + 1) % 2;
                }
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                {
                    setup[currentCursor]--;
                    if (setup[currentCursor] == -1)
                    {
                        if (currentCursor == 0)
                            setup[currentCursor] = 2;
                        else
                            setup[currentCursor] = 1;
                    }
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    FileStream fs = new FileStream("setup", FileMode.Truncate);
                    StreamWriter sw = new StreamWriter(fs);
                    if (setup[0] == 0)
                        sw.WriteLine("resolution=1000x800");
                    else if (setup[0] == 1)
                        sw.WriteLine("resolution=1200x800");
                    else if (setup[0] == 2)
                        sw.WriteLine("resolution=1400x800");
                    if (setup[1] == 0)
                        sw.WriteLine("fullscreen=off");
                    else if (setup[1] == 1)
                        sw.WriteLine("fullscreen=on");
                    if (setup[2] == 0)
                        sw.WriteLine("musique=off");
                    else if (setup[2] == 1)
                        sw.WriteLine("musique=on");
                    sw.Close();
                    fs.Close();

                    state = 0;
                    currentCursor = 0;
                    menuObject[1].S_PosX(42);
                    menuObject[3].S_Position(new Vector2(500, 475));
                    menuObject[4].S_Position(new Vector2(500, 600));
                    speedX = 0;
                    Program.Restart();
                }
            }
            #endregion

            else if (state == 9)
            {
                Program.gs.Exit();
            }

            else if (state >= 10)
            {
                menuObject[1].S_PosX(menuObject[1].G_Position().X + speedX / 2);
                if (menuObject[1].G_Position().X > 1200 || menuObject[1].G_Position().X < 0)
                {
                    state /= 10;
                    currentCursor = 0;
                }
            }

            oldState = newState;
            return 0;
        }

        public static void Load(Personnage[] perso, Monster[] monster, PNJ[] pnj, ContentManager Content, GameTime gameTime)
        {
            Mob.Load(Content);
            Personnage.Load(perso, Content);
            Monster.Load(monster, Content);
            PNJ.Load(pnj, Content);
            Map.Load(Content);
            Map.Update(gameTime, perso, Content);
        }

        public static void Draw(SpriteBatch sb, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            sb.Begin(SpriteBlendMode.AlphaBlend);

            menuObject[0].Draw(sb); 
            menuObject[2].Draw(sb);
            menuObject[3].Draw(sb);
            menuObject[4].Draw(sb);

            if (state == 0 || state >= 10)
            {
                menuObject[1].Draw(sb);
            }
            if (state == 1)
            {
                sb.DrawString(menuFont, nbPlayer.ToString(), new Vector2(642, 370), Color.Black);
            }
            if (state == 2)
            {
                menuObject[5].Draw(sb);
                menuObject[6].Draw(sb);
                menuObject[7].Draw(sb);
            }
            sb.End();
        }
    }
}
