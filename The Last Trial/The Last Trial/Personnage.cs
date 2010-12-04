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
    public class Personnage : Objet
    {
        // Nombre de collision sur la map
        const int taille = 3;

        // DECLARATION VARIABLES
        int img_state;
        Vector2 speed;
        bool[] collision = new bool[taille];
        int collision_state;
        KeyboardState old_state;
        double tempsActuel;

        // CONSTRUCTEUR
        public Personnage()
        {
            position = new Vector2(0.0f, 500.0f);
            img_state = 40;
            speed = new Vector2(0.0f, 0.0f);
            old_state = Keyboard.GetState();
            tempsActuel = 0;
            collision_state = 0;
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
            position += speed * time;
        }

        /**********************
         * METHODE : FONCTION *
         **********************/

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

        public void F_Collision_Objets(Rectangle item)
        {
            
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
            }
        }

        public void F_Deplacer(
            Keys bas, Keys droite, 
            Keys haut, Keys gauche, 
            KeyboardState new_state)
        {
            // DROITE
            if (new_state.IsKeyDown(droite))
            {
                if (!old_state.IsKeyDown(droite))
                {
                    speed.X = 150.0f;
                }
            }
            else if (old_state.IsKeyDown(droite))
            {
                if (speed.X > 0)
                {
                    speed.X = 0.0f;
                }
            }

            // GAUCHE
            if (new_state.IsKeyDown(gauche))
            {
                if (!old_state.IsKeyDown(gauche))
                {
                    speed.X = -150.0f;
                }
            }
            else if (old_state.IsKeyDown(gauche))
            {
                if (speed.X < 0)
                {
                    speed.X = 0.0f;
                }
            }

            // HAUT
            if (new_state.IsKeyDown(haut))
            {
                if (!old_state.IsKeyDown(haut))
                {
                    speed.Y = -80.0f;
                }
            }
            else if (old_state.IsKeyDown(haut))
            {
                if (speed.Y < 0)
                {
                    speed.Y = 0.0f;
                }
            }

            // BAS
            if (new_state.IsKeyDown(bas))
            {
                if (!old_state.IsKeyDown(bas))
                {
                    speed.Y = 80.0f;
                }
            }
            else if (old_state.IsKeyDown(bas))
            {
                if (speed.Y > 0)
                {
                    speed.Y = 0.0f;
                }
            }

            // BOOST
            /*if (newState.IsKeyDown(Keys.B))
            {
                if (!oldState.IsKeyDown(Keys.B))
                {
                    oldSpeed = spriteSpeed;
                    spriteSpeed = Vector2.Multiply(spriteSpeed, 2.0f);
                }
            }
            else if (oldState.IsKeyDown(Keys.B))
            {
                if (spriteSpeed.X == 0 && spriteSpeed.Y == 0)
                {
                    spriteSpeed = Vector2.Zero;
                }
                else
                {
                    spriteSpeed = oldSpeed;
                }
            }*/

            old_state = new_state;
        }

        public void F_UpdateImage(GameTime gameTime, double delai)
        {
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
            }


        }
    }
}
