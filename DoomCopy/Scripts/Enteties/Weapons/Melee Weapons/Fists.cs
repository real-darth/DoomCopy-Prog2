using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoomCopy.Components.StatusEffect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomCopy.Enteties
{
    class Fists : Weapon
    {
        private TimeSpan bulletReach = TimeSpan.FromMilliseconds(50);

        public Fists(Texture2D playerSpríte) : base(playerSpríte)
        {
            name = "Fists";
            sprite = Art.Bullet;
            
            radius = 0;
            this.damage = 5;

            capacity = 0;
            currentClip = capacity;
            shootingCooldown = 20;

            reloadTime = TimeSpan.FromMilliseconds(30);
            reloadingTimer = reloadTime;

            melee = true;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            var bullet = new Bullet(position, velocity * 1.5f, bulletReach, new StatusEffectKnockback(this, 25.5f));
            //bullet.hideSprite = true;
            EntityManager.Add(bullet);

            playerWeaponSprite = Art.Player_Stab;
               
            Extensions.DelayAction(100, delegate () { playerWeaponSprite = Art.Player_Melee; });
        }
    }
}
