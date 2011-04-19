using System;

namespace The_Last_Trial
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static GameState gs = new GameState();
        
        static void Main(string[] args)
        {
            gs.Run();
        }
    }
}

