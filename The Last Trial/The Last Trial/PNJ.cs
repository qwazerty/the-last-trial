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
    public class PNJ : Mob
    {
        public static string message = "Sorry Mario, your princess \n is in another castle...";
        private static SpriteFont textFont;
        public static bool parler;

        public PNJ(Vector2 position, int id) : base()
        {
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
        public static void Load(PNJ pnj, ContentManager Content)
        {
            pnj.F_Load(Content);

            textFont = Content.Load<SpriteFont>("textfont");
        }

        public static void Update(PNJ pnj, Personnage[] perso, GameTime gameTime, GraphicsDeviceManager graphics, ContentManager Content)
        {
            pnj.F_Update(pnj, perso, Content, gameTime, graphics);
        }

        public static void Draw(PNJ pnj, SpriteBatch spriteBatch)
        {
            pnj.F_Draw(spriteBatch);

            if (parler)
                spriteBatch.DrawString(textFont, message, new Vector2(pnj.position.X - 170, pnj.position.Y - 60), Color.DeepPink);
        }
        #endregion

        #region Load, Update & Draw
        private void F_Load(ContentManager content)
        {
            objet = content.Load<Texture2D>("perso/" + 2 + "/" + 40);
        }

        private void F_Update(PNJ pnj, Personnage[] perso, ContentManager Content, GameTime gameTime, GraphicsDeviceManager graphics)
        {
            S_Deplacement(gameTime);
            F_Parler(perso, pnj);
        }

        private void F_Draw(SpriteBatch sb)
        {
            sb.Draw(objet, position, Color.White);
        }
        #endregion

        public bool F_Parler(Personnage[] perso, PNJ pnj)
        {
            parler = false;
            foreach (Personnage p in perso)
            {
                if (p.F_Interact(pnj))
                    parler = true;
            }
            return parler;
        }
    }
}
