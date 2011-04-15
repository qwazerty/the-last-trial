using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_Last_Trial
{
    public class Monster : Mob
    {

        // DECLARATION VARIABLES
        private double tempsRandom;
        private Personnage target;
        private Rectangle spawn;
        private double[] tempsAttaque = new double[1];

        // CONSTRUCTEUR
        public Monster(Vector2 init, int id) : base()
        {
            this.target = null;
            this.id = id;
            this.spawn = new Rectangle((int)init.X - 200, (int)init.Y - 200, 400, 400);
            this.position = init;
            this.life = 100;
            this.initLife = this.life;
            this.lifeMax = this.life;
            tempsRandom = 0;
            tempsAttaque[0] = -5;
        }

        /***********\
         * METHODE *
        \***********/

        #region GET & SET

        public bool G_Killed()
        {
            return life - degats <= 0;
        }

        public int G_MaxLife()
        {
            return lifeMax;
        }

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

        #region Load, Update & Draw

        private void F_Init(ContentManager Content)
        {
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

                F_Attaque(perso, gameTime);
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
            if (F_Collision_Objets(p.G_Rectangle()) && p.G_IsAlive())
            {
                target = p;
                speed = Vector2.Zero;
            }
        }

        #endregion

        #region IA

        private void F_FollowPlayer()
        {
            if (!target.G_IsAlive())
            {
                target = null;
                spawn = new Rectangle((int)position.X - 200 - (int)Map.G_ScreenX(), (int)position.Y - 200, 400, 400);
            }
            else
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

        #region Attaque & Magie

        private void F_Attaque(Personnage[] perso, GameTime gameTime)
        {
            newState = Keyboard.GetState();
            tempsActuel = (float)gameTime.TotalGameTime.TotalSeconds;
            if (tempsActuel > tempsAttaque[0] + 1)
            {
                oldImage = imgState;
                bool attaque = false;
                foreach (Personnage p in perso)
                {
                    if (G_Interact().Intersects(p.G_Rectangle()) && p.G_IsAlive() && !attaque)
                    {
                        attaque = true;
                        p.S_Degat(5 + random.Next(5));
                    }
                }
                if (attaque)
                {
                    Son.Play(2);
                    tempsAttaque[0] = tempsActuel;
                }
            }
        }

        #endregion

        #region Health

        private void DrawHealth(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(health, new Rectangle((int)position.X + 76, (int)position.Y + objet.Height + 15, life, 12), new Rectangle(0, 12, health.Width, 12), Color.Red);
            spriteBatch.Draw(health, new Rectangle((int)position.X + 75, (int)position.Y + objet.Height + 15, health.Width, 12), new Rectangle(0, 0, health.Width, 12), Color.White);
        }

        public void F_DrawDegats(SpriteBatch sb)
        {
            if (degats != 0)
            {
                initLife = life - degats;
                oldDegats = degats;
                degats = 0;
            }
            if (initLife < life && G_IsAlive())
            {
                sb.DrawString(gameFont, oldDegats.ToString(), new Vector2(position.X + 100, position.Y + 40), Color.Red);
                life -= 3;
            }
            else
                oldDegats = 0;
        }

        #endregion

    }
}
