using DoomCopy.Enteties;
using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DoomCopy
{
    static class GameMaster
    {
        #region FIELDS

        #region MENU VARS
        private static Menu menu;

        public enum MenuState { Menu, Run, HighScore, Quit }
        public static MenuState curMenuState;
        #endregion

        #region HIGHSCORE VARS
        private enum HighscoreState { PrintHighScore, EnterHighScore, Loading };
        private static HighscoreState curHighscoreState = HighscoreState.EnterHighScore;
        #endregion

        // Global referens till Particle Manager
        public static ParticleManager<ParticleState> ParticleManager { get; private set; }

        #endregion

        #region METHODS
        public static void Initialize()
        {
            // Initiera spelaren
            EntityManager.Add(Player.Instance);

            // Initiera spawners mer rätt spawn rate
            WeaponSpawner.Initialize(10000);
            EnemySpawner.Initialize(100);

            Sound.Initialize();

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
        }

        public static void LoadContent(ContentManager content)
        {
            // Load art
            Art.Load(content);
            Sound.Load(content);

            // Create background
            Background.Initialize();

            // Ladda in highscore
            PlayerManager.Initialize();

            // Initialize menu
            menu = new Menu((int)MenuState.Menu);

            menu.AddItem(Art.MenuBtnStart, (int)MenuState.Run);
            menu.AddItem(Art.MenuBtnHighscore, (int)MenuState.HighScore);
            menu.AddItem(Art.MenuBtnExit, (int)MenuState.Quit);
        }

        // MENU HANDELER
        #region MENU LOGIC
        public static MenuState MenuUpdate()
        {
            return (MenuState)menu.Update();
        }

        public static void MenuDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            menu.Draw(spriteBatch);
            spriteBatch.End();
        }

        // GAME LOGIC / HANDELER
        public static MenuState RunUpdate(ContentManager content, GameWindow window)
        {
            // Updatera spelarens status
            PlayerManager.Update();

            // Updatera partikeleffekter
            ParticleManager.Update();

            // Updatera ljud när det behövs
            Sound.Update();

            // Updatera alla enteties
            EntityManager.Update();

            // Check att det är säkert att lämna till menyn
            if (PlayerManager.isGameOver && curHighscoreState == HighscoreState.PrintHighScore)
            {
                if (Input.WasKeyPressed(Keys.X))
                {
                    return MenuState.Menu;
                }
                else if (Input.WasKeyPressed(Keys.Z))
                {
                    Reset();
                }
            }

            // Om spelaren är död, disable spawners
            if (!PlayerManager.isGameOver)
            {
                // Updatera Spanwers
                EnemySpawner.Update();
                WeaponSpawner.Update();
            }

            return MenuState.Run;
        }
        #endregion

        #region GAME LOGIC
        public static void RunDraw(SpriteBatch spriteBatch)
        {
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

            //spriteBatch.End();

            // Check om splearen har slut på kulor i både vapnet och i reserv
            // Samt om spelaren är död eller håller i knytnävar
            // knytnävar har oändligt med ammunition men behandlas som om de är tomma
            if (int.Parse(PlayerManager.EquipedWeapon.GetAmmunition()) <= 0 &&
                int.Parse(PlayerManager.EquipedWeapon.GetBullets()) <= 0 &&
                !Player.Instance.isDead && !(PlayerManager.EquipedWeapon is Fists))
            {
                // Sinus funktion för att få en flash effekt på texten
                float pulse = 2 * (float)Math.Sin(3f * GameRoot.GameTime.TotalGameTime.TotalSeconds - 1.5f);

                if (pulse > 1.5f)
                {
                    string text = "EMPTY";
                    Vector2 textSize = Art.Font.MeasureString(text);

                    spriteBatch.DrawString(Art.Font, text, (Player.Instance.position - Player.Instance.size) + textSize / 2, Color.White);
                }
                else if (pulse < -1.5f)
                {
                    string text = "PRESS (Q)";
                    Vector2 textSize = Art.Font.MeasureString(text);

                    spriteBatch.DrawString(Art.Font, text, (Player.Instance.position - Player.Instance.size) + textSize / 8, Color.White);
                }
            }

            if (PlayerManager.isGameOver)
            {
                switch (curHighscoreState)
                {
                    case HighscoreState.PrintHighScore:
                        string text = "Game Over";
                        Vector2 textSize = Art.Font.MeasureString(text);
                        Vector2 margin = new Vector2(0, -100);

                        spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - textSize / 2 + margin, Color.Red);

                        PlayerManager.highscore.PrintDraw(spriteBatch);

                        text = "PRESS (x) to GO to Menu - - - PRESS (z) to PLAY AGAIN";
                        textSize = Art.Font.MeasureString(text);
                        margin = new Vector2(0, 100);
                        spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - textSize / 2 + margin, Color.Red);
                        break;

                    case HighscoreState.EnterHighScore:
                        PlayerManager.highscore.EnterDraw(spriteBatch);
                        if (PlayerManager.highscore.EnterUpdate(PlayerManager.Score))
                        {
                            curHighscoreState = HighscoreState.Loading;

                        }
                        break;
                    case HighscoreState.Loading:
                        PlayerManager.highscore.EnterLoading(spriteBatch);
                        Extensions.DelayAction(200, delegate () {
                            curHighscoreState = HighscoreState.PrintHighScore;
                        });
                        break;
                }
            }
            spriteBatch.End();
        }
        #endregion

        #region HIGHSCORE LOGIC
        public static MenuState HighScoreUpdate()
        {
            if (Input.WasKeyPressed(Keys.X))
            {
                return MenuState.Menu;
            }

            return MenuState.HighScore;
        }

        public static void HighScoreDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(0, BlendState.AlphaBlend);
            PlayerManager.highscore.PrintDraw(spriteBatch);

            string text = "PRESS [x] to GO to Menu";
            Vector2 textSize = Art.Font.MeasureString(text);
            Vector2 margin = new Vector2(0, 100);
            spriteBatch.DrawString(Art.Font, text, GameRoot.ScreenSize / 2 - (textSize) / 2 + margin, Color.Red);

            spriteBatch.End();
        }
        #endregion

        private static void Reset()
        {
            PlayerManager.ResetPlayer();

            // Reset highscore enter
            curHighscoreState = HighscoreState.EnterHighScore;
        }

        #endregion

        #region HJÄLPAR FUNKTIONER
        private static void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            GameRoot.spriteBatch.DrawString(Art.Font, text, new Vector2(GameRoot.ScreenSize.X - textWidth - 5, y), Color.White);
        }
        #endregion
    }
}