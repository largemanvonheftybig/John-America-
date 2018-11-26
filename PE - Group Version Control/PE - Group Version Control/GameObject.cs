using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PE___Group_Version_Control
{
    // Dan Robinson
    class GameObject
    {
        protected Texture2D texture;
        protected Rectangle hitBox;
        protected SpriteFont debug;

        public Texture2D GameObj
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle HitBox
        {
            get { return hitBox; }
            set { hitBox = value; }
        }

        public int X
        {
            get { return hitBox.X; }
            set { hitBox.X = value; }
        }
        public int Y
        {
            get { return hitBox.Y; }
            set { hitBox.Y = value; }
        }
        public GameObject(Rectangle hitBox, Texture2D texture, SpriteFont debug)
        {
            this.hitBox = hitBox;
            this.texture = texture;
            this.debug = debug;
        }

        public virtual void Draw(SpriteBatch sb, GameTime gameTime) // draws the basic object sprite
        {
            sb.Draw(texture, hitBox, Color.White);
        }
    }
}
