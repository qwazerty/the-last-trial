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
        private int rand;
        private double tempsRandom;
        private static Random random = new Random();
        private Personnage target = null;
        private Rectangle spawn;

        // CONSTRUCTEUR
        public Monster(Vector2 init) : base()
        {
            spawn = new Rectangle((int)init.X - 50, (int)init.Y - 50, 100, 100);
            position = init;
            tempsRandom = 0;
            life = 100;
        }

        public void S_Resu()
        {
            life = 30;
            imgState = 40;
            position.X = spawn.X;
            position.Y = spawn.Y;
            target = null;
        }

        // STATIC

        public static void Load(Monster[] monster, ContentManager Content)
        {

            monster[0] = new Monster(new Vector2(500f, 500f));
            monster[1] = new Monster(new Vector2(700f, 500f));

            foreach (Monster m in monster)
                m.F_Load(Content);
        }

        public static void Update(Monster[] monster, GameTime gameTime, Personnage[] perso, ContentManager Content)
        {
            foreach (Monster m in monster)
            {
                m.F_Update(gameTime, perso, Content);
            }
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

        /**********************\
         * METHODE : FONCTION *
        \(**********************/

        public void F_Load(ContentManager content)
        {
            base.objet = content.Load<Texture2D>("mob/1/" + imgState);
        }

        public void F_Update(GameTime gameTime, Personnage[] perso, ContentManager content)
        {
            F_IA(gameTime, perso);
            F_Load(content);
            F_Collision_Joueur(perso);
            S_Deplacement(gameTime);
            F_UpdateState(perso);
        }

        public void F_Draw(SpriteBatch sb)
        {
            sb.Draw(base.objet, base.position, Color.White);
        }

        public bool F_Collision_Objets(Rectangle item)
        {
             return item.Intersects(new Rectangle((int)position.X, (int)position.Y + (objet.Height) / 2, objet.Width, objet.Height * 2 / 3));
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

        public void F_IA(GameTime gameTime, Personnage[] perso)
        {
            if (life > 0)
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

                //==\\
                foreach (Personnage p in perso)
                {
                    p.F_Collision_Objets(new Rectangle((int)position.X + (base.objet.Width) / 2,
                         (int)position.Y + (objet.Height) * 2 / 3, 6, base.objet.Height / 3), gameTime);
                }
            }
        }

        private void F_FollowPlayer()
        {
            if (target.G_Position().X < position.X)
                speed.X = -30f;
            else if (target.G_Position().X > position.X)
                speed.X = 30f;

            if (target.G_Position().Y < position.Y)
                speed.Y = -30f;
            else if (target.G_Position().Y > position.Y)
                speed.Y = 30f;

        }

        private Personnage F_DetectPlayer(Personnage[] perso)
        {
            Personnage p_ = null;
            foreach (Personnage p in perso)
            {
                if (p.G_Rectangle().Intersects(new Rectangle((int)position.X - 200, (int)position.Y - 200, 400 + objet.Width, 400 + objet.Height)))
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
                Console.WriteLine(rand);
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
                if (!spawn.Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1)))
                    speed *= -1;
            }

            
        }

        public void F_Collision_Joueur(Personnage[] perso)
        {
            foreach (Personnage p in perso)
            {
                if (F_Collision_Objets(p.G_Rectangle()))
                {
                    target = p;
                    speed = Vector2.Zero;
                }
            }
        }

        public void F_UpdateState(Personnage[] perso)
        {
            foreach (Personnage p in perso)
            {
                if (F_Collision_Objets(p.G_Rectangle()))
                {
                    if (p.F_Attaque(Keyboard.GetState()))
                    {
                        Son.Play(1);
                        if (!F_IsAlive(0))
                            imgState = 3;

                        else if (!F_IsAlive(50))
                            imgState = 2;
                    }
                }
            }
        }

        public void F_UpdateImage(GameTime gameTime, double delai)
        {
            
        }
    }
}
