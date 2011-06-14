using System;
using System.IO;

namespace The_Last_Trial
{
    class SaveLoad
    {
        public static int Loading()
        {
            FileStream fs = new FileStream("Save/" + Program.save + ".save", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadLine();
            str = str.Substring(9);
            GameState.MaxLevel = int.Parse(str);
            str = sr.ReadLine();
            str = str.Substring(12);

            sr.Close();
            fs.Close();

            return int.Parse(str);
        }

        public static void Save(Personnage[] p)
        {
            FileStream fs = new FileStream("Save/" + Program.save + ".save", FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("levelMap=" + GameState.MaxLevel);
            sw.WriteLine("nombrePerso=" + GameState.Player);
                
            foreach (Personnage perso in p)
            {
                sw.WriteLine("perso=" + perso.G_Nom());
                sw.WriteLine("classe=" + perso.G_Class());
                sw.WriteLine("xp=" + perso.G_Xp().ToString());
            }

            sw.Close();
            fs.Close();
        }

        public static void NewGame(int x)
        {
            FileStream fs = new FileStream("Save/" + Program.save + ".save", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("levelMap=1");
            sw.WriteLine("nombrePerso=" + x);

            for (int i = 0; i < x; i++)
            {
                sw.WriteLine("perso=Perso" + (i + 1));
                sw.WriteLine("classe=" + LoadingMenu.PersoClasse[i]);
                sw.WriteLine("xp=0");
            }

            sw.Close();
            fs.Close();
        }

        public static void LoadPerso(Personnage[] perso) 
        {
            FileStream fs = new FileStream("Save/" + Program.save + ".save", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            
            sr.ReadLine();
            sr.ReadLine();

            foreach (Personnage p in perso)
            {
                string str = sr.ReadLine();
                str = str.Substring(6);
                p.S_Nom(str);
                sr.ReadLine();
                str = sr.ReadLine();
                str = str.Substring(3);
                p.S_Xp(int.Parse(str));
            }

            sr.Close();
            fs.Close();
        }

        public static void LoadClass(ref int[] persoClass, Personnage[] perso)
        {
            FileStream fs = new FileStream("Save/" + Program.save + ".save", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            sr.ReadLine();
            sr.ReadLine();

            persoClass = new int[GameState.Player];

            int i = 0;
            foreach (Personnage p in perso)
            {
                sr.ReadLine();
                string str = sr.ReadLine();
                str = str.Substring(7);
                persoClass[i] = int.Parse(str);
                sr.ReadLine();
                i++;
            }

            sr.Close();
            fs.Close();
        }

    }
}
