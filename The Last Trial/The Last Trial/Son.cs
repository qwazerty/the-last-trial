using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace The_Last_Trial
{
    class Son
    {
        private static SoundEffect[] mySound = new SoundEffect[5];
        private static SoundEffectInstance instance;

        public static void InitLoopSound(int i)
        {
            instance = mySound[i].CreateInstance();
            instance.IsLooped = true;
            instance.Volume = ((float)Program.volume / 100f);
        }

        public static void Load(ContentManager Content)
        {
            mySound[0] = Content.Load<SoundEffect>("sound/soundTrack/1");
            mySound[1] = Content.Load<SoundEffect>("sound/soundEffect/sword_1");
            mySound[2] = Content.Load<SoundEffect>("sound/soundEffect/fireball_1");
            mySound[3] = Content.Load<SoundEffect>("sound/soundEffect/thunder_1");
            mySound[4] = Content.Load<SoundEffect>("sound/soundEffect/monsterKill");
        }

        public static void Play(int i)
        {
            mySound[i].Play();
        }

        public static void InstancePlay()
        {
            instance.Play();
        }

        public static void InstanceVolume()
        {
            instance.Volume = ((float)Program.volume / 100f);
        }

        public static void InstanceStop()
        {
            instance.Stop();
        }
    }
}