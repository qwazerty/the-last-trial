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
        private Vector2 oldspeed;
        private KeyboardState oldState;
        private Keys[] key;
        /** KEYS STATES **\
         * 0 : BAS       *
         * 1 : DROITE    *
         * 2 : HAUT      *
         * 3 : GAUCHE    *
         * 4 : BOOST     *
         * 5 : ATTAQUE   *
        \*****************/

        // CONSTRUCTEUR
        public Personnage(Keys[] keys, Vector2 position) : base()
        {
            key = keys;
            this.position = position;
            imgState = 40;
            life = 100;
            oldState = Keyboard.GetState();
        }

        /*****************\
         * METHODE : GET *
        \*****************/

        public Rectangle G_Rectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y + (objet.Height * 2) / 3, objet.Width, objet.Height / 3);
        }

        /**********************\
         * METHODE : FONCTION *
        \**********************/

        public static void Load(Personnage[] perso, ContentManager Content)
        {
            perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.B, Keys.Space }, new Vector2(300f, 500f));
            perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.W, Keys.A, Keys.E, Keys.F }, new Vector2(300f, 500f));

            foreach (Personnage p in perso)
                p.F_Load(Content);
        }

        public static void Update(Personnage[] perso, GameTime gameTime, Monster[] monster, GraphicsDeviceManager graphics, ContentManager content)
        {
            foreach (Personnage p in perso)
            {
                foreach (Rectangle rect in Map.G_Collision())
                    p.F_Collision_Objets(rect, gameTime);

                p.F_Collision_Ecran(graphics, gameTime);
                p.F_Update(content, gameTime, graphics);
            }
        }

        public static void Draw(Personnage[] perso, SpriteBatch spriteBatch)
        {
            foreach (Personnage p in perso)
                p.F_Draw(spriteBatch);
        }

        private void F_Load(ContentManager content)
        {
            objet = content.Load<Texture2D>("perso/1/" + imgState);
        }

        private void F_Update(ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            F_Deplacer(Keyboard.GetState());
            F_UpdateImage(gameTime, 0.1);
            S_Deplacement(gameTime);
            F_Load(Content);
        }

        private void F_Draw(SpriteBatch sb)
        {
            sb.Draw(base.objet, base.position, Color.White);
        }

        private void F_Collision_Ecran(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            int MaxX = graphics.GraphicsDevice.Viewport.Width;

            if (position.X > MaxX * 0.77)
            {
                position.X -= (speed.X - Map.G_Speed().X) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                speed.X = 0;
            }

            else if (position.X < MaxX * 0.20)
            {
                position.X -= (speed.X - Map.G_Speed().X) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                speed.X = 0;
            }
        }

        public void F_Collision_Objets(Rectangle rect, GameTime gameTime)
        {
            Rectangle persoRectangle = new Rectangle((int)position.X, (int)position.Y + (objet.Height * 2) / 3, objet.Width, objet.Height / 3);

            if (persoRectangle.Intersects(rect))
            {
                base.position -= (speed - Map.G_Speed()) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                speed = Vector2.Zero;
            }
        }

        private void F_Deplacer(KeyboardState newState)
        {
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

            // BOOST
            if (newState.IsKeyDown(key[4]))
            {
                if (!oldState.IsKeyDown(key[4]))
                {
                    oldspeed = speed;
                    speed = Vector2.Multiply(speed, 2.0f);
                }
            }
            else if (oldState.IsKeyDown(key[4]))
            {
                if (speed.X == 0 && speed.Y == 0)
                {
                    speed = Vector2.Zero;
                }
                else
                {
                    speed = oldspeed;
                }
            }

            oldState = newState;
        }

        public bool F_Attaque(KeyboardState newState)
        {
            return newState.IsKeyDown(key[5]);
        }

        private void F_UpdateImage(GameTime gameTime, double delai)
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

                if (tempsImage < temps - delai)
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
