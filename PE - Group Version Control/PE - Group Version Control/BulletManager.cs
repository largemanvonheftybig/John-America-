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
    //Owen King
    class BulletManager
    {
        private WeaponState wState;
        //fields for x value and y value of when to remove bullet if no collision occurs
        private int xMax;
        private int yMax;
        //field for texture 2d 
        private Texture2D bulletTexture;
        private SpriteFont weaponName;
        //field for Bullet List
        private List<Bullet> bulletManager;
        //field for returning amount of bullets in list
        public String BulletManagerCount { get { string s = Convert.ToString(bulletManager.Count); return s; } }
        public List<Bullet> BulletList { get { return bulletManager; } set { bulletManager = value; } }
        Timer timer = new Timer();
        private int shotgunAmmo;
        private int rifleAmmo;
        private bool firing;
        private Player player;

        public WeaponState WState
        {
            get { return wState; }
            set { wState = value; }
        }

        public int ShotgunAmmo
        {
            get { return shotgunAmmo; }
            set { shotgunAmmo = value; }
        }

        public int RifleAmmo
        {
            get { return rifleAmmo; }
            set { rifleAmmo = value; }
        }

        /// <summary>
        /// sets the value for the x and y maximum 
        /// sets the texture 2d object
        /// initializes the bullet list
        /// </summary>
        /// <param name="xMaximum"></param>
        /// <param name="yMaximum"></param>
        public BulletManager(int xMaximum, int yMaximum, Texture2D tex, SpriteFont weaponName, Player player)
        {
            xMax = xMaximum;
            yMax = yMaximum;
            bulletTexture = tex;
            bulletManager = new List<Bullet>();
            timer = new Timer();
            timer.Elapsed += Wait;
            timer.Interval = 80;
            timer.Enabled = true;
            this.weaponName = weaponName;
            this.player = player;
           
        }

        private void Wait(object sender, ElapsedEventArgs e)
        {
            firing = true;
        }

        /// <summary>
        /// add a bullet to the bulletmanager
        /// checks to see if a bullet can be added, takes in the parameters for 2 keyboardstates and rectangle to get where to shot
        /// </summary>
        /// <param name="b"></param>
        public void Add(KeyboardState kb, KeyboardState kbInitial, Rectangle player)
        {
            switch (wState)
            {
                case WeaponState.Pistol:
                    this.player.GunOffset = 0;
                    if (kb.IsKeyDown(Keys.Right) && kbInitial.IsKeyUp(Keys.Right))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.right));
                        this.player._GunDirection = GunDirection.Right;
                    }
                    else if (kb.IsKeyDown(Keys.Left) && kbInitial.IsKeyUp(Keys.Left))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.left));
                        this.player._GunDirection = GunDirection.Left;
                    }
                    else if (kb.IsKeyDown(Keys.Down) && kbInitial.IsKeyUp(Keys.Down))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.down));
                        this.player._GunDirection = GunDirection.Down;
                    }
                    else if (kb.IsKeyDown(Keys.Up) && kbInitial.IsKeyUp(Keys.Up))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.up));
                        this.player._GunDirection = GunDirection.Up;
                    }
                    break;
                case WeaponState.AssualtRifle:
                    this.player.GunOffset = 20;
                    if (rifleAmmo <= 0) wState = WeaponState.Pistol;
                    if (kb.IsKeyDown(Keys.Right) && firing)
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.right));
                        rifleAmmo -= 1;
                        this.player._GunDirection = GunDirection.Right;
                    }
                    else if (kb.IsKeyDown(Keys.Left) && firing)
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.left));
                        rifleAmmo -= 1;
                        this.player._GunDirection = GunDirection.Left;
                    }
                    else if (kb.IsKeyDown(Keys.Down) && firing)
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.down));
                        rifleAmmo -= 1;
                        this.player._GunDirection = GunDirection.Down;
                    }
                    else if (kb.IsKeyDown(Keys.Up) && firing)
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.up));
                        rifleAmmo -= 1;
                        this.player._GunDirection = GunDirection.Up;
                    }
                    firing = false;
                    break;
                case WeaponState.Shotgun:
                    this.player.GunOffset = 40;
                    if (shotgunAmmo <= 0) wState = WeaponState.Pistol;
                    if (kb.IsKeyDown(Keys.Right) && kbInitial.IsKeyUp(Keys.Right))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.rightup));
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 10, DirectionState.right));
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + -6, DirectionState.rightdown));
                        shotgunAmmo -= 1;
                        this.player._GunDirection = GunDirection.Right;
                    }
                    else if (kb.IsKeyDown(Keys.Left) && kbInitial.IsKeyUp(Keys.Left))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.leftup));
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 10, DirectionState.left));
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + -6, DirectionState.leftdown));
                        shotgunAmmo -= 1;
                        this.player._GunDirection = GunDirection.Left;
                    }
                    else if (kb.IsKeyDown(Keys.Down) && kbInitial.IsKeyUp(Keys.Down))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.downright));
                        bulletManager.Add(new Bullet(player.X + 10, player.Y + 2, DirectionState.down));
                        bulletManager.Add(new Bullet(player.X + -6, player.Y + 2, DirectionState.downleft));
                        shotgunAmmo -= 1;
                        this.player._GunDirection = GunDirection.Down;
                    }
                    else if (kb.IsKeyDown(Keys.Up) && kbInitial.IsKeyUp(Keys.Up))
                    {
                        bulletManager.Add(new Bullet(player.X + 2, player.Y + 2, DirectionState.upright));
                        bulletManager.Add(new Bullet(player.X + 10, player.Y + 2, DirectionState.up));
                        bulletManager.Add(new Bullet(player.X + -6, player.Y + 2, DirectionState.upleft));
                        shotgunAmmo -= 1;
                        this.player._GunDirection = GunDirection.Up;
                    }
                    break;
            }
        }
        


        /// <summary>
        /// draws each bullet within the list
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Bullet b in bulletManager)
            {
                spriteBatch.Draw(bulletTexture, b.HitBox, Color.White);
            }
            //spriteBatch.DrawString(weaponName, "Shotgun Ammo: " + shotgunAmmo, new Vector2(0, 90), Color.White);
            //spriteBatch.DrawString(weaponName, "Rifle Ammo: " + rifleAmmo, new Vector2(0, 120), Color.White);

            switch (wState)
            {
                case WeaponState.Pistol:
                    //spriteBatch.DrawString(weaponName, "PISTOL", new Vector2(200, 0), Color.Black);
                    break;
                case WeaponState.AssualtRifle:
                    //spriteBatch.DrawString(weaponName, "ASSUALT RIFLE", new Vector2(200, 0), Color.Yellow);
                    break;
                case WeaponState.Shotgun:
                    //spriteBatch.DrawString(weaponName, "SHOTGUN", new Vector2(200, 0), Color.Orange);
                    break;
            }
        }
        /// <summary>
        /// checks to see if bullet is out of range and if it is, it removes it
        /// </summary>
        public void OutofRange()
        {
            bulletManager.RemoveAll(Bullet => Bullet.HitBox.X <= 0);
            bulletManager.RemoveAll(Bullet => Bullet.HitBox.X >= xMax);
            bulletManager.RemoveAll(Bullet => Bullet.HitBox.Y <= 0);
            bulletManager.RemoveAll(Bullet => Bullet.HitBox.Y >= yMax);
            bulletManager.RemoveAll(Bullet => Bullet.Collision == true);
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            
        }
        /// <summary>
        /// updates the position for each bullet
        /// </summary>
        public void UpdateBullet()
        {
            foreach(Bullet b in bulletManager)
            {
                b.ChangePos();
            }
        }


        public void Reset()
        {
            bulletManager.Clear();
        }
        //will initialize once we have enemylist
        /*
        public void HitEnemy()
        {

        }
        */
    }
}
