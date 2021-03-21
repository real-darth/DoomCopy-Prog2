using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    static class Sound
    {
        private static Song[] gameMusic { get; set; }
        private static Song GameMusic { get { return gameMusic[rand.Next(gameMusic.Length)]; } }
        public static Song menuMusic { get; private set; }

        private static float countDuration = 0f; // 1f = 1 second

        private static readonly Random rand = new Random();

        private static SoundEffect player;
        public static SoundEffect Player { get { return player; } }

        private static SoundEffect[] explosions;
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        #region WEAPONS
        private static SoundEffect pistol;
        public static SoundEffect Pistol { get { return pistol; } }

        private static SoundEffect grenadeLauncher;
        public static SoundEffect GrenadeLauncher { get { return grenadeLauncher; } }

        private static SoundEffect rocketLauncher;
        public static SoundEffect RocketLauncher { get { return rocketLauncher; } }

        private static SoundEffect ak47;
        public static SoundEffect Ak47 { get { return ak47; } }

        public static Dictionary<string, SoundEffect> Shotgun = new Dictionary<string, SoundEffect>();
        public static Dictionary<string, SoundEffect> Minigun = new Dictionary<string, SoundEffect>();
        #endregion

        #region ENEMIES
        private static SoundEffect loomerDeath;
        public static SoundEffect LoomerDeath { get { return loomerDeath; } }

        private static SoundEffect[] zombieDeath;
        public static SoundEffect ZombieDeath { get { return zombieDeath[rand.Next(zombieDeath.Length)]; } }
        #endregion

        public static void Load(ContentManager content)
        {
            gameMusic = Enumerable.Range(1, 3).Select(x => content.Load<Song>("Sound/Songs/Game Music 0" + x)).ToArray();
            menuMusic = content.Load<Song>("Sound/Songs/Main Menu");

            pistol = content.Load<SoundEffect>("Sound/Effects/Pistol/Shoot");
            ak47 = content.Load<SoundEffect>("Sound/Effects/Rifle/Shoot");
            grenadeLauncher = content.Load<SoundEffect>("Sound/Effects/Grenade Launcher/Shoot");
            rocketLauncher = content.Load<SoundEffect>("Sound/Effects/Rocket Launcher/Shoot");

            Minigun.Add("Start", content.Load<SoundEffect>("Sound/Effects/Minigun/Start"));
            Minigun.Add("Shoot", content.Load<SoundEffect>("Sound/Effects/Minigun/Shoot"));
            Minigun.Add("Stop", content.Load<SoundEffect>("Sound/Effects/Minigun/Stop"));
            Minigun.Add("Empty", content.Load<SoundEffect>("Sound/Effects/Minigun/Empty"));

            Shotgun.Add("Shoot", content.Load<SoundEffect>("Sound/Effects/Shotgun/Shoot"));
            Shotgun.Add("Cock", content.Load<SoundEffect>("Sound/Effects/Shotgun/Cock"));
            Shotgun.Add("Reload", content.Load<SoundEffect>("Sound/Effects/Shotgun/Reload"));

            explosions = Enumerable.Range(1, 2).Select(x => content.Load<SoundEffect>("Sound/Effects/Explosions/explosion 0" + x)).ToArray();

            player = content.Load<SoundEffect>("Sound/Effects/Player/Death");
            loomerDeath = content.Load<SoundEffect>("Sound/Effects/Enemies/Loomer Death");
            zombieDeath = Enumerable.Range(1, 4).Select(x => content.Load<SoundEffect>("Sound/Effects/Enemies/Zombie Death 0" + x)).ToArray();


        }

        public static void Initialize()
        {
            PlaySong();
        }

        public static void Update()
        {
            if (GameRoot.GameTime.ElapsedGameTime.TotalSeconds >= countDuration)
            {
                PlaySong();
            }
        }
        public static void PlaySong()
        {
            var song = GameMusic;
            MediaPlayer.Play(song);
            countDuration = song.Duration.Seconds;
        }

        public static void PlaySong(Song song)
        {
            MediaPlayer.Play(song);
            countDuration = song.Duration.Seconds;
        }
    }
}

