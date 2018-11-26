using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PE___Group_Version_Control
{
    // Owen King
    class Heart : GameObject
    {
            public Heart(Rectangle hitBox, Texture2D texture, SpriteFont debug)
               : base(hitBox, texture, debug)
            {
                this.hitBox = hitBox;
                this.texture = texture;
                this.debug = debug;

            }

            public override void Draw(SpriteBatch sb, GameTime gameTime)
            {
                sb.Draw(texture, hitBox, Color.White);
            }
        }
    }



