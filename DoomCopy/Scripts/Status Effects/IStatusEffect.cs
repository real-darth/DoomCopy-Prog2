using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Components.StatusEffect
{
    public interface IStatusEffect
    {
        Entity entity { get; set; }
        bool Done { get; set; }
        void Update();
    }
}
