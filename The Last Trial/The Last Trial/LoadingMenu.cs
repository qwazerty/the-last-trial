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
        private static int[] setup = new int[5];
        private static float speedX;
        private static KeyboardState oldState, newState;
        private static SpriteFont menuFont;
        private static Objet[] menuObject = new Objet[6];
        private static Objet background;
        private static int[] persoClasse;
        private static string[] local, save;
        private static FileStream fs;
        private static StreamReader sr;

        public static int[] PersoClasse { get { return persoClasse; } }
        public static string[] Local { get { return local; } }

        public static void Init(ContentManager Content)
        {
            menuObject[0] = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("menu/1"));
            menuObject[1] = new Objet(new Vector2(-42, 350), Content.Load<Texture2D>("menu/epee"));
            background = new Objet(new Vector2(0, 0), Content.Load<Texture2D>("load/1"));
            speedX = 0;
            state = (int)MenuState.Principal;
            nbPlayer = 1;
            currentCursor = 0;
            menuFont = Content.Load<SpriteFont>("font/menufont");
            local = new string[Program.MAXLOCAL];
            oldState = Keyboard.GetState();

            InitSetup();
        }

        private static void InitSetup()
        {
            if (Program.width == 1024)
            {
                setup[0] = 0;
            }
            else if (Program.width == 1280)
            {
                setup[0] = 1;
            }
            else if (Program.width == 1400)
            {
                setup[0] = 2;
            }

            if (!Program.fullscreen)
            {
                setup[1] = 0;
            }
            else
            {
                setup[1] = 1;

            }

            if (!Program.musique)
            {
                setup[2] = 0;
            }
            else
            {
                setup[2] = 1;
            }

            setup[3] = Program.volume;

            if (Program.local == "fr")
            {
                setup[4] = 0;
            }
            else
            {
                setup[4] = 1;
            }

            // LOCAL PART
            FileStream fs;
            StreamReader sr;
            fs = new FileStream("Config/local_" + Program.local + ".tlt", FileMode.Open);
            sr = new StreamReader(fs);
            for (int i = 0; i <= Program.MAXLOCAL - 1; i++)
            {
                local[i] = sr.ReadLine();
            }
            sr.Close();
            fs.Close();
        }

        enum MenuState { Principal = 0, NombrePerso, Setup, SetupVideo, SetupAudio, 
            SelecClasse1, SelecClasse2, SelecClasse3, SelecClasse4, SetupLocal, LoadSave = 11, ChooseLevel = 12, Exit = 30, 
            NombrePerso_ = 10, Setup_ = 20, LoadSave_ = 110, Exit_ = 300}

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
                return 1;
            }
            if (newState.IsKeyDown(Keys.F12))
            {
                persoClasse = new int[2];
                persoClasse[0] = 1;
                persoClasse[1] = 3;
                return 2;
            }
            #endregion

            if (state <= 5 || state == 9)
            {
                float posX = menuObject[1].Position.X;
                if (posX > 42)
                    speedX -= 0.2f;
                if (posX < 42)
                    speedX += 0.2f;
                menuObject[1].S_PosX(posX + speedX / 2);
                menuObject[1].S_PosY(300 + 125 * currentCursor);
            }

            #region Menu principal
            if (state == (int)MenuState.Principal)
            {
                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    currentCursor--;
                    if (currentCursor == -1)
                        currentCursor = 3;
                }
                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    currentCursor = (currentCursor + 1) % 4;
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    if (currentCursor == 0)
                    {
                        state = (int)MenuState.NombrePerso_;
                    }
                    else if (currentCursor == 1)
                    {
                        temp = 0;
                        fs = new FileStream("Config/save", FileMode.Open);
                        sr = new StreamReader(fs);
                        save = new string[int.Parse(sr.ReadLine())];
                        for (int i = 0; i < save.GetLength(0); i++)
                        {
                            save[i] = sr.ReadLine();
                        }
                        sr.Close();
                        fs.Close();
                        if (save.GetLength(0) != 0)
                        {
                            fs = new FileStream("Save/" + save[0] + ".save", FileMode.Open);
                            sr = new StreamReader(fs);
                            state = (int)MenuState.LoadSave_;
                        }
                    }
                    else if (currentCursor == 2)
                    {
                        state = (int)MenuState.Setup_;
                    }
                    else if (currentCursor == 3)
                    {
                        state = (int)MenuState.Exit_;
                    }

                    speedX = 35;
                }
            }
            #endregion
            #region Load
            else if (state == (int)MenuState.LoadSave)
            {
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
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                {
                    sr.Close();
                    fs.Close();
                    fs = new FileStream("Save/" + save[temp] + ".save", FileMode.Open);
                    sr = new StreamReader(fs);
                    temp--;
                    if (temp == -1)
                        temp = save.GetLength(0) - 1;
                }
                if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right))
                {
                    sr.Close();
                    fs.Close();
                    fs = new FileStream("Save/" + save[temp] + ".save", FileMode.Open);
                    sr = new StreamReader(fs);
                    temp = (temp + 1) % save.GetLength(0);
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    sr.Close();
                    fs.Close();
                    if (currentCursor == 0)
                    {
                        Program.save = save[temp];
                        return SaveLoad.Loading();
                    }
                    else if (currentCursor == 1)
                    {
                        state = (int)MenuState.Principal;
                    }
                }
            }
            #endregion
            #region ChooseLevel
            else if (state == (int)MenuState.ChooseLevel)
            { 
                
            }
            #endregion
            #region Nombre Perso
            else if (state == (int)MenuState.NombrePerso)
            {

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
                        menuObject[1].S_PosX(-42);
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
                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
                {
                    currentCursor--;
                    if (currentCursor == -1)
                        currentCursor = 3;

                }
                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
                {
                    currentCursor = (currentCursor + 1) % 4;
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
                        state = (int)MenuState.SetupLocal;
                    }
                    else if (currentCursor == 3)
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
                    else if (currentCursor == 1)
                        setup[currentCursor] = (setup[currentCursor] + 1) % 2;
                }
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left))
                {
                    if (currentCursor != 2)
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
                }
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    if (currentCursor <= 1)
                    {
                        ApplyChanges(Content);
                    }
                    else
                    {
                        currentCursor = 0;
                        state = (int)MenuState.Setup;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                        speedX = 0;
                    }
                }
            }
            else if (state == (int)MenuState.SetupAudio)
            {
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
                    if (currentCursor <= 1)
                    {
                        ApplyChanges(Content);
                    }
                    else
                    {
                        currentCursor = 0;
                        state = (int)MenuState.Setup;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                        speedX = 0;
                    }
                }
            }
            else if (state == (int)MenuState.SetupLocal)
            {
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
                    if (currentCursor <= 1)
                    {
                        setup[4] = currentCursor;
                        ApplyChanges(Content);
                    }
                    else
                    {
                        currentCursor = 0;
                        state = (int)MenuState.Setup;
                        menuObject[1] = new Objet(new Vector2(0, 350), Content.Load<Texture2D>("menu/epee"));
                        speedX = 0;
                    }
                }
            }
            #endregion
            #region Selection Classe
            else if (state >= 5 && state <= 8)
            {
                menuObject[1] = new Objet(new Vector2(100, 500), Content.Load<Texture2D>("perso/1/10"));
                menuObject[2] = new Objet(new Vector2(300, 500), Content.Load<Texture2D>("perso/2/10"));
                menuObject[3] = new Objet(new Vector2(500, 500), Content.Load<Texture2D>("perso/3/10"));
                menuObject[4] = new Objet(new Vector2(700, 500), Content.Load<Texture2D>("perso/4/10"));
                menuObject[5] = new Objet(new Vector2(92 + (200 * currentCursor), 480), Content.Load<Texture2D>("menu/selection"));

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
                            SaveLoad.NewGame(nbPlayer);
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

            else if (state == (int)MenuState.Exit)
            {
                Program.gs.Exit();
            }
            if (state == 10 || state == 20 || state == 110 || state == 300)
            {
                menuObject[1].S_PosX(menuObject[1].Position.X + speedX / 2);
                if (menuObject[1].Position.X > Program.width)
                {
                    state /= 10;
                    currentCursor = 0;
                    menuObject[1].S_PosX(-42);
                    speedX = 0;
                }
            }

            oldState = newState;
            return 0;
        }

        private static void ApplyChanges(ContentManager Content)
        {
            FileStream fs = new FileStream("Config/setup", FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);
            if (setup[0] == 0)
            {
                sw.WriteLine("resolution=1024x800");
                Program.width = 1000;
                Program.height = 800;
            }
            else if (setup[0] == 1)
            {
                sw.WriteLine("resolution=1280x800");
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
            if (setup[4] == 0)
            {
                sw.WriteLine("local=fr");
                Program.local = "fr";
            }
            else if (setup[4] == 1)
            {
                sw.WriteLine("local=en");
                Program.local = "en";
            }
            Son.InstanceStop();
            sw.Close();
            fs.Close();

            GameState.Restart(Content);
        }

        public static void Load(Personnage[] perso, Monster[] monster, PNJ[] pnj, ContentManager Content, GameTime gameTime)
        {
            Mob.Load(Content);
            SaveLoad.LoadClass(ref persoClasse, perso);
            Personnage.Load(perso, Content);
            SaveLoad.LoadPerso(perso);
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
            if (state == (int)MenuState.Principal || state >= 20 || state == 10)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                sb.DrawString(menuFont, Local[0], new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[23], new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[1], new Vector2(350, 550), color);
                if (currentCursor == 3)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[2], new Vector2(350, 675), color);
                menuObject[1].Draw(sb);
            }
            else if (state == (int)MenuState.NombrePerso)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                sb.DrawString(menuFont, Local[4] + " : " + nbPlayer, new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[2], new Vector2(350, 550), color);
                menuObject[1].Draw(sb);
            }

            else if (state == (int)MenuState.Setup)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                sb.DrawString(menuFont, Local[5], new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[6], new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[20], new Vector2(350, 550), color);
                if (currentCursor == 3)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 675), color);
                menuObject[1].Draw(sb);
            }
            else if (state == (int)MenuState.SetupAudio)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                string str;
                if (setup[2] == 0)
                {
                    str = "off";
                }
                else
                {
                    str = "on";
                }
                sb.DrawString(menuFont, Local[7] + " : " + str, new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[8] + " : " + setup[3], new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 550), color);
                menuObject[1].Draw(sb);
            }
            else if (state == (int)MenuState.SetupVideo)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                string str;
                if (setup[0] == 0)
                {
                    str = "1024*800";
                }
                else if (setup[0] == 1)
                {
                    str = "1280*800";
                }
                else
                {
                    str = "1400*800";
                }
                sb.DrawString(menuFont, Local[9] + " : " + str, new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                if (setup[1] == 0)
                {
                    str = "off";
                }
                else
                {
                    str = "on";
                }
                sb.DrawString(menuFont, Local[10] + " : " + str, new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 550), color);
                menuObject[1].Draw(sb);
            }
            else if (state == (int)MenuState.SetupLocal)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                sb.DrawString(menuFont, Local[21], new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[22], new Vector2(350, 425), color);
                if (currentCursor == 2)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 550), color);
                menuObject[1].Draw(sb);
            }
            else if (state == (int)MenuState.LoadSave)
            {
                Color color = Color.DarkKhaki;
                if (currentCursor == 0)
                {
                    color = Color.Gold;
                }
                sb.DrawString(menuFont, save[temp], new Vector2(350, 300), color);
                if (currentCursor == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(350, 425), color);
            }
            else if (state >= 5 && state <= 8) // Choix du personnage
            {
                Color color = Color.DarkKhaki;
                try
                {
                    menuObject[1].Draw(sb);
                    menuObject[2].Draw(sb);
                    menuObject[3].Draw(sb);
                    menuObject[4].Draw(sb);
                    if (temp == 0)
                        menuObject[5].Draw(sb);
                }
                catch (NullReferenceException) { }
                sb.DrawString(menuFont, Local[11] + " " + (state - 4) + ", " + Local[12], new Vector2(200, 342), color);

                if (temp == 1)
                {
                    color = Color.Gold;
                }
                else
                {
                    color = Color.DarkKhaki;
                }
                sb.DrawString(menuFont, Local[3], new Vector2(300, 650), color);
            }
            sb.End();
        }
    }
}
