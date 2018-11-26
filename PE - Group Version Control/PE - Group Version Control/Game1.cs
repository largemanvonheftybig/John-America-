using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PE___Group_Version_Control
{
    /// <summary>
    /// It is me, Owen
    /// </summary>
    enum DirectionState { up, down, left, right, upleft, upright, downleft, downright, leftup, leftdown, rightup, rightdown }
    enum PlayerState { FaceLeft, WalkLeft, FaceRight, WalkRight, FaceUp, WalkUp, FaceDown, WalkDown,
                       WalkUpLeft, WalkUpRight, WalkDownLeft, WalkDownRight}
    enum GunDirection { Left, Right, Up, Down, UpLeft, UpRight, DownLeft, DownRight}
    enum GruntState { Walking, Attacking, Dead}
    enum WeaponState { Pistol, AssualtRifle,Shotgun}
    enum GameState { Title, HowToPlay, Exit, InGame, Pause, GameOver}
    enum Option { NewGame, HowToPlay, Exit}

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // File IO
        private FileStream fileIn;
        private BinaryReader binaryIn;

        // Fields for textures
        private Texture2D blue;
        private Texture2D red;
        private Texture2D rifle;
        private Texture2D shotgun;
        private Texture2D pistolTexture;
        private Texture2D spriteSheet;
        private Texture2D bar;
        private Texture2D barBG;
        private Texture2D heart;
        private Texture2D bg;
        private Texture2D htpBG;
        private Texture2D grass;
        private Texture2D gameover;

        private SpriteFont debug;
        private SpriteFont count;
        private SpriteFont title;

        // Sounds
        private Song song;


        // Objects
        private Player player;
        private AssaultRifle ar;
        private Shotgun sg;
        private Heart Heart;


        //one more time
        private int width;
        private int height;
        private int kills;
        private Random rng;
        private int highscore;
        private int pScore;
        private bool scoreRead;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        // Rectangles for testing
        private KeyboardState kbInitial;

        // Managers
        private BulletManager bm;
        private EnemyManager em;
        private ItemManager im;
        private UserInterface ui;

        private int killsBeforeSpawn;
        private int rifleSpawnAmmo;
        private int shotgunSpawnAmmo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            kbInitial = Keyboard.GetState();
            scoreRead = false;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            binaryIn = null;
            fileIn = null;
            width = GraphicsDevice.Viewport.Width;
            height = GraphicsDevice.Viewport.Height;
            rng = new Random();
            debug = Content.Load<SpriteFont>("Debug");
            count = Content.Load<SpriteFont>("Debug");
            title = Content.Load<SpriteFont>("Title");
            red = Content.Load<Texture2D>("redbox");
            spriteSheet = Content.Load<Texture2D>("Mario");
            pistolTexture = Content.Load<Texture2D>("Pistol");
            rifle = Content.Load<Texture2D>("rifle");
            shotgun = Content.Load<Texture2D>("shotgun");
            heart = Content.Load<Texture2D>("HEART");
            bar = Content.Load<Texture2D>("bar");
            barBG = Content.Load<Texture2D>("barBG");
            bg = Content.Load<Texture2D>("bg");
            htpBG = Content.Load<Texture2D>("htpBG");
            grass = Content.Load<Texture2D>("grass");
            gameover = Content.Load<Texture2D>("gameover");
            song = Content.Load<Song>("real_american_good_version");

            //MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState kb = Keyboard.GetState();
            if (player == null)
            {
                player = new Player(new Rectangle(0, 0, 20, 20), spriteSheet, debug);
                em = new EnemyManager(player, spriteSheet, debug);
                im = new ItemManager(new Rectangle(0, 0, 0, 0), rifle, debug);
                ar = new AssaultRifle(new Rectangle(0, 0, 0, 0), rifle, debug);
                sg = new Shotgun(new Rectangle(0, 0, 0, 0), shotgun, debug);
                Heart = new Heart(new Rectangle(0, 0, 0, 0), heart, debug);
                bm = new BulletManager(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, red, debug, player);
                ui = new UserInterface(player, title, pistolTexture, bar, barBG, bg, htpBG, grass, gameover);
                LoadSettings();
            }
            if (ui.GameState == GameState.Title)
            {
                ui.UpdateMenu(kb);
            }
            else if (ui.GameState == GameState.HowToPlay)
            {
                ui.UpdateHTP(kb);
            }
            else if (ui.GameState == GameState.Exit)
            {
                Exit();
            }
            else if (ui.GameState == GameState.InGame)
            {
                if (ui.NewGame == true)
                {
                    player.Reset();
                    em.Reset();
                    bm.Reset();
                    ui.NewGame = false;
                }
                if (player != null) player.UpdateSprite(kb, gameTime);
                SpawnItem();
                CheckCollision();
                bm.Add(kb, kbInitial, player.HitBox);
                bm.UpdateBullet();
                em.Update(player.HitBox.X, player.HitBox.Y);
                bm.OutofRange();
                em.RemoveEnemy();
                ui.CheckPaused(kb);
                ui.CheckGameOver();
                kbInitial = kb;
            }
            else if (ui.GameState == GameState.Pause)
            {
                ui.UpdatePaused(kb);
            }
            else if (ui.GameState == GameState.GameOver)
            {
                ScoreCheck();
                ui.UpdateGameOver(kb);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            if (ui.GameState == GameState.Title)
            {
                ui.DrawMenu(spriteBatch);
            }
            else if (ui.GameState == GameState.HowToPlay)
            {
                ui.DrawHTP(spriteBatch);
            }
            else if (ui.GameState == GameState.InGame && ui.NewGame == false)
            {
                ui.DrawBG(spriteBatch);
                player.Draw(spriteBatch, gameTime);
                bm.Draw(spriteBatch);
                em.Draw(spriteBatch, gameTime, title);
                sg.Draw(spriteBatch, gameTime);
                ar.Draw(spriteBatch, gameTime);
                Heart.Draw(spriteBatch, gameTime);
                //spriteBatch.DrawString(debug, bm.BulletManagerCount, new Vector2(100, 20), Color.Black);
                //spriteBatch.DrawString(debug, "kills: " + kills, new Vector2(100, 100), Color.Black);
                //spriteBatch.DrawString(debug, "current wave: " + em.CurrentWave, new Vector2(400, 20), Color.Black);
                ui.DrawUserInterface(spriteBatch);
            }
            else if (ui.GameState == GameState.Pause)
            {
                ui.DrawPaused(spriteBatch);
            }
            else if (ui.GameState == GameState.GameOver)
            {
                ui.DrawGameOver(spriteBatch, highscore, em.Score);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
        /// <summary>
        /// checks the collision between the bullets and the enemy list
        /// collision between player and enemy has yet to be created
        /// </summary>
        public void CheckCollision()
        {
            List<Bullet> bl = bm.BulletList;
            List<Grunt> el = em.EnemyList;
            foreach(Bullet b in bl)
            {
                foreach(Grunt g in el)
                {
                    if (b.HitBox.Intersects(g.HitBox) && b.Collision != true)
                    {
                        b.Collision = true;
                        g.Hit();
                        if(bm.WState == WeaponState.Pistol)
                        {
                           kills++;
                        }
                      
                    }
                }
            }
            foreach (Grunt g in el)
            {
                if (g.HitBox.Intersects(player.HitBox))
                {
                    player.CurrentHP--;
                }
            }

            bm.BulletList = bl;
            em.EnemyList = el;

            if (player.HitBox.Intersects(ar.HitBox))
            {
                bm.RifleAmmo = rifleSpawnAmmo;
                bm.WState = WeaponState.AssualtRifle;
                ar = new AssaultRifle(new Rectangle(0, 0, 0, 0), rifle, debug);
                sg = new Shotgun(new Rectangle(0, 0, 0, 0), shotgun, debug);

            }
            if (player.HitBox.Intersects(sg.HitBox))
            {
                bm.ShotgunAmmo = shotgunSpawnAmmo;
                bm.WState = WeaponState.Shotgun;
                sg = new Shotgun(new Rectangle(0, 0, 0, 0), shotgun, debug);
                ar = new AssaultRifle(new Rectangle(0, 0, 0, 0), rifle, debug);

            }
            if (player.HitBox.Intersects(Heart.HitBox))
            {
                player.CurrentHP += 25;
                Heart = new Heart(new Rectangle(0, 0, 0, 0), heart, debug);
            }
        }
        public void SpawnItem()
        {
            if (kills >= killsBeforeSpawn)
            {
                kills = 0;
                int randNum = rng.Next(1, 3);
                int randX = rng.Next(0, 800);
                int randY = rng.Next(0, 480);
                int heartX = rng.Next(0, 800);
                int heartY = rng.Next(0, 480);
                if (randNum == 1)
                {
                    ar = new AssaultRifle(new Rectangle(randX, randY, 40, 10), rifle, debug);
                }
                else if (randNum == 2)
                {
                    sg = new Shotgun(new Rectangle(randX, randY, 40, 10), shotgun, debug);
                }
                if(player.CurrentHP <= 40)
                {
                    Heart = new Heart(new Rectangle(heartX, heartY, 40, 40), heart, debug);
                }
            }
        }
        
        /// <summary>
        /// loads settings from external tool and reads them in as binary
        /// </summary>
        public void LoadSettings()
        {
            try
            {
                fileIn = File.OpenRead("../../../../data.txt");
                binaryIn = new BinaryReader(fileIn);
                player.Speed = binaryIn.ReadInt32();
                player.MaxHp = binaryIn.ReadInt32();
                em.CurrentWave = binaryIn.ReadInt32();
                em.EnemiesPerWave = binaryIn.ReadInt32();
                int gruntSpawnChance = binaryIn.ReadInt32();
                int gunmanSpawnChance = binaryIn.ReadInt32();
                em.GruntSpeed = binaryIn.ReadInt32();
                em.GruntHealth = binaryIn.ReadInt32();
                em.GunmanSpeed = binaryIn.ReadInt32();
                em.GunmanHealth = binaryIn.ReadInt32();
                killsBeforeSpawn = binaryIn.ReadInt32();
                rifleSpawnAmmo = binaryIn.ReadInt32();
                shotgunSpawnAmmo = binaryIn.ReadInt32();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (binaryIn != null)
            {
                binaryIn.Close();
            }
        }
        /// <summary>
        /// I am ashamed of the try catch try catch, but it works and that is all the matter
        /// </summary>
        public void ScoreCheck()
        {
            string fileName = "highscore";
            pScore = em.Score;
            if(scoreRead != true)
            {
                try
                {
                    FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader input = new BinaryReader(inStream);
                    try
                    {
                        highscore = input.ReadInt32();
                        if (highscore < pScore)
                        {
                            inStream.Close();
                            System.Threading.Thread.Sleep(100);
                            BinaryWriter output = new BinaryWriter(inStream);
                            output.Write(pScore);
                            output.Close();
                            highscore = pScore;
                        }
                    }
                    catch
                    {
                        FileStream outStream = File.OpenWrite(fileName);
                        BinaryWriter output = new BinaryWriter(outStream);
                        output.Write(pScore);
                        output.Close();
                        highscore = pScore;
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        FileStream outStream = File.OpenWrite(fileName);
                        BinaryWriter output = new BinaryWriter(outStream);
                        output.Write(pScore);
                        output.Close();
                        highscore = pScore;
                    }
                    catch
                    {
                        FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        BinaryReader input = new BinaryReader(inStream);
                        highscore = input.ReadInt32();
                        if (highscore < pScore)
                        {
                            inStream.Close();
                            System.Threading.Thread.Sleep(100);
                            BinaryWriter output = new BinaryWriter(inStream);
                            output.Write(pScore);
                            output.Close();
                            highscore = pScore;
                        }
                    }
                }
            }
            
            /*string fileName = "highscore";
            if(scoreRead != true)
            {
                pScore = em.Score;
                try
                {
                    FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader input = new BinaryReader(inStream);
                    pScore = em.Score;
                    highscore = input.ReadInt32();
                    if (highscore < pScore)
                    {
                        inStream.Close();
                        BinaryWriter output = new BinaryWriter(inStream);
                        output.c
                        output.Write(pScore);
                        output.Close();
                        highscore = pScore;
                    }
                    scoreRead = true;
                }
                catch
                {
                    FileStream outStream = File.OpenWrite(fileName);
                    BinaryWriter output = new BinaryWriter(outStream);
                    output.Write(pScore);
                    output.Close();
                    highscore = pScore;
                }
                scoreRead = true;
            }*/
        }   
    }
}
