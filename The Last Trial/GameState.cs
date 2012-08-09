using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace The_Last_Trial
{
    class GameState : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private static int nbPlayer, nbMonster, state, level, maxLevel;
        private static bool newLevel, applyChanges;

        private static Personnage[] perso;
        private static Monster[] monster;
        private static PNJ[] pnj;

        public static int Player { get { return nbPlayer; } }
        public static int Monster { get { return nbMonster; } }
        public static int Level { get { return level; } set { level = value; } }
        public static int MaxLevel { get { return maxLevel; } set { maxLevel = value; } }
        public static SpriteFont gameFont, overKill, textFont, bossFont, menuFont;

        public GameState()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = Program.width;
            this.graphics.PreferredBackBufferHeight = Program.height;
            this.graphics.IsFullScreen = Program.fullscreen;
            this.graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Son.Load(Content);
            Son.InitLoopSound(0);
            Restart(Content);
            menuFont = Content.Load<SpriteFont>("font/menufont");
            overKill = Content.Load<SpriteFont>("font/overkillfont");
            gameFont = Content.Load<SpriteFont>("font/gamefont");
            textFont = Content.Load<SpriteFont>("font/textfont");
            bossFont = Content.Load<SpriteFont>("font/bossfont");
            base.Initialize();
        }

        public static void Restart(ContentManager Content)
        {
            Son.InstanceStop();
            if (Program.musique)
            {
                Son.InitLoopSound(0);
                Son.InstancePlay();
            }
            LoadingMenu.Init(Content);
            Menu.Init(Content);
            Game1.Restart();
            state = 0;
            level = 1;
            maxLevel = 1;
            nbPlayer = 0;
            newLevel = false;
            applyChanges = true;
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (state == 0)
            {
                if (newLevel)
                {
                    Launcher.LoadNewMap(gameTime, Content, graphics, spriteBatch, ref perso, ref monster, ref pnj, ref nbMonster);
                    state = 1;
                    newLevel = false;
                }
                else if (Launcher.Update(gameTime, Content, graphics, spriteBatch, ref perso, ref monster, ref pnj, ref nbPlayer, ref nbMonster))
                {
                    Cinematic.Load(Content);
                    state = 2;
                }
            }
            else if (state == 1)
            {
                Game1.Update(gameTime, graphics, Content, perso, monster, pnj);
                if (Map.G_EndLevel(monster))
                {
                    level++;
                    if (level > maxLevel)
                    {
                        maxLevel = level;
                    }
                    state = 0;
                    newLevel = true;
                    SaveLoad.Save(perso);
                }
            }
            else if (state == 2)
            {
                if (Cinematic.Update(gameTime))
                {
                    state = 1;
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (applyChanges)
            {
                this.graphics.PreferredBackBufferWidth = Program.width;
                this.graphics.PreferredBackBufferHeight = Program.height;
                this.graphics.IsFullScreen = Program.fullscreen;
                this.graphics.ApplyChanges();
                applyChanges = false;
            }
            if (state == 0 && !newLevel)
            {
                Launcher.Draw(spriteBatch, graphics);
            }
            else if (state == 1)
            {
                Game1.Draw(gameTime, spriteBatch, graphics, Content, perso, monster, pnj);
            }
            else if (state == 2)
            {
                Cinematic.Draw(spriteBatch, graphics);
            }
            base.Draw(gameTime);
        }
    }
}
