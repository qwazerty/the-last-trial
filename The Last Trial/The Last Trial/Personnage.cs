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

        private static Objet[] portrait;
        private Keys[] key;
        private int classe, xp, xpMax, level;
        private double[] tempsAttaque = new double[2];
        private double tempsRegen, tempsLevelUp, power, powerMax, force, mana, esquive;
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
            this.life = 100;
            this.lifeMax = this.life;
            this.xp = 0;
            this.xpMax = 142;
            this.level = 1;
            this.tempsRegen = 0;
            this.tempsLevelUp = 0;

            if (classe == 1)
            {
                force = 0.7;
                mana = 1.5;
                esquive = 5;
            }

            if (classe == 2)
            {
                force = 1;
                mana = 0.5;
                esquive = 1;
            }

            if (classe == 3)
            {
                force = 0.5;
                mana = 2;
                esquive = 1;
            }

            if (classe == 4)
            {
                force = 0.5;
                mana = 2;
                esquive = 1;
            }

            powerMax = 500;
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

        private void S_Xp(int xp_, GameTime gameTime)
        {
            this.xp += xp_;
            while (this.xp >= xpMax)
            {
                S_Stats();
                this.xp = this.xp - xpMax;
                xpMax *= 2;
                level++;
                life = lifeMax;
                tempsLevelUp = gameTime.TotalRealTime.TotalSeconds;
            }
        }

        private void S_Life(int newLife)
        {
            life += newLife;
            if (life > lifeMax)
                life = lifeMax;
        }

        private void S_Stats()
        {
            
            if(classe == 3 || classe == 4)
            {
                mana += 0.2;
                powerMax = 1000 * mana;
                lifeMax += 30;
                esquive += 0.2;
            }
            if (classe == 2)
            {
                force += 0.1;
                lifeMax += 50;
                mana += 0.1;
            }
            if (classe == 1)
            {
                force += 0.05;
                esquive += 0.5;
                mana += 0.1;
                lifeMax += 25;
            }
        }

        public void S_Degat(int degat, GameTime gameTime)
        {
            if (random.NextDouble() * 20 > esquive)
            {
                life -= degat;
                this.degats = degat;
                tempsDegats = gameTime.TotalGameTime.TotalSeconds;
            }
            else
            {
                this.degats = 0;
                tempsDegats = gameTime.TotalGameTime.TotalSeconds;
            }
        }


        #endregion

        #region Static Load & Update

        public static void Load(Personnage[] perso, ContentManager Content)
        {
            if (GameState.G_Level() == 1)
            {
                Personnage.portrait = new Objet[GameState.G_Player()];
                if (GameState.G_Player() > 0)
                {
                    perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.Space, Keys.RightShift }, new Vector2(300f, 350f), 1, LoadingMenu.G_PersoClasse()[0]);
                    portrait[0] = new Objet(new Vector2(15, 10), Content.Load<Texture2D>("ui/" + LoadingMenu.G_PersoClasse()[0]));
                }

                if (GameState.G_Player() > 1)
                {
                    perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.W, Keys.A, Keys.F, Keys.D1 }, new Vector2(330f, 450f), 2, LoadingMenu.G_PersoClasse()[1]);
                    portrait[1] = new Objet(new Vector2(Program.width - 95, 10), Content.Load<Texture2D>("ui/" + LoadingMenu.G_PersoClasse()[1]));
                }

                if (GameState.G_Player() > 2)
                {
                    perso[2] = new Personnage(new Keys[] { Keys.NumPad5, Keys.NumPad6, Keys.NumPad8, Keys.NumPad4, Keys.NumPad0, Keys.NumPad7 }, new Vector2(360f, 550f), 3, LoadingMenu.G_PersoClasse()[2]);
                    portrait[2] = new Objet(new Vector2(15, 115), Content.Load<Texture2D>("ui/" + LoadingMenu.G_PersoClasse()[2]));
                }

                if (GameState.G_Player() > 3)
                {
                    perso[3] = new Personnage(new Keys[] { Keys.L, Keys.M, Keys.O, Keys.K, Keys.J, Keys.D3 }, new Vector2(390f, 650f), 4, LoadingMenu.G_PersoClasse()[3]);
                    portrait[3] = new Objet(new Vector2(Program.width - 95, 115), Content.Load<Texture2D>("ui/" + LoadingMenu.G_PersoClasse()[3]));
                }
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
            for (int i = 0; i <= 1; i++)
            {
                tempsAttaque[i] = -5;
            }
            this.life = this.lifeMax;
            this.power = this.powerMax;
        }

        private void F_Update(Personnage[] perso, Monster[] monster, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (G_IsAlive())
            {
                F_Deplacer();
                F_Attaque(monster, gameTime);
                if (classe == 1)
                {
                    
                }
                else if (classe == 2)
                {

                }
                else if (classe == 3)
                {
                    F_Healing(perso, gameTime);
                }
                else
                {
                    F_OverKill(monster, perso, gameTime);
                }
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

        public void F_Draw(SpriteBatch sb, GameTime gameTime)
        {
            // CODE SALE
            if (imgState < 0 && classe == 1)
                sb.Draw(objet, new Vector2((int)position.X - 40, (int)position.Y - 30), Color.White);
            else if (imgState < 100)
                sb.Draw(objet, new Vector2((int)position.X, (int)position.Y), Color.White);
            else
                sb.Draw(objet, new Vector2((int)position.X - 240, (int)position.Y - 210), Color.White);

            if (imgState > 100)
                sb.DrawString(overKill, "OVERKILL", new Vector2(position.X - 100, position.Y - 120), Color.Firebrick);
            
            if (tempsLevelUp + 1 > gameTime.TotalRealTime.TotalSeconds)
                sb.DrawString(overKill, "Level Up", new Vector2(position.X - 100, position.Y - 80), Color.DarkOrange);
            
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
            float time;
            if (classe == 1)
                time = 0.4f;
            else
                time = 0.5f;
            if (tempsActuel > tempsAttaque[0] + time)
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
                            m.S_Degat((int)((42 + random.Next(10) + 10 * level) * force), gameTime);
                            if (m.G_Killed())
                            {
                                S_Xp(m.G_MaxLife(), gameTime);
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
                if (tempsActuel > tempsAttaque[0] + 0.35)
                {
                    if (imgState % 10 == -3)
                    {
                        Son.Play(1);
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.25)
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
                else if (tempsActuel > tempsAttaque[0] + 0.16)
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
                else if (tempsActuel > tempsAttaque[0] + 0.07)
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
                        Son.Play(3);
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
            bool p_target = false;
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

                    for (int i = 0; i < GameState.G_Player(); i++)
                    {
                        p_target_ovrkl[i] = null;
                    }

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
                            monster[i].S_Degat((int)((50 + level * 15) * mana), gameTime);
                            if (monster[i].G_Killed())
                            {
                                S_Xp(monster[i].G_MaxLife(), gameTime);
                            }
                        }
                    }

                    for (int i = 0; i < GameState.G_Player(); i++)
                    {
                        if (i != id - 1)
                        {
                            p_target_ovrkl[i] = F_DetectAllies(perso[i]);
                            foreach (Personnage p in p_target_ovrkl)
                            {
                                if (p != null)
                                    p_target = true;
                            }

                            if (p_target && perso[i].G_IsAlive() && p_target_ovrkl[i] != null)
                            {
                                perso[i].S_Degat((int)((level * 3) * mana), gameTime);
                            }
                        }
                    }

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

        private void F_Healing(Personnage[] perso, GameTime gameTime)
        {
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[1] + 0.5)
            {
                if (newState.IsKeyDown(key[5]) && power >= 500)
                {
                    power -= 500;
                    foreach (Personnage p in perso)
                    {
                        p.S_Life(50);
                    }
                    tempsAttaque[1] = tempsActuel;
                }
            }
        }

        private Personnage F_DetectAllies(Personnage p)
        {
            Personnage p_ = null;

            if (p.G_Rectangle().Intersects(new Rectangle((int)position.X - 50, (int)position.Y - 50, 100 + (int)objet.Width, 100 + (int)objet.Height)))
                p_ = p;

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

        #region Deplacement

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

        #endregion

        #region UI

        private void F_UpdateRegen(GameTime gameTime)
        {
            if (tempsRegen + 0.05 < gameTime.TotalGameTime.TotalSeconds)
            {
                tempsRegen = gameTime.TotalGameTime.TotalSeconds;
                power += 2 * mana;
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
                portrait[0].Draw(spriteBatch);
                spriteBatch.DrawString(textFont, "Ilean", new Vector2(113, 7), Color.White);
                spriteBatch.DrawString(textFont, "Ilean", new Vector2(112, 6), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(241, 7), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(240, 6), Color.Black);
            }
            else if (id == 2)
            {
                spriteBatch.Draw(ui2, new Rectangle(Program.width - 262, 10, ui2.Width, ui2.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 42, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 62, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 82, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                portrait[1].Draw(spriteBatch);
                spriteBatch.DrawString(textFont, "Qwazerty", new Vector2(Program.width - 264, 7), Color.White);
                spriteBatch.DrawString(textFont, "Qwazerty", new Vector2(Program.width - 265, 6), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(Program.width - 133, 7), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(Program.width - 134, 6), Color.Black);
            }
            else if (id == 3)
            {
                spriteBatch.Draw(ui1, new Rectangle(10, 110, ui1.Width, ui1.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(110, 142, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(110, 162, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(110, 182, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                portrait[2].Draw(spriteBatch);
                spriteBatch.DrawString(textFont, "Flint", new Vector2(113, 107), Color.White);
                spriteBatch.DrawString(textFont, "Flint", new Vector2(112, 106), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(241, 107), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(240, 106), Color.Black);
            }
            else if (id == 4)
            {
                spriteBatch.Draw(ui2, new Rectangle(Program.width - 262, 110, ui2.Width, ui2.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 142, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 162, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, 182, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                portrait[3].Draw(spriteBatch);
                spriteBatch.DrawString(textFont, "Menkar", new Vector2(Program.width - 264, 107), Color.White);
                spriteBatch.DrawString(textFont, "Menkar", new Vector2(Program.width - 265, 106), Color.Black);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(Program.width - 133, 107), Color.White);
                spriteBatch.DrawString(textFont, level.ToString(), new Vector2(Program.width - 134, 106), Color.Black);
            }

        }

        public void F_DrawDegats(SpriteBatch sb, GameTime gameTime)
        {
            if (tempsDegats + 0.5 > gameTime.TotalGameTime.TotalSeconds)
            {
                if (degats == 0)
                {
                    sb.DrawString(gameFont, "Miss", new Vector2(position.X + 10, position.Y - 18), Color.Black);
                    sb.DrawString(gameFont, "Miss", new Vector2(position.X + 8, position.Y - 20), Color.Orange);
                }
                else
                {
                    sb.DrawString(gameFont, degats.ToString(), new Vector2(position.X + 25, position.Y - 18), Color.Black);
                    sb.DrawString(gameFont, degats.ToString(), new Vector2(position.X + 23, position.Y - 20), Color.White);
                }
            }
        }

        #endregion

    }
}
