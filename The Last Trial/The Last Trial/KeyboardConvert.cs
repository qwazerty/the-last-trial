using Microsoft.Xna.Framework.Input;

namespace The_Last_Trial
{
    public class KeyboardConvert
    {
        public static Keys GetKeyboard(string str)
        {
            switch (str)
            {
                case "Up": return Keys.Up;
                case "Down": return Keys.Down;
                case "Right": return Keys.Right;
                case "Left": return Keys.Left;
            }
            return Keys.A;
        }
    }
}
