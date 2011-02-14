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
        private const int nbMob = 1;

        // Declaration Objets
        private Personnage[] perso = new Personnage[nbPlayer];
        private Monstre[] monster = new Monstre[nbMob];
        private Map map = new Map(1);
        private Son backsound = new Son();

        // TEMP
        private Rectangle[] mur = new Rectangle[2];
        private Objet[] carte = new Objet[2];
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 900;
            this.graphics.PreferredBackBufferHeight = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadPlayer();
            LoadMonster();
            LoadMap();
            LoadSound();
        }

        protected override void Update(GameTime gameTime)
        {
            // QUITTER
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            Monstre.Update(monster, gameTime, perso, graphics, Content);
            Personnage.Update(perso, gameTime, monster, graphics, Content, mur);
            UpdateTest();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(carte[0].G_Texture(), carte[0].G_Position(), Color.White);

            foreach (Monstre m in monster)
                m.F_Draw(spriteBatch);

            foreach (Personnage p in perso)
                p.F_Draw(spriteBatch);

            spriteBatch.Draw(carte[1].G_Texture(), carte[1].G_Position(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadPlayer()
        {
            perso[0] = new Personnage(new Keys[] { Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.B, Keys.Space });
            perso[1] = new Personnage(new Keys[] { Keys.S, Keys.D, Keys.W, Keys.A, Keys.E, Keys.F });

            foreach (Personnage p in perso)
                p.F_Load(Content);
        }

        private void LoadMonster()
        {
            monster[0] = new Monstre(new Vector2(500f, 500f));

            foreach (Monstre m in monster)
                m.F_Load(Content);
        }

        private void LoadMap()
        {
            carte[0] = new Objet();
            carte[1] = new Objet();

            mur[0] = new Rectangle(0, 260, 979, 27);
            mur[1] = new Rectangle(0, 800, 986, 128);

            carte[0].S_Texture(Content.Load<Texture2D>("map/1"));
            carte[1].S_Texture(Content.Load<Texture2D>("map/2"));

            carte[0].S_Position(new Vector2(0, 0));
            carte[1].S_Position(new Vector2(0, 737));
            
        }

        private void LoadSound()
        {
            //backsound.Load(Content.Load<Song>("music/Kalimba"));
        }

        private void UpdateTest()
        {
            Monstre.Resu(monster);
        }
    }
}
