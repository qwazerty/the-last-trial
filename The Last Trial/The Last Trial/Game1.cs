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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int nbPlayer = 2;
        const int nbMob = 1;

        // Declaration Objets
        Personnage[] perso = new Personnage[nbPlayer];
        Monstre[] monster = new Monstre[nbMob];
        Rectangle r_mur_haut = new Rectangle(0, 260, 979, 27);
        Rectangle r_mur_bas = new Rectangle(0, 800, 986, 128);
        Objet map1 = new Objet();
        Objet map_first = new Objet();
        Son backsound = new Son();
        KeyboardState new_state = Keyboard.GetState();
        KeyboardState old_state = Keyboard.GetState();
        
       
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

            perso[0] = new Personnage(new Keys[] {Keys.Down, Keys.Right, Keys.Up, Keys.Left, Keys.B, Keys.Space});
            perso[1] = new Personnage(new Keys[] {Keys.S,    Keys.D,     Keys.W,  Keys.A,    Keys.E, Keys.F });
            monster[0] = new Monstre(new Vector2(500f, 500f));

            LoadPlayer();
            LoadMonster();
            
            map1.S_Texture(Content.Load<Texture2D>("map/1"));
            map_first.S_Texture(Content.Load<Texture2D>("map/2"));

            map1.S_Position(new Vector2(0, 0));
            map_first.S_Position(new Vector2(0, 737));

            //backsound.Load(Content.Load<Song>("music/Kalimba"));
        }

        protected override void UnloadContent()
        {
		
        }

        protected override void Update(GameTime gameTime)
        {
            // QUITTER
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            UpdatePlayer(gameTime);
            UpdateMonster(gameTime);
            UpdateTest();           

            backsound.UpdateSon();

            base.Update(gameTime);
        }

        private void LoadPlayer()
        {
            for (int i = 0; i < nbPlayer; i++)
            {
                perso[i].F_Load(Content);
            }
        }

        private void LoadMonster()
        {
            for (int i = 0; i < nbMob; i++)
            {
                monster[i].F_Load(Content);
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            for (int i = 0; i < nbPlayer; i++)
            {
                perso[i].F_Deplacer(Keyboard.GetState());
                perso[i].F_UpdateImage(gameTime, 0.1);
                perso[i].S_Deplacement((float)gameTime.ElapsedGameTime.TotalSeconds);
                perso[i].F_CollisionEcran(graphics);
                perso[i].F_Collision_Objets(r_mur_haut);
                perso[i].F_Collision_Objets(r_mur_bas);
                perso[i].F_Load(Content);
            }
        }

        private void UpdateMonster(GameTime gameTime)
        {
            // MONSTER
            for (int i = 0; i < nbMob; i++)
            {
                // PLAYER
                for (int j = 0; j < nbPlayer; j++)
                {
                    monster[i].F_Collision_Joueur(perso[j].G_Rectangle());
                    monster[i].S_Texture(Content.Load<Texture2D>("mob/1/" + monster[i].F_UpdateState(perso[j])));
                    if (monster[i].G_Life())
                        perso[j].F_Collision_Objets(monster[i].G_Rect());
                }

                monster[i].F_Deplacer(gameTime);
                monster[i].S_Deplacement((float)gameTime.ElapsedGameTime.TotalSeconds);
                monster[i].F_CollisionEcran(graphics);
            }
        }

        private void UpdateTest()
        {
            //RESU LE MONSTRE
            new_state = Keyboard.GetState();
            if (new_state.IsKeyDown(Keys.U))
            {
                if (!old_state.IsKeyDown(Keys.U))
                {
                    monster[0].S_Resu();
                }
            }
            old_state = new_state;
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);

            // DESSINE LES SPRITES
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(map1.G_Texture(), map1.G_Position(), Color.White);

            for (int i = 0; i < nbMob; i++)
                monster[i].F_Draw(spriteBatch);

            for (int i = 0; i < nbPlayer; i++)
                perso[i].F_Draw(spriteBatch);

            spriteBatch.Draw(map_first.G_Texture(), map_first.G_Position(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
