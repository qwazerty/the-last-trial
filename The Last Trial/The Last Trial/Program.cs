using System;

namespace The_Last_Trial
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameState gs = new GameState())
            {
                gs.Run();
            }
        }
    }
}

