using DoomCopy.Enteties;
using DoomCopy.Particle_Effects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    class EnemySpawner : Spawner
    {
        #region METHODS

        public static void Update()
        {
            if (!Player.Instance.isDead && EntityManager.entityCount < 200)
            {
                if (random.Next((int)inverseSpawnChance) == 0)
                {
                    var temppos = GetSpawnPosition();
                    EntityManager.Add(new Zombie(Art.Zombie, temppos, 20, 2));
                    EnemySpawnParticle(temppos);
                }
                if (random.Next((int)inverseSpawnChance) == 10)
                {
                    var temppos = GetSpawnPosition();
                    EntityManager.Add(new Loomer(Art.Loomer, temppos, 10, 1));
                    EnemySpawnParticle(temppos);
                }

                if (EntityManager.eyeCount < 2 && random.Next((int)inverseSpawnChance) == 0)
                {
                    var temppos = GetSpawnPosition();
                    EntityManager.Add(new Eye(Art.Eye, temppos, 50, 5));
                    EnemySpawnParticle(temppos, 4f);
                }
            }

            // öka hastigheten som fiender skapas över tid
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;
        }

        public static void Initialize(float initVal)
        {
            inverseSpawnChance = initVal;
        }

        #region HELPER FUNCTION

        private static Random rand = new Random();
        private static void EnemySpawnParticle(Vector2 position)
        {
            float hue1 = 0.4f;
            Color color1 = Color.DarkRed;
            Color color2 = Extensions.HueUtility.HSVToColor(hue1, 0.8f, 1f);

            double rotation = rand.NextDouble() * 2 * Math.PI;

            var state = new ParticleState()
            {
                velocity = new Vector2(0, 0),
                type = ParticleType.None,
                lengthMultiplier = 17f
            };

            Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
            GameMaster.ParticleManager.CreateParticle(Art.SpawnCircle, position, color, 300, new Vector2(1.8f, 1.8f), state);
        }

        private static void EnemySpawnParticle(Vector2 position, float size)
        {
            float hue1 = 0.4f;
            Color color1 = Color.DarkRed;
            Color color2 = Extensions.HueUtility.HSVToColor(hue1, 0.8f, 1f);

            double rotation = rand.NextDouble() * 2 * Math.PI;

            var state = new ParticleState()
            {
                velocity = new Vector2(0, 0),
                type = ParticleType.None,
                lengthMultiplier = 10f * size
            };

            Color color = Color.Lerp(color1, color2, rand.NextFloat(0, 1));
            GameMaster.ParticleManager.CreateParticle(Art.SpawnCircle, position, color, 300, new Vector2(size, size), state);
        }

        #endregion

        #endregion
    }
}
