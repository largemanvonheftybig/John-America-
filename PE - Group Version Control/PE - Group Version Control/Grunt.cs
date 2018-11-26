using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;



namespace PE___Group_Version_Control
{
    //Adam McAree
    //Dan Robinson
    class Grunt : GameObject
    {
        // Fields

        private int hp;
        private int speed;
        private float angleToPlayer;
        private float scale;
        private GruntState gruntState;
        private bool attacking;
        private bool dying;
        private int initialSpeed;
        Timer timer;
        Timer timer2;
        Timer timer3;

        // Animation
        int frame;              // The current animation frame
        double timeCounter;     // The amount of time that has passed
        double fps;             // The speed of the animation
        double timePerFrame;    // The amount of time (in fractional seconds) per frame
        int frameCount = 8;       // The number of frames in the animation


        // Constants for "source" rectangle (inside the image)
        const int OffsetY = 20;            // How far down in the image are the frames?
        const int RectHeight = 20;          // The height of a single frame
        const int RectWidth = 20;           // The width of a single frame
        



        // Properties

        public int Hp { get { return hp; } set { hp = value; } }

      

        // Constructors

        public Grunt(Rectangle hitBox, Texture2D texture, int hp, int speed, SpriteFont debug)
            : base(hitBox, texture, debug)
        {
            fps = 10.0;
            timePerFrame = 1.0 / fps;
            this.hitBox = hitBox;
            this.texture = texture;
            this.debug = debug;
            this.hp = hp;
            this.speed = speed;
            initialSpeed = speed;
            dying = false;
            scale = 2.0f;
            gruntState = GruntState.Walking;
            //hp = 1;

            timer = new Timer();
            timer.Elapsed += SpeedUp;
            timer.Interval = 3500;
            timer.Enabled = true;

            timer2 = new Timer();
            timer2.Elapsed += AttackDelay;
            timer2.Interval = 500;
            timer2.Enabled = false;

            timer3 = new Timer();
            timer3.Elapsed += Death;
            timer3.Interval = 500;
            timer3.Enabled = false;
        }

        // Methods
        /// <summary>
        /// Makes the enemies speed up after two seconds if they are still alive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUp(object sender, ElapsedEventArgs e)
        {
            speed = speed + 1;
        }

        private void AttackDelay(object sender, ElapsedEventArgs e)
        {
            speed = initialSpeed;
            attacking = false;
            timer2.Enabled = false;
        }

        private void Death(object sender, ElapsedEventArgs e)
        {
            hp = 0;
        }



        /// <summary>
        /// method to take damage
        /// </summary>
        public void Hit()
        {
            if (hp == 1)
            {
                hitBox.Width = 0;
                hitBox.Height = 0;
                frame = 0;
                timer3.Enabled = true;
                dying = true;
                gruntState = GruntState.Dead;
            }
            else
            {
                hp--;
            }
        }
        public void FollowPlayer(Player player, List<Grunt> gruntList)
        {
            Vector2 enemyPosition = new Vector2(hitBox.X, hitBox.Y);
            Vector2 playerPosition = new Vector2(player.X, player.Y);
            Vector2 difference = playerPosition - enemyPosition;
            angleToPlayer = AngleBetween(playerPosition, enemyPosition);
            difference = Vector2.Normalize(difference);
            enemyPosition = enemyPosition + (difference * speed);
            
            if (HitBox.Intersects(player.HitBox) && !dying)
            {
                if (frame > 5) frame = 1;
                frameCount = 5;
                gruntState = GruntState.Attacking;
                attacking = true;
                timer2.Enabled = true;
            }
            else if (!attacking && !dying)
            {
                frameCount = 8;
                gruntState = GruntState.Walking;
                if((Convert.ToInt32(difference.X) * speed) < 1 && (Convert.ToInt32(difference.X) * speed) > -1)
                {
                    if(player.X > HitBox.X)
                    {
                        hitBox.X++;
                    }
                    else
                    {
                        hitBox.X--;
                    }
                }
                else
                {
                    hitBox.X = Convert.ToInt32(enemyPosition.X) + (Convert.ToInt32(difference.X) * speed);
                }
                if ((Convert.ToInt32(difference.Y) * speed) < 1 && (Convert.ToInt32(difference.Y) * speed) > - 1)
                {
                    if (player.Y > HitBox.Y)
                    {
                        hitBox.Y++;
                    }
                    else
                    {
                        hitBox.Y--;
                    }
                }
                else
                {
                    hitBox.Y = Convert.ToInt32(enemyPosition.Y) + (Convert.ToInt32(difference.Y) * speed);
                }
            }
            if (gruntList == null)
            {

            }
            else
            {
                foreach (Grunt g in gruntList)
                {
                    if (hitBox.Intersects(g.hitBox))
                    {
                        
                        while (hitBox.Intersects(g.hitBox))
                        {
                            /*if (hitBox.X >= g.X)
                            {
                                hitBox.X = hitBox.X + 6;
                            }
                            else if (hitBox.X < g.X)
                            {
                                hitBox.X = hitBox.X - 6;
                            }
                            if (hitBox.Y >= g.Y)
                            {
                                hitBox.Y = hitBox.Y + 6;
                            }
                            else if (hitBox.Y < g.Y)
                            {
                                hitBox.Y = hitBox.Y - 6;
                            }*/
                            Vector2 gVector = new Vector2(g.X, g.Y);
                            enemyPosition = new Vector2(hitBox.X, hitBox.Y);
                            difference = gVector - enemyPosition;
                            difference = difference * -1;
                            difference = Vector2.Normalize(difference);
                            hitBox.X = Convert.ToInt32(enemyPosition.X) + (Convert.ToInt32(difference.X) * speed);
                            hitBox.Y = Convert.ToInt32(enemyPosition.Y) + (Convert.ToInt32(difference.Y) * speed);
                        }
                    }
                }
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

                if (frame > frameCount)     // Check the bounds
                    frame = 1;                  // Back to 1 (since 0 is the "standing" frame)

                timeCounter -= timePerFrame;    // Remove the time we "used"
            }
        }


        /// <summary>
        /// method for drawing enemy
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            switch (gruntState)
            {
                case GruntState.Walking:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            frame * RectWidth,
                            OffsetY,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        angleToPlayer - 1.57f,
                        new Vector2(10, 10),
                        scale,
                        SpriteEffects.None,
                        0);
                    break;
                case GruntState.Attacking:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            frame * RectWidth,
                            OffsetY + 20,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        angleToPlayer - 1.57f,
                        new Vector2(10, 10),
                        scale,
                        SpriteEffects.None,
                        0);
                    break;
                case GruntState.Dead:
                    sb.Draw(
                        texture,
                        new Vector2(hitBox.X, hitBox.Y),
                        new Rectangle(
                            frame * RectWidth,
                            OffsetY + 40,
                            RectWidth,
                            RectHeight),
                        Color.White,
                        0,
                        new Vector2(10, 10),
                        scale*1.2f,
                        SpriteEffects.None,
                        0);
                    break;
            }
        }

        /// <summary>
        /// gets the angle between two vectors
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        private float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            return (float)Math.Atan2(vector2.Y - vector1.Y, vector2.X - vector1.X);
        }
    }
}
