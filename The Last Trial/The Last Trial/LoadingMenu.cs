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
        private static int state, currentCursor, nbPlayer, temp;
        private static int[] setup = new int[4];
        private static float speedX;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;
        private static Objet[] menuObject = new Objet[7];
        private static Objet background;
        private static int[] persoClasse;

        public static int[] G_PersoClasse()
        {
            return persoClasse;
        }

        public static void Init(ContentManager Content)
        {
            menuObject[0] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("menu/1"));
            menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
            menuObject[2] = new Objet(new Vector2(300, 350), Content.Load<Texture2D>("menu/new"));
            menuObject[3] = new Objet(new Vector2(300, 475), Content.Load<Texture2D>("menu/config"));
            menuObject[4] = new Objet(new Vector2(300, 600), Content.Load<Texture2D>("menu/quit"));
            menuObject[5] = new Objet(new Vector2(600, 350), Content.Load<Texture2D>("menu/on"));
            menuObject[6] = new Objet(new Vector2(600, 475), Content.Load<Texture2D>("menu/on"));
            background = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("load/1"));
            speedX = 0;
            state = (int)MenuState.Principal;
            nbPlayer = 1;
            currentCursor = 0;
            menuFont = Content.Load<SpriteFont>("font/menufont");
            oldState = Keyboard.GetState();

            InitSetup();
        }

        private static void InitSetup()
        {
            if (Program.width == 1000)
                setup[0] = 0;
            else if (Program.width == 1200)
                setup[0] = 1;
            else if (Program.width == 1400)
                setup[0] = 2;

            if (!Program.fullscreen)
                setup[1] = 0;
            else
                setup[1] = 1;

            if (!Program.musique)
                setup[2] = 0;
            else
                setup[2] = 1;

            setup[3] = Program.volume;
        }

        enum MenuState { Principal = 0, NombrePerso, Setup, SetupVideo, SetupAudio, 
            SelecClasse1, SelecClasse2, SelecClasse3, SelecClasse4, Exit = 9, 
            NombrePerso_ = 10, Setup_ = 20, Exit_ = 90}

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
            if (newState.IsKeyDown(Keys.F11))
            {
                persoClasse = new int[1];
                persoClasse[0] = 1;
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/back"));
                menuObject[2].S_Position(new Vector2(150, 400));
                menuObject[3].S_Position(new Vector2(150, 560));
                return 1;
            }
            if (newState.IsKeyDown(Keys.F12))
            {
                persoClasse = new int[2];
                persoClasse[0] = 1;
                persoClasse[1] = 3;
                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/back"));
                menuObject[2].S_Position(new Vector2(150, 400));
                menuObject[3].S_Position(new Vector2(150, 560));
                return 2;
            }
            #endregion
            #region Menu principal
            if (state == (int)MenuState.Principal)
            {
                float posX = menuObject[1].G_Position().X;
                if (posX > 42)
                    speedX -= 0.2f;
                if (posX < 42)
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
                        state = (int)MenuState.NombrePerso_;
                    }
                    else if (currentCursor == 1)
                    {
                        state = (int)MenuState.Setup_;
                    }
                    else if (currentCursor == 2)
                    {
                        state = (int)MenuState.Exit_;
                    }

                    speedX = 35;
                }
            }
            #endregion
            #region Nombre Perso
            else if (state == (int)MenuState.NombrePerso)
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
                menuObject[2] = new Objet(new Vector2(42, 350), Content.Load<Texture2D>("menu/player" + cursor[0]));
                menuObject[3] = new Objet(new Vector2(300, 475), Content.Load<Texture2D>("menu/back" + cursor[1]));
                menuObject[4] = new Objet(new Vector2(300, 600), Content.Load<Texture2D>("menu/quit" + cursor[2]));

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
                        persoClasse = new int[nbPlayer];
                        state = (int)MenuState.SelecClasse1;
                        temp = 0;
                    }
                    else if (currentCursor == 1)
                    {
                        state = (int)MenuState.Principal;
                        currentCursor = 0;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                        menuObject[2] = new Objet(new Vector2(300, 350), Content.Load<Texture2D>("menu/new"));
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
            else if (state == (int)MenuState.Setup)
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

                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/video" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/audio" + cursor[1]));
                menuObject[4].S_Texture(Content.Load<Texture2D>("menu/back" + cursor[2]));

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
                        state = (int)MenuState.SetupVideo;
                    }
                    else if (currentCursor == 1)
                    {
                        state = (int)MenuState.SetupAudio;
                    }
                    else if (currentCursor == 2)
                    {
                        state = (int)MenuState.Principal;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                        speedX = 0;
                    }
                    currentCursor = 0;
                }
            }
            else if (state == (int)MenuState.SetupVideo)
            {
                string[] cursor = new string[2];
                if (currentCursor == 0)
                {
                    cursor[0] = "_";
                    cursor[1] = "";
                }
                else if (currentCursor == 1)
                {
                    cursor[0] = "";
                    cursor[1] = "_";
                }

                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/resolution" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/fullscreen" + cursor[1]));
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

                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    currentCursor--;
                    if (currentCursor == -1)
                        currentCursor = 1;

                }
                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    currentCursor = (currentCursor + 1) % 2;
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
                    ApplyChanges(Content);
                }
            }
            else if (state == (int)MenuState.SetupAudio)
            {
                string[] cursor = new string[2];
                if (currentCursor == 0)
                {
                    cursor[0] = "_";
                    cursor[1] = "";
                }
                else if (currentCursor == 1)
                {
                    cursor[0] = "";
                    cursor[1] = "_";
                }

                menuObject[2].S_Texture(Content.Load<Texture2D>("menu/musique" + cursor[0]));
                menuObject[3].S_Texture(Content.Load<Texture2D>("menu/volume" + cursor[1]));
                if (setup[2] == 0)
                    menuObject[5].S_Texture(Content.Load<Texture2D>("menu/off"));
                else if (setup[2] == 1)
                    menuObject[5].S_Texture(Content.Load<Texture2D>("menu/on"));

                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    currentCursor--;
                    if (currentCursor == -1)
                        currentCursor = 1;

                }
                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    currentCursor = (currentCursor + 1) % 2;
                }
                if (currentCursor == 0)
                {
                    if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                    {
                        setup[2] = (setup[2] + 1) % 2;
                    }
                    if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                    {
                        setup[2]--;
                        if (setup[2] == -1)
                        {
                            setup[2] = 1;
                        }
                    }
                }
                if (currentCursor == 1)
                {
                    if (newState.IsKeyDown(Keys.Right))
                    {
                        setup[3]++;
                        if (setup[3] > 100)
                            setup[3] = 100;
                    }
                    if (newState.IsKeyDown(Keys.Left))
                    {
                        setup[3]--;
                        if (setup[3] < 0)
                            setup[3] = 0;
                    }
                    Program.volume = setup[3];
                    Son.InstanceVolume();
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    ApplyChanges(Content);
                }
            }
            #endregion
            #region Selection Classe
            else if (state >= 5 && state <= 8)
            {
                string str = "";
                if (temp == 1)
                {
                    str = "_";
                }
                menuObject[1].S_Texture(Content.Load<Texture2D>("menu/back" + str));
                menuObject[1].S_Position(new Vector2(300, 650));
                menuObject[2].S_Texture(Content.Load<Texture2D>("perso/1/10"));
                menuObject[2].S_Position(new Vector2(100, 500));
                menuObject[3].S_Texture(Content.Load<Texture2D>("perso/2/10"));
                menuObject[3].S_Position(new Vector2(300, 500));
                menuObject[4].S_Texture(Content.Load<Texture2D>("perso/3/10"));
                menuObject[4].S_Position(new Vector2(500, 500));
                menuObject[5].S_Texture(Content.Load<Texture2D>("perso/4/10"));
                menuObject[5].S_Position(new Vector2(700, 500));
                menuObject[6].S_Texture(Content.Load<Texture2D>("menu/selection"));
                menuObject[6].S_Position(new Vector2(92 + (200 * currentCursor), 480));

                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    temp--;
                    if (temp == -1)
                        temp = 1;

                }
                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    temp = (temp + 1) % 2;
                }
                if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                {
                    if (temp == 0)
                        currentCursor = (currentCursor + 1) % 4;
                }
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                {
                    if (temp == 0)
                    {
                        currentCursor--;
                        if (currentCursor == -1)
                            currentCursor = 3;
                    }
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    if (temp == 0)
                    {
                        persoClasse[state - 5] = currentCursor + 1;
                        state++;
                        if (state - 5 == nbPlayer)
                        {
                            return nbPlayer;
                        }
                    }
                    else
                    {
                        state = 1;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                    }
                }
            }
            #endregion

            else if (state == 9)
            {
                Program.gs.Exit();
            }
            else if (state == 10 || state == 20 || state == 90)
            {
                menuObject[1].S_PosX(menuObject[1].G_Position().X + speedX / 2);
                if (menuObject[1].G_Position().X > Program.width || menuObject[1].G_Position().X < 0)
                {
                    state /= 10;
                    currentCursor = 0;
                }
            }

            oldState = newState;
            return 0;
        }

        private static void ApplyChanges(ContentManager Content)
        {
            FileStream fs = new FileStream("setup", FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);
            if (setup[0] == 0)
            {
                sw.WriteLine("resolution=1000x800");
                Program.width = 1000;
                Program.height = 800;
            }
            else if (setup[0] == 1)
            {
                sw.WriteLine("resolution=1200x800");
                Program.width = 1200;
                Program.height = 800;
            }
            else if (setup[0] == 2)
            {
                sw.WriteLine("resolution=1400x800");
                Program.width = 1400;
                Program.height = 800;
            }
            if (setup[1] == 0)
            {
                sw.WriteLine("fullscreen=off");
                Program.fullscreen = false;
            }
            else if (setup[1] == 1)
            {
                sw.WriteLine("fullscreen=on");
                Program.fullscreen = true;
            }
            if (setup[2] == 0)
            {
                sw.WriteLine("musique=off");
                Program.musique = false;
            }
            else if (setup[2] == 1)
            {
                sw.WriteLine("musique=on");
                Program.musique = true;
            }
            sw.WriteLine("volume=" + setup[3]);
            Program.volume = setup[3];
            Son.InstanceStop();
            sw.Close();
            fs.Close();

            GameState.Restart(Content);
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

            if (state != (int)MenuState.SetupAudio && state != (int)MenuState.SetupVideo)
            {
                menuObject[4].Draw(sb);
            }
            if (state == (int)MenuState.Principal || state >= 10)
            {
                menuObject[1].Draw(sb);
            }
            if (state == (int)MenuState.NombrePerso)
            {
                sb.DrawString(menuFont, nbPlayer.ToString(), new Vector2(642, 370), Color.Black);
            }
            if (state == 3)
            {
                menuObject[5].Draw(sb);
                menuObject[6].Draw(sb);
            }
            if (state == 4)
            {
                menuObject[5].Draw(sb);
                sb.DrawString(menuFont, setup[3].ToString(), new Vector2(630, 500), Color.DarkKhaki);
            }
            if (state >= 5 && state <= 8)
            {
                menuObject[5].Draw(sb);
                if (temp == 0)
                {
                    menuObject[6].Draw(sb);
                }
                sb.DrawString(menuFont, "Joueur " + (state - 4) + ", choisissez une classe", new Vector2(200, 350), Color.DarkKhaki);
                menuObject[1].Draw(sb);
            }
            sb.End();
        }
    }
}
