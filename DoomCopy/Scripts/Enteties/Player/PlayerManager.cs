using DoomCopy.Enteties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    static class PlayerManager
    {
        #region FIELDS

        private const float multiplierBaseTime = 3.5f;
        private const int maxMultiplier = 20;

        public static int Lives { get; private set; }
        public static int Score { get; private set; }
        public static int Multiplier { get; private set; }
        public static float SpeedModifier { get; set; }

        private static float multiplierTimeLeft;
        private static int scoreForExtraLife;
        public static bool isGameOver { get { return Lives <= 0; } }

        private const string highScoreFilename = "highscore.txt";
        public static HighScore highscore;

        public static Weapon EquipedWeapon { get; private set; }

        #endregion

        #region METHODS

        public static void Initialize()
        {
            highscore = new HighScore(5);
            highscore.LoadHighscore(highScoreFilename);
            ResetPlayer();
        }

        // Anropa när spelet startas om
        public static void ResetPlayer()
        {
            Score = 0;
            Multiplier = 1;
            Lives = 3;
            scoreForExtraLife = 2000;
            multiplierTimeLeft = 0;
            SpeedModifier = 1;

            // Player börjar med knytnävar som vapen
            EquipWeapon(new Fists(Art.Player_Melee));

            // Om spelaren har redan ett vapen
            // Förstör det och equip fists
            DropWeapon();

            Player.Instance.ResetRespawn();
        }

        public static void Update()
        {
            if (Multiplier > 1)
            {
                // uppdatera multiplier timer
                if ((multiplierTimeLeft -= (float)GameRoot.GameTime.ElapsedGameTime.TotalSeconds) <= 0)
                {
                    multiplierTimeLeft = multiplierBaseTime;
                    ResetMultiplier();
                }
            }
        }

        // Equip vapen till spelaren (gör gemom att collidera med vapen på marken)
        public static void EquipWeapon(Weapon weapon)
        {
            if(EquipedWeapon is Fists || EquipedWeapon == null)
            {
                weapon.SetPickedUp(true);
                EquipedWeapon = weapon;
            }
        }

        // Släpp spelarens vapen (gör genom Q)
        public static void DropWeapon()
        {
            if (!(EquipedWeapon is Fists))
            {
                EquipedWeapon.isDestroyed = true;
                EquipedWeapon = null;

                EquipWeapon(new Fists(Art.Player_Melee));
            }
        }

        #region POÄNG HANTERING

        public static void AddPoints(int basePoints)
        {
            if (Player.Instance.isDead)
                return;

            Score += basePoints * Multiplier;
            while (Score >= scoreForExtraLife)
            {
                scoreForExtraLife += 2000;
                Lives++;
            }
        }

        public static void IncreaseMultiplier()
        {
            if (Player.Instance.isDead)
                return;

            multiplierTimeLeft = multiplierBaseTime;
            if (Multiplier < maxMultiplier)
                Multiplier++;
        }

        public static void ResetMultiplier()
        {
            Multiplier = 1;
            //highscore.Add();
        }

        #endregion

        public static void RemoveLife()
        {
            Lives--;
        }

        #endregion
    }
}