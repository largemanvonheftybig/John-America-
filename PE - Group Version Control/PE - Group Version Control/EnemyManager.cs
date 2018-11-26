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
    class EnemyManager 
    {
        // Fields
        Game1 window = new Game1();
        List<Grunt> enemyList;
        private int currentWave;
        private int numberOfEnemies;
        private int enemiesPerWave;
        private int gruntSpeed;
        private int gruntHealth;
        private int gunmanSpeed;
        private int gunmanHealth;
        private SpriteFont debug;
        private Texture2D gruntTexture;
        private Rectangle spawn;
        private Player player;
        private Random rng;
        private int score;
        private Timer timer;
        private Timer timer2;
        private bool nextWave;
        private bool nextEnemy;
        private bool maxEnemiesReached;



        // Properties
        public int Score { get { return score; } }
        public List<Grunt> EnemyList { get { return enemyList; } set { enemyList = value; } }
        public int CurrentWave { get { return currentWave; } set { currentWave = value; } }
        public int EnemiesPerWave { get { return enemiesPerWave; } set { enemiesPerWave = value; } }
        public int GruntSpeed { get { return gruntSpeed; } set { gruntSpeed = value; } }
        public int GruntHealth { get { return gruntHealth; } set { gruntHealth = value; } }
        public int GunmanSpeed { get { return gunmanSpeed; } set { gunmanSpeed = value; } }
        public int GunmanHealth { get { return gunmanHealth; } set { gunmanHealth = value; } }

        // Constructors

        public EnemyManager(Player player, Texture2D gruntTexture, SpriteFont debug)
        {
            currentWave = 1;
            enemiesPerWave = 3;
            numberOfEnemies = currentWave + enemiesPerWave;
            enemyList = new List<Grunt>();
            spawn = new Rectangle(0, 0, 20, 20);
            this.gruntTexture = gruntTexture;
            rng = new Random();
            this.player = player;
            this.debug = debug;
            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = 5000;
            timer2 = new Timer();
            timer2.Enabled = false;
            timer2.Interval = 500;
            nextWave = false;
            nextEnemy = false;
            maxEnemiesReached = false;
            score = 0;
        }

        // Methods

        /// <summary>
        /// checks to see if grunt intercepts another grunt before it in the list, if not it will move
        /// otherwise it won't
        /// </summary>
        /// <param name="playerx"></param>
        /// <param name="playery"></param>
        public void Update(int playerx, int playery)
        {
            int c = 0;
            foreach (Grunt g in enemyList)
            {
                List<Grunt> enemycheckList = new List<Grunt>();
                enemycheckList.AddRange(enemyList);
                enemycheckList.Remove(g);
                g.FollowPlayer(player, enemycheckList);
                c++;
            }
            if (enemyList.Count == numberOfEnemies)
            {
                maxEnemiesReached = true;
            }

            if (enemyList.Count == 0 && maxEnemiesReached)
            {
                timer.Start();
                timer.Elapsed += Wait;
                if (nextWave) IncrementWave(playerx, playery);
            }
            AddEnemies();
            Console.WriteLine(maxEnemiesReached);
        }

        /// <summary>
        /// sets up necessary settings to increment the wave
        /// </summary>
        /// <param name="playerx"></param>
        /// <param name="playery"></param>
        public void IncrementWave(int playerx, int playery)
        {
            maxEnemiesReached = false;
            timer.Stop();
            currentWave += 1;
            numberOfEnemies = currentWave + enemiesPerWave;
            spawn = new Rectangle(400, 0, 10, 10);
            enemyList.Add(new Grunt(spawn, gruntTexture, gruntHealth, gruntSpeed, debug));
            nextWave = false;
            nextEnemy = false;
        }

        /// <summary>
        /// adds enemies at random points on outside the screen's bounds
        /// </summary>
        private void AddEnemies()
        {
            int spawnPointPicker = rng.Next(1, 9);
            int x = 0;
            int y = 0;
            switch (spawnPointPicker)
            {
                case 1:
                    x = -20;
                    y = -20;
                    break;
                case 2:
                    x = 400;
                    y = -20;
                    break;
                case 3:
                    x = 820;
                    y = -20;
                    break;
                case 4:
                    x = 820;
                    y = 240;
                    break;
                case 5:
                    x = 820;
                    y = 500;
                    break;
                case 6:
                    x = 400;
                    y = 500;
                    break;
                case 7:
                    x = -20;
                    y = 500;
                    break;
                case 8:
                    x = -20;
                    y = 240;
                    break;
            }
            spawn = new Rectangle(x, y, 20, 20);

            timer2.Start();
            timer2.Elapsed += Wait2;
            if (nextEnemy && maxEnemiesReached == false)
            {
                enemyList.Add(new Grunt(spawn, gruntTexture, gruntHealth, gruntSpeed, debug));
                nextEnemy = false;
            }

        }

        private void Wait(object sender, ElapsedEventArgs e)
        {
            nextWave = true;
        }
        private void Wait2(object sender, ElapsedEventArgs e)
        {
            nextEnemy = true;
        }

        /// <summary>
        /// method to remove enemies if hp is less than or equal to 0
        /// </summary>
        public void RemoveEnemy()
        {
            int countDefeated = 0;
            foreach(Grunt g in enemyList)
            {
                if(g.Hp <= 0)
                {
                    countDefeated++;
                }
            }
            score += (countDefeated * 100);
            enemyList.RemoveAll(Grunt => Grunt.Hp <= 0);
        }

        /// <summary>
        /// Draws each enemy
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="gameTime"></param>
        /// <param name="title"></param>
        public void Draw(SpriteBatch sb, GameTime gameTime, SpriteFont title)
        {
            Vector2 titleSize1 = title.MeasureString("Wave 0");
            Grunt grunt = new Grunt(spawn, gruntTexture, 1, 1, debug);
            //sb.DrawString(debug, "Enemy Count: " + enemyList.Count , new Vector2(0, 40), Color.Black);
            foreach (Grunt g in enemyList)
            {
                g.Draw(sb, gameTime);
            }
            if (timer.Enabled)
            {
                sb.DrawString(title, "Wave " + currentWave, new Vector2(400 - (titleSize1.X/2), 240), Color.Red);
            }
        }

        /// <summary>
        /// Resets relavent settings after game over
        /// </summary>
        public void Reset()
        {
            enemyList.Clear();
            currentWave = 0;
            score = 0;
        }
    }
}
