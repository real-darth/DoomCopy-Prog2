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
    class RocketLauncher : Weapon, ShootingSound
    {
        public RocketLauncher(Vector2 position, int damage, int ammunition, int clipCapacity, TimeSpan reloadTime, Texture2D weaponSprite) : base(weaponSprite, position, damage, ammunition, clipCapacity)
        {
            name = "Rocket Launcher";
            sprite = Art.RocketLauncher;

            radius = sprite.Width / 2f;

            this.reloadTime = reloadTime;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            EntityManager.Add(new Rocket(position, velocity * 1.6f));
            currentClip--;

            ShootSoundeffect();

            if (currentClip <= 0)
            {
                reloading = true;
            }
        }

        public void ShootSoundeffect()
        {
            Sound.RocketLauncher.Play();
        }
    }
}