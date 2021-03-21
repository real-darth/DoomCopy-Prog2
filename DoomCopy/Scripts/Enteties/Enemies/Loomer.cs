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
    class Loomer : Enemy
    {
        #region METHODS

        public Loomer(Texture2D sprite, Vector2 position, int health, int points) : base(sprite, position, health, points)
        {
            base.sprite = sprite;
            base.position = position;
            radius = sprite.Width / 2f;
            color = Color.Transparent;
            this.health = health;
        }

        public override void Update()
        {
            base.Update();

            if (timeUntilStart <= 0)
            {
                // Enemy AI logik
                MoveRandomly();
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            position += Velocity;
            position = Vector2.Clamp(position, size / 2, GameRoot.ScreenSize - size / 2);
        }

        private void MoveRandomly()
        {
            float direction = rand.NextFloat(0, MathHelper.TwoPi);

            direction += rand.NextFloat(-0.1f, 0.1f);
            direction = MathHelper.WrapAngle(direction);

            for (int i = 0; i < 6; i++)
            {
                Velocity += Extensions.FromPolar(direction, 0.2f);
                rotation -= 0.05f;

                var bounds = GameRoot.Viewport.Bounds;
                bounds.Inflate(-sprite.Width, -sprite.Height);

                // Om fienden är nära kanten eller utanför banan gör så den ändrar riktning
                if (!bounds.Contains(position.ToPoint()))
                    direction = (GameRoot.ScreenSize / 2 - position).ToAngle() + rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }

        public override void CreateParticleEffect()
        {
            for (int i = 0; i < 50; i++)
            {
                float speed = 20f * (1f - 1 / rand.NextFloat(1f, 5f));
                double rotation = rand.NextDouble() * 2 * Math.PI;

                var state = new ParticleState()
                {
                    velocity = new Vector2(speed * (float)Math.Cos(rotation), speed * (float)Math.Sin(rotation)),
                    type = ParticleType.Enemy,
                    lengthMultiplier = 1f
                };

                GameMaster.ParticleManager.CreateParticle(Art.LineParticle, position, Color.YellowGreen, 35, new Vector2(1.1f, 1.5f), state);
            }
        }

        public override void DeathSound()
        {
            Sound.LoomerDeath.Play();
        }

        #endregion
    }
}
