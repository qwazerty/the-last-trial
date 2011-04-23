using System;
using System.IO;

namespace The_Last_Trial
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static GameState gs;
        public static int width, height;
        public static bool fullscreen;
        
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
                sw.WriteLine("musique=on");
                sw.Close();
                fs.Close();
                return;
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
            string fullscreen = sr.ReadLine();
            fullscreen = fullscreen.Substring(11);
            sr.Close();
            fs.Close();
            Program.width = int.Parse(width);
            Program.height = int.Parse(height);
            Program.fullscreen = (fullscreen == "on");
            gs = new GameState();
            gs.Run();
        }

        public static void Restart()
        {
            gs.Exit();
            System.Diagnostics.Process P = System.Diagnostics.Process.Start("The Last Trial.exe");
        }
    }
}

