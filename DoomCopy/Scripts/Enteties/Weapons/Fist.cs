using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomCopy.Enteties
{
    class Fist : Weapon
    {
        public Fist(Texture2D weaponSprite, Vector2 position, int damage, int ammunition, int clipCapacity, float reloadTime) 
            : base(weaponSprite, position, damage, ammunition, clipCapacity, reloadTime)
        {
            name = avalibleNames[0];
            melee = true;
        }
    }
}
