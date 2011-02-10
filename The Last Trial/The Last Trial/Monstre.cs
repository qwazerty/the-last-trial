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
    public class Monstre : Objet
    {
        // Nombre de collision sur la map
        const int taille = 3;

        // DECLARATION VARIABLES
        int imgState;
        Vector2 speed;
        bool[] collision = new bool[taille];
        double tempsActuel;
        Vector2 spawn;
        private Random random;
        int rand;
        int life;
        Personnage target = null;
        Rectangle interact;
        //Rectangle spawnRectangle = new Rectangle((int)spawn.X - 100, (int)spawn.Y - 100, 200, 200);

        // CONSTRUCTEUR
        public Monstre(Vector2 spawn_get)
        {
            spawn = spawn_get;
            position = spawn;
            imgState = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsActuel = 0;
            life = 30;
            random = new Random();
            for (int i = 0; i < taille; i++)
            {
                collision[i] = false;
            }
        }

        /*****************
         * METHODE : GET *
         *****************/

        public Vector2 G_Speed()
        {
            return speed;
        }

        public bool G_Life()
        {
            return life > 0;
        }

        public Rectangle G_Rect()
        {
            return interact;
        }

        /*****************
         * METHODE : SET *
         *****************/

        public void S_Deplacement(float time)
        {
            /*persoRectangle += speed;
            if (!persoRectangle.Intersects(spawnRectangle))
            {*/
            if (life > 0)
            {
                position += speed * time;
            }
            //}
        }

        public void S_Resu()
        {
            life = 30;
            imgState = 40;
        }

        /**********************
         * METHODE : FONCTION *
         **********************/

        public void F_Load(ContentManager content)
        {
            base.objet = content.Load<Texture2D>("mob/1/" + imgState);
        }

        public void F_Draw(SpriteBatch sb)
        {
            sb.Draw(base.objet, base.position, Color.White);
        }

        public void F_CollisionEcran(GraphicsDeviceManager graphics)
        {

            int MaxX = graphics.GraphicsDevice.Viewport.Width - objet.Width;
            int MinX = 0;
            int MaxY = graphics.GraphicsDevice.Viewport.Height - objet.Height;
            int MinY = 0;

            if (position.X > MaxX)
            {
                speed.X = 0;
                position.X = MaxX;
            }

            else if (position.X < MinX)
            {
                speed.X = 0;
                position.X = MinX;
            }

            if (position.Y > MaxY)
            {
                speed.Y = 0;
                position.Y = MaxY;
            }

            else if (position.Y < MinY)
            {
                speed.Y = 0;
                position.Y = MinY;
            }
        }

        public bool F_Collision_Objets(Rectangle item)
        {
                Rectangle mobRectangle = new Rectangle((int)position.X, (int)position.Y + (objet.Height * 2) / 3, objet.Width, objet.Height / 3);
                return item.Intersects(mobRectangle);

                //if (persoRectangle.Intersects(item) && !collision[collision_state])
                //{
                //    collision[collision_state] = true;
                //    speed = Vector2.Multiply(speed, -1.0f);
                //}
                //else if (!(persoRectangle.Intersects(item)) && collision[collision_state])
                //{
                //    speed = Vector2.Zero;
                //    collision[collision_state] = false;
                //}

                //collision_state++;

                //if (collision_state == taille)
                //{
                //    collision_state = 0;
                //}
        }

        public bool F_IsAlive(int life2)
        {
            life--;
            if (life <= life2)
            {
                return false;
            }
            return true;
        }

        public void F_IA(GameTime gameTime)
        {
            if (target == null || F_DetectPlayer())
            { 
                
            }
        }

        public bool F_DetectPlayer()
        {
            return false;
        }

        public void F_Deplacer(GameTime gameTime)
        {
            double temps = gameTime.TotalGameTime.TotalSeconds;

            if (tempsActuel < temps - 1.5)
            {
                rand = random.Next(1, 6);
                // DROITE
                if (rand == 2)
                {
                    speed = new Vector2(30.0f, 0.0f);
                }

                // GAUCHE
                if (rand == 4)
                {
                    speed = new Vector2(-30.0f, 0.0f);
                }

                // HAUT
                if (rand == 3)
                {
                    speed = new Vector2(0.0f, -30.0f);
                }

                // BAS
                if (rand == 1)
                {
                    speed = new Vector2(0.0f, 30.0f);
                }

                // STOP
                if (rand == 5)
                {
                    speed = Vector2.Zero;
                }
                tempsActuel = temps;
                
            }

            interact = new Rectangle((int)position.X + (base.objet.Width) / 2,
                (int)position.Y, 5, base.objet.Height);
        }

        public void F_Collision_Joueur(Rectangle perso)
        {
            if (F_Collision_Objets(perso))
            {
                speed = Vector2.Zero;
            }
        }

        public void F_SpawnCollision()
        { 
            
        }

        public int F_UpdateState(Personnage p)
        {
            if (F_Collision_Objets(p.G_Rectangle()))
            {
                if (p.F_Attaque(Keyboard.GetState()))
                {
                    if (!F_IsAlive(0))
                        imgState = 3;

                    else if (!F_IsAlive(15))
                        imgState = 2;
                }
            }

            return imgState;
        }

        public void F_UpdateImage(GameTime gameTime, double delai)
        {
            /*
            // Test si le personnage est en collision
            bool in_collision = false;

            for (int i = 0; i < taille; i++)
            {
                if (collision[i])
                {
                    in_collision = true;
                }
            }

            if (!in_collision)
            {

                // Affichage images direction normale
                if (speed.X > 0 && speed.Y == 0 && (img_state < 20 || img_state > 27))
                {
                    img_state = 20;
                }
                else if (speed.X < 0 && speed.Y == 0 && (img_state < 40 || img_state > 47))
                {
                    img_state = 40;
                }
                else if (speed.X == 0 && speed.Y > 0 && (img_state < 10 || img_state > 17))
                {
                    img_state = 10;
                }
                else if (speed.X == 0 && speed.Y < 0 && (img_state < 30 || img_state > 37))
                {
                    img_state = 30;
                }

                // Affichage images direction diagonales
                else if (speed.X > 0 && speed.Y > 0 && (img_state < 50 || img_state > 57))
                {
                    img_state = 50;
                }
                else if (speed.X > 0 && speed.Y < 0 && (img_state < 60 || img_state > 67))
                {
                    img_state = 60;
                }
                else if (speed.X < 0 && speed.Y > 0 && (img_state < 80 || img_state > 87))
                {
                    img_state = 80;
                }
                else if (speed.X < 0 && speed.Y < 0 && (img_state < 70 || img_state > 77))
                {
                    img_state = 70;
                }

            }

            // Update l'image de X0 à X7
            if (speed != Vector2.Zero)
            {
                double temps = gameTime.TotalGameTime.TotalSeconds;

                if (tempsActuel < temps - delai)
                {
                    img_state++;
                    if (img_state % 10 > 7)
                    {
                        img_state -= img_state % 10;
                    }
                    tempsActuel = temps;
                }
            }*/
            
        }
    }
}