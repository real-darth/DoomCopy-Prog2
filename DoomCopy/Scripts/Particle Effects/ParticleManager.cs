using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    public class ParticleManager<T>
    {
        public class Particle
        {
            #region FIELDS

            // Partikel fysisk information
            public Texture2D texture;
            public Vector2 position;
            public float rotation;
            
            public Vector2 scale = Vector2.One;

            public Color color;
            public float duration;

            // Mellan 1 och 0
            public float life = 1f;
            public T state;
        }

        #region HELPER CLASS
        // Partikel listan
        private class ParticleArray
        {
            private Particle[] list;

            // Listans startvärde
            private int _start;
            public int Start
            {
                get { return _start; }
                set { _start = value % list.Length; }
            }
            public int particleCount { get; set; }
            public int Capacity { get { return list.Length; } }

            // Konstruktor
            public ParticleArray(int capacity)
            {
                list = new Particle[capacity];
            }

            // Partikelreferens i listan
            public Particle this[int i]
            {
                get { return list[(_start + i) % list.Length]; }
                set { list[(_start + i) % list.Length] = value; }
            }
        }
        #endregion

        // Delaget / event som ropas för varje partikel
        private Action<Particle> updateParticle;

        // Lista över alla aktiva partiklar
        // Använder en speciell class
        private ParticleArray particleList;

        #endregion

        #region METHODS

        // Konstruktor
        public ParticleManager(int capacity, Action<Particle> updateParticle)
        {
            this.updateParticle = updateParticle;

            // Poppulera listan med partiklar (ges genom en förbestämd kapacitet)
            particleList = new ParticleArray(capacity);
            for (int i = 0; i < capacity; i++)
                particleList[i] = new Particle();
        }

        public void Update()
        {
            int removalCount = 0;

            // Uppdatera alla partiklar
            for (int i = 0; i < particleList.particleCount; i++)
            {
                var particle = particleList[i];
                updateParticle(particle);
                particle.life -= 1f / particle.duration;

                // När partikeln uppdaterats så skickas den ned mot längst bak i listan
                Swap(particleList, i - removalCount, i);

                // om partikeln har dött / avaktiverats så förstör den
                if (particle.life < 0)
                    removalCount++;
            }

            // Ta bort förstörda partiklar ur count
            particleList.particleCount -= removalCount;
        }

        // sawp metod för att byta plats på partiklar i partikellistan
        // partiklar som har precis updaterats hamnar längts bak i listan
        private static void Swap(ParticleArray list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        // Skapa partikeln
        public void CreateParticle(Texture2D texture, Vector2 position, Color color, float duration, Vector2 scale, T state, float rot = 0)
        {
            Particle particle;
            if (particleList.particleCount == particleList.Capacity)
            {
                // Om listan är full, overrida den äldsta partikeln för att spara på minne
                // Objekt-pooling
                particle = particleList[0];
                particleList.Start++;
            }
            else
            {
                // Om listan inte är full välj en ny partikel
                // från den första poppulationen
                particle = particleList[particleList.particleCount];
                particleList.particleCount++;
            }

            // Skapa partikeln
            particle.texture = texture;
            particle.position = position;
            particle.color = color;

            particle.duration = duration;
            particle.life = 1f;
            particle.scale = scale;
            particle.rotation = rot;
            particle.state = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Rita alla partiklar
            for (int i = 0; i < particleList.particleCount; i++)
            {
                var particle = particleList[i];

                Vector2 origin = new Vector2(particle.texture.Width / 2, particle.texture.Height / 2);
                spriteBatch.Draw(particle.texture, particle.position, null, particle.color, particle.rotation, origin, particle.scale, 0, 0);
            }
        }

        #endregion
    }
}
