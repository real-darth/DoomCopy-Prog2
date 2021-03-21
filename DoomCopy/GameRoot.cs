using DoomCopy.Enteties;
using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DoomCopy
{
    public class GameRoot : Game
    {
        private GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        // Global referens till GameMaster
        public static GameRoot Instance { get; private set; }

        // Global referens till skärmen
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }

        // Global referens till spelets klocka
        public static GameTime GameTime { get; private set; }

        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Sätt en standard skärmstorlek
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 1042;
        }

        protected override void Initialize()
        {
            base.Initialize();

            GameMaster.curMenuState = GameMaster.MenuState.Menu;
            GameMaster.Initialize();

            /*
            // Initiera spelaren
            EntityManager.Add(Player.Instance);

            // Initiera spawners mer rätt spawn rate
            WeaponSpawner.Initialize(10000);
            EnemySpawner.Initialize(100);
            */

            //Sound.Initialize();

            //ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
        }

        protected override void LoadContent()
        {
            /*
            // Ladda in alla sprites och ljud i spelet
            Art.Load(Content);
            Sound.Load(Content);

            Background.Initialize();

            // Ladda in highscore
            PlayerManager.Initialize();
            */
            // Ladda in highscore
            //PlayerManager.highscore.LoadHighscore("highscore.txt");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Content
            GameMaster.LoadContent(Content);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            PlayerManager.highscore.SaveHighscore("highscore.txt");
        }

        protected override void Update(GameTime gameTime)
        {
            // Global variable, game time
            GameTime = gameTime;

            // Updatera player input
            // Måste vara aktiv alltid
            Input.Update();

            // Close Game shortcut
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (GameMaster.curMenuState)
            {
                case GameMaster.MenuState.Run:
                    GameMaster.curMenuState = GameMaster.RunUpdate(Content, Window);
                    break;
                case GameMaster.MenuState.HighScore:
                    GameMaster.curMenuState = GameMaster.HighScoreUpdate();
                    break;
                case GameMaster.MenuState.Quit:
                    Exit();
                    break;
                default: // MENU
                    GameMaster.curMenuState = GameMaster.MenuUpdate();
                    break;
            }

            /*
            // Updatera spelarens status
            PlayerManager.Update();

            // Updatera partikeleffekter
            //ParticleManager.Update();

            // Updatera ljud när det behövs
            Sound.Update();



            // Updatera alla enteties
            EntityManager.Update();

            if (PlayerManager.isGameOver)
                return;

            // Spawna fiender
            EnemySpawner.Update();
            WeaponSpawner.Update();
            */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Pulserande bakgrundfärg
            float pulseSize = 0.22f + -0.2f * (float)Math.Sin(0.8f * GameTime.TotalGameTime.TotalSeconds);
            Color color = Extensions.HueUtility.HSVToColor(pulseSize, 0.7f, 0.8f);

            // Clear Screen every frame
            GraphicsDevice.Clear(color);

            // Rita background
            spriteBatch.Begin(0);
            Background.Draw(spriteBatch);
            spriteBatch.End();

            switch (GameMaster.curMenuState)
            {
                case GameMaster.MenuState.Run:
                    GameMaster.RunDraw(spriteBatch);
                    break;
                case GameMaster.MenuState.HighScore:
                    GameMaster.HighScoreDraw(spriteBatch);
                    break;
                case GameMaster.MenuState.Quit:
                    Exit();
                    break;
                default: // MENU
                    GameMaster.MenuDraw(spriteBatch);
                    break;
            }

            // Rita custom muspekare
            spriteBatch.Begin(0, BlendState.Additive);
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            spriteBatch.End();

            /*
            // Draw Partikeleffekter
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            ParticleManager.Draw(spriteBatch);
            spriteBatch.End();

            // Draw alla aktiva enteties, sotera sprites för bättre batching
            spriteBatch.Begin(0, BlendState.AlphaBlend);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            // Draw user interface
            spriteBatch.Begin(0, BlendState.Additive);

            spriteBatch.DrawString(Art.Font, "Lives: " + PlayerManager.Lives, new Vector2(5), Color.White);
            DrawRightAlignedString("Score: " + PlayerManager.Score, 5);
            DrawRightAlignedString("Multiplier: " + PlayerManager.Multiplier, 35);

            DrawRightAlignedString(PlayerManager.EquipedWeapon.GetReloadTime(), 80);
            DrawRightAlignedString(
                PlayerManager.EquipedWeapon.ToString() +
                " | " +
                PlayerManager.EquipedWeapon.GetBullets() +
                "/" +
                PlayerManager.EquipedWeapon.GetAmmunition(),
                60);

            // Draw custom muspekare
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);
            */

            //spriteBatch.End();

            base.Draw(gameTime);
        }

        /*
        #region HJÄLPAR FUNKTIONER
        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
        #endregion
        */
    }
}
