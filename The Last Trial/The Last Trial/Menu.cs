﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    class Menu : Objet
    {
        private bool pause;

        public bool G_Pause() { return pause; }
        public void S_Pause(bool p) { pause = p; }

        public void Load(ContentManager Content)
        {
            //objet = Content.Load<Texture2D>("menu/1/");
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(base.objet, base.position, Color.White);
        }
    }
}
