///////////////////////////////////////////////////////////////////////////////
// PORTFOLIO ASSIGNMENT 2
//
// To complete this assignment:
//
// 1) Declare an array of GameSprites for the bricks and initialise them all.
// 2) Use a loop in the Draw method and draw them inside that loop.
// 3) Use another loop in Update that will check each brick for collision.
// 4) Upon collision with a brick...
//   4a) Gain points
//   4b) Remove the brick from play (eg by moving it off-screen)
//   4c) Make the ball respond to the collision (eg by bouncing toward ground)
// 5) Keep count of bricks destroyed.  When all are gone, restart level.
// 6) Add suitable comments to every non-trivial block of code you added
//
///////////////////////////////////////////////////////////////////////////////
//By Liam Cosgrove

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
    ///////////////////////////////////////////////////////////////////////////
    // SHARED DATA: PUT VARIABLES HERE TO BE USED ACROSS DIFFERENT SCREENS
    // Remember to make them 'public' and 'static'.
    ///////////////////////////////////////////////////////////////////////////
    public static class sharedData
    {
        public static bool soundsOn = true;
        public static bool musicOn = true;
    }

    public class InGameScreen : Screen
    {
        ///////////////////////////////////////////////////////////////////////
        // DECLARATIONS
        ///////////////////////////////////////////////////////////////////////

        // content objects
        Texture2D batImage, ballImage, brickImage;
        SoundEffect bounceSound, pointScoredSound, loseSound;
        SoundEffectInstance bounceSoundInst;

        // gameplay objects
        GameSprite bat, ball;
        GameSprite[] bricks = new GameSprite[40];
        const float FRICTION_COEFFICIENT = 0.8f;

        int lives;
        int score;
        int highscore = 0;
        int destroyed = 0;

        ///////////////////////////////////////////////////////////////////////
        // INITIALIZE THE SCREEN
        ///////////////////////////////////////////////////////////////////////
        public override void Initialize(ScreenManager mgr)
        {
            base.Initialize(mgr);
            batImage = Content.Load<Texture2D>("bat");
            ballImage = Content.Load<Texture2D>("ball");
            bat = new GameSprite(batImage, Vector2.Zero);
            ball = new GameSprite(ballImage, Vector2.Zero);
            bounceSound = Content.Load<SoundEffect>("boinggg");
            bounceSoundInst = bounceSound.CreateInstance();
            pointScoredSound = Content.Load<SoundEffect>("break crate");
            loseSound = Content.Load<SoundEffect>("fail");
            brickImage = Content.Load<Texture2D>("brick-bw");
            ResetLevel();
        }


        ///////////////////////////////////////////////////////////////////////
        // START A NEW GAME
        ///////////////////////////////////////////////////////////////////////
        public override void Start()
        {
            lives = 3;
            score = 0;
            ResetLevel();
        }

        private void ResetLevel()
        {
            ResetBat();
            ResetBall();
            ResetBricks();
        }
        private void ResetBall()
        {
            //resets the balls position to the middle of the screen and resets the balls speed
            ball.position = new Vector2(screenWidth / 2, screenHeight / 2);
            ball.velocity = new Vector2(5, -4);
        }

        private void ResetBat()
        {
            //resets bats position to middle of the screen
            bat.position = new Vector2(screenWidth / 2, 
                screenHeight - batImage.Height / 2);
            bat.velocity = new Vector2(0, 0);
        }

        private void ResetBricks()
        {
           //this code will spawn the bricks in a grid pattern
            for (int bricknum = 0; bricknum < 10; bricknum += 1)
            {
                bricks[bricknum] = new GameSprite(brickImage, new Vector2(bricknum * 100 + 100, 100));
            }
            
            for (int bricknum = 10; bricknum < 20; bricknum += 1)
            {
                bricks[bricknum] = new GameSprite(brickImage, new Vector2(bricknum * 100 - 900, 150));
            } 
            
            for (int bricknum = 20; bricknum < 30; bricknum += 1)
            {
                bricks[bricknum] = new GameSprite(brickImage, new Vector2(bricknum * 100 - 1900, 200));
            } 
            
            for (int bricknum = 30; bricknum < 40; bricknum += 1)
            {
                bricks[bricknum] = new GameSprite(brickImage, new Vector2(bricknum * 100 - 2900, 250));
            }
        }


        ///////////////////////////////////////////////////////////////////////
        // UPDATE THE GAME WORLD
        ///////////////////////////////////////////////////////////////////////
        public override void Update(GameTime gameTime)
        {
                       MoveBat();
            MoveBall();
            BatBallCollisionCheck();
            BrickBallCollisionCheck();

            if (score > highscore)
            {
                highscore = score;  // keep high score up to date
            }

            if (lives <= 0)
            {
                manager.GoToScreen("gameOver");
                destroyed = 0;
            }

            base.Update(gameTime);
        }

        private void BatBallCollisionCheck()
        {
            if (ball.collision(bat))
            {
                // find the current speed of the ball, and speed it up a bit
                float newspeed = ball.velocity.Length() * 1.025f;
                
                // next, get find an angle it bounces off based on how far
                // along the bat the ball hit.
                float maxDistance = ball.origin.X + bat.origin.X;
                float newAngle = (ball.position.X - bat.position.X) /
                    maxDistance - (float)Math.PI / 2;
                
                // turn angle & speed into a Vector with X and Y values
                ball.velocity = new Vector2((float)Math.Cos(newAngle),
                    (float)Math.Sin(newAngle));
                ball.velocity *= newspeed;

                if (sharedData.soundsOn)
                    bounceSoundInst.Play(); // use instance to avoid distortion
            }
        }

        private void BrickBallCollisionCheck()
        {
            //if the ball collides with a brick, move it off screen, gain 100 points
            //reverse ball's direction and add one to destroyed, when 40 bricks are destroyed, reset level.
            for (int bricknum = 0; bricknum < 40; bricknum += 1)
                if (ball.collision(bricks[bricknum]))
            {
                
                bricks[bricknum].position.X = 2000;
                score += 100;
                ball.velocity.Y =- ball.velocity.Y;
                destroyed += 1;
                if (destroyed == 40 )
                {
                    ResetLevel();
                    destroyed = 0;
                }
                break;
            }
            
        }

        private void MoveBall()
        {
            ball.position += ball.velocity;             // move ball
            ball.rotation += ball.velocity.X * 0.02f;   // spin it

            // if ball collides with the left or right of screen then reverse the direction and increase the speed

            if (ball.position.X < ball.origin.X)
            {
                ball.position.X = ball.origin.X;
                ball.velocity.X *= -1;
                if (sharedData.soundsOn)
                    bounceSound.Play();
            }

            if (ball.position.X > screenWidth - ball.origin.X)
            {
                ball.position.X = screenWidth - ball.origin.X;
                ball.velocity.X *= -1;
                if (sharedData.soundsOn)
                    bounceSound.Play();
            }
            // i fball collides with top of screen then reverse direction and increase speed
            if (ball.position.Y < ball.origin.Y)
            {
                ball.velocity.Y *= -1;
                if (sharedData.soundsOn)
                    bounceSound.Play();
            }

            // collision at bottom of screen
            if (ball.position.Y > screenHeight + ball.origin.Y)
            {
                ResetBall();
                lives--;
                if (sharedData.soundsOn)
                    loseSound.Play();

               
            }

        }

        private void MoveBat()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) bat.velocity.X -= 3;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) bat.velocity.X += 3;
            bat.velocity *= FRICTION_COEFFICIENT;
            bat.position += bat.velocity;
            if (bat.position.X < bat.origin.X) bat.position.X = bat.origin.X;
            if (bat.position.X > screenWidth - bat.origin.X) bat.position.X =
                screenWidth - bat.origin.X;
        }

        ///////////////////////////////////////////////////////////////////////
        // DRAW THE SCREEN
        ///////////////////////////////////////////////////////////////////////
        public override void Draw(GameTime gameTime)
        {
            DrawText.AlignedScaledAndRotated("score: " + score,
                HorizontalAlignment.Left, VerticalAlignment.Top,
                0, 0, 0, 0.5f, Color.White, manager.defaultFont);
            DrawText.AlignedScaledAndRotated("lives: " + lives,
                HorizontalAlignment.Right, VerticalAlignment.Top,
                1, 0, 0, 0.5f, Color.White, manager.defaultFont);
            DrawText.AlignedScaledAndRotated("Hiscore: " + highscore,
                HorizontalAlignment.Center, VerticalAlignment.Top,
                0.5f, 0, 0, 0.5f, Color.White, manager.defaultFont);
            bat.Draw(spriteBatch);

            //draw 40 bricks on screen 
            for (int bricknum = 0; bricknum < 40; bricknum += 1)
            {
                bricks[bricknum].Draw(spriteBatch);
            }
                // only draw the ball if you are playing the game right now
                // so if this screen is drawn under another, the ball is hidden
                if (manager.ActiveScreen == this)
                    ball.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
