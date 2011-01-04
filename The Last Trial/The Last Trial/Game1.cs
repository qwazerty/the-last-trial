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

        // Declaration Objets
        Personnage perso1 = new Personnage();
        Personnage perso2 = new Personnage();
        Monstre monster = new Monstre(new Vector2(500.0f, 500.0f));
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
            perso1.S_Texture(Content.Load<Texture2D>("perso/1/" + (perso1.G_ImgState())));
            perso2.S_Texture(Content.Load<Texture2D>("perso/1/" + (perso2.G_ImgState())));
            monster.S_Texture(Content.Load<Texture2D>("mob/1/40"));
            map1.S_Texture(Content.Load<Texture2D>("map/1"));
            map_first.S_Texture(Content.Load<Texture2D>("map/2"));

            //backsound.Load(Content.Load<Song>("music/Kalimba"));

            map1.S_Position(new Vector2(0, 0));
            map_first.S_Position(new Vector2(0, 737));
            monster.S_Position(new Vector2(500, 500));
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

            // DEPLACEMENT
            perso1.F_Deplacer(
                Keys.Down, Keys.Right, 
                Keys.Up, Keys.Left,
                Keys.B,
                Keyboard.GetState());
            perso2.F_Deplacer(
                Keys.S, Keys.D,
                Keys.W, Keys.A,
                Keys.E,
                Keyboard.GetState());
            monster.F_Deplacer(gameTime);
            monster.F_Collision_Joueur(perso1.G_Rectangle());
            monster.F_Collision_Joueur(perso2.G_Rectangle());

            //RESU LE MONSTRE
            new_state = Keyboard.GetState();
            if (new_state.IsKeyDown(Keys.U))
            {
                if (!old_state.IsKeyDown(Keys.U))
                {
                    monster.S_Resu();
                    monster.S_Texture(Content.Load<Texture2D>("mob/1/40"));
                }
            }
            old_state = new_state;

            // MODIFIE LES SPRITES
            perso1.F_UpdateImage(gameTime, 0.1);
            perso2.F_UpdateImage(gameTime, 0.1);

            // DEPLACEMENT DU PERSO
            perso1.S_Deplacement((float)gameTime.ElapsedGameTime.TotalSeconds);
            perso2.S_Deplacement((float)gameTime.ElapsedGameTime.TotalSeconds);
            monster.S_Deplacement((float)gameTime.ElapsedGameTime.TotalSeconds);

            // TEST DE COLISIONS ECRAN
            perso1.F_CollisionEcran(graphics);
            perso2.F_CollisionEcran(graphics);
            monster.F_CollisionEcran(graphics);

            // COLISIONS PERSO - OBJETS
            Rectangle monster_rect = new Rectangle((int)monster.G_Position().X + (monster.G_Texture().Width) / 2,
                (int)monster.G_Position().Y, 5, monster.G_Texture().Height);

            if ((monster.F_Collision_Objets(perso1.G_Rectangle())))
            {
                if (perso1.F_Attaque(Keys.Space, Keyboard.GetState()))
                {
                    if (!monster.F_IsAlive(0))
                    {
                        monster.S_Texture(Content.Load<Texture2D>("mob/1/3"));
                    }
                    else if (!monster.F_IsAlive(15))
                    {
                        monster.S_Texture(Content.Load<Texture2D>("mob/1/2"));
                    }
                }
            }
            if ((monster.F_Collision_Objets(perso2.G_Rectangle())))
            {
                if (perso2.F_Attaque(Keys.Q, Keyboard.GetState()))
                {
                    if (!monster.F_IsAlive(0))
                    {
                        monster.S_Texture(Content.Load<Texture2D>("mob/1/3"));
                    }
                    else if (!monster.F_IsAlive(15))
                    {
                        monster.S_Texture(Content.Load<Texture2D>("mob/1/2"));
                    }
                }
            }

            if (monster.G_Life() > 0)
            {
                perso1.F_Collision_Objets(monster_rect);
                perso2.F_Collision_Objets(monster_rect);
            }

            perso1.F_Collision_Objets(r_mur_haut);
            perso2.F_Collision_Objets(r_mur_haut);

            perso1.F_Collision_Objets(r_mur_bas);
            perso2.F_Collision_Objets(r_mur_bas);

            // UPDATE L'IMAGE DU PERSO
            perso1.S_Texture(Content.Load<Texture2D>("perso/1/" + perso1.G_ImgState()));
            perso2.S_Texture(Content.Load<Texture2D>("perso/1/" + perso2.G_ImgState()));

            backsound.UpdateSon();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);

            // DESSINE LES SPRITES
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(map1.G_Texture(), map1.G_Position(), Color.White);
            spriteBatch.Draw(monster.G_Texture(), monster.G_Position(), Color.White);
            spriteBatch.Draw(perso1.G_Texture(), perso1.G_Position(), Color.White);
            spriteBatch.Draw(perso2.G_Texture(), perso2.G_Position(), Color.White);
            spriteBatch.Draw(map_first.G_Texture(), map_first.G_Position(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
