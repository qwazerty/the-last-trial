﻿using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Last_Trial
{
    public class Personnage : Mob
    {
        #region VAR

        private static Objet[] portrait;
        private Keys[] key;
        private bool AtkSpe = false;
        private int classe, xp, xpMax, level, healState;
        private double[] tempsAttaque = new double[2];
        private double tempsRegenPower, tempsRegenLife, tempsLevelUp, power, powerMax, force, mana, esquive, esprit, forcetemp;
        private string name;
        private float time = 0;
        private List<Projectile> projectile = new List<Projectile>();
        private Texture2D heal;
        /** KEYS STATES **\
         * 0 : BAS       *
         * 1 : DROITE    *
         * 2 : HAUT      *
         * 3 : GAUCHE    *
         * 4 : ATTAQUE   *
         * 5 : OVERKILL  *
        \*****************/

        #endregion

        #region Constructor
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
            this.xpMax = 100;
            this.level = 1;
            this.tempsRegenPower = 0;
            this.tempsRegenLife = 0;
            this.tempsLevelUp = 0;
            this.healState = 0;

            if (classe == 1) // ROGUE
            {
                force = 0.7;
                mana = 1;
                esquive = 2;
                esprit = 0.2;
            }
            else if (classe == 2) // WARRIOR
            {
                force = 1;
                mana = 0.5;
                esquive = 0.5;
                esprit = 1;
            }
            else if (classe == 3) // HEAL
            {
                force = 0.5;
                mana = 2;
                esquive = 1;
                esprit = 2;
            }
            else if (classe == 4) // MAGE
            {
                force = 0.6;
                mana = 2;
                esquive = 1;
                esprit = 2;
            }

            switch (classe)
            {
                case 1: time = 0.4f; break;
                case 2: time = 0.6f; break;
                case 3: time = 0.6f; break;
                case 4: time = 1.0f; break;
            }

            powerMax = 500;
            if (classe == 1)
                power = 0;
            else
                power = powerMax;

            for (int i = 0; i <= 1; i++)
            {
                tempsAttaque[i] = -5;
            }

            this.oldState = Keyboard.GetState();
        }
        #endregion

        #region GET & SET

        public void S_Nom(string name)
        {
            this.name = name;
        }

        public void S_ImgState(int state)
        {
            this.imgState = state;
        }

        public string G_Nom()
        {
            return name;
        }

        public int G_Class()
        {
            return classe;
        }

        public Rectangle G_Rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y + 66, 60, 33);
        }

        public bool G_Interact(PNJ pnj)
        {
            return G_Rectangle().Intersects(pnj.G_Interact());
        }

        public int G_Xp()
        {
            int x = 0;
            for (int i = 1; i < level; i++)
            {
                x += i * 100;
            }
            return xp + x;
        }

        public void S_Xp(int xp_, GameTime gameTime)
        {
            this.xp += xp_;
            while (this.xp >= xpMax)
            {
                S_Stats();
                this.xp = this.xp - xpMax;
                level++;
                xpMax = 100 * level;
                life = lifeMax;
                power = powerMax;
                tempsLevelUp = gameTime.TotalRealTime.TotalSeconds;
            }
        }

        public void S_Xp(int xp_)
        {
            this.xp = xp_;
            while (this.xp >= xpMax)
            {
                S_Stats();
                this.xp = this.xp - xpMax;
                level++;
                xpMax = 100 * level;
                life = lifeMax;
                power = powerMax;
            }
        }
        public void S_Xp(int xp_, GameTime gameTime, Personnage[] perso)
        {
            this.xp = xp_;
            foreach (Personnage p in perso)
            {
                p.S_Xp(xp_ / 4, gameTime);
            }
            while (this.xp >= xpMax)
            {
                S_Stats();
                this.xp = this.xp - xpMax;
                level++;
                xpMax = 100 * level;
                life = lifeMax;
                power = powerMax;
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
            if (classe == 1) // ROGUE
            {
                force += 0.05;
                esquive += 0.35;
                lifeMax += 35;
            }
            else if (classe == 2) // WARRIOR
            {
                force += 0.2;
                esquive += 0.2;
                mana += 0.1;
                esprit += 0.07;
                lifeMax += 75;
            }
            else if (classe == 3) // HEAL
            {
                force += 0.01;
                esquive += 0.2;
                mana += 0.2;
                esprit += 0.3;
                lifeMax += 30;

                powerMax = 1000 * mana;
            }
            else if (classe == 4) // MAGE
            {
                force += 0.01;
                esquive += 0.2;
                mana += 0.15;
                esprit += 0.2;
                lifeMax += 30;

                powerMax = 1000 * mana;
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
            Personnage.portrait = new Objet[GameState.Player];
            if (GameState.Player > 0)
            {
                perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.Space, Keys.RightShift }, new Vector2(300f, 350f), 1, LoadingMenu.PersoClasse[0]);
                portrait[0] = new Objet(new Vector2(15, 10), Content.Load<Texture2D>("ui/" + LoadingMenu.PersoClasse[0]));
            }

            if (GameState.Player > 1)
            {
                perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.Z, Keys.Q, Keys.F, Keys.E }, new Vector2(330f, 450f), 2, LoadingMenu.PersoClasse[1]);
                portrait[1] = new Objet(new Vector2(Program.width - 95, 10), Content.Load<Texture2D>("ui/" + LoadingMenu.PersoClasse[1]));
            }

            if (GameState.Player > 2)
            {
                perso[2] = new Personnage(new Keys[] { Keys.NumPad5, Keys.NumPad6, Keys.NumPad8, Keys.NumPad4, Keys.NumPad0, Keys.NumPad7 }, new Vector2(360f, 550f), 3, LoadingMenu.PersoClasse[2]);
                portrait[2] = new Objet(new Vector2(15, 115), Content.Load<Texture2D>("ui/" + LoadingMenu.PersoClasse[2]));
            }

            if (GameState.Player > 3)
            {
                perso[3] = new Personnage(new Keys[] { Keys.L, Keys.M, Keys.O, Keys.K, Keys.J, Keys.P }, new Vector2(390f, 650f), 4, LoadingMenu.PersoClasse[3]);
                portrait[3] = new Objet(new Vector2(Program.width - 95, 115), Content.Load<Texture2D>("ui/" + LoadingMenu.PersoClasse[3]));
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
            this.heal = Content.Load<Texture2D>("magic/heal");
            this.life = this.lifeMax;
            this.power = this.powerMax;
        }

        private void F_Update(Personnage[] perso, Monster[] monster, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            if (G_IsAlive())
            {
                F_Deplacer();
                F_Cheat(gameTime, perso);
                if (newState.IsKeyDown(Keys.LeftShift))
                {
                    S_ImgState(20);
                }
                F_Attaque(monster, gameTime, Content, perso);
                F_SpecialAttaque(monster, perso, gameTime);
                foreach (Rectangle collision in Map.G_Collision())
                {
                    F_Collision_Objets(collision, gameTime);
                }
                foreach (Monster m in monster)
                {
                    if (m.G_IsAlive())
                        F_Collision_Objets(m.G_Rectangle(), gameTime);
                }
                for (int i = 0; i < projectile.Count; i++)
                {
                    if (projectile[i].Update(gameTime, Content))
                    {
                        projectile.RemoveAt(i);
                    }
                }
                F_Collision_Ecran(graphics, gameTime);
                F_UpdateRegen(gameTime);
            }

            F_UpdateImage(gameTime);
            try
            {
                objet = Content.Load<Texture2D>("perso/" + classe + "/" + imgState);
            }
            catch (ContentLoadException) { }
            S_Deplacement(gameTime);
        }

        public void F_Draw(SpriteBatch sb, GameTime gameTime, Personnage[] perso)
        {
            if (imgState < 0 && classe == 1)
                sb.Draw(objet, new Vector2((int)position.X - 40, (int)position.Y - 30), Color.White);
            else if (imgState < 100)
                sb.Draw(objet, new Vector2((int)position.X, (int)position.Y), Color.White);
            else
                sb.Draw(objet, new Vector2((int)position.X - 240, (int)position.Y - 210), Color.White);

            if (imgState > 100)
                sb.DrawString(GameState.overKill, "OVERKILL", new Vector2(position.X - 100, position.Y - 120), Color.Firebrick);
            
            if (tempsLevelUp + 1 > gameTime.TotalRealTime.TotalSeconds)
                sb.DrawString(GameState.overKill, LoadingMenu.Local[18], new Vector2(position.X - 100, position.Y - 80), Color.DarkOrange);
            
            F_DrawHealth(sb);
            foreach (Projectile proj in projectile)
            {
                proj.Draw(sb);
            }
            if (healState != 0)
            {
                foreach (Personnage p in perso)
                {
                    sb.Draw(heal, new Vector2(p.G_Rectangle().X, p.G_Rectangle().Y - (healState * 10)), Color.White);
                }
            }
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

        private void F_Cheat(GameTime gameTime, Personnage[] perso)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.C))
            {
                S_Xp(xpMax/10, gameTime);
            }
        }

        private void F_Attaque(Monster[] monster, GameTime gameTime, ContentManager Content, Personnage[] perso)
        {
            newState = Keyboard.GetState();
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[0] + time)
            {
                if (newState.IsKeyDown(key[4]) && (classe == 1 || (classe == 2 && AtkSpe) || power >= 100))
                {
                    oldImage = imgState;
                    bool attaque = false;
                    if (classe == 1 || (classe == 2 && AtkSpe))
                    { }
                    else
                        power -= 100;
                    foreach (Monster m in monster)
                    {
                        if (m.G_IsAlive() && !attaque && ((G_Rectangle().Intersects(m.G_Aggro()) && classe == 4) || (G_Rectangle().Intersects(m.G_Interact()) && classe != 4)))
                        {
                            attaque = true;
                            if (classe == 4)
                            { 
                                projectile.Add(new Projectile(Content, m, this, position, (int)((42 + random.Next(10) + 10 * level) * force)));
                            }
                            else
                            {
                                m.S_Degat((int)((42 + random.Next(10) + 10 * level) * force), gameTime);
                                if (m.G_Killed())
                                {
                                    S_Xp(m.G_MaxLife() / 4, gameTime, perso);
                                }
                            }
                        }
                    }
                    if (id == 3)
                        Son.Play(3);

                    tempsAttaque[0] = tempsActuel;
                }
            }
            else if (classe == 1 && !AtkSpe)
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
                    ImageTest(-3);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.16)
                {
                    ImageTest(-2);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.07)
                {
                    ImageTest(-1);
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    ImageTest(0);
                }
            }
            else if (classe == 1 && AtkSpe)
            {
                if (tempsActuel > tempsAttaque[0] + 0.175)
                {
                    if (imgState % 10 == -3)
                    {
                        Son.Play(1);
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.125)
                {
                    ImageTest(-3);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.08)
                {
                    ImageTest(-2);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.035)
                {
                    ImageTest(-1);
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    ImageTest(0);
                }
            }
            else if (classe == 2 && !AtkSpe)
            {
                if (tempsActuel > tempsAttaque[0] + 0.3)
                {
                    if (imgState % 10 == -2)
                    {
                        Son.Play(1);
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.2)
                {
                    ImageTest(-2);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.1)
                {
                    ImageTest(-1);
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    ImageTest(0);
                }
 
            }
            else if (classe == 2 && AtkSpe)
            {
                if (tempsActuel > tempsAttaque[0] + 0.3 - (0.3 * 1/6))
                {
                    if (imgState % 10 == -2)
                    {
                        Son.Play(1);
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.2 - (0.2 * 1/6))
                {
                    ImageTest(-2);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.1 - (0.1 * 1/6))
                {
                    ImageTest(-1);
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    ImageTest(0);
                }

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
            }
            else if (classe == 4)
            {
                if (tempsActuel > tempsAttaque[0] + 0.8)
                {
                    if (imgState % 10 == -3)
                    {
                        imgState = oldImage / 10 * 10;
                    }
                }
                else if (tempsActuel > tempsAttaque[0] + 0.6)
                {
                    for (int i = 0; i < projectile.Count; i++)
                    {
                        projectile[i].StartFiring();
                    }

                    ImageTest(-3);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.4)
                {
                    ImageTest(-2);
                }
                else if (tempsActuel > tempsAttaque[0] + 0.2)
                {
                    ImageTest(-1);
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    ImageTest(0);
                }
            }

            if (imgState < 0)
                speed = Vector2.Zero;
        }

        private void ImageTest(int image)
        {
            if (imgState % 10 == image + 1)
                imgState--;
            else if (imgState > 0)
            {
                if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                    imgState = -50 + image;
                else if (oldImage / 10 == 6 || oldImage / 10 == 3)
                    imgState = -60 + image;
                else if (oldImage / 10 == 7)
                    imgState = -70 + image;
                else if (oldImage / 10 == 4 || oldImage / 10 == 8)
                    imgState = -80 + image;
            }
        }

        private void F_SpecialAttaque(Monster[] monster, Personnage[] perso, GameTime gameTime)
        {
            switch (classe)
            {
                case 1: F_Booster(gameTime); break;
                case 2: F_BERZERKER(gameTime); break;
                case 3: F_Healing(perso, gameTime); break;
                case 4: F_OverKill(monster, perso, gameTime); break;
            }
        }

        private void F_OverKill(Monster[] monster, Personnage[] perso, GameTime gameTime)
        {
            newState = Keyboard.GetState();

            Monster[] m_target_ovrkl = new Monster[GameState.Monster];
            Personnage[] p_target_ovrkl = new Personnage[GameState.Player];
            bool p_target = false;
            bool m_target = false;

            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[1] + 3)
            {
                if (newState.IsKeyDown(key[5]) && power >= 500)
                {
                    power -= 500;
                    for (int i = 0; i < GameState.Monster; i++)
                    {
                        m_target_ovrkl[i] = null;
                    }

                    for (int i = 0; i < GameState.Player; i++)
                    {
                        p_target_ovrkl[i] = null;
                    }

                    for (int i = 0; i < GameState.Monster; i++)
                    {
                        m_target_ovrkl[i] = F_DetectMonsters(monster[i]);
                        foreach (Monster m in m_target_ovrkl)
                        {
                            if (m != null)
                                m_target = true;
                        }

                        if (m_target && monster[i].G_IsAlive() && m_target_ovrkl[i] != null)
                        {
                            monster[i].S_Degat((int)((10 + level * 7) * mana), gameTime);
                            if (monster[i].G_Killed())
                            {
                                S_Xp(monster[i].G_MaxLife(), gameTime, perso);
                            }
                        }
                    }

                    for (int i = 0; i < GameState.Player; i++)
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
                        p.S_Life((int)(20 * mana));
                    }
                    tempsAttaque[1] = tempsActuel;
                }
            }
            else if (tempsActuel > tempsAttaque[1] + 0.45)
            {
                healState = 0;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.35)
            {
                healState = 8;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.3)
            {
                healState = 7;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.25)
            {
                healState = 6;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.2)
            {
                healState = 5;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.15)
            {
                healState = 4;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.1)
            {
                healState = 3;
            }
            else if (tempsActuel > tempsAttaque[1] + 0.05)
            {
                healState = 2;
            }
            else if (tempsActuel > tempsAttaque[1])
            {
                healState = 1;
            }
        }

        private void F_Booster(GameTime gameTime)
        {
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (newState.IsKeyDown(key[5]) && power == powerMax && !AtkSpe)
            {
                tempsAttaque[1] = tempsActuel;
                time = 0.2f;
                power = 0;
                AtkSpe = true;
            }
            if (tempsActuel > tempsAttaque[1] + 5 && AtkSpe)
            {
                time = 0.4f;
                AtkSpe = false;
                imgState = oldImage / 10 * 10;
            }
        }

        private void F_BERZERKER(GameTime gameTime)
        {
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (newState.IsKeyDown(key[5]) && power >= 500 && !AtkSpe)
            {
                forcetemp = level / 2;
                tempsAttaque[1] = tempsActuel;
                time = 0.5f;
                force += forcetemp;
                power -= 250;
                life -= lifeMax / 4;
                AtkSpe = true;
            }
            if (tempsActuel > tempsAttaque[1] + 5 && AtkSpe)
            {
                time = 0.6f;
                force -= forcetemp;
                AtkSpe = false;
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

        private void F_UpdateImage(GameTime gameTime)
        {
            if (life <= 0)
            {
                imgState = 0;
            }
            else if (imgState == 0 && life > 0)
            {
                imgState = 20;
            }
            else
            {
                // Affichage images direction normale
                if (speed.X > 0 && speed.Y == 0 && (imgState < 20 || imgState > 27))
                    imgState = 20;

                else if (speed.X < 0 && speed.Y == 0 && (imgState < 40 || imgState > 47))
                    imgState = 40;

                else if (speed.X == 0 && speed.Y > 0 && (imgState < 10 || imgState > 17))
                    imgState = 10;

                else if (speed.X == 0 && speed.Y < 0 && (imgState < 30 || imgState > 37))
                    imgState = 30;

                // Affichage images direction diagonales
                else if (speed.X > 0 && speed.Y > 0 && (imgState < 50 || imgState > 57))
                    imgState = 50;

                else if (speed.X > 0 && speed.Y < 0 && (imgState < 60 || imgState > 67))
                    imgState = 60;

                else if (speed.X < 0 && speed.Y > 0 && (imgState < 80 || imgState > 87))
                    imgState = 80;

                else if (speed.X < 0 && speed.Y < 0 && (imgState < 70 || imgState > 77))
                    imgState = 70;

                // Update l'image de X0 à X7
                if (speed != Vector2.Zero)
                {
                    double temps = gameTime.TotalGameTime.TotalSeconds;

                    if (tempsImage < temps - 0.1)
                    {
                        imgState++;
                        if (imgState % 10 > 7)
                            imgState -= imgState % 10;

                        tempsImage = temps;
                    }
                }
            }
        }

        #endregion

        #region UI

        private void F_UpdateRegen(GameTime gameTime)
        {
            if (tempsRegenLife + 1 < gameTime.TotalGameTime.TotalSeconds)
            {
                tempsRegenLife = gameTime.TotalGameTime.TotalSeconds;
                life += (int)(lifeMax / 100);
                if (life > lifeMax)
                {
                    life = lifeMax;
                }
            }
            if (tempsRegenPower + 0.05 < gameTime.TotalGameTime.TotalSeconds)
            {
                tempsRegenPower = gameTime.TotalGameTime.TotalSeconds;
                power += 2 * esprit;
                if (power > powerMax)
                {
                    power = powerMax;
                }
            }
        }

        private void F_DrawHealth(SpriteBatch spriteBatch)
        {
            int y;
            if (id <= 2)
            {
                y = 10;
            }
            else
            {
                y = 110;
            }
            if (id % 2 == 1)
            {
                spriteBatch.Draw(ui1, new Rectangle(10, y, ui1.Width, ui1.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(110, y + 32, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(110, y + 52, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(110, y + 72, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                portrait[id - 1].Draw(spriteBatch);
                spriteBatch.DrawString(GameState.textFont, name, new Vector2(113, y - 3), Color.White);
                spriteBatch.DrawString(GameState.textFont, name, new Vector2(112, y - 4), Color.Black);
                spriteBatch.DrawString(GameState.textFont, level.ToString(), new Vector2(241, y - 3), Color.White);
                spriteBatch.DrawString(GameState.textFont, level.ToString(), new Vector2(240, y - 4), Color.Black);
            }
            else
            {
                spriteBatch.Draw(ui2, new Rectangle(Program.width - 262, y, ui2.Width, ui2.Height), Color.White);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, y + 32, (int)(life * 146 / lifeMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Red);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, y + 52, (int)(power * 146 / powerMax), 8), new Rectangle(0, 12, health.Width, 12), Color.Blue);
                spriteBatch.Draw(health, new Rectangle(Program.width - 254, y + 72, (int)(xp * 146 / xpMax), 2), new Rectangle(0, 12, health.Width, 2), Color.Green);
                portrait[id - 1].Draw(spriteBatch);
                spriteBatch.DrawString(GameState.textFont, name, new Vector2(Program.width - 264, y - 3), Color.White);
                spriteBatch.DrawString(GameState.textFont, name, new Vector2(Program.width - 265, y - 4), Color.Black);
                spriteBatch.DrawString(GameState.textFont, level.ToString(), new Vector2(Program.width - 133, y - 3), Color.White);
                spriteBatch.DrawString(GameState.textFont, level.ToString(), new Vector2(Program.width - 134, y - 4), Color.Black);
            }

        }

        public void F_DrawDegats(SpriteBatch sb, GameTime gameTime)
        {
            if (tempsDegats + 0.5 > gameTime.TotalGameTime.TotalSeconds)
            {
                if (degats == 0)
                {
                    sb.DrawString(GameState.gameFont, LoadingMenu.Local[19], new Vector2(position.X + 10, position.Y - 18), Color.Black);
                    sb.DrawString(GameState.gameFont, LoadingMenu.Local[19], new Vector2(position.X + 8, position.Y - 20), Color.Orange);
                }
                else
                {
                    sb.DrawString(GameState.gameFont, degats.ToString(), new Vector2(position.X + 25, position.Y - 18), Color.Black);
                    sb.DrawString(GameState.gameFont, degats.ToString(), new Vector2(position.X + 23, position.Y - 20), Color.White);
                }
            }
        }

        #endregion

    }
}
