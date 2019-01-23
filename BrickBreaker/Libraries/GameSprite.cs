using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
    class GameSprite
    {
        public Texture2D image;
        public Vector2 position;
        public Vector2 velocity;
        public int animationframes;
        public int currentAnimationTime;
        public int animationTimeMax;
        public Vector2 origin;
        public float rotation = 0;

        public GameSprite(Texture2D initialTexture, Vector2 initialPosition, int frames, int animTime)
        {
            animationframes = frames;
            animationTimeMax = animTime;
            image = initialTexture;
            position = initialPosition;
            origin = new Vector2(image.Width / 2 / animationframes, image.Height / 2);
        }

        public GameSprite(Texture2D initialTexture, Vector2 initialPosition)
        {
            animationframes = 1;
            animationTimeMax = 1;
            image = initialTexture;
            position = initialPosition;
            origin = new Vector2(image.Width / 2, image.Height / 2);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch batch, Color tint)
        {
            batch.Draw(image, position, null, tint, rotation, origin, 1, SpriteEffects.None, 0);
        }

        public void DrawAnimated(SpriteBatch batch)
        {
            if (animationTimeMax <= 0) return;
            while (currentAnimationTime > animationTimeMax) currentAnimationTime -= animationTimeMax;
            int currentFrame = (animationframes * currentAnimationTime) / animationTimeMax;
            int pixelsPerFrame = image.Width / animationframes;
            Rectangle animRect = new Rectangle(currentFrame * pixelsPerFrame, 0, pixelsPerFrame, image.Height);
            batch.Draw(image, position, animRect, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
        }

        public bool collision(GameSprite target)
        {
            if (target == null) return false;
            Rectangle sourceRectangle = new Rectangle(
                (int)(position.X - origin.X), (int)(position.Y - origin.Y),
                image.Width / animationframes, image.Height / animationframes);
            Rectangle targetRectangle = new Rectangle(
                (int)(target.position.X - target.origin.X), (int)(target.position.Y - target.origin.Y),
                target.image.Width / target.animationframes, target.image.Height / target.animationframes);
            return (sourceRectangle.Intersects(targetRectangle));
        }
    }

}
