using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace The_Last_Trial
{
    public class Personnage : Mob
    {
        #region VAR

        private Keys[] key;
        private int classe, power, powerMax, xp, xpMax, level;
        private double[] tempsAttaque = new double[2];
        private double tempsRegen;
        /** KEYS STATES **\
         * 0 : BAS       *
         * 1 : DROITE    *
         * 2 : HAUT      *
         * 3 : GAUCHE    *
         * 4 : ATTAQUE   *
         * 5 : OVERKILL  *
        \*****************/

        #endregion

        // CONSTRUCTEUR
        public Personnage(Keys[] key, Vector2 position, int id, int classe) : base()
        {
            this.id = id;
            this.classe = classe;
            this.key = key;
            this.position = position;
            this.imgState = 20;
            this.initLife = 100;
            this.life = this.initLife;
            this.lifeMax = this.life;
            this.xp = 0;
            this.xpMax = 142;
            this.level = 0;
            this.tempsRegen = 0;

            if (id == 2)
            {
                powerMax = 1000;
            }
            else
            {
                powerMax = 500;
            }
            power = powerMax;

            for (int i = 0; i <= 1; i++)
            {
                tempsAttaque[i] = -5;
            }

            this.oldState = Keyboard.GetState();
        }

        #region GET & SET

        public Rectangle G_Rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y + 66, 60, 33);
        }

        public bool G_Interact(PNJ pnj)
        {
            return G_Rectangle().Intersects(pnj.G_Interact());
        }

        private void S_Xp(int xp_)
        {
            this.xp += xp_;
            while (this.xp >= xpMax)
            {
                this.xp = this.xp - xpMax;
                xpMax *= 2;
                level++;
                life = lifeMax;
                initLife = lifeMax;
            }
        }

        #endregion
        
        #region Static Load & Update

        public static void Load(Personnage[] perso, ContentManager Content)
        {
            //DEBUT SETUP
            //string str = "";
            //string line;
            //StreamReader sr = new StreamReader("setup");
            //line = sr.ReadLine();
            if (GameState.G_Level() == 1)
            {
                if (GameState.G_Player() > 0)
                    perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.Space, Keys.RightShift }, new Vector2(300f, 350f), 1, 1);

                if (GameState.G_Player() > 1)
                    perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.Z, Keys.Q, Keys.F, Keys.D1 }, new Vector2(330f, 450f), 2, 3);

                if (GameState.G_Player() > 2)
                    perso[2] = new Personnage(new Keys[] { Keys.NumPad2, Keys.NumPad6, Keys.NumPad8, Keys.NumPad4, Keys.NumPad0, Keys.D2 }, new Vector2(360f, 550f), 3, 2);

                if (GameState.G_Player() > 3)
                    perso[3] = new Personnage(new Keys[] { Keys.L, Keys.M, Keys.O, Keys.K, Keys.J, Keys.D3 }, new Vector2(390f, 650f), 4, 4);
            }
            foreach (Personnage p in perso)
                p.F_Load(Content);

        }

        public static void Update(Personnage[] perso, GameTime gameTime, Monster[] monster, GraphicsDeviceManager graphics, ContentManager Content)
        {
            foreach (Personnage p in perso)
                p.F_Update(perso, monster, Content, gameTime, graphics);
        }

        #endregion

        #region Load, Update & Draw

        private void F_Load(ContentManager Content)
        {
            position = new Vector2(270 + 30 * id, 250 + 100 * id);
            this.imgState = 20;
            objet = Content.Load<Texture2D>("perso/" + classe + "/" + imgState);
            this.life = this.initLife;
            for (int i = 0; i <= 1; i++)
            {
                tempsAttaque[i] = -5;
            }
        }

        private void F_Update(Personnage[] perso, Monster[] monster, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (G_IsAlive())
            {
                F_Deplacer();
                F_Attaque(monster, gameTime);
                F_OverKill(monster, perso, gameTime);
                foreach (Rectangle collision in Map.G_Collision())
                {
                    F_Collision_Objets(collision, gameTime);
                }
                foreach (Monster m in monster)
                {
                    if (m.G_IsAlive())
                        F_Collision_Objets(m.G_Rectangle(), gameTime);
                }
                F_Collision_Ecran(graphics, gameTime);
                F_UpdateRegen(gameTime);
            }

            F_UpdateImage(gameTime);
            objet = Content.Load<Texture2D>("perso/" + classe + "/" + imgState);
            S_Deplacement(gameTime);
        }

        public void F_Draw(SpriteBatch sb)
        {
            // CODE SALE
            if (imgState < 0 && id == 1)
                sb.Draw(objet, new Vector2((int)position.X - 40, (int)position.Y - 30), Color.White);
            else if (imgState < 100)
                sb.Draw(objet, new Vector2((int)position.X, (int)position.Y), Color.White);
            else
                sb.Draw(objet, new Vector2((int)position.X - 240, (int)position.Y - 210), Color.White);

            if (imgState > 100)
                sb.DrawString(overKill, "OVERKILL", new Vector2(position.X - 100, position.Y - 120), Color.Firebrick);

            
                F_DrawHealth(sb);
        }

        #endregion

        #region Collision

        private void F_Collision_Ecran(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            int MaxX = graphics.GraphicsDevice.Viewport.Width;

            if (position.X > MaxX * 0.79)
            {
                position.X -= (speed.X - Map.G_Speed().X) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                speed.X = 0;
            }

            else if (position.X < MaxX * 0.185 /*0.6*/)
            {
                position.X -= (speed.X - Map.G_Speed().X) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                speed.X = 0;
            }
        }

        private void F_Collision_Objets(Rectangle rect, GameTime gameTime)
        {
            if (rect.Intersects(G_Rectangle()))
            {
                position -= (speed - Map.G_Speed()) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Test pour detecter si collision X ou Y
                if (new Rectangle((int)position.X + (int)((speed.X - Map.G_Speed().X) * (float)gameTime.ElapsedGameTime.TotalSeconds * 2), 
                    (int)position.Y + (objet.Height * 2) / 3, objet.Width, objet.Height / 3).Intersects(rect))
                {
                    speed.X = 0;
                }
                if (new Rectangle((int)position.X, (int)position.Y + (int)((speed.Y - Map.G_Speed().Y) * (float)gameTime.ElapsedGameTime.TotalSeconds * 2) + 
                    (objet.Height * 2) / 3, objet.Width, objet.Height / 3).Intersects(rect))
                {
                    speed.Y = 0;
                }
            }
        }

        #endregion

        #region Attaque & Magie

        private void F_Attaque(Monster[] monster, GameTime gameTime)
        {
            newState = Keyboard.GetState();
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[0] + 0.5)
            {
                if (newState.IsKeyDown(key[4]) && power >= 100)
                {
                    oldImage = imgState;
                    bool attaque = false;
                    power -= 100;
                    foreach (Monster m in monster)
                    {
                        if (G_Rectangle().Intersects(m.G_Interact()) && m.G_IsAlive() && !attaque)
                        {
                            attaque = true;
                            m.S_Degat(42 + random.Next(10) + 10 * level);
                            if (m.G_Killed())
                            {
                                S_Xp(m.G_MaxLife());
                            }
                        }
                    }
                    if (id == 3)
                        Son.Play(3);

                    tempsAttaque[0] = tempsActuel;
                }
            }
            else if (classe == 1)
            {
                if (tempsActuel > tempsAttaque[0] + 0.4)
                {
                    if (imgState % 10 == -3)
                    {
                        Son.Play(1);
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.3)
                {
                    if (imgState % 10 == -2)
                        imgState--;
                    else if (imgState > 0)
                    {
                        if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                            imgState = -23;
                        else if (oldImage / 10 == 3 || oldImage / 10 == 6)
                            imgState = -13;
                        else if (oldImage / 10 == 7)
                            imgState = -43;
                        else if (oldImage / 10 == 4 || oldImage / 10 == 8)
                            imgState = -33;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.2)
                {
                    if (imgState % 10 == -1)
                        imgState--;
                    else if (imgState > 0)
                    {
                        if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                            imgState = -22;
                        else if (oldImage / 10 == 3 || oldImage / 10 == 6)
                            imgState = -12;
                        else if (oldImage / 10 == 7)
                            imgState = -42;
                        else if (oldImage / 10 == 4 || oldImage / 10 == 8)
                            imgState = -32;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.1)
                {
                    if (imgState % 10 == 0)
                        imgState--;
                    else if (imgState > 0)
                    {
                        if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                            imgState = -21;
                        else if (oldImage / 10 == 3 || oldImage / 10 == 6)
                            imgState = -11;
                        else if (oldImage / 10 == 7)
                            imgState = -41;
                        else if (oldImage / 10 == 4 || oldImage / 10 == 8)
                            imgState = -31;
                    }
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    if (imgState > 0)
                    {
                        if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                            imgState = -20;
                        else if (oldImage / 10 == 3 || oldImage / 10 == 6)
                            imgState = -10;
                        else if (oldImage / 10 == 7)
                            imgState = -40;
                        else if (oldImage / 10 == 4 || oldImage / 10 == 8)
                            imgState = -30;
                    }
                }

                if (imgState < 0)
                    speed = Vector2.Zero;
            }
            else if (classe == 3)
            {
                if (tempsActuel > tempsAttaque[0] + 0.4)
                {
                    if (imgState < 0)
                    {
                        imgState = oldImage / 10 * 10;
                    }
                }
                else
                {
                    if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 3 || oldImage / 10 == 5 || oldImage / 10 == 6)
                        imgState = -10;
                    else
                        imgState = -20;
                }
                if (imgState < 0)
                    speed = Vector2.Zero;
            }
        }

        private void F_OverKill(Monster[] monster, Personnage[] perso, GameTime gameTime)
        {
            newState = Keyboard.GetState();

            Monster[] m_target_ovrkl = new Monster[GameState.G_Monster()];
            Personnage[] p_target_ovrkl = new Personnage[GameState.G_Player()];
            //bool p_target = false;
            bool m_target = false;

            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[1] + 3)
            {
                if (newState.IsKeyDown(key[5]) && power >= 500)
                {
                    power -= 500;
                    for (int i = 0; i < GameState.G_Monster(); i++)
                    {
                        m_target_ovrkl[i] = null;
                    }

                    //for (int i = 0; i < Game1.G_Player(); i++)
                    //{
                    //    p_target_ovrkl[i] = null;
                    //}

                    for (int i = 0; i < GameState.G_Monster(); i++)
                    {
                        m_target_ovrkl[i] = F_DetectMonsters(monster[i]);
                        foreach (Monster m in m_target_ovrkl)
                        {
                            if (m != null)
                                m_target = true;
                        }

                        if (m_target && monster[i].G_IsAlive() && m_target_ovrkl[i] != null)
                        {
                            monster[i].S_Degat(1337);
                        }
                    }

                    //for (int i = 0; i < Game1.G_Player(); i++)
                    //{
                    //    if (F_DetectAllies(perso) != null)
                    //    {
                    //        p_target_ovrkl[i] = F_DetectAllies(perso);
                    //        p_target = true;
                    //    }
                    //}

                    Son.Play(4);
                    imgState = 101;
                    tempsAttaque[1] = tempsActuel;
                }
            }
            else
            {
                if (tempsActuel > tempsAttaque[1] + 1)
                    if (imgState == 105)
                        imgState = 20;
                    else { }
                else if (tempsActuel > tempsAttaque[1] + 0.8)
                    imgState = 105;
                else if (tempsActuel > tempsAttaque[1] + 0.6)
                    imgState = 104;
                else if (tempsActuel > tempsAttaque[1] + 0.4)
                    imgState = 103;
                else if (tempsActuel > tempsAttaque[1] + 0.2)
                    imgState = 102;
                if (imgState > 100)
                    speed = Vector2.Zero;
            }
        }

        private Personnage F_DetectAllies(Personnage[] perso)
        {
            Personnage p_ = null;
            foreach (Personnage p in perso)
            {
                if (p.G_Rectangle().Intersects(new Rectangle((int)position.X - 200, (int)position.Y - 200, 400 + objet.Width, 400 + objet.Height)))
                    p_ = p;
            }
            return p_;
        }

        private Monster F_DetectMonsters(Monster m)
        {
            Monster m_ = null;

            if (m.G_Aggro().Intersects(G_Rectangle()))
                m_ = m;

            return m_;
        }

        #endregion

        private void F_Deplacer()
        {
            newState = Keyboard.GetState();
            // DROITE
            if (newState.IsKeyDown(key[1]))
            {
                if (!oldState.IsKeyDown(key[1]))
                    speed.X = 150.0f;
            }
            else if (oldState.IsKeyDown(key[1]))
            {
                if (speed.X > 0)
                    speed.X = 0.0f;
            }

            // GAUCHE
            if (newState.IsKeyDown(key[3]))
            {
                if (!oldState.IsKeyDown(key[3]))
                    speed.X = -150.0f;
            }
            else if (oldState.IsKeyDown(key[3]))
            {
                if (speed.X < 0)
                    speed.X = 0.0f;
            }

            // HAUT
            if (newState.IsKeyDown(key[2]))
            {
                if (!oldState.IsKeyDown(key[2]))
                    speed.Y = -80.0f;
            }
            else if (oldState.IsKeyDown(key[2]))
            {
                if (speed.Y < 0)
                    speed.Y = 0.0f;
            }

            // BAS
            if (newState.IsKeyDown(key[0]))
            {
                if (!oldState.IsKeyDown(key[0]))
                    speed.Y = 80.0f;
            }
            else if (oldState.IsKeyDown(key[0]))
            {
                if (speed.Y > 0)
                    speed.Y = 0.0f;
            }

            oldState = newState;
        }

        #region UI

        private void F_UpdateRegen(GameTime gameTime)
        {
            if (tempsRegen + 0.05 < gameTime.TotalGameTime.TotalSeconds)
            {
                tempsRegen = gameTime.TotalGameTime.TotalSeconds;
                power += 2;
                if (power > powerMax)
                {
                    power = powerMax;
                }
            }
        }

        private void F_DrawHealth(SpriteBatch spriteBatch)
        {
            if (id == 1)
            {
                spriteBatch.Draw(ui1, new Rectangle(10, 10, ui1.Width, ui1.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(110, 42, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(110, 62, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(110, 82, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                spriteBatch.DrawString(textFont, "Ilean", new Vector2(112, 6), Color.White);
                spriteBatch.DrawString(textFont, "Ilean", new Vector2(113, 7), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(240, 6), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(241, 7), Color.Black);
            }
            else if (id == 2)
            {
                spriteBatch.Draw(ui2, new Rectangle(938, 10, ui2.Width, ui2.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(946, 42, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(946, 62, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(946, 82, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                spriteBatch.DrawString(textFont, "Waydjinn", new Vector2(935, 6), Color.White);
                spriteBatch.DrawString(textFont, "Waydjinn", new Vector2(936, 7), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(1066, 6), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(1067, 7), Color.Black);
            }
            else if (id == 3)
            {
                spriteBatch.Draw(ui1, new Rectangle(10, 700, ui1.Width, ui1.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(110, 742, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(110, 762, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(110, 782, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
            }
            else if (id == 4)
            {
                spriteBatch.Draw(ui2, new Rectangle(938, 700, ui2.Width, ui2.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(946, 742, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(946, 762, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(946, 782, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
            }

        }

        public void F_DrawDegats(SpriteBatch sb)
        {
            if (degats != 0)
            {
                initLife = life - degats;
                oldDegats = degats;
                degats = 0;
            }
            if (initLife < life && G_IsAlive())
            {
                sb.DrawString(gameFont, oldDegats.ToString(), new Vector2(position.X + 10, position.Y - 30), Color.Red);
                life -= 3;
            }
            else
                oldDegats = 0;
        }

        #endregion

    }
}
