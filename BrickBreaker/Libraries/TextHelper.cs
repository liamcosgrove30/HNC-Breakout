////////////////////////////////////////////////////////////////////////////////
// KeyboardHelper library
// by David Marshall
// Last update: 23rd October 2014
//
// Classes in this library:
// KeyboardHelper - for dealing with ASCII keyboard input
// DrawText - for drawing text with alignment, scale, rotation etc.
////////////////////////////////////////////////////////////////////////////////

#region using commands
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace UIControls
{
    static class KeyboardHelper
    {
        // a dictionary can be used to convert between XNA Keys into ASCII letters.
        // This one is for when you're NOT pressing shift.
        private static Dictionary<Keys, char> unShiftedKeys = new Dictionary<Keys, char>() {
            {Keys.Decimal,'.'},
            {Keys.Add,'+'},
            {Keys.Subtract,'-'},
            {Keys.Multiply,'*'},
            {Keys.Divide,'/'},
            {Keys.Oem8, '`'},
            {Keys.OemMinus, '-'},
            {Keys.OemPlus,'='},     // UK keyboard has an equals sign on this key unless you hold shift
            {Keys.OemOpenBrackets, '['},
            {Keys.OemCloseBrackets, ']'},
            {Keys.OemSemicolon, ';'},
            {Keys.OemTilde, '\''},  // UK keyboard has an apostrophe instead of a tilde on that key
            {Keys.OemQuotes, '#'},  // UK keyboard has a hash symbol instead of quotes on that key
            {Keys.OemPipe, '\\'},   // UK keyboard has a backslash on this key unless you hold shift down
            {Keys.OemComma,','},
            {Keys.OemPeriod, '.'},
            {Keys.OemQuestion, '/'} // UK keyboard has a forward slash on this key unless you hold shift
        };

        // a dictionary can be used to convert between XNA Keys into ASCII letters.
        //This one is for when you ARE pressing shift.
        private static Dictionary<Keys, char> shiftedKeys = new Dictionary<Keys, char>() {
            {Keys.D1,'!'},
            {Keys.D2,'"'},
            //{Keys.D3,'£'},    // this character is disabled as it is not in the standard spritefont
            {Keys.D4,'$'},
            {Keys.D5,'%'},
            {Keys.D6,'^'},
            {Keys.D7,'&'},
            {Keys.D8,'*'},
            {Keys.D9,'('},
            {Keys.D0,')'},
            {Keys.Decimal,'.'},
            {Keys.Add,'+'},
            {Keys.Subtract,'-'},
            {Keys.Multiply,'*'},
            {Keys.Divide,'/'},
            //{Keys.Oem8, '¬'},   // this character is disabled as it is not in the standard spritefont
            {Keys.OemMinus, '_'},
            {Keys.OemPlus,'+'},
            {Keys.OemOpenBrackets, '{'},
            {Keys.OemCloseBrackets, '}'},
            {Keys.OemSemicolon, ':'},
            {Keys.OemTilde, '@'},
            {Keys.OemQuotes, '~'},
            {Keys.OemPipe, '|'},
            {Keys.OemComma,'<'},
            {Keys.OemPeriod, '>'},
            {Keys.OemQuestion, '?'}
        };

        public static char ConvertKeytoChar(Keys keyToConvert, bool allowPunctuation = true, bool allowLowerCase = true)
        {
            bool capitalise = false;
            char converted;

            KeyboardState keys = Keyboard.GetState();  // we may need to know if the shift keys are being pressed

            if (keys.IsKeyDown(Keys.Space)) return ' ';     // add a space if space is pressed

            if (keys.IsKeyDown(Keys.LeftShift) || keys.IsKeyDown(Keys.RightShift)) capitalise = true;

            // alphabetical keys
            if (keyToConvert >= Keys.A && keyToConvert <= Keys.Z)
            {
                if (CapsLockIsOn()) capitalise = !capitalise;
                char key = keyToConvert.ToString()[0];
                if (capitalise || !allowLowerCase)
                    return key;
                else
                    return (char)(key + 32);
            }

            // number keys on main keyboard
            string keyString = keyToConvert.ToString();
            if (keyToConvert >= Keys.D0 && keyToConvert <= Keys.D9)
            {
                if (keys.IsKeyUp(Keys.LeftShift) && keys.IsKeyUp(Keys.RightShift))
                    return keyString[keyString.Length - 1];
            }

            // number keys on numpad - only if numlock is on
            if (keyToConvert >= Keys.NumPad0 && keyToConvert <= Keys.NumPad9 && NumLockIsOn())
                return keyString[keyString.Length - 1];

            // punctuation characters, if allowed
            if (allowPunctuation)
                if (capitalise)
                {
                    if (shiftedKeys.ContainsKey(keyToConvert))
                    {
                        shiftedKeys.TryGetValue(keyToConvert, out converted);
                        return converted;
                    }
                }
                else
                    if (unShiftedKeys.ContainsKey(keyToConvert))
                    {
                        unShiftedKeys.TryGetValue(keyToConvert, out converted);
                        return converted;
                    }

            // if no valid character is found, return zero.
            return (char)0;
        }

        // Import a function from an external DLL to use here.  This function checks a particular keycode
        [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern short GetKeyState(int keycode);

        // methods which return true if the lock is on
        public static bool CapsLockIsOn() { return (((ushort)GetKeyState(0x14)) & 0xffff) != 0; }
        public static bool NumLockIsOn() { return (((ushort)GetKeyState(0x90)) & 0xffff) != 0; }
        public static bool ScrollLockIsOn() { return (((ushort)GetKeyState(0x91)) & 0xffff) != 0; }

        private static KeyboardState previousFrameKeyboard;
        private static KeyboardState currentFrameKeyboard;
        private static double lastKeyboardUpdate;

        // note: dont use this at the same time as UpdateStringInput.  Results may be unpredictable.
        public static bool NewKeyPressed(Keys key, GameTime time)
        {
            bool pressed = false;

            if (time.TotalGameTime.TotalMilliseconds > lastKeyboardUpdate)
            {
                lastKeyboardUpdate = time.TotalGameTime.TotalMilliseconds;
                previousFrameKeyboard = currentFrameKeyboard;
                currentFrameKeyboard = Keyboard.GetState();
            }

            if (currentFrameKeyboard.IsKeyDown(key) && previousFrameKeyboard.IsKeyUp(key))
                pressed = true;

            return pressed;
        }

        // note: dont use this at the same time as NewKeyPressed.  Results may be unpredictable.
        public static void UpdateStringInput(ref string editString, GameTime time, bool allowPunctuation = true, bool allowLowerCase = true)
        {
            // store the keyboard state from one frame to the next, to allow new key checking
            if (time.TotalGameTime.TotalMilliseconds > lastKeyboardUpdate)
            {
                lastKeyboardUpdate = time.TotalGameTime.TotalMilliseconds;
                previousFrameKeyboard = currentFrameKeyboard;
                currentFrameKeyboard = Keyboard.GetState();
            }

            currentFrameKeyboard = Keyboard.GetState();
            Keys[] pressed = currentFrameKeyboard.GetPressedKeys();
            foreach (Keys k in pressed)
            {
                // only deal with keys that have just been pressed, not ones that are held down from earlier
                if(previousFrameKeyboard.IsKeyUp(k))    
                {
                    char c = KeyboardHelper.ConvertKeytoChar(k, allowPunctuation, allowLowerCase);    // convert to ascii text
                    if(c!=0) editString += c;                       // add to the string
                }
            }
            
            // Backspace key to delete a character
            if (currentFrameKeyboard.IsKeyDown(Keys.Back) && previousFrameKeyboard.IsKeyUp(Keys.Back) && editString.Length > 0)
                editString = editString.Substring(0, editString.Length - 1);
        }
    }


    public enum VerticalAlignment { Top, Center, Bottom }
    public enum HorizontalAlignment { Left, Center, Right }
    static class DrawText
    {
        public struct Settings
        {
            static public SpriteBatch batch = null;
            static public SpriteFont font = null;
            static public GraphicsDeviceManager graphics = null;
            static public SpriteEffects flipping = SpriteEffects.None;
            static public Color textColor = Color.White;
        }

        static public void Initialize(SpriteBatch batch, GraphicsDeviceManager gdm)
        {
            Settings.batch = batch;
            Settings.graphics = gdm;
        }

        static public void Initialize(SpriteBatch Batch, SpriteFont newFont, GraphicsDeviceManager gdm)
        {
            Settings.batch = Batch;
            Settings.font = newFont;
            Settings.graphics = gdm;
        }

        public static void Aligned(string text, HorizontalAlignment horizontal, VerticalAlignment vertical,
            float Xproportion, float Yproportion, Color? newTextColor = null, SpriteFont font = null)
        {
            AlignedScaledAndRotated(text, horizontal, vertical, Xproportion, Yproportion, 0, 1, newTextColor, font);
        }

        public static void AlignedScaledAndRotated(string text, HorizontalAlignment horizontal, VerticalAlignment vertical, 
            float Xproportion, float Yproportion, float rotation, float scale, Color? newTextColor = null, SpriteFont font = null)
        {
            SpriteFont fontToUse = font == null ? Settings.font : font;
            if (fontToUse == null) throw new System.Exception("No font selected, and no default font specified.");
            if (Settings.graphics == null) throw new System.Exception("DrawText must be initialized with a graphics device.");
            Viewport view = Settings.graphics.GraphicsDevice.Viewport;
            Vector2 origin = fontToUse.MeasureString(text);
            if (horizontal == HorizontalAlignment.Left) origin.X = 0;
            if (horizontal == HorizontalAlignment.Center) origin.X /= 2;
            if (vertical == VerticalAlignment.Top) origin.Y = 0;
            if (vertical == VerticalAlignment.Center) origin.Y /= 2;
            Vector2 position = new Vector2(view.Width * Xproportion, view.Height * Yproportion);
            Settings.batch.DrawString(fontToUse, text, position,
                newTextColor == null ? Settings.textColor : (Color)newTextColor, rotation, origin, scale,
                Settings.flipping, 0);
        }
    }
}