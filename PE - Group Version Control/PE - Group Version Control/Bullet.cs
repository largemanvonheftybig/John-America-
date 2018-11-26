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
    //Adam McAree
    //Dan Robinson
    class Bullet
    {
        //fields for velocity
        private int xVel;
        private int yVel;
        private int speed;
        //fields for position
        private int xPos;
        private int yPos;
        private Point position;
        //field for rectangle
        private Rectangle hitBox;
        private int size;
        private DirectionState direction;
        private bool collision;
        public bool Collision { get { return collision; } set { collision = value; } }
        public Rectangle HitBox { get { return hitBox; } }
        public Bullet(int posX, int posY, DirectionState dir)
        {
            size = 8;
            speed = 10;
            xPos = posX;
            yPos = posY;
            position = new Point(xPos, yPos);
            direction = dir;
            switch (direction)
            {
                case DirectionState.up:
                    yVel = -speed;
                    xVel = 0;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.down:
                    yVel = speed;
                    xVel = 0;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.left:
                    yVel = 0;
                    xVel = -speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.right:
                    yVel = 0;
                    xVel = speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.upleft:
                    yVel = -speed;
                    xVel = speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.upright:
                    yVel = -speed;
                    xVel = -speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.downright:
                    yVel = speed;
                    xVel = speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.downleft:
                    yVel = speed;
                    xVel = -speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.leftup:
                    yVel = speed;
                    xVel = -speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.leftdown:
                    yVel = -speed;
                    xVel = -speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.rightdown:
                    yVel = speed;
                    xVel = speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
                case DirectionState.rightup:
                    yVel = -speed;
                    xVel = speed;
                    hitBox = new Rectangle(position, new Point(size, size));
                    break;
            }
            collision = false;
        }
        public void ChangePos()
        {
            hitBox.X = hitBox.X + xVel;
            hitBox.Y = hitBox.Y + yVel;
        }
    }
}
