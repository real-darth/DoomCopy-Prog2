using DoomCopy.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Components.StatusEffect
{
    class StatusEffectFreeze : IStatusEffect
    {
        public bool Done { get; set; }
        public Entity entity { get; set; }

        private int freezeTime;
        private bool callOnce = false;

        public StatusEffectFreeze(Entity entity, int freezeTime)
        {
            this.entity = entity;
            this.freezeTime = freezeTime;
        }

        public void Update()
        {
            if (callOnce == false)
            {
                callOnce = true;

                Extensions.DelayAction(50, delegate () {
                    if (entity is Enemy)
                    {
                        (entity as Enemy).freezeMovement = true;
                    }
                });

                Extensions.DelayAction(freezeTime, delegate () {
                    if (entity is Enemy)
                    {
                        (entity as Enemy).freezeMovement = false;
                    }
                });
            }
            else
            {
                Done = true;
            }
        }
    }
}
