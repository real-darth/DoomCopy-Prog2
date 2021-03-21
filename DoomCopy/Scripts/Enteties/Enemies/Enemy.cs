using DoomCopy.Components.StatusEffect;
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
    abstract class Enemy : Entity
    {
        #region FIELDS

        protected int timeUntilStart = 60;
        public bool isActive { get { return timeUntilStart <= 0; } }
        public int pointValue { get; private set; }

        // enemy health
        protected int health;

        #endregion

        #region METHODS

        // konstruktor som skapar fienden
        public Enemy(Texture2D sprite, Vector2 position, int health, int pointval)
        {
            base.sprite = sprite;
            base.position = position;
            radius = sprite.Width / 2f;
            color = Color.Transparent;
            pointValue = pointval;
            this.health = health;
        }

        // alternativ konstruktor utan poäng
        public Enemy(Texture2D sprite, Vector2 position, int health)
        {
            base.sprite = sprite;
            base.position = position;
            radius = sprite.Width / 2f;
            color = Color.Transparent;
            this.health = health;
        }

        // hantera kollision
        public void HandleCollision(Enemy other)
        {
            var d = position - other.position;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }

        // kallas när fiende blir träffad av bullet
        public virtual void WasHit(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                isDestroyed = true;

                // Spela ljudeffekt
                //Sound.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);

                // Skapa partikeleffect
                CreateParticleEffect();

                // Spela death sound
                DeathSound();

                PlayerManager.AddPoints(pointValue);
                PlayerManager.IncreaseMultiplier();
            }
        }

        // partikel effekt som skapas vid death
        public static Random rand = new Random();
        public virtual void CreateParticleEffect()
        {
            // ställ in animerade färger
            float hue1 = 0.4f;
            Color color1 = Color.DarkRed; // första färg
            Color color2 = Extensions.HueUtility.HSVToColor(hue1, 0.8f, 1f); // andra färg som altenerar lite från första

            // skapa partikeleffekten
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

                Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
                GameMaster.ParticleManager.CreateParticle(Art.LineParticle, position, color, 65, new Vector2(1f, 1.3f), state);
            }
        }

        public override void Update()
        {
            foreach (var component in statusEffects.StatusEffectList)
            {
                component.Update();
            }
        }

        // varje fiende har en unik death sound
        // därför ärver de denna funktion
        public abstract void DeathSound();

        #endregion
    }
}
