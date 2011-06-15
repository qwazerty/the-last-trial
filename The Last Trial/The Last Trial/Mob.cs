using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Last_Trial
{
    public abstract class Mob : Objet
    {
        #region VAR
        protected int id, imgState, life, lifeMax, oldDegats;
        protected Vector2 speed;
        protected double tempsImage, tempsActuel, tempsDegats;
        protected int degats = 0;
        private static int[] array;

        protected int rand;
        protected static Texture2D health, ui1, ui2;
        protected KeyboardState newState, oldState;
        protected int oldImage;
        public static Random random = new Random();
        #endregion

        protected Mob()
        {
            imgState = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsImage = 0;
        }

        public bool G_IsAlive() { return life > 0; }

        public static void Load(ContentManager Content)
        {
            ui1 = Content.Load<Texture2D>("ui/L");
            ui2 = Content.Load<Texture2D>("ui/R");
            health = Content.Load<Texture2D>("mob/health");
            array = new int[GameState.Player + GameState.Monster];
        }

        public static void Draw(Personnage[] perso, Monster[] monster, PNJ[] pnj, SpriteBatch sb, GameTime gameTime)
        {
            Vector2[] sort = new Vector2[GameState.Player + GameState.Monster];
            for (int k = 0; k < GameState.Player; k++)
            {
                sort[k].X = k;
                sort[k].Y = perso[k].position.Y + 65;
                if (! perso[k].G_IsAlive())
                    sort[k].Y -= 4242;
            }
            for (int k = 0; k < GameState.Monster; k++)
            {
                sort[k + GameState.Player].X = k + GameState.Player;
                sort[k + GameState.Player].Y = monster[k].position.Y + 155;
                if (!monster[k].G_IsAlive())
                    sort[k + GameState.Player].Y -= 4242;
            }

            for (int i = 0; i < GameState.Player + GameState.Monster; i++)
            {
                int min = -1;
                for (int k = 0; k < GameState.Player + GameState.Monster; k++)
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
                if (i < GameState.Player)
                    perso[i].F_Draw(sb, gameTime);
                else
                    monster[i - GameState.Player].F_Draw(sb);
            }

            PNJ.Draw(pnj, sb);

            foreach (Monster m in monster)
            {
                m.F_DrawDegats(sb, gameTime);
            }

            foreach (Personnage p in perso)
            {
                p.F_DrawDegats(sb, gameTime);
            }
        }

        protected void S_Deplacement(GameTime gt)
        {
            if (life <= 0)
                speed = Vector2.Zero;

            base.position += (speed - Map.G_Speed()) * (float)gt.ElapsedGameTime.TotalSeconds;
        }

    }
}
