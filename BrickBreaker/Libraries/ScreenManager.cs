////////////////////////////////////////////////////////////////////////////////
// ScreenManager library
// by David Marshall
// Last update: 26th October 2015
//
// Classes in this library:
// Screen - base class for creating screen classes
// ScreenManager - class to handle initializing, drawing etc of active screen
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary
{
    public class Screen
    {
        protected ScreenManager manager;

        protected SpriteBatch spriteBatch { get { return manager.spriteBatch; } }
        protected ContentManager Content { get { return manager.content; } }
        protected GraphicsDeviceManager graphics { get { return manager.graphics; } }
        protected int screenWidth { get { return manager.screenWidth; } }
        protected int screenHeight { get { return manager.screenHeight; } }
        protected GameWindow Window { get { return manager.Window; } }
        protected SpriteFont defaultFont { get { return manager.defaultFont; } }

        public virtual void Initialize(ScreenManager mgr)
        {
            if (mgr == null) throw new Exception("You must set up the screen manager before initializing screens!");
            manager = mgr;
        }

        public virtual void Start() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }

    public class ScreenManager
    {
        public SpriteBatch spriteBatch;
        public GraphicsDeviceManager graphics;

        private Dictionary<string, Screen> screens = new Dictionary<string, Screen>();
        public Screen ActiveScreen;
        public SpriteFont defaultFont;
        public Game parentGame;

        public ContentManager content { get { return parentGame.Content; } }
        public GameWindow Window { get { return parentGame.Window; } }
        public bool shuttingDown = false;
        public int screenWidth { get { return graphics.GraphicsDevice.Viewport.Width; } }
        public int screenHeight { get { return graphics.GraphicsDevice.Viewport.Height; } }

        public ScreenManager(Game parent, SpriteBatch batch, GraphicsDeviceManager gdm)
        {
            spriteBatch = batch;
            parentGame = parent;
            graphics = gdm;
        }

        public void GoToScreen(int number)
        {
            if (screens.Count() < number && number >= 0)
            {
                ActiveScreen = screens.ElementAt(number).Value;
                ActiveScreen.Start();
            }
            else
                throw new Exception("You tried to go to a screen number that does not exist.");
        }

        internal void GoToScreen(string name)
        {
            if(screens.ContainsKey(name))
            {
                ActiveScreen = screens[name];
                ActiveScreen.Start();
            }
            else
                throw new Exception("You tried to go to a named screen that doesn't exist.  Check the spelling, including capitalisation.");
        }

        public void Add(Screen ScreenToAdd, string name)
        {
            screens.Add(name, ScreenToAdd);
            if (ActiveScreen == null) ActiveScreen = ScreenToAdd;
        }

        public void InitializeAllScreens()
        {
            for(int i = 0; i<screens.Count(); i++)
                screens.ToArray()[i].Value.Initialize(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }

        public void Update(GameTime gameTime)
        {
            if (shuttingDown) parentGame.Exit();
            if (ActiveScreen == null)
                throw new Exception("there is no active screen to update!");
            ActiveScreen.Update(gameTime);
        }

        public void DrawScreen(string name, GameTime gameTime)
        {
            screens[name].Draw(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (ActiveScreen == null)
                throw new Exception("there is no active screen to draw!");
            ActiveScreen.Draw(gameTime);
        }
    }
}