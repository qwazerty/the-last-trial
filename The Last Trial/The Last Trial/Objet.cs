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

        // METHODE GET
        public Texture2D G_Texture() { return objet; }
        public Vector2 G_Position() { return new Vector2((int)position.X, (int)position.Y); }

        // METHODE SET
        public void S_Texture(Texture2D text) { this.objet = text; }
        public void S_Position(Vector2 pos) { this.position = pos; }        
    }
}
