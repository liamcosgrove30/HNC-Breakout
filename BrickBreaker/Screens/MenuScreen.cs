using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using GameLibrary;
using UIControls;
using Microsoft.Xna.Framework.Media;

namespace BrickBreaker
{
    public class MenuScreen : Screen
    {
        ////////////////////////////////////////////////////////////////////////////////
        // DECLARATIONS
        ////////////////////////////////////////////////////////////////////////////////

        SoundEffect selectSound;

        ////////////////////////////////////////////////////////////////////////////////
        // INITIALIZE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Initialize(ScreenManager mgr)
        {
            base.Initialize(mgr);
            selectSound = Content.Load<SoundEffect>("start");
        }

        ////////////////////////////////////////////////////////////////////////////////
        // UPDATE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyboardHelper.NewKeyPressed(Keys.P, gameTime))
            {
                manager.GoToScreen("ingame");
                if (sharedData.soundsOn)
                    selectSound.Play();
            }
            if (KeyboardHelper.NewKeyPressed(Keys.Q, gameTime))
            {
                manager.shuttingDown = true;
            }
            if (KeyboardHelper.NewKeyPressed(Keys.S, gameTime))
            {
                sharedData.soundsOn = !sharedData.soundsOn;
            }
            if (KeyboardHelper.NewKeyPressed(Keys.M, gameTime))
            {
                sharedData.musicOn= !sharedData.musicOn;
                if (sharedData.musicOn) MediaPlayer.Resume();
                else MediaPlayer.Pause();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////
        // DRAW THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Draw(GameTime gameTime)
        {
            manager.DrawScreen("ingame", gameTime); // draws the game behind this menu
            DrawText.Aligned("P to play!", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.4f, Color.White, manager.defaultFont);
            DrawText.Aligned("Q to quit!", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.5f, Color.White, manager.defaultFont);
            DrawText.Aligned("S for Sound: " + (sharedData.soundsOn ? "on" : "off"), HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.6f, Color.White, manager.defaultFont);
            DrawText.Aligned("M for Music: " + (sharedData.musicOn ? "on" : "off"), HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.7f, Color.White, manager.defaultFont);

            base.Draw(gameTime);
        }
    }
}
