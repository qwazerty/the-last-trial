using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace The_Last_Trial
{
    class Map
    {
        private const int PIXEL = 32;

        private static int id, screenHeight, screenWidth, MaxX;
        private static Texture2D[] first, middle, back;
        private static Vector2 screenPos, speed;
        private static Vector2 originBack, originMiddle, originFirst;
        private static Rectangle[] collision;
        private static int scroll, currentScreen;
        private static float offsetY;
        private static bool scrollable;

        public static int Init(int id, Monster[] monster)
        {
            Map.id = id;

            if (id == 1)
            {
                MaxX = 4 * 1024;
                first = new Texture2D[3];
                middle = new Texture2D[3];
                back = new Texture2D[3];
                collision = new Rectangle[2];
                collision[0] = new Rectangle(0, 320, MaxX, PIXEL);
                collision[1] = new Rectangle(0, 768, MaxX, PIXEL);

                originBack = new Vector2(0f, 0f);
                originMiddle = new Vector2(0f, -320f);
                originFirst = new Vector2(0f, -704f);

                return 5;
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
                return monster;
            }
            return null;
        }

        public static void Load(GraphicsDevice device, ContentManager Content)
        {
            currentScreen = 1;
            screenPos = new Vector2(0, 0);
            speed = new Vector2(0f, 0f);

            screenHeight = device.Viewport.Height;
            screenWidth = device.Viewport.Width;

            first[0] = Content.Load<Texture2D>("map/1/1-" + currentScreen);
            first[1] = Content.Load<Texture2D>("map/1/1-" + (currentScreen + 1));
            first[2] = Content.Load<Texture2D>("map/1/1-" + (currentScreen + 2));

            middle[0] = Content.Load<Texture2D>("map/1/2-" + currentScreen);
            middle[1] = Content.Load<Texture2D>("map/1/2-" + (currentScreen + 1));
            middle[2] = Content.Load<Texture2D>("map/1/2-" + (currentScreen + 2));

            back[0] = Content.Load<Texture2D>("map/1/3-1");
            back[1] = Content.Load<Texture2D>("map/1/3-2");
            back[2] = Content.Load<Texture2D>("map/1/3-3");
 
        }

        public static void Update(GameTime gameTime, Personnage[] perso, ContentManager Content)
        {
            float deltaX = (float)gameTime.ElapsedGameTime.TotalSeconds;

            currentScreen = (int)(-screenPos.X) / 1024 + 1;

            first[0] = Content.Load<Texture2D>("map/1/1-" + currentScreen);
            first[1] = Content.Load<Texture2D>("map/1/1-" + (currentScreen + 1));
            first[2] = Content.Load<Texture2D>("map/1/1-" + (currentScreen + 2));

            middle[0] = Content.Load<Texture2D>("map/1/2-" + currentScreen);
            middle[1] = Content.Load<Texture2D>("map/1/2-" + (currentScreen + 1));
            middle[2] = Content.Load<Texture2D>("map/1/2-" + (currentScreen + 2));

            offsetY = 0;
            scroll = 0;
            scrollable = true; 
            speed = new Vector2(0f, 0f);
            foreach (Personnage p in perso)
            {
                if (p.G_Position().X > screenWidth * 0.75 && (-screenPos.X < MaxX - screenWidth))
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
                else if (p.G_Position().X < screenWidth * 0.22 /*0.2*/ && (screenPos.X < 0))
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

                offsetY += p.G_Position().Y;
            }
            offsetY /= (Game1.G_Player() * 5);
            offsetY -= back[0].Height - (screenHeight - first[0].Height - middle[0].Height);
            if (scrollable && G_Scroll())
            {
                screenPos.X -= (deltaX) * speed.X;
            }
            else
            {
                speed = new Vector2(0f, 0f);
            }
        }

        public static void DrawBack(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(back[0], new Vector2(screenPos.X / 5, screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(back[1], new Vector2(screenPos.X / 5 + 1024, screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(back[2], new Vector2(screenPos.X / 5 + 2048, screenPos.Y + offsetY), null,
                 Color.White, 0, originBack, 1, SpriteEffects.None, 0f);
        }

        public static void DrawMiddle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(middle[0], new Vector2(screenPos.X + 1024 * (currentScreen - 1), screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(middle[1], new Vector2(screenPos.X + 1024 * currentScreen, screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(middle[2], new Vector2(screenPos.X + 1024 * (currentScreen + 1), screenPos.Y), null,
                 Color.White, 0, originMiddle, 1, SpriteEffects.None, 0f);
        }

        public static void DrawFirst(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(first[0], new Vector2(screenPos.X + 1024 * (currentScreen - 1), screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(first[1], new Vector2(screenPos.X + 1024 * currentScreen, screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
            spriteBatch.Draw(first[2], new Vector2(screenPos.X + 1024 * (currentScreen + 1), screenPos.Y), null,
                 Color.White, 0, originFirst, 1, SpriteEffects.None, 0f);
        }

        public static float G_ScreenX() { return screenPos.X; }
        public static Vector2 G_Speed() { return speed; }
        public static bool G_Scroll() { return speed != Vector2.Zero; }
        public static Rectangle[] G_Collision() { return collision; }
    }
}
