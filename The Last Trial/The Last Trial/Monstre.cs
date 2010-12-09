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
    public class Monstre : Objet
    {
        // Nombre de collision sur la map
        const int taille = 3;

        // DECLARATION VARIABLES
        int img_state;
        Vector2 speed;
        bool[] collision = new bool[taille];
        //int collision_state;
        double tempsActuel;
        Vector2 spawn;
        Random random = new Random();
        int rand;
        /*Rectangle spawnRectangle = new Rectangle((int)spawn.X - 100, (int)spawn.Y - 100, 200, 200);
        Rectangle persoRectangle = new Rectangle((int)position.X, (int)position.Y, objet.Width, objet.Height);*/
        

        // CONSTRUCTEUR
        public Monstre(Vector2 spawn_get)
        {
            spawn = spawn_get;
            position = spawn;
            img_state = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsActuel = 0;
            //collision_state = 0;
            for (int i = 0; i < taille; i++)
            {
                collision[i] = false;
            }
        }

        /*****************
         * METHODE : GET *
         *****************/

        public int G_ImgState()
        {
            return img_state;
        }

        public Vector2 G_Speed()
        {
            return speed;
        }

        /*****************
         * METHODE : SET *
         *****************/

        public void S_Deplacement(float time)
        {
            /*persoRectangle += speed;
            if (!persoRectangle.Intersects(spawnRectangle))
            {*/
                position += speed * time;
            //}
        }

        /**********************
         * METHODE : FONCTION *
         **********************/

        public void F_Collision_Objets(Rectangle item)
        {
            /*
            Rectangle persoRectangle = new Rectangle((int)position.X, (int)position.Y + (objet.Height * 2) / 3, objet.Width, objet.Height / 3);

            if (persoRectangle.Intersects(item) && !collision[collision_state])
            {
                collision[collision_state] = true;
                speed = Vector2.Multiply(speed, -1.0f);
            }
            else if (!(persoRectangle.Intersects(item)) && collision[collision_state])
            {
                speed = Vector2.Zero;
                collision[collision_state] = false;
            }

            collision_state++;

            if (collision_state == taille)
            {
                collision_state = 0;
            }*/
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

        }

        public void F_SpawnCollision()
        { 
            
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
