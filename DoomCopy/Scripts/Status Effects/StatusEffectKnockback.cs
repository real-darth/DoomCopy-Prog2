using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Components.StatusEffect
{
    class StatusEffectKnockback : IStatusEffect
    {
        public bool Done { get; set; }
        public Entity entity { get; set; }
        private enum States { Start, End };
        private States currentState;

        private float pushForce;

        public StatusEffectKnockback(Entity entity, float force)
        {
            this.entity = entity;
            currentState = States.Start;
            pushForce = force;
        }

        public void Update()
        {
            switch (currentState)
            {
                case States.Start:
                    entity.PushBackwards(pushForce);
                    currentState = States.End;
                    break;
                
                case States.End:
                    Done = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
