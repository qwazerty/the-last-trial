using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace The_Last_Trial
{
    class Map
    {
        #region VAR
        private const int PIXEL = 32;

        private static int id, MaxX, scroll, currentScreen;
        private static Texture2D[] first, middle, back;
        private static Vector2 screenPos, speed;
        private static Vector2 originBack, originMiddle, originFirst;
        private static int speedBack, speedMiddle, speedFirst;
        private static Rectangle[] collision;
        private static float offsetY;
        private static bool scrollable, firstHide, parallax;
        #endregion

        /***********\
         * METHODE *
        \***********/

        #region GET

        public static float G_ScreenX() { return screenPos.X; }
        public static Vector2 G_Speed() { return speed; }
        public static bool G_Scroll() { return speed != Vector2.Zero; }
        public static Rectangle[] G_Collision() { return collision; }
        public static bool G_FirstHide() { return firstHide; }

        public static bool G_EndLevel(Monster[] monster)
        {
            bool mobAlive = false;
            try
            {
                foreach (Monster m in monster)
                {
                    if (m.G_IsAlive())
                    {
                        mobAlive = true;
                    }
                }
            }
            catch (NullReferenceException) { }
            return ((-screenPos.X >= MaxX - Program.width && Keyboard.GetState().IsKeyDown(Keys.Enter) && !mobAlive) || (Keyboard.GetState().IsKeyDown(Keys.Enter) && Keyboard.GetState().IsKeyDown(Keys.F1)));
        }

        #endregion

        #region Init & Load

        public static int Init(int id, Monster[] monster, PNJ[] pnj)
        {
            Map.id = id;
            first = new Texture2D[3];
            middle = new Texture2D[3];
            back = new Texture2D[3];

            if (id == 1)
            {
                MaxX = 4 * 1024;
                collision = new Rectangle[2];
                collision[0] = new Rectangle(0, 320, MaxX, PIXEL);
                collision[1] = new Rectangle(0, 768, MaxX, PIXEL);

                originBack = new Vector2(0f, 0f);
                originMiddle = new Vector2(0f, -320f);
                originFirst = new Vector2(0f, -704f);
                speedBack = 5;
                speedMiddle = 1;
                speedFirst = 1;
                firstHide = true;
                parallax = true;

                pnj[0] = new PNJ(new Vector2(800, 500), 42, "Hi. Good luck, have fun !");
                pnj[1] = new PNJ(new Vector2(3900, 500), 42, "Sorry Mario, your princess\n is in another castle...");

                return 6;
            }
            if (id == 2)
            {
                MaxX = 4 * 1024;
                collision = new Rectangle[2];
                collision[0] = new Rectangle(0, 288, MaxX, PIXEL);
                collision[1] = new Rectangle(0, 800, MaxX, PIXEL);

                originBack = new Vector2(0f, 0f);
                originMiddle = new Vector2(0f, -224f);
                originFirst = new Vector2(0f, -320f);
                speedBack = 1;
                speedMiddle = 1;
                speedFirst = 1;
                firstHide = false;
                parallax = false;

                return 6;
            }
            return 0;
        }

        public static int InitPNJ(int id)
        {
            if (id == 1)
            {
                return 2;
            }
            if (id == 2)
            {
                return 0;
            }
            return 0;
        }

        public static Monster[] LoadMonster(Monster[] monster)
        {
            if (id == 1)
            {
                monster[0] = new Monster(new Vector2(1220f, 400f), 1);
                monster[1] = new Monster(new Vector2(1600f, 300f), 1);
                monster[2] = new Monster(new Vector2(2000f, 350f), 1);
                monster[3] = new Monster(new Vector2(2500f, 300f), 1);
                monster[4] = new Monster(new Vector2(2600f, 400f), 1);
                monster[5] = new Monster(new Vector2(3500f, 350f), 2);
            }
            if (id == 2)
            { 
                monster[0] = new Monster(new Vector2(1220f, 400f), 1);
                monster[1] = new Monster(new Vector2(1300f, 500f), 1);
                monster[2] = new Monster(new Vector2(1500f, 350f), 1);
                monster[3] = new Monster(new Vector2(1800f, 300f), 1);
                monster[4] = new Monster(new Vector2(2000f, 400f), 1);
                monster[5] = new Monster(new Vector2(2500f, 350f), 2);
            }
            return monster;
        }

        public static void Load(ContentManager Content)
        {
            currentScreen = 0;
            screenPos = new Vector2(0, 0);
            speed = new Vector2(0f, 0f);

        }

        #endregion

        public static void Update(GameTime gameTime, Personnage[] perso, ContentManager Content)
        {
            float deltaX = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentScreen = (int)(-screenPos.X) / 1024;
            try
            {
                first[0] = Content.Load<Texture2D>("map/" + id + "/1-" + (currentScreen / speedFirst));
            }
            catch (FileNotFoundException)
            {
                Program.Restart();
            }
            first[1] = Content.Load<Texture2D>("map/" + id + "/1-" + (currentScreen / speedFirst + 1));
            first[2] = Content.Load<Texture2D>("map/" + id + "/1-" + (currentScreen / speedFirst + 2));

            middle[0] = Content.Load<Texture2D>("map/" + id + "/2-" + (currentScreen / speedMiddle));
            middle[1] = Content.Load<Texture2D>("map/" + id + "/2-" + (currentScreen / speedMiddle + 1));
            middle[2] = Content.Load<Texture2D>("map/" + id + "/2-" + (currentScreen / speedMiddle + 2));

            back[0] = Content.Load<Texture2D>("map/" + id + "/3-" + (currentScreen / speedBack));
            back[1] = Content.Load<Texture2D>("map/" + id + "/3-" + (currentScreen / speedBack + 1));
            back[2] = Content.Load<Texture2D>("map/" + id + "/3-" + (currentScreen / speedBack + 2));

            offsetY = 0;
            scroll = 0;
            scrollable = true; 
            speed = new Vector2(0f, 0f);
            foreach (Personnage p in perso)
            {
                if (p.G_IsAlive())
                {
                    if (p.G_Position().X > Program.width * 0.75 && (-screenPos.X < MaxX - Program.width))
                    {
                        if (scroll >= 0)
                        {
                            scroll = 1;
                            speed = new Vector2(150f, 0f);
                        }
                        else
                        {
                            scrollable = false;
                        }
                    }
                    else if (p.G_Position().X < Program.width * 0.22 && (screenPos.X < 0))
                    {
                        if (scroll <= 0)
                        {
                            scroll = -1;
                            speed = new Vector2(-150f, 0f);
                        }
                        else
                        {
                            scrollable = false;
                        }
                    }
                }

                if (parallax)
                {
                    offsetY += p.G_Position().Y;
                }
            }
            if (parallax)
            {
                offsetY /= (GameState.G_Player() * 5);
                offsetY -= back[0].Height - (Program.height - first[0].Height - middle[0].Height);
            }
            if (scrollable && G_Scroll())
            {
                screenPos.X -= (deltaX) * speed.X;
            }
            else
            {
                speed = new Vector2(0f, 0f);
            }
        }

        #region Draw

        public static void DrawBack(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(back[0], new Vector2(screenPos.X / speedBack + 1024 * (currentScreen / speedBack), screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(back[1], new Vector2(screenPos.X / speedBack + 1024 * (currentScreen / speedBack + 1), screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(back[2], new Vector2(screenPos.X / speedBack + 1024 * (currentScreen / speedBack + 2), screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
        }

        public static void DrawMiddle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(middle[0], new Vector2(screenPos.X / speedMiddle + 1024 * (currentScreen), screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(middle[1], new Vector2(screenPos.X / speedMiddle + 1024 * (currentScreen + 1), screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(middle[2], new Vector2(screenPos.X / speedMiddle + 1024 * (currentScreen + 2), screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
        }

        public static void DrawFirst(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(first[0], new Vector2(screenPos.X / speedFirst + 1024 * (currentScreen), screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(first[1], new Vector2(screenPos.X / speedFirst + 1024 * (currentScreen + 1), screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(first[2], new Vector2(screenPos.X / speedFirst + 1024 * (currentScreen + 2), screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
        }

        #endregion
    }
}
