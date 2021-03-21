using DoomCopy.Scripts.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties.Weapons
{
    class Shotgun : Weapon, ShootingSound
    {
        private bool burstCooldown = false;
        private TimeSpan burst = TimeSpan.FromMilliseconds(700);
        private TimeSpan burstTimer;

        private Random random = new Random();  

        public Shotgun(Vector2 position, int damage, int ammunition, int clipCapacity, TimeSpan reloadTime, Texture2D weaponSprite) : base(weaponSprite, position, damage, ammunition, clipCapacity)
        {
            // HARDCODE VAPNET
            name = "Fortnite Shotgun";
            sprite = Art.Shotgun;

            // KONSTRUKTOR VARIABLER
            radius = sprite.Width / 2f;

            this.reloadTime = reloadTime;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            // random bullet count
            int bulletCount = random.Next(15, 30);

            for (int i = 0; i < bulletCount; i++)
            {
                // ta spelaresn vy och ta en random vinkel inom den (sätter till totalt 100 grader)
                float randomDirection = Player.Instance.rotation + random.NextFloat(-50 * MathHelper.Pi / 180, 50 * MathHelper.Pi / 180);

                // Ny bullet hastighet vector i vinkelns riktning
                Vector2 vel = Extensions.FromPolar(randomDirection, 11f);

                EntityManager.Add(new Bullet(position, velocity + vel));
            }

            ShootSoundeffect();

            currentClip--;

            // Sätt igång reload om värdet är noll
            if (currentClip <= 0)
            {
                reloading = true;
                Sound.Shotgun["Reload"].Play();
            }
            // annars sätt en cooldown för shooting
            // för att emulera pump shotgun
            else if (currentClip > 0)
            {
                burstCooldown = true;
                reloading = true;

                Extensions.DelayAction(150, delegate
                {
                    Sound.Shotgun["Cock"].Play();
                });
            }
        }

        public void ShootSoundeffect()
        {
            Sound.Shotgun["Shoot"].Play();
        }

        public override void Update()
        {
            if (burstCooldown)
            {
                burstTimer -= GameRoot.GameTime.ElapsedGameTime;

                if (burstTimer < TimeSpan.Zero)
                {
                    burstTimer = burst;
                    burstCooldown = false;
                    reloading = false;
                }

                return;
            }

            base.Update();
        }
    }
}
