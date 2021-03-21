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
    class GrenadeLauncher : Weapon, ShootingSound
    {
        public GrenadeLauncher(Vector2 position, int damage, int ammunition, int clipCapacity, TimeSpan reloadTime, int shootingCooldown, Texture2D weaponSprite) : base(weaponSprite, position, damage, ammunition, clipCapacity)
        {
            name = "Grenade Launcher";
            sprite = Art.GrenadeLauncher;

            radius = sprite.Width / 2f;

            this.shootingCooldown = shootingCooldown;
            this.reloadTime = reloadTime;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            EntityManager.Add(new Grenade(position, velocity * 1.6f));
            currentClip--;

            ShootSoundeffect();

            if (currentClip <= 0)
            {
                reloading = true;
            }
        }

        public void ShootSoundeffect()
        {
            Sound.GrenadeLauncher.Play();
        }
    }
}
