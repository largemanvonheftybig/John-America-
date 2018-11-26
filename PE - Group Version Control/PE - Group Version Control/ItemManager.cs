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

    class ItemManager : GameObject
    {
        private Random rng;

        public ItemManager(Rectangle hitBox, Texture2D texture, SpriteFont debug)
           : base(hitBox, texture, debug)
        {
            this.hitBox = hitBox;
            this.texture = texture;
            this.debug = debug;
            rng = new Random();
        }

        public void SpawnItem(Rectangle hitbox, Texture2D texture, SpriteFont debug)
        {
            int randNum = rng.Next(1, 3);
            if(randNum == 1)
            {
                AssaultRifle ar = new AssaultRifle(hitBox, texture, debug);
            }
            else if(randNum == 2)
            {
                Shotgun sg = new Shotgun(hitBox, texture, debug);
            }

        }
        public override void Draw(SpriteBatch sb, GameTime gameTime)
        {
            sb.Draw(texture, hitBox, Color.White);
        }

    }
}
