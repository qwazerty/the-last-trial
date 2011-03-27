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
    public class Monster : Mob
    {

        // DECLARATION VARIABLES
        private static Texture2D health;
        private int rand;
        private double tempsRandom;
        private static Random random = new Random();
        private Personnage target = null;
        private Rectangle spawn;

        // CONSTRUCTEUR
        public Monster(Vector2 init, int id) : base()
        {
            this.id = id;
            this.spawn = new Rectangle((int)init.X - 200, (int)init.Y - 200, 400, 400);
            this.position = init;
            this.life = 100;
            this.initLife = this.life;
            tempsRandom = 0;
        }

        /*****************\
         * METHODE : GET *
        \*****************/

        #region GET & SET

        public Rectangle G_Rectangle()
        {
            if (id == 1)
            {
                return new Rectangle((int)position.X + 115, (int)position.Y + 165, 20, 20);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle G_Interact()
        {
            if (id == 1)
            {
                return new Rectangle((int)position.X + 70, (int)position.Y + 125, 100, 100);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        public Rectangle G_Aggro()
        {
            return new Rectangle((int)position.X - 100, (int)position.Y + 100, objet.Width + 200, objet.Height + 270);
        }

        public Rectangle G_Random()
        {
            return new Rectangle((int)position.X - 100 - (int)Map.G_ScreenX(), (int)position.Y + 100, objet.Width + 200, objet.Height + 270);
        }

        public void S_Resu()
        {
            life = 100;
            initLife = 100;
            imgState = 40;
        }

        #endregion

        /*****************\
         *   STATIC FUN   *
        \*****************/

        #region Static Load & Update

        public static void Load(Monster[] monster, ContentManager Content)
        {
            foreach (Monster m in monster)
                m.F_Init(Content);
        }

        public static void Update(Monster[] monster, GameTime gameTime, Personnage[] perso, ContentManager Content)
        {
            foreach (Monster m in monster)
                m.F_Update(gameTime, perso, Content);
        }

        public static void Resu(Monster[] monster)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.U))
            {
                foreach (Monster m in monster)
                {
                    m.S_Resu();
                }
            }
        }

        #endregion

        /*****************\
         * METHODE : FUN *
        \*****************/

        #region Load, Update & Draw

        private void F_Init(ContentManager Content)
        {
            health = Content.Load<Texture2D>("mob/health");
            F_Load(Content);
        }

        private void F_Load(ContentManager Content)
        {
            base.objet = Content.Load<Texture2D>("mob/1/" + imgState);
        }

        private void F_Update(GameTime gameTime, Personnage[] perso, ContentManager content)
        {
            if (G_IsAlive())
            {
                if (target == null)
                {
                    target = F_DetectPlayer(perso);
                    F_RandomSpeed(gameTime);
                }
                else
                {
                    F_FollowPlayer();
                }

                foreach (Personnage p in perso)
                {
                    F_Collision_Joueur(p);
                }
            }
            F_UpdateImage(gameTime);
            F_Load(content);
            S_Deplacement(gameTime);
        }

        public void F_Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(objet, new Vector2((int)position.X, (int)position.Y), Color.White);
            if (G_IsAlive())
                DrawHealth(spriteBatch);
        }

        #endregion

        #region Collision

        private bool F_Collision_Objets(Rectangle item)
        {
             return item.Intersects(G_Interact());
        }

        private void F_Collision_Joueur(Personnage p)
        {
            if (F_Collision_Objets(p.G_Rectangle()))
            {
                target = p;
                speed = Vector2.Zero;
            }
        }

        #endregion

        #region IA

        private void F_FollowPlayer()
        {
            if (target.G_Position().X + 10 < position.X + 130)
                speed.X = -50f;
            else if (target.G_Position().X - 10 > position.X + 130)
                speed.X = 50f;

            if (target.G_Position().Y + 10 < position.Y + 79)
                speed.Y = -50f;
            else if (target.G_Position().Y - 10 > position.Y + 79)
                speed.Y = 50f;

        }

        private Personnage F_DetectPlayer(Personnage[] perso)
        {
            Personnage p_ = null;
            foreach (Personnage p in perso)
            {
                if (p.G_Rectangle().Intersects(G_Aggro()))
                    p_ = p;
            }
            return p_;
        }

        private void F_RandomSpeed(GameTime gameTime)
        {
            double temps = gameTime.TotalGameTime.TotalSeconds;

            if (tempsRandom < temps - 0.5)
            {
                rand = random.Next(1, 6);
                // DROITE
                if (rand == 2)
                    speed = new Vector2(30f, 0f);

                // GAUCHE
                if (rand == 4)
                    speed = new Vector2(-30f, 0f);

                // HAUT
                if (rand == 3)
                    speed = new Vector2(0f, -30f);

                // BAS
                if (rand == 1)
                    speed = new Vector2(0f, 30f);

                // STOP
                if (rand == 5)
                    speed = Vector2.Zero;

                tempsRandom = temps;
            }
            else
            {
                if (!spawn.Intersects(G_Random()))
                    speed *= -1;
            }

        }

        #endregion

        private void DrawHealth(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(health, new Rectangle((int)position.X + 76, (int)position.Y + objet.Height + 15, life, 12), new Rectangle(0, 12, health.Width, 12), Color.Red);
            spriteBatch.Draw(health, new Rectangle((int)position.X + 75, (int)position.Y + objet.Height + 15, health.Width, 12), new Rectangle(0, 0, health.Width, 12), Color.White);
        }
    }
}
