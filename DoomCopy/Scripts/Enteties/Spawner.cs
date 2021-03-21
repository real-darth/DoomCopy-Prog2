using DoomCopy.Enteties;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    class Spawner
    {
        static protected Random random = new Random();
        static protected float inverseSpawnChance = 60;

        protected static Vector2 GetSpawnPosition()
        {
            Vector2 spawnPosition;
            do
            {
                // hitta en random spawn position inom skärmen
                spawnPosition = new Vector2(random.Next((int)GameRoot.ScreenSize.X), random.Next((int)GameRoot.ScreenSize.Y));
            } while (Vector2.DistanceSquared(spawnPosition, Player.Instance.position) < 250 * 250);

            return spawnPosition;
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
        }
    }
}
