using System;
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
        private Keys[] key;
        /** KEYS STATES **\
         * 0 : BAS       *
         * 1 : DROITE    *
         * 2 : HAUT      *
         * 3 : GAUCHE    *
         * 4 : ATTAQUE   *
         * 5 : OMGOVERKILL*
        \*****************/

        // CONSTRUCTEUR
        public Personnage(Keys[] key, Vector2 position) : base()
        {
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
            return new Rectangle((int)position.X, (int)position.Y + objet.Height * 2 / 3, objet.Width, objet.Height / 3);
        }
        
        public void S_Resu()
        {
            life = 100;
        }

        /*****************\
         *   STATIC FUN   *
        \*****************/

        #region Static Load, Update & Draw

        public static void Load(Personnage[] perso, ContentManager Content, int player)
        {
            if (player > 0)
                perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.Space, Keys.RightShift }, new Vector2(300f, 500f));

            if (player > 1)
                perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.W, Keys.A, Keys.F, Keys.D0 }, new Vector2(330f, 600f));

            if (player > 2)
                perso[2] = new Personnage(new Keys[] { Keys.NumPad2, Keys.NumPad6, Keys.NumPad8, Keys.NumPad4, Keys.NumPad0, Keys.D0 }, new Vector2(300f, 650f));

            if (player > 3)
                perso[3] = new Personnage(new Keys[] { Keys.L, Keys.OemSemicolon, Keys.O, Keys.K, Keys.J, Keys.D0 }, new Vector2(330f, 350f));

            foreach (Personnage p in perso)
                p.F_Load(Content);

            gameFont = Content.Load<SpriteFont>("gamefont");
        }

        public static void Update(Personnage[] perso, GameTime gameTime, Monster[] monster, GraphicsDeviceManager graphics, ContentManager Content)
        {
            foreach (Personnage p in perso)
                p.F_Update(perso, monster, Content, gameTime, graphics);
        }

        public static void Draw(Personnage[] perso, SpriteBatch spriteBatch)
        {
            foreach (Personnage p in perso)
                p.F_Draw(spriteBatch);

            Personnage personnage = F_IsOverKilling(perso);
            if (personnage != null)
                spriteBatch.DrawString(gameFont, "OVERKILL", new Vector2(personnage.G_Position().X - 100, personnage.G_Position().Y - 120), Color.Firebrick);
        }

        #endregion

        /*****************\
         * METHODE : FUN *
        \*****************/

        #region Load, Update & Draw

        private void F_Load(ContentManager content)
        {
            objet = content.Load<Texture2D>("perso/1/" + imgState);
        }

        private void F_Update(Personnage[] perso, Monster[] monster, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            F_Deplacer();
            F_Attaque(monster, gameTime);
            F_OverKill(monster, perso, gameTime);
            F_UpdateImage(gameTime);
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

        private void F_Draw(SpriteBatch sb)
        {
            if (imgState != 99)
                sb.Draw(base.objet, base.position, Color.White);
            else
                sb.Draw(base.objet, new Vector2(position.X - 142, position.Y - 50), Color.White);
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

            else if (position.X < MaxX * 0.185)
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

        #region Attaque & Magie

        public void F_Attaque(Monster[] monster, GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(key[4]))
            {
                tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
                if (tempsActuel > tempsAttaque[0] + 0.5)
                {
                    Son.Play(1);
                    foreach (Monster m in monster)
                    {
                        if (G_Rectangle().Intersects(m.G_Interact()) && m.G_IsAlive())
                        {
                            m.S_Degat(42);
                        }
                    }
                    tempsAttaque[0] = tempsActuel;
                }
            }

        }

        public void F_OverKill(Monster[] monster, Personnage[] perso, GameTime gameTime)
        {
            newState = Keyboard.GetState();

            Monster[] m_target_ovrkl = new Monster[Game1.G_Monster()];
            Personnage[] p_target_ovrkl = new Personnage[Game1.G_Player()];
            bool p_target = false;
            bool m_target = false;

            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[1] + 5)
            {
                if (newState.IsKeyDown(key[5]))
                {
                    for (int i = 0; i < Game1.G_Monster(); i++)
                    {
                        m_target_ovrkl[i] = null;
                    }

                    /*for (int i = 0; i < Game1.G_Player(); i++)
                    {
                        p_target_ovrkl[i] = null;
                    }*/

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
                            Console.WriteLine(i + " : " + m_target_ovrkl[i]);
                            monster[i].S_Degat(1337);
                        }
                    }

                    /*for (int i = 0; i < Game1.G_Player(); i++)
                    {
                        if (F_DetectAllies(perso) != null)
                        {
                            p_target_ovrkl[i] = F_DetectAllies(perso);
                            p_target = true;
                        }
                    }*/

                    Son.Play(4);
                    imgState = 99;
                    tempsAttaque[1] = tempsActuel;
                }
            }
            else
            { 
                
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

        public static Personnage F_IsOverKilling(Personnage[] perso)
        {
            foreach (Personnage p in perso)
            {
                if (p.imgState == 99)
                    return p;
            }
            return null;
        }

        #endregion

        private void F_UpdateImage(GameTime gameTime)
        {
            // Test si le personnage est en collision
            bool in_collision = false;

            if (!in_collision)
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

            }

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
}
