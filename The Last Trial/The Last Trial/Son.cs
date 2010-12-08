﻿using System;
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
    class Son
    {
        Song back_sound;
        bool songstart = false;

        public Son()
        {
             MediaPlayer.IsRepeating = true;
        }

        public void UpdateSon()
        {
            // MUSIQUE DE FOND
            if (!songstart)
            {
                MediaPlayer.Play(back_sound);
                songstart = true;
            }
        }

        public void Load(Song content)
        {
            back_sound = content;
        }
    }
}
