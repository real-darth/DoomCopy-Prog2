using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Eye : Enemy
    {
        #region FIELDS

        // Påverkar fiender inom x pixlar
        private float attractionRadius = 330f;

        // Partikel Spray Vinkel
        private float sprayAngle = 0;

        #endregion

        #region METHODS

        public Eye(Texture2D sprite, Vector2 position, int health, int points) : base(sprite, position, health, points) { }

        public override void Update()
        {
            base.Update();

            if (timeUntilStart <= 0)
            {
                // Enemy AI logik
                GravityPull();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            rotation -= 0.03f;

            // Stora ögat skjuter ut små mindre ögon
            // För att skapa en sorts timer används modulu
            if ((GameRoot.GameTime.TotalGameTime.Milliseconds / 1000) % 5 == 0)
            {
                Vector2 sprayVel = Extensions.FromPolar(sprayAngle, rand.NextFloat(12, 15));
                Color color = Extensions.HueUtility.HSVToColor(4.5f, 0.5f, 0.5f);
                Vector2 pos = position + 2f * new Vector2(sprayVel.Y, -sprayVel.X);

                var state = new ParticleState()
                {
                    velocity = sprayVel,
                    lengthMultiplier = 1,
                    type = ParticleType.Enemy
                };

                GameMaster.ParticleManager.CreateParticle(Art.FloatingEyes, pos, color, 100, new Vector2(1, 1), state);
            }

            // rotera spray direction
            sprayAngle -= MathHelper.TwoPi / 50f;
        }

        public void GravityPull()
        {
            var entities = EntityManager.GetNearbyEntities(position, attractionRadius);

            foreach (Entity entity in entities)
            {
                if (entity is Enemy && !(entity as Enemy).isActive)
                    continue;

                if (entity is Bullet)
                    entity.Velocity += (entity.position - position).ScaleTo(0.43f);
                else
                {
                    Vector2 distance = position - entity.position;
                    float distanceLenght = distance.Length();

                    entity.Velocity += distance.ScaleTo(MathHelper.Lerp(2, 0, distanceLenght / attractionRadius));
                }
            }
        }
        public override void CreateParticleEffect()
        {
            float hue1 = 5f;
            float hue2 = (hue1 + rand.NextFloat(0, 5)) % 6f;
            Color color1 = Extensions.HueUtility.HSVToColor(hue1, 0.8f, 0.8f);
            Color color2 = Extensions.HueUtility.HSVToColor(hue2, 0.5f, 1);

            for (int i = 0; i < 100; i++)
            {
                float speed = 16f * (1f - 1 / rand.NextFloat(1f, 5f));
                double rotation = rand.NextDouble() * 2 * Math.PI;

                var state = new ParticleState()
                {
                    velocity = new Vector2(speed * (float)Math.Cos(rotation), speed * (float)Math.Sin(rotation)),
                    type = ParticleType.Enemy,
                    lengthMultiplier = 1f
                };

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                GameMaster.ParticleManager.CreateParticle(Art.LineParticle, position, color, 90, new Vector2(1f, 1.3f), state);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Pulserande effect
            float pulseSize = 1.2f + 0.25f * (float)Math.Sin(3f * GameRoot.GameTime.TotalGameTime.TotalSeconds);
            if (!hideSprite)
                spriteBatch.Draw(sprite, position, null, color, rotation, size / 2f, pulseSize, 0, 0);
        }

        public override void DeathSound()
        {

        }

        #endregion
    }
}
