using DoomCopy.Components.StatusEffect;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Grenade : Bullet
    {
        #region FIELDS

        private float acceleration = 1.1f;
        private float explosivePower = 1.7f;
        private int explosiveRadius = 200;

        private TimeSpan detonationTime = TimeSpan.FromMilliseconds(3000);

        #endregion

        #region METHODS

        public Grenade(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
            sprite = Art.Grenade;
            base.position = position;
            base.Velocity = velocity;
            rotation = base.Velocity.VectorToAngle();
            radius = 8;
        }

        public override void Update()
        {
            // fixa granatens rotation
            // lägg till liten animation genom sin
            if (Velocity.LengthSquared() > 0)
            {
                float rotationCycle = MathHelper.ToRadians((float)(Math.PI * Math.Sin(3f * GameRoot.GameTime.TotalGameTime.TotalSeconds)));
                rotation = Velocity.VectorToAngle() + rotationCycle;
            }

            // flytta granat frammot
            position += Velocity * acceleration;

            // sänk granat hastighet över tiden
            if (acceleration > 0)
            {
                acceleration -= 0.01f;
            }

            // Om granat är påväg att lämna skärmen, gör så den studsar mot väggen
            if (!GameRoot.Viewport.Bounds.Contains(position.ToPoint()))
            {
                if(GameRoot.Viewport.Bounds.Width < position.X || position.X < 0)
                {
                    Velocity = new Vector2(-Velocity.X, Velocity.Y);
                }

                if (GameRoot.Viewport.Bounds.Height < position.Y || position.Y < 0)
                {
                    Velocity = new Vector2(Velocity.X, -Velocity.Y);
                }
            }

            // Detonera när tiden är över
            if (detonationTime <= TimeSpan.Zero)
            {
                Detoante();
            }
            else
            {
                detonationTime -= GameRoot.GameTime.ElapsedGameTime;
            }
        }

        // Skapa explosion
        public void Detoante()
        {
            new Explosion(position, explosiveRadius, explosivePower);
            isDestroyed = true;
        }

        #endregion
    }
}
