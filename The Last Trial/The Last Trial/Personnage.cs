﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace The_Last_Trial
{
    public class Personnage : Mob
    {

        // DECLARATION VARIABLES
        private static SpriteFont gameFont;
        private KeyboardState newState, oldState;
        private int oldImage;
        private Keys[] key;
        /** KEYS STATES **\
         * 0 : BAS       *
         * 1 : DROITE    *
         * 2 : HAUT      *
         * 3 : GAUCHE    *
         * 4 : ATTAQUE   *
         * 5 : OVERKILL  *
        \*****************/

        // CONSTRUCTEUR
        public Personnage(Keys[] key, Vector2 position, int id) : base()
        {
            this.id = id;
            this.key = key;
            this.position = position;
            this.imgState = 20;
            this.life = 100;
            this.oldState = Keyboard.GetState();
            this.initLife = this.life;
        }

        /*****************\
         * METHODE : GET *
        \*****************/

        public Rectangle G_Rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y + 66, 60, 33);
        }

        /*****************\
         * STATIC : FUN  *
        \*****************/

        #region Static Load & Update

        public static void Load(Personnage[] perso, ContentManager Content, int player)
        {
            if (player > 0)
                perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.Space, Keys.RightShift }, new Vector2(300f, 500f), 1);

            if (player > 1)
                perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.Z, Keys.Q, Keys.F, Keys.D1 }, new Vector2(330f, 600f), 3);

            if (player > 2)
                perso[2] = new Personnage(new Keys[] { Keys.NumPad2, Keys.NumPad6, Keys.NumPad8, Keys.NumPad4, Keys.NumPad0, Keys.D2 }, new Vector2(300f, 650f), 1);

            if (player > 3)
                perso[3] = new Personnage(new Keys[] { Keys.L, Keys.M, Keys.O, Keys.K, Keys.J, Keys.D3 }, new Vector2(330f, 350f), 3);

            foreach (Personnage p in perso)
                p.F_Load(Content);

            gameFont = Content.Load<SpriteFont>("overkillfont");
        }

        public static void Update(Personnage[] perso, GameTime gameTime, Monster[] monster, GraphicsDeviceManager graphics, ContentManager Content)
        {
            foreach (Personnage p in perso)
                p.F_Update(perso, monster, Content, gameTime, graphics);
        }

        #endregion

        /*****************\
         * METHODE : FUN *
        \*****************/

        #region Load, Update & Draw

        private void F_Load(ContentManager content)
        {
            objet = content.Load<Texture2D>("perso/" + id + "/" + imgState);
        }

        private void F_Update(Personnage[] perso, Monster[] monster, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            F_Deplacer();
            F_UpdateImage(gameTime);
            F_Attaque(monster, gameTime);
            F_OverKill(monster, perso, gameTime);
            foreach (Rectangle collision in Map.G_Collision())
                F_Collision_Objets(collision, gameTime);
            foreach (Monster m in monster)
            {
                if (m.G_IsAlive())
                    F_Collision_Objets(m.G_Rectangle(), gameTime);
            }
            F_Collision_Ecran(graphics, gameTime);
            S_Deplacement(gameTime);
            F_Load(Content);
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
                sb.DrawString(gameFont, "OVERKILL", new Vector2(position.X - 100, position.Y - 120), Color.Firebrick);
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

        public void F_Collision_Objets(Rectangle rect, GameTime gameTime)
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

        public void F_Attaque(Monster[] monster, GameTime gameTime)
        {
            newState = Keyboard.GetState();
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[0] + 0.5)
            {
                if (newState.IsKeyDown(key[4]))
                {
                    oldImage = imgState;
                    bool attaque = false;
                    foreach (Monster m in monster)
                    {
                        if (G_Rectangle().Intersects(m.G_Interact()) && m.G_IsAlive() && !attaque)
                        {
                            attaque = true;
                            m.S_Degat(42);
                        }
                    }
                    if (id == 3)
                        Son.Play(3);

                    tempsAttaque[0] = tempsActuel;
                }
            }
            // CODE SALE !
            else if (id == 1)
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
                    if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                        imgState = -23;
                    if (oldImage / 10 == 3 || oldImage / 10 == 6)
                        imgState = -13;
                    if (oldImage / 10 == 7)
                        imgState = -43;
                    if (oldImage / 10 == 4 || oldImage / 10 == 8)
                        imgState = -33;
                }
                else if (tempsActuel > tempsAttaque[0] + 0.2)
                {
                    if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                        imgState = -22;
                    if (oldImage / 10 == 3 || oldImage / 10 == 6)
                        imgState = -12;
                    if (oldImage / 10 == 7)
                        imgState = -42;
                    if (oldImage / 10 == 4 || oldImage / 10 == 8)
                        imgState = -32;
                }
                else if (tempsActuel > tempsAttaque[0] + 0.1)
                {
                    if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                        imgState = -21;
                    if (oldImage / 10 == 3 || oldImage / 10 == 6)
                        imgState = -11;
                    if (oldImage / 10 == 7)
                        imgState = -41;
                    if (oldImage / 10 == 4 || oldImage / 10 == 8)
                        imgState = -31;
                }
                else if (tempsActuel > tempsAttaque[0])
                {
                    if (oldImage / 10 == 1 || oldImage / 10 == 2 || oldImage / 10 == 5)
                        imgState = -20;
                    if (oldImage / 10 == 3 || oldImage / 10 == 6)
                        imgState = -10;
                    if (oldImage / 10 == 7)
                        imgState = -40;
                    if (oldImage / 10 == 4 || oldImage / 10 == 8)
                        imgState = -30;
                }

                if (imgState < 0)
                    speed = Vector2.Zero;
            }
            else if (id == 3)
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

        public void F_OverKill(Monster[] monster, Personnage[] perso, GameTime gameTime)
        {
            newState = Keyboard.GetState();

            Monster[] m_target_ovrkl = new Monster[Game1.G_Monster()];
            Personnage[] p_target_ovrkl = new Personnage[Game1.G_Player()];
            //bool p_target = false;
            bool m_target = false;

            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[1] + 3)
            {
                if (newState.IsKeyDown(key[5]))
                {
                    for (int i = 0; i < Game1.G_Monster(); i++)
                    {
                        m_target_ovrkl[i] = null;
                    }

                    //for (int i = 0; i < Game1.G_Player(); i++)
                    //{
                    //    p_target_ovrkl[i] = null;
                    //}

                    for (int i = 0; i < Game1.G_Monster(); i++)
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

        public Personnage F_DetectAllies(Personnage[] perso)
        {
            Personnage p_ = null;
            foreach (Personnage p in perso)
            {
                if (p.G_Rectangle().Intersects(new Rectangle((int)position.X - 200, (int)position.Y - 200, 400 + objet.Width, 400 + objet.Height)))
                    p_ = p;
            }
            return p_;
        }

        public Monster F_DetectMonsters(Monster m)
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

        public bool F_Interact(PNJ pnj)
        {
            return G_Rectangle().Intersects(pnj.G_Interact());
        }


    }
}
