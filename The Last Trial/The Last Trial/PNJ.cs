using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace The_Last_Trial
{
    public class PNJ : Mob
    {
        private string message;
        private bool parler;

        public PNJ(Vector2 position, int id, string message) : base()
        {
            this.message = message;
            this.id = id;
            this.position = position;
            this.imgState = imgState;
        }

        #region GET & SET
        public Rectangle G_Interact()
        {
            if (id == 42)
            {
                return new Rectangle((int)position.X - 150, (int)position.Y - 500, 250, 1000);
            }

            return new Rectangle(0, 0, 0, 0);
        }

        #endregion

        #region Static Load, Update & Draw

        public static void Load(PNJ[] pnj, ContentManager Content)
        {
            foreach (PNJ p in pnj)
            {
                p.F_Load(Content);
            }
        }

        public static void Update(PNJ[] pnj, Personnage[] perso, GameTime gameTime, GraphicsDeviceManager graphics, ContentManager Content)
        {
            foreach (PNJ p in pnj)
            {
                p.F_Update(perso, gameTime);
            }
        }

        public static void Draw(PNJ[] pnj, SpriteBatch spriteBatch)
        {
            foreach (PNJ p in pnj)
            {
                p.F_Draw(spriteBatch);
            }

        }

        #endregion

        #region Load, Update & Draw

        private void F_Load(ContentManager content)
        {
            objet = content.Load<Texture2D>("pnj/1");
        }

        private void F_Update(Personnage[] perso, GameTime gameTime)
        {
            S_Deplacement(gameTime);
            F_Parler(perso);
        }

        private void F_Draw(SpriteBatch sb)
        {
            sb.Draw(objet, position, Color.White);
            if (parler)
            {
                sb.DrawString(textFont, message, new Vector2(position.X - 170, position.Y - 65), Color.DeepPink);
            }
        }

        #endregion

        private void F_Parler(Personnage[] perso)
        {
            parler = false;
            foreach (Personnage p in perso)
            {
                if (p.G_Interact(this))
                {
                    parler = true;
                }
            }
        }
    }
}
