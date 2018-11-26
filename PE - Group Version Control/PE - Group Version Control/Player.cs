using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PE___Group_Version_Control
{
    // Dan Robinson

    class Player : GameObject
    {
        // Fields

        private PlayerState playerState;
        private GunDirection gunDirection;
        private int gunOffset;
        private int maxHP;
        private int speed;
        private int currentHP;
        private float scale;
        private bool firing;

        // Animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame

        // Constants for "source" rectangle (inside the image)
        const int WalkFrameCount = 4;       // The number of frames in the animation
        const int OffsetY = 0;            // How far down in the image are the frames?
        const int RectHeight = 20;          // The height of a single frame
        const int RectWidth = 20;           // The width of a single frame


        // Properties

        public int MaxHp { get { return maxHP; } set { maxHP = value; } }
        public int CurrentHP { get { return currentHP; } set { currentHP = value; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public int GunOffset { get { return gunOffset; } set { gunOffset = value; } }
        public GunDirection _GunDirection { get { return gunDirection; } set { gunDirection = value; } }


        // Constructors

        public Player(Rectangle hitBox, Texture2D texture, SpriteFont debug)
            : base(hitBox, texture, debug)
        {
            firing = false;
            gunOffset = 0;
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            this.hitBox = hitBox;
            this.texture = texture;
            this.debug = debug;
            scale = 2.5f;
        }

        // Methods

        /// <summary>
        /// Checks if the player is firing and returns true/false
        /// </summary>
        /// <param name="kb"></param>
        /// <returns></returns>
        private bool CheckFiring(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Left) ||
                kb.IsKeyDown(Keys.Up) ||
                kb.IsKeyDown(Keys.Right) ||
                kb.IsKeyDown(Keys.Down)) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// parameter keyboard state to see if playerstate needs to change
        /// </summary>
        /// <param name="kb"></param>
        public void UpdateSprite(KeyboardState kb, GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            firing = CheckFiring(kb);
            switch (playerState)
            {
                case PlayerState.FaceLeft:
                    gunDirection = GunDirection.Left;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.Left)) playerState = PlayerState.FaceLeft;
                    if (kb.IsKeyDown(Keys.Up)) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.Right)) playerState = PlayerState.FaceRight;
                    if (kb.IsKeyDown(Keys.Down)) playerState = PlayerState.FaceDown;
                    break;
                case PlayerState.FaceUp:
                    gunDirection = GunDirection.Up;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.Left)) playerState = PlayerState.FaceLeft;
                    if (kb.IsKeyDown(Keys.Up)) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.Right)) playerState = PlayerState.FaceRight;
                    if (kb.IsKeyDown(Keys.Down)) playerState = PlayerState.FaceDown;
                    break;
                case PlayerState.FaceRight:
                    gunDirection = GunDirection.Right;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.Left)) playerState = PlayerState.FaceLeft;
                    if (kb.IsKeyDown(Keys.Up)) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.Right)) playerState = PlayerState.FaceRight;
                    if (kb.IsKeyDown(Keys.Down)) playerState = PlayerState.FaceDown;
                    break;
                case PlayerState.FaceDown:
                    gunDirection = GunDirection.Down;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.Left)) playerState = PlayerState.FaceLeft;
                    if (kb.IsKeyDown(Keys.Up)) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.Right)) playerState = PlayerState.FaceRight;
                    if (kb.IsKeyDown(Keys.Down)) playerState = PlayerState.FaceDown;
                    break;
                case PlayerState.WalkLeft:
                    if (!firing) gunDirection = GunDirection.Left;
                    if (kb.IsKeyDown(Keys.A) == false) playerState = PlayerState.FaceLeft;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkUp:
                    if (!firing) gunDirection = GunDirection.Up;
                    if (kb.IsKeyDown(Keys.W) == false) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkRight:
                    if (!firing) gunDirection = GunDirection.Right;
                    if (kb.IsKeyDown(Keys.D) == false) playerState = PlayerState.FaceRight;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkDown:
                    if (!firing) gunDirection = GunDirection.Down;
                    if (kb.IsKeyDown(Keys.S) == false) playerState = PlayerState.FaceDown;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkUpLeft:
                    if (!firing) gunDirection = GunDirection.UpLeft;
                    if (kb.IsKeyDown(Keys.A) == false && kb.IsKeyDown(Keys.W) == false) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkUpRight:
                    if (!firing) gunDirection = GunDirection.UpRight;
                    if (kb.IsKeyDown(Keys.D) == false && kb.IsKeyDown(Keys.W) == false) playerState = PlayerState.FaceUp;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkDownRight:
                    if (!firing) gunDirection = GunDirection.DownRight;
                    if (kb.IsKeyDown(Keys.D) == false && kb.IsKeyDown(Keys.S) == false) playerState = PlayerState.FaceDown;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
                case PlayerState.WalkDownLeft:
                    if (!firing) gunDirection = GunDirection.DownLeft;
                    if (kb.IsKeyDown(Keys.A) == false && kb.IsKeyDown(Keys.S) == false) playerState = PlayerState.FaceDown;
                    if (kb.IsKeyDown(Keys.A)) playerState = PlayerState.WalkLeft;
                    if (kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUp;
                    if (kb.IsKeyDown(Keys.D)) playerState = PlayerState.WalkRight;
                    if (kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDown;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpLeft;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.W)) playerState = PlayerState.WalkUpRight;
                    if (kb.IsKeyDown(Keys.D) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownRight;
                    if (kb.IsKeyDown(Keys.A) && kb.IsKeyDown(Keys.S)) playerState = PlayerState.WalkDownLeft;
                    break;
            }

        }
        /// <summary>
        /// takes in a spritebatch to draw the player depending on the state
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            DrawPlayerWeapon(sb);
            switch (playerState)
            {
                case PlayerState.FaceLeft:
                    DrawPlayerFacingLeft(sb);
                    break;
                case PlayerState.WalkLeft:
                    DrawPlayerWalkingLeft(sb);
                    break;
                case PlayerState.FaceRight:
                    DrawPlayerFacingRight(sb);
                    break;
                case PlayerState.WalkRight:
                    DrawPlayerWalkingRight(sb);
                    break;
                case PlayerState.FaceUp:
                    DrawPlayerFacingUp(sb);
                    break;
                case PlayerState.WalkUp:
                    DrawPlayerWalkingUp(sb);
                    break;
                case PlayerState.FaceDown:
                    DrawPlayerFacingDown(sb);
                    break;
                case PlayerState.WalkDown:
                    DrawPlayerWalkingDown(sb);
                    break;
                case PlayerState.WalkUpLeft:
                    DrawPlayerWalkingUpLeft(sb);
                    break;
                case PlayerState.WalkUpRight:
                    DrawPlayerWalkingUpRight(sb);
                    break;
                case PlayerState.WalkDownRight:
                    DrawPlayerWalkingDownRight(sb);
                    break;
                case PlayerState.WalkDownLeft:
                    DrawPlayerWalkingDownLeft(sb);
                    break;
            }
        }
        /// <summary>
        /// group of methods to draw depending on position
        /// </summary>
        /// <param name="sb"></param>
        private void DrawPlayerFacingLeft(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                4.71f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingLeft(SpriteBatch sb)
        {
            hitBox.X -= speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                4.71f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerFacingRight(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                1.57f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingRight(SpriteBatch sb)
        {
            hitBox.X += speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                1.57f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerFacingUp(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                0,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingUp(SpriteBatch sb)
        {
            hitBox.Y -= speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                0,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerFacingDown(SpriteBatch sb)
        {
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                3.14f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingDown(SpriteBatch sb)
        {
            hitBox.Y += speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                3.14f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingUpLeft(SpriteBatch sb)
        {
            hitBox.Y -= speed;
            hitBox.X -= speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                5.50f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingUpRight(SpriteBatch sb)
       {
            hitBox.Y -= speed;
            hitBox.X += speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                0.79f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingDownRight(SpriteBatch sb)
       {
            hitBox.Y += speed;
            hitBox.X += speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                2.36f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }
        private void DrawPlayerWalkingDownLeft(SpriteBatch sb)
       {
            hitBox.Y += speed;
            hitBox.X -= speed;
            sb.Draw(
                texture,
                new Vector2(hitBox.X, hitBox.Y),
                new Rectangle(
                    frame * RectWidth,
                    OffsetY,
                    RectWidth,
                    RectHeight),
                Color.White,
                3.93f,
                new Vector2(10, 10),
                scale,
                SpriteEffects.None,
                0);
        }

        /// <summary>
        /// draws the players weapon in the direction he's firing
        /// </summary>
        /// <param name="sb"></param>
        private void DrawPlayerWeapon(SpriteBatch sb)
        {
            switch (gunDirection)
            {
                case GunDirection.Left:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        4.71f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.Up:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        0,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.Right:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        1.57f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.Down:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        3.14f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.UpLeft:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        5.50f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.UpRight:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        0.79f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.DownRight:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        2.36f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
                case GunDirection.DownLeft:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            RectWidth + gunOffset,
                            80,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        3.93f,
                        new Vector2(10, 10),
                        scale * 1.2f,
                        SpriteEffects.None,
                        0);
                    break;
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            // Handle animation timing
            // - Add to the time counter
            // - Check if we have enough "time" to advance the frame
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                     // Adjust the frame

                if (frame > WalkFrameCount)     // Check the bounds
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used"
            }
        }

        /// <summary>
        /// resets player stats after game over
        /// </summary>
        public void Reset()
        {
            currentHP = maxHP;
            hitBox.X = 400;
            hitBox.Y = 240;
        }
    }
}
