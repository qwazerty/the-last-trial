using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Last_Trial
{
    public class Objet
    {
        // DECLARATION VARIABLES
        protected Texture2D objet;
        protected Vector2 position;

        public Objet() { }

        public Objet(Vector2 position)
        {
            this.position = position;
        }

        public Objet(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.objet = texture;
        }

        // METHODE GET
        public Texture2D G_Texture() { return objet; }
        public Vector2 G_Position() { return new Vector2((int)position.X, (int)position.Y); }

        // METHODE SET
        public void S_Texture(Texture2D text) { this.objet = text; }
        public void S_Position(Vector2 pos) { this.position = pos; }
        public void S_PosX(float x) { this.position.X = x; }
        public void S_PosY(float y) { this.position.Y = y; }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(objet, new Vector2((int)position.X, (int)position.Y), Color.White);
        }
    }
}
