using System;
using System.IO;

namespace The_Last_Trial
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public const int MAXLOCAL = 25;
        public static GameState gs;
        public static int width, height, volume;
        public static bool fullscreen, musique;
        public static string local, save;
        
        static void Main(string[] args)
        {
            FileStream fs;
            StreamReader sr;
            try
            {
                fs = new FileStream("setup", FileMode.Open);
                sr = new StreamReader(fs);
            }
            catch (FileNotFoundException)
            {
                fs = new FileStream("setup", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("resolution=1200x800");
                sw.WriteLine("fullscreen=off");
                sw.WriteLine("musique=on");
                sw.WriteLine("volume=100");
                sw.WriteLine("local=fr");
                sw.Close();
                fs.Close();
                fs = new FileStream("setup", FileMode.Open);
                sr = new StreamReader(fs);
            } 
            string str = sr.ReadLine();
            str = str.Substring(11);
            int i = 0;
            string width = "";
            string height = "";
            while (str[i] != 'x')
            {
                width += str[i];
                i++;
            }
            i++;
            while (i < str.Length)
            {
                height += str[i];
                i++;
            } 
            str = sr.ReadLine();
            str = str.Substring(11);
            Program.fullscreen = (str == "on");
            str = sr.ReadLine();
            str = str.Substring(8);
            Program.musique = (str == "on");
            str = sr.ReadLine();
            str = str.Substring(7);
            volume = int.Parse(str);
            str = sr.ReadLine();
            str = str.Substring(6);
            Program.local = str;
            sr.Close();
            fs.Close();
            Program.width = int.Parse(width);
            Program.height = int.Parse(height);
            Program.save = "partie1";
            gs = new GameState(); 
            gs.Run();
        }
    }
}

