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
        private static int id;
        private static Vector2 screenPos, origin;
        private static Texture2D[] myTexture = new Texture2D[2];
        private static int screenHeight;
        private static int screenWidth;
        private static Vector2 speed;

        public static void Init(int id)
        {
            Map.id = id;
            speed = new Vector2(0f, 0f);
        }

        public static void Load(GraphicsDevice device, ContentManager Content)
        {
            myTexture[0] = Content.Load<Texture2D>("map/1/1-1");
            myTexture[1] = Content.Load<Texture2D>("map/1/1-2");

            screenHeight = device.Viewport.Height;
            screenWidth = device.Viewport.Width;

            origin = new Vector2(0, -352);
            screenPos = new Vector2(0, 0);
        }

        public static void Update(float deltaX, Personnage[] perso)
        {
            int scroll = 0;
            bool scrollable = true; 
            speed = new Vector2(0f, 0f);
            foreach (Personnage p in perso)
            {
                if (p.G_Position().X >= screenWidth * 0.75)
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
                else if (p.G_Position().X <= screenWidth * 0.22)
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
            if (scrollable && G_Scroll())
            {
                screenPos.X -= (deltaX) * speed.X;
            }
            else
            {
                speed = new Vector2(0f, 0f);
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            batch.Draw(myTexture[0], screenPos, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            batch.Draw(myTexture[1], new Vector2(screenPos.X + 1024, screenPos.Y), null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);

        }

        public static float G_ScreenX() { return screenPos.X; }
        public static Vector2 G_Speed() { return speed; }
        public static bool G_Scroll() { return speed != Vector2.Zero; }
    }
}
