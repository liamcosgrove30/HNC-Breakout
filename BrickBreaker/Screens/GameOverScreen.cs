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

namespace BrickBreaker
{
    public class GameOverScreen : Screen
    {
        ////////////////////////////////////////////////////////////////////////////////
        // DECLARATIONS
        ////////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////////
        // INITIALIZE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Initialize(ScreenManager mgr)
        {
            base.Initialize(mgr);
        }

        ////////////////////////////////////////////////////////////////////////////////
        // UPDATE THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (KeyboardHelper.NewKeyPressed(Keys.P, gameTime))
                manager.GoToScreen("menu");
        }

        ////////////////////////////////////////////////////////////////////////////////
        // DRAW THE SCREEN
        ////////////////////////////////////////////////////////////////////////////////
        public override void Draw(GameTime gameTime)
        {
            manager.DrawScreen("ingame", gameTime); // draws the game behind this menu
            DrawText.Aligned("GAME OVER", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.5f, Color.White, manager.defaultFont);
            DrawText.Aligned("P to continue.", HorizontalAlignment.Center, VerticalAlignment.Center, 0.5f, 0.65f, Color.White, manager.defaultFont);

            base.Draw(gameTime);
        }
    }
}
