///////////////////////////////////////////////////////////////////////////////
//                                                                           //
//                THIS CODE FILE DOES NOT RUN YOUR GAME!                     //
//                   DO NOT ADD GAMEPLAY CODE IN HERE!                       //
//                    GO TO INGAMESCREEN.CS FOR THAT.                        //
//                                                                           //
///////////////////////////////////////////////////////////////////////////////
//
// The Game1 class (below) sets up the various screens (gameplay, menu etc)
// and then passes control on to the ScreenManager class to control them.
//
// If you want to add new screens, create a new screen class like the ones
// provided, and add them to the screen manager in the Initialize method.
//
////////////////////////////////////////////////////////////////////////////////

#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameLibrary;
using UIControls;
#endregion

namespace BrickBreaker
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager manager;
        Texture2D defaultBackgroundImage;
        Song defaultMusic;

        // Constructor method for the main game object
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();  // Calls LoadContent(), among other things
            DrawText.Initialize(spriteBatch, graphics);
            manager = new ScreenManager(this, spriteBatch, graphics);
            manager.defaultFont = Content.Load<SpriteFont>("on the rocks");
            manager.graphics.PreferredBackBufferWidth= 1024;
            manager.graphics.PreferredBackBufferHeight = 768;
            manager.graphics.ApplyChanges();
            // Add game screens here.  The first one to be added will be the active screen.
            manager.Add(new MenuScreen(), "menu");
            manager.Add(new InGameScreen(), "ingame");
            manager.Add(new GameOverScreen(), "gameOver");
            manager.InitializeAllScreens();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);            // Create a new SpriteBatch, which can be used to draw textures.
            defaultBackgroundImage = Content.Load<Texture2D>("rainbow-lines");
            defaultMusic = Content.Load<Song>("DST-AngryRobotIII");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(defaultMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            manager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(defaultBackgroundImage, new Rectangle(0, 0, manager.screenWidth, manager.screenHeight), Color.White);
            manager.Draw(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
