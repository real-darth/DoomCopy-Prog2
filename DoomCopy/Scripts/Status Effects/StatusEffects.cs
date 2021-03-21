using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Components.StatusEffect
{
    public class StatusEffects : Component
    {
        public List<IStatusEffect> StatusEffectList { get; set; }

        public StatusEffects()
        {
            StatusEffectList = new List<IStatusEffect>();
        }

        public override void Update()
        {
            if (StatusEffectList.Count == 0)
                return;

            int i = 0;
            while (i < StatusEffectList.Count)
            {
                StatusEffectList[i].Update();

                if (StatusEffectList[i].Done)
                {
                    StatusEffectList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
