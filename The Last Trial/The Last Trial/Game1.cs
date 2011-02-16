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
        private const int nbPlayer = 2;
        private const int nbMob = 2;

        // Declaration Objets
        private Personnage[] perso = new Personnage[nbPlayer];
        private Monster[] monster = new Monster[nbMob];
        private Son backsound = new Son();
        private Menu menu = new Menu();

        // TEMP
        private Rectangle[] mur = new Rectangle[2];
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Map.Init(1);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Personnage.Load(perso, Content);
            Monster.Load(monster, Content);
            LoadMap();
            Map.Load(GraphicsDevice, Content);
            Son.Load(Content);
            menu.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            // QUITTER
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (!menu.G_Pause())
            {
                Monster.Update(monster, gameTime, perso, Content);
                Personnage.Update(perso, gameTime, monster, graphics, Content, mur);
                UpdateTest();
                Map.Update((float)gameTime.ElapsedGameTime.TotalSeconds, perso, Content); 
                menu.S_Pause(Keyboard.GetState().IsKeyDown(Keys.Escape));
            }
            else
            {
                menu.S_Pause(Keyboard.GetState().IsKeyDown(Keys.Escape));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            Map.DrawBack(spriteBatch);
            Map.DrawMiddle(spriteBatch);
            foreach (Monster m in monster)
                m.F_Draw(spriteBatch);

            foreach (Personnage p in perso)
                p.F_Draw(spriteBatch);
            menu.Draw(spriteBatch);
            Map.DrawFirst(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadMap()
        {
        }

        private void UpdateTest()
        {
            Monster.Resu(monster);
        }
    }
}
