using DoomCopy.Enteties;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Particle_Effects
{
    #region FEILD
    public enum ParticleType { None, Enemy, Bullet }
    
	// struct används för att spara på minne
	// eftersom det skapas och förstörs så många partiklar hela tiden
	public struct ParticleState
    {
        public Vector2 velocity;
        public ParticleType type;
        public float lengthMultiplier;

		// konstruktor
		public ParticleState(Vector2 velocity, ParticleType type, float lengthMultiplier = 1f)
		{
			this.velocity = velocity;
			this.type = type;
			this.lengthMultiplier = lengthMultiplier;
		}

        #region METHOD

        public static void UpdateParticle(ParticleManager<ParticleState>.Particle particle)
        {
            var vel = particle.state.velocity;
            float speed = vel.Length();

			Vector2.Add(ref particle.position, ref vel, out particle.position);
			float alpha = 1f;

			// gör partiklel mer transparent om den har lägre liv kvar eller lägre hastighet
			if (particle.state.type != ParticleType.None)
			{
				alpha = Math.Min(1, Math.Min(particle.life * 2, speed * 1f));
			}
			// gör partiklel mer transparent om den har lägre liv kvar
			else if (particle.state.type == ParticleType.None)
			{
				alpha = Math.Min(1, particle.life * 2);
			}
			alpha *= alpha;

			particle.color.A = (byte)(255 * alpha);

			// the length of bullet particles will be less dependent on their speed than other particles
			if (particle.state.type == ParticleType.Bullet)
				particle.scale.X = particle.state.lengthMultiplier * Math.Min(Math.Min(1f, 0.1f * speed + 0.1f), alpha);
			else
				particle.scale.X = particle.state.lengthMultiplier * Math.Min(Math.Min(1f, 0.2f * speed + 0.1f), alpha);

			particle.rotation = vel.ToAngle();

			var pos = particle.position;
			int width = (int)GameRoot.ScreenSize.X;
			int height = (int)GameRoot.ScreenSize.Y;

			foreach (var eye in EntityManager.enemies)
			{
				if (eye is Eye && particle.state.type != ParticleType.None)
				{
					var dPos = (eye as Eye).position - pos;
					float distance = dPos.Length();
					var n = dPos / distance;
					vel += 10000 * n / (distance * distance + 10000);

					// add tangential acceleration for nearby particles
					if (distance < 400)
						vel += 45 * new Vector2(n.Y, -n.X) / (distance + 100);
				}
			}

			// kollidera med skärmens kanter
			if (pos.X < 0)
				vel.X = Math.Abs(vel.X);
			else if (pos.X > width)
				vel.X = -Math.Abs(vel.X);
			if (pos.Y < 0)
				vel.Y = Math.Abs(vel.Y);
			else if (pos.Y > height)
				vel.Y = -Math.Abs(vel.Y);

			if (Math.Abs(vel.X) + Math.Abs(vel.Y) < 0.00000000001f) // denormalized floats cause significant performance issues
				vel = Vector2.Zero;
			else if (particle.state.type == ParticleType.Enemy)
				vel *= 0.94f;
			else
				vel *= 0.96f + Math.Abs(pos.X) % 0.04f; // rand.Next() isn't thread-safe, so use the position for pseudo-randomness

			particle.state.velocity = vel;
		}

        #endregion METHOD
    }

    #endregion
}
