using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    static class Art
    {
        #region FIELDS

        #region PLAYER ART
        public static Texture2D Player_Melee { get; private set; }
        public static Texture2D Player_Stab { get; private set; }
        public static Texture2D Player_Pistol { get; private set; }
        public static Texture2D Player_Rifle { get; private set; }
        public static Texture2D Player_Shotgun { get; private set; }
        #endregion

        #region WEAPON ART
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Rocket { get; private set; }
        public static Texture2D Grenade { get; private set; }
        public static Texture2D Pistol { get; private set; }
        public static Texture2D Rifle { get; private set; }
        public static Texture2D Shotgun { get; private set; }
        public static Texture2D Minigun { get; private set; }
        public static Texture2D RocketLauncher { get; private set; }
        public static Texture2D GrenadeLauncher { get; private set; }
        #endregion

        #region ENEMY ART
        public static Texture2D Zombie { get; private set; }
        public static Texture2D Loomer { get; private set; }
        public static Texture2D Eye { get; private set; }
        #endregion

        #region EFFECT ART
        public static Texture2D MuzzleFlash { get; private set; }
        public static Texture2D LineParticle { get; private set; }
        public static Texture2D Explosion { get; private set; }
        public static Texture2D FloatingEyes { get; private set; }
        public static Texture2D SpawnCircle { get; private set; }
        #endregion

        #region MISC ART
        public static Texture2D Pointer { get; private set; }
        public static Texture2D Shadow { get; private set; }

        // BACKGROUND TILES
        public static Texture2D BackgroundTile { get; private set; }
        public static Texture2D TransparetnBackgroundTile { get; private set; }

        // MENU ITEMS UI
        public static Texture2D MenuBtnStart { get; private set; }
        public static Texture2D MenuBtnHighscore { get; private set; }
        public static Texture2D MenuBtnExit { get; private set; }

        // FONT
        public static SpriteFont Font { get; private set; }
        #endregion

        #endregion

        #region METHODS

        public static void Load(ContentManager content)
        {
            Zombie = content.Load<Texture2D>("Art/Enemies/Zombie");
            Loomer = content.Load<Texture2D>("Art/Enemies/Loomer");
            Eye = content.Load<Texture2D>("Art/Enemies/Eye");

            Player_Melee = content.Load<Texture2D>("Art/Players/Player_Melee");
            Player_Stab = content.Load<Texture2D>("Art/Players/Player_Stab");
            Player_Pistol = content.Load<Texture2D>("Art/Players/Player_Pistol");
            Player_Rifle = content.Load<Texture2D>("Art/Players/Player_Rifle");
            Player_Shotgun = content.Load<Texture2D>("Art/Players/Player_Shotgun");

            Bullet = content.Load<Texture2D>("Art/Weapons/Bullet");
            Rocket = content.Load<Texture2D>("Art/Weapons/Rocket");
            Grenade = content.Load<Texture2D>("Art/Weapons/Grenade");

            Explosion = content.Load<Texture2D>("Art/Effects/Explosion");
            LineParticle = content.Load<Texture2D>("Art/Effects/Basic");
            FloatingEyes = content.Load<Texture2D>("Art/Effects/Floating_Eye");
            MuzzleFlash = content.Load<Texture2D>("Art/Effects/MuzzleFlash");
            SpawnCircle = content.Load<Texture2D>("Art/Effects/SpawnCircle");

            Pistol = content.Load<Texture2D>("art/Weapons/Pistol");
            Rifle = content.Load<Texture2D>("art/Weapons/Rifle");
            Shotgun = content.Load<Texture2D>("art/Weapons/Shotgun");
            Minigun = content.Load<Texture2D>("art/Weapons/Minigun");
            RocketLauncher = content.Load<Texture2D>("art/Weapons/Rocket_Launcher");
            GrenadeLauncher = content.Load<Texture2D>("art/Weapons/Grenade_Launcher");

            BackgroundTile = content.Load<Texture2D>("art/Background/bg");
            TransparetnBackgroundTile = content.Load<Texture2D>("art/Background/bgt");

            MenuBtnStart = content.Load<Texture2D>("art/menu/start");
            MenuBtnHighscore = content.Load<Texture2D>("art/menu/highscore");
            MenuBtnExit = content.Load<Texture2D>("art/menu/exit");

            Pointer = content.Load<Texture2D>("Art/Pointer");
            Shadow = content.Load<Texture2D>("Art/Shadow");

            Font = content.Load<SpriteFont>("Fonts/GameFont");
        }

        #endregion
    }
}
