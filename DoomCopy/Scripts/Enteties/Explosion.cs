using DoomCopy.Components.StatusEffect;
using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Explosion
    {
        private Vector2 origin;
        private float radius;
        private float explosivePower;

        public Explosion(Vector2 origin, float radius, float power)
        {
            this.origin = origin;
            this.radius = radius;
            explosivePower = power;

            Explode();
        }

        public void Explode()
        {
            var entities = EntityManager.GetNearbyEntities(origin, radius);

            foreach (Entity entity in entities)
            {
                Vector2 distance = origin - entity.position;
                float distanceExplosionPowerRelation = Math.Abs(100 - distance.Length());
                distanceExplosionPowerRelation = MathHelper.Clamp(distanceExplosionPowerRelation, 40, 90);

                entity.position += (entity.position - origin).ScaleTo((distanceExplosionPowerRelation * 1.1f));

                if (entity is Enemy && (entity as Enemy).isActive)
                {
                    (entity as Enemy).WasHit((int)((distanceExplosionPowerRelation / 10) * explosivePower));
                    entity.AddEffect(new StatusEffectFreeze(entity, 500));
                }

                if (entity is Player)
                {
                    entity.Velocity += (entity.position - origin).ScaleTo(distanceExplosionPowerRelation * 1.1f);
                }

                // Play Explosion Sound
                Sound.Explosion.Play();

                // Particle Effect
                CreateParticleEffect();
            }
        }

        private Random rand = new Random();

        private void CreateParticleEffect()
        {
            for (int i = 0; i < 70; i++)
            {
                float speed = 8.5f * (1f - 1 / rand.NextFloat(1f, 5f));
                double rotation = rand.NextDouble() * 2 * Math.PI;

                var state = new ParticleState()
                {
                    velocity = new Vector2(speed * (float)Math.Cos(rotation), speed * (float)Math.Sin(rotation)),
                    type = ParticleType.Enemy,
                    lengthMultiplier = 1f
                };

                GameMaster.ParticleManager.CreateParticle(Art.Explosion, origin, Color.White, 65, new Vector2(0.5f, 0.6f), state);
            }
        }
    }
}
