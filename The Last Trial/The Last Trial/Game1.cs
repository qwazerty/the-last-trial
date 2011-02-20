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
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static int nbPlayer;
        private static int nbMonster;
        private static bool play;

        // Declaration Objets
        private Personnage[] perso;
        private Monster[] monster;
        private PNJ pnj;
        private Menu menu;

        public static int G_Player() { return nbPlayer; }
        public static int G_Monster() { return nbMonster; }
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
            play = false;
        }

        protected override void Initialize()
        {
            menu = new Menu(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (play)
                Menu.Update(monster, perso, pnj, graphics, gameTime, Content, nbPlayer, this);
            else
            {
                nbPlayer = Menu.Init(perso);
                play = nbPlayer != 0;
                if (play)
                    Load();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Menu.Draw(menu, perso, monster, pnj, spriteBatch, graphics, play);
            base.Draw(gameTime);
        }

        private void Load()
        {
            nbMonster = Map.Init(1, monster);
            perso = new Personnage[nbPlayer];
            monster = new Monster[nbMonster];
            monster = Map.LoadMonster(monster); 
            pnj = new PNJ(new Vector2(3900, 500), 42);
            Menu.Load(menu, perso, monster, pnj, Content, GraphicsDevice, nbPlayer);
        }
    }
}
