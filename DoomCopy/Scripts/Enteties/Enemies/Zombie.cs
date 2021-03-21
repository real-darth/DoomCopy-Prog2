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
    class Zombie : Enemy
    {
        #region FIELDS

        private Random random = new Random();
        private float randomSpeed;

        #endregion

        #region METHODS

        public Zombie(Texture2D sprite, Vector2 position, int health, int points) : base(sprite, position, health, points) { }

        public override void Update()
        {
            base.Update();

            if (timeUntilStart <= 0)
            {
                // Enemy AI logik
                FollowPlayer(1.5f);
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            position += Velocity;
            position = Vector2.Clamp(position, size / 2, GameRoot.ScreenSize - size / 2);

            randomSpeed = (float)(random.NextDouble());
            Velocity *= randomSpeed;
        }

        private void FollowPlayer(float acceleration)
        {
            if (!Player.Instance.isDead)
                Velocity += (Player.Instance.position - position).ScaleTo(acceleration);

            if (Velocity != Vector2.Zero)
                rotation = Velocity.ToAngle();
        }

        public override void WasHit(int damage)
        {
            AddEffect(new StatusEffectKnockback(this, 20.5f));

            health -= damage;

            if (health <= 0)
            {
                isDestroyed = true;

                // Spela ljudeffekt
                DeathSound();

                CreateParticleEffect();

                PlayerManager.AddPoints(pointValue);
                PlayerManager.IncreaseMultiplier();
            }
        }

        public override void DeathSound()
        {
            Sound.ZombieDeath.Play();
        }

        #endregion
    }
}
