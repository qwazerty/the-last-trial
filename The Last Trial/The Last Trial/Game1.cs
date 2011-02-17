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
        private int nbPlayer;
        private int nbMob;

        // Declaration Objets
        private Personnage[] perso;
        private Monster[] monster;
        private Menu menu = new Menu();

        public int G_Player() { return nbPlayer; }
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            nbPlayer = Menu.Init(perso);
            nbMob = Map.Init(1, monster);
            perso = new Personnage[nbPlayer];
            monster = new Monster[nbMob];
            monster = Map.LoadMonster(monster);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.Load(menu, perso, monster, Content, GraphicsDevice, nbPlayer);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Menu.Update(monster, perso, graphics, gameTime, Content);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Menu.Draw(menu, perso, monster, spriteBatch, graphics);
            base.Draw(gameTime);
        }
    }
}
