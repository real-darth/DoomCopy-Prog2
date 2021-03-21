using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoomCopy.Scripts.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomCopy.Enteties.Weapons
{
    class Pistol : Weapon, ShootingSound
    {
        public Pistol(Vector2 position, int damage, int ammunition, int clipCapacity, TimeSpan reloadTime, int shootingCooldown, Texture2D weaponSprite) : base(weaponSprite, position, damage, ammunition, clipCapacity)
        {
            name = "Pistol";
            sprite = Art.Pistol;

            radius = sprite.Width / 2f;

            this.shootingCooldown = shootingCooldown;

            this.reloadTime = reloadTime;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            base.Shoot(position, velocity);
            ShootSoundeffect();
        }

        public void ShootSoundeffect()
        {
            Sound.Pistol.Play();
        }
    }
}
