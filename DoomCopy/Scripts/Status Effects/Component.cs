using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Components
{
    public abstract class Component
    {
        public abstract void Update();

        public virtual void Initialize() { }

        public virtual void Uninitalize() { }

    }
}
