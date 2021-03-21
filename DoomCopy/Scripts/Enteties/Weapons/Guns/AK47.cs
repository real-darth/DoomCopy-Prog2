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
    class AK47 : Weapon, ShootingSound
    {
        #region FIELDS

        private bool burstCooldown = false;
        private TimeSpan burst = TimeSpan.FromMilliseconds(240);
        private TimeSpan burstTimer;

        #endregion

        #region METHODS

        // Konstruktor
        public AK47(Vector2 position, int damage, int ammunition, int clipCapacity, TimeSpan reloadTime, int shootingCooldown, Texture2D weaponSprite) : base(weaponSprite, position, damage, ammunition, clipCapacity)
        {
            name = "AK-47";
            sprite = Art.Rifle;
            radius = sprite.Width / 2f;

            this.shootingCooldown = shootingCooldown;
            this.reloadTime = reloadTime;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            // Skapa bullet
            EntityManager.Add(new Bullet(position, velocity * 1.8f));
            currentClip--;

            // Ljudeffekt
            ShootSoundeffect();

            // Kolla för reload och kolla för burst cooldown 
            if (currentClip <= 0)
            {
                reloading = true;
            }
            else if (currentClip % 3 == 0)
            {
                burstCooldown = true;
                reloading = true;
            }
        }

        public void ShootSoundeffect()
        {
            Sound.Ak47.Play();
        }

        public override void Update()
        {
            // Timer för burst cooldown
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

        #endregion
    }
}
