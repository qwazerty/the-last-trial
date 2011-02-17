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
        protected int imgState, life, initLife;
        protected Vector2 speed;
        protected double tempsImage, tempsAttaque, tempsActuel;

        protected Mob()
        {
            imgState = 40;
            speed = new Vector2(0.0f, 0.0f);
            tempsImage = 0;
            tempsAttaque = 0;
        }

        public bool G_IsAlive() { return life > 0; }

        protected void S_Deplacement(GameTime gt)
        {
            if (life < 0)
                speed = Vector2.Zero;

            base.position += (speed - Map.G_Speed()) * (float)gt.ElapsedGameTime.TotalSeconds;
        }

        public void S_Degat(int degat)
        {
            life -= degat;
        }
    }
}
