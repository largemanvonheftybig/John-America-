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
    class UserInterface
    {
        // Fields
        private GameState gameState;
        private Option option;
        private Player player;
        private SpriteFont title;
        private Texture2D pistolTexture;
        private Texture2D bar;
        private Texture2D barBG;
        private Texture2D bg;
        private Texture2D htpBG;
        private Texture2D grass;
        private Texture2D gameover;
        private Rectangle selectorLocation;
        private KeyboardState kbOld;
        private bool newGame;

        // Properties
        public GameState GameState
        {
            get { return gameState; }
        }
        public bool NewGame
        {
            get { return newGame; }
            set { newGame = value; }
        }

        // Constructors
        public UserInterface(Player player, SpriteFont title, Texture2D pistolTexture, Texture2D bar, Texture2D barBG, Texture2D bg, Texture2D htpBG, Texture2D grass, Texture2D gameover)
        {
            this.player = player;
            this.title = title;
            this.pistolTexture = pistolTexture;
            this.bar = bar;
            this.barBG = barBG;
            this.bg = bg;
            this.htpBG = htpBG;
            this.grass = grass;
            this.gameover = gameover;

            selectorLocation = new Rectangle(170, 200, 40, 29);
            gameState = GameState.Title;
            option = Option.NewGame;
            newGame = false;
        }

        // Methods
        public void UpdateMenu(KeyboardState kb)
        {
            switch (option)
            {
                case Option.NewGame:
                    selectorLocation.Y = 200;
                    selectorLocation.X = 170;
                    if ((kb.IsKeyDown(Keys.S) && kbOld.IsKeyUp(Keys.S)) || (kb.IsKeyDown(Keys.Down) && kbOld.IsKeyUp(Keys.Down)))
                    {
                        option = Option.HowToPlay;
                    }
                    if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter))
                    {
                        gameState = GameState.InGame;
                        newGame = true;
                    }
                    kbOld = kb;
                    break;
                case Option.HowToPlay:
                    selectorLocation.Y = 230;
                    selectorLocation.X = 170;
                    if ((kb.IsKeyDown(Keys.S) && kbOld.IsKeyUp(Keys.S)) || (kb.IsKeyDown(Keys.Down) && kbOld.IsKeyUp(Keys.Down)))
                    {
                        option = Option.Exit;
                    }
                    else if ((kb.IsKeyDown(Keys.W) && kbOld.IsKeyUp(Keys.W)) || (kb.IsKeyDown(Keys.Up) && kbOld.IsKeyUp(Keys.Up)))
                    {
                        option = Option.NewGame;
                    }
                    if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter)) gameState = GameState.HowToPlay;
                    kbOld = kb;
                    break;
                case Option.Exit:
                    selectorLocation.Y = 260;
                    selectorLocation.X = 170;
                    if ((kb.IsKeyDown(Keys.W) && kbOld.IsKeyUp(Keys.W)) || (kb.IsKeyDown(Keys.Up) && kbOld.IsKeyUp(Keys.Up)))
                    {
                        option = Option.HowToPlay;
                    }
                    if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter)) gameState = GameState.Exit;
                    kbOld = kb;
                    break;

            }

        }

        /// <summary>
        /// Draws the main menu
        /// </summary>
        /// <param name="sb"></param>
        public void DrawMenu(SpriteBatch sb)
        {
            sb.Draw(bg, new Vector2(0, 0), Color.White);
            Vector2 titleSize1 = title.MeasureString("John America");
            Vector2 titleSize2 = title.MeasureString("Jabroni Apocalypse");
            sb.DrawString(title, "JOHN AMERICA", new Vector2(400, 120), Color.Red, 0, new Vector2(titleSize1.X / 2, 0), 0.8f, SpriteEffects.None, 0);
            sb.DrawString(title, "JABRONI APOCALYPSE", new Vector2(400, 150), Color.Red, 0, new Vector2(titleSize2.X/2, 0), 1.5f, SpriteEffects.None, 0);
            sb.DrawString(title, "Start New Game", new Vector2(230, 200), Color.Red);
            sb.DrawString(title, "How to Play", new Vector2(230, 230), Color.Red);
            sb.DrawString(title, "Exit Game", new Vector2(230, 260), Color.Red);

            sb.Draw(pistolTexture, selectorLocation, Color.White);
        }

        /// <summary>
        /// Check if user selected how to play
        /// </summary>
        /// <param name="kb"></param>
        public void UpdateHTP(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter)) gameState = GameState.Title;
            kbOld = kb;
        }

        /// <summary>
        /// draws how to play screen
        /// </summary>
        /// <param name="sb"></param>
        public void DrawHTP(SpriteBatch sb)
        {
            sb.Draw(htpBG, new Vector2(0, 0), Color.White);
            selectorLocation.X = 240;
            selectorLocation.Y = 300;
            sb.DrawString(title,
                "you are john america, president of " +
                "\nthe united states of america. an " +
                "\nevil alien race from planet jabronin " +
                "\nhas invaded earth and is trying to " +
                "\ntake over! it is your duty as " +
                "\npresident to rid earth of these " +
                "\nwretched jabronies! " +
                "\n\nuse wasd to move and use the " +
                "\narrow keys to fire.",
                new Vector2(20, 20),
                Color.Crimson,
                0,
                new Vector2(0, 0),
                0.8f,
                SpriteEffects.None,
                0);
            sb.DrawString(title,
                "return to menu",
                new Vector2(290, 300),
                Color.Crimson,
                0,
                new Vector2(0, 0),
                0.8f,
                SpriteEffects.None,
                0);
            sb.Draw(pistolTexture, selectorLocation, Color.White);
        }

        /// <summary>
        /// draws grass bg
        /// </summary>
        /// <param name="sb"></param>
        public void DrawBG(SpriteBatch sb)
        {
            sb.Draw(grass, new Vector2(0, 0), Color.White);
        }

        /// <summary>
        /// draws health bar
        /// </summary>
        /// <param name="sb"></param>
        public void DrawUserInterface(SpriteBatch sb)
        {
            sb.Draw(barBG, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.8f, SpriteEffects.None, 0.1f);
            sb.Draw(bar, new Vector2(22, 0), new Rectangle(0, 0, player.CurrentHP, 15), Color.White, 0, new Vector2(0, 0), 1.8f, SpriteEffects.None, 0.1f);
        }

        /// <summary>
        /// checks if user has paused the game
        /// </summary>
        /// <param name="kb"></param>
        public void CheckPaused(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Q) && kbOld.IsKeyUp(Keys.Q)) gameState = GameState.Pause;
            if (kb.IsKeyDown(Keys.K) && kbOld.IsKeyUp(Keys.K)) player.CurrentHP = 0;
            kbOld = kb;
        }

        /// <summary>
        /// exits pause menu if enter is pressed
        /// </summary>
        /// <param name="kb"></param>
        public void UpdatePaused(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter)) gameState = GameState.InGame;
            kbOld = kb;
        }

        /// <summary>
        /// draws the pause menu
        /// </summary>
        /// <param name="sb"></param>
        public void DrawPaused(SpriteBatch sb)
        {
            sb.Draw(bg, new Vector2(0, 0), Color.White);
            sb.DrawString(title,
                "paused",
                new Vector2(290, 260),
                Color.Crimson,
                0,
                new Vector2(0, 0),
                1.5f,
                SpriteEffects.None,
                0);
            sb.DrawString(title,
                "press enter",
                new Vector2(290, 300),
                Color.Crimson,
                0,
                new Vector2(0, 0),
                0.8f,
                SpriteEffects.None,
                0);
        }

        /// <summary>
        /// checks if the player's hp is 0 and goes to game over screen
        /// </summary>
        public void CheckGameOver()
        {
            if(player.CurrentHP <= 0)
            {
                gameState = GameState.GameOver;
            }
        }

        /// <summary>
        /// returns to main menu if enter is pressed
        /// </summary>
        /// <param name="kb"></param>
        public void UpdateGameOver(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Enter) && kbOld.IsKeyUp(Keys.Enter)) gameState = GameState.Title;
            kbOld = kb;
        }

        /// <summary>
        /// draws the game over screen
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="highscore"></param>
        /// <param name="score"></param>
        public void DrawGameOver(SpriteBatch sb, int highscore, int score)
        {
            sb.Draw(gameover, new Vector2(0, 0), Color.White);
            sb.DrawString(title,
                "GAME OVER",
                new Vector2(100, 80),
                Color.Red,
                0,
                new Vector2(0, 0),
                2.3f,
                SpriteEffects.None,
                0);
            sb.DrawString(title,
                "PRESS ENTER",
                new Vector2(220, 400),
                Color.Red,
                0,
                new Vector2(0, 0),
                1.2f,
                SpriteEffects.None,
                0);
            string sHighscore = string.Format("Highscore: {0}", highscore);
            string sScore = string.Format("Player Score: {0}", score);
            sb.DrawString(title, sHighscore, new Vector2(220, 300), Color.Red);
            sb.DrawString(title, sScore, new Vector2(215, 330), Color.Red);
        }

    }
}
