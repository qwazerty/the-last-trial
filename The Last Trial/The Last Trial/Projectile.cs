using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace The_Last_Trial
{
    class Projectile : Objet
    {
        #region VAR
        private Monster target;
        private Personnage caster;
        private int damage, imageState;
        private double time;
        private bool willExplodeAnd_DESTROY_THE_FACE_OF_DA_MONSSSTTTTTEEEEEERRRRRRRRRRRRRRRRRR, GOOOOOOOOOO;
        private Vector2 speed;
        #endregion

        #region Constructor
        public Projectile(ContentManager Content, Monster target, Personnage caster, Vector2 position, int damage)
        {
            willExplodeAnd_DESTROY_THE_FACE_OF_DA_MONSSSTTTTTEEEEEERRRRRRRRRRRRRRRRRR = false;
            GOOOOOOOOOO = false;
            imageState = 0;
            objet = Content.Load<Texture2D>("magic/fireball" + imageState);
            this.position = new Vector2(position.X + 40, position.Y + 30);
            this.target = target;
            this.caster = caster;
            this.damage = damage;
            this.time = 0;
        }
        #endregion

        public void StartFiring()
        {
            GOOOOOOOOOO = true;
        }

        #region Update
        public bool Update(GameTime gameTime, ContentManager Content)
        {
            if (GOOOOOOOOOO)
            {
                if (time == 0)
                    time = gameTime.TotalGameTime.TotalSeconds;

                if (time + 0.01 < gameTime.TotalGameTime.TotalSeconds)
                {
                    if (willExplodeAnd_DESTROY_THE_FACE_OF_DA_MONSSSTTTTTEEEEEERRRRRRRRRRRRRRRRRR)
                    {
                        return true;
                    }
                    if (new Rectangle((int)position.X, (int)position.Y, objet.Width, objet.Height).Intersects(target.G_Interact()))
                    {
                        target.S_Degat(damage, gameTime);
                        if (target.G_Killed())
                        {
                            caster.S_Xp(target.G_MaxLife() / 4, gameTime);
                        }
                        willExplodeAnd_DESTROY_THE_FACE_OF_DA_MONSSSTTTTTEEEEEERRRRRRRRRRRRRRRRRR = true;
                        Son.Play(2);
                        imageState = 6;
                    }
                    speed = Vector2.Zero;
                    if (position.X > target.G_Rectangle().X)
                    {
                        speed.X = -8;
                    }
                    if (position.X < target.G_Rectangle().X)
                    {
                        speed.X = 8;
                    }
                    if (position.Y > target.G_Rectangle().Y)
                    {
                        speed.Y = -2;
                    }
                    if (position.Y < target.G_Rectangle().Y)
                    {
                        speed.Y = 2;
                    }
                    position.X += speed.X;
                    position.Y += speed.Y;

                    time = gameTime.TotalGameTime.TotalSeconds;
                }
                objet = Content.Load<Texture2D>("magic/fireball" + imageState);
            }
            return false;
        }
        #endregion

        public void Draw(SpriteBatch sb)
        {
            if (GOOOOOOOOOO)
            {
                SpriteEffects se = SpriteEffects.None;
                if (speed.X < 0)
                    se = SpriteEffects.FlipHorizontally;

                sb.Draw(objet, new Rectangle((int)position.X, (int)position.Y, objet.Width, objet.Height), null, Color.White, 0f, Vector2.Zero, se, 0);
            }
        }
    }
}
