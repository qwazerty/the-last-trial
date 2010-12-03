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
        public Texture2D objet;
        public Vector2 position;

        // CONSTRUCTEUR
        public Objet()
        {

        }

        /*****************
         * METHODE : GET *
         *****************/

        public Texture2D G_Texture()
        {
            return objet;
        }

        public Vector2 G_Position()
        {
            return position;
        }

        /*****************
         * METHODE : SET *
         *****************/

        public void S_Texture(Texture2D text)
        {
            objet = text;
        }
        
        public void S_Position(Vector2 pos)
        {
            position = pos;
        }

        
    }
}
