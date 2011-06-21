using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.IO;

namespace The_Last_Trial
{
    class Cinematic
    {
        private static Objet background;
        private static string text, printedText;
        private static double time;

        public static void Load(ContentManager Content)
        {
            if (GameState.Level == 1)
            {
                time = 0;
                text = "";
                printedText = "";
                background = new Objet(Vector2.Zero, Content.Load<Texture2D>("cinematic/" + GameState.Level));
                FileStream fs = new FileStream("Content/cinematic/t1", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
        }

        public static bool Update(GameTime gameTime)
        {
            if (GameState.Level != 1 || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                return true;
            }
            int temps = 0;
            try
            {
                if (printedText[printedText.Length - 1] == '.')
                {
                    temps = 2;
                }
            }
            catch (IndexOutOfRangeException) { }
            if (gameTime.TotalGameTime.TotalSeconds > time + 0.05 + temps)
            {
                if (text.Length > 0)
                {
                    if (text[0] == '/')
                    {
                        printedText += '\n';
                    }
                    else
                    {
                        printedText += text[0].ToString();
                    }
                    text = text.Remove(0, 1);
                }
                else
                {
                    return true;
                }
                time = gameTime.TotalGameTime.TotalSeconds;
            }
            return false;
        }

        public static void Draw(SpriteBatch sb, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.Pink);
            sb.Begin(SpriteBlendMode.AlphaBlend);

            if (GameState.Level == 1)
            {
                background.Draw(sb);

                sb.DrawString(GameState.textFont, printedText, new Vector2(202, 198), Color.Black);
                sb.DrawString(GameState.textFont, printedText, new Vector2(200, 200), Color.White);
            }
            sb.End();
        }
    }
}
