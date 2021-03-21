using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using System;

namespace DoomCopy.Enteties
{
    class Rocket : Bullet
    {
        private float acceleration = 1.2f;
        private float explosivePower = 3f;
        private int explosiveRadius = 300;

        public Rocket(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
            sprite = Art.Rocket;
            base.position = position;
            base.Velocity = velocity / 3;
            rotation = base.Velocity.VectorToAngle();
            radius = 8;
        }

        public override void Update()
        {
            // fixa rocket rotation
            if (Velocity.LengthSquared() > 0)
                rotation = Velocity.VectorToAngle();

            // flytta rocket frammot
            position += Velocity * acceleration;

            // öka raketens hastighet över tiden
            acceleration += 0.1f;

            // Effekt
            RocketTrailEffect();

            // Om rocket lämnar skärmen, explodera raketen
            if (!GameRoot.Viewport.Bounds.Contains(position.ToPoint()))
                Detoante();
        }

        private void RocketTrailEffect()
        {
            if (Velocity.LengthSquared() > 0.1f)
            {
                // set up some variables
                rotation = Velocity.ToAngle();
                Quaternion rot = Quaternion.CreateFromYawPitchRoll(0f, 0f, rotation);

                Vector2 baseVel = Velocity.ScaleTo(-3); 
                Color midColor = new Color(255, 187, 30);   // orange-yellow
                Vector2 pos = position + Vector2.Transform(new Vector2(-25, 0), rot);   // position of the ship's exhaust pipe.
                const float alpha = 0.7f;

                // middle particle stream
                Vector2 velMid = baseVel;
                GameMaster.ParticleManager.CreateParticle(Art.LineParticle, pos, midColor * alpha, 40f, new Vector2(0.7f, 0.6f),
                    new ParticleState(velMid, ParticleType.Enemy));
            }
        }

        public void Detoante()
        {
            new Explosion(position, explosiveRadius, explosivePower);
            isDestroyed = true;
        }
    }
}
