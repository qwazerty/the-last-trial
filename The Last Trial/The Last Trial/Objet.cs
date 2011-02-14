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

        // CONSTRUCTEUR
        public Objet() { }

        // METHODE GET
        public Texture2D G_Texture() { return objet; }
        public Vector2 G_Position() { return position; }

        // METHODE SET
        public void S_Texture(Texture2D text) { this.objet = text; }
        public void S_Position(Vector2 pos) { this.position = pos; }        
    }

    public abstract class Mob : Objet
    {
        protected int imgState, life;
        protected Vector2 speed;
        protected double tempsImage, tempsAttaque;

        protected Mob()
        {
            imgState = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsImage = 0;
            tempsAttaque = 0;
            /*collision = new bool[taille];
            for (int i = 0; i < taille; i++ )
                collision[i] = false;*/
        }

        protected void S_Deplacement(GameTime gt)
        {
            if (life > 0 && speed != Vector2.Zero)
                base.position += speed * (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
