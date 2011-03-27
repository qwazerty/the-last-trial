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
    public abstract class Mob : Objet
    {
        protected int id;
        protected int imgState, life, initLife, oldDegats;
        protected Vector2 speed;
        protected double tempsImage, tempsActuel;
        protected double[] tempsAttaque = new double[2];
        protected int degats = 0;
        private static SpriteFont gameFont;
        private static int[] array = new int[Game1.G_Player() + Game1.G_Monster()];

        protected Mob()
        {
            imgState = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsImage = 0;
            for (int i = 0; i <= 1; i++)
            {
                tempsAttaque[i] = -5;
            }
        }

        public static void Load(ContentManager Content)
        {
            gameFont = Content.Load<SpriteFont>("gamefont");
        }

        public static void Draw(Personnage[] perso, Monster[] monster, PNJ pnj, SpriteBatch sb)
        {
            Vector2[] sort = new Vector2[Game1.G_Player() + Game1.G_Monster()];
            for (int k = 0; k < Game1.G_Player(); k++)
            {
                sort[k].X = k;
                sort[k].Y = perso[k].position.Y + 65;
                if (! perso[k].G_IsAlive())
                    sort[k].Y -= 4242;
            }
            for (int k = 0; k < Game1.G_Monster(); k++)
            {
                sort[k + Game1.G_Player()].X = k + Game1.G_Player();
                sort[k + Game1.G_Player()].Y = monster[k].position.Y + 140;
                if (!monster[k].G_IsAlive())
                    sort[k + Game1.G_Player()].Y -= 4242;
            }

            for (int i = 0; i < Game1.G_Player() + Game1.G_Monster(); i++)
            {
                int min = -1;
                for (int k = 0; k < Game1.G_Player() + Game1.G_Monster(); k++)
                {
                    if ((sort[k].X != -1) && (min == -1 || sort[k].Y < sort[min].Y))
                    {
                        min = k;
                    }
                }
                array[i] = (int)sort[min].X;
                sort[min].X = -1;
            }


            foreach (int i in array)
            {
                if (i < Game1.G_Player())
                    perso[i].F_Draw(sb);
                else
                    monster[i - Game1.G_Player()].F_Draw(sb);
            }

            PNJ.Draw(pnj, sb);

            foreach (Monster m in monster)
            {
                m.F_DrawDegats(m, sb);
            }
        }

        private void F_DrawDegats(Monster m, SpriteBatch sb)
        {
            if (degats != 0)
            {
                initLife = life - degats;
                oldDegats = degats;
                degats = 0;
            }
            if (initLife < life && G_IsAlive())
            {
                sb.DrawString(gameFont, Convert.ToString(oldDegats), new Vector2(m.position.X + 100, m.position.Y + 40), Color.Red);
                life -= 3;
            }
            else
                oldDegats = 0;
        }

        public bool G_IsAlive() { return life > 0; }

        protected void S_Deplacement(GameTime gt)
        {
            if (life <= 0)
                speed = Vector2.Zero;

            base.position += (speed - Map.G_Speed()) * (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public void S_Degat(int degat)
        {
            degats = degat;
        }

        protected void F_UpdateImage(GameTime gameTime)
        {
            if (life <= 0)
            {
                imgState = 0;
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
    }
}
