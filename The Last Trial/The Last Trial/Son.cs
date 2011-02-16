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
    class Son
    {
        private static SoundEffect[] mySound = new SoundEffect[4];

        public static void Load(ContentManager Content)
        {
            //mySound[0] = Content.Load<SoundEffect>("sound/soundTrack/1");
            mySound[1] = Content.Load<SoundEffect>("sound/soundEffect/sword_1");
            mySound[2] = Content.Load<SoundEffect>("sound/soundEffect/fireball_1");
            mySound[3] = Content.Load<SoundEffect>("sound/soundEffect/thunder_1");
        }

        public static void Play(int i)
        {
            mySound[i].Play();
        }
    }
}
