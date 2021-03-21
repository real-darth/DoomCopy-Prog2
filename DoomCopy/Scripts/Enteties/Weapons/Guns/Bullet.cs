using DoomCopy.Components.StatusEffect;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Bullet : Entity
    {
        private TimeSpan destructionTimer = TimeSpan.FromMinutes(1);
        public IStatusEffect effectOnHit
        {
            get; private set;
        }

        // För skjutvapen
        public Bullet(Vector2 position, Vector2 velocity)
        {
            sprite = Art.Bullet;
            base.position = position;
            base.Velocity = velocity;
            rotation = base.Velocity.VectorToAngle();
            radius = 8;
        }

        // För närstridsvapen
        public Bullet(Vector2 position, Vector2 velocity, TimeSpan bulletTime)
        {
            sprite = Art.Bullet;
            base.position = position;
            base.Velocity = velocity;
            rotation = base.Velocity.VectorToAngle();
            radius = 8;
            destructionTimer = bulletTime;
        }

        #region OVEREXTENTION FÖR NÄRSTRIDSVAPEN
        public Bullet(Vector2 position, Vector2 velocity, TimeSpan bulletTime, IStatusEffect applySatusEffect)
        {
            sprite = Art.Bullet;
            base.position = position;
            base.Velocity = velocity;
            rotation = base.Velocity.VectorToAngle();
            radius = 8;
            destructionTimer = bulletTime;
            effectOnHit = applySatusEffect;
        }
        #endregion

        public override void Update()
        {
            if(destructionTimer != TimeSpan.FromMinutes(1))
            {
                if(destructionTimer <= TimeSpan.Zero)
                {
                    isDestroyed = true;
                }
                else
                {
                    destructionTimer -= GameRoot.GameTime.ElapsedGameTime;
                }
            }

            // fixa bullet rotation
            if (Velocity.LengthSquared() > 0)
                rotation = Velocity.VectorToAngle();

            // flytta bullet frammot
            position += Velocity;

            // Om bullet lämnar skärmen, förstör den
            if (!GameRoot.Viewport.Bounds.Contains(position.ToPoint()))
                isDestroyed = true;
        }
    }
}
