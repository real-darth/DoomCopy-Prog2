using DoomCopy.Enteties;
using DoomCopy.Enteties.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    class WeaponSpawner : Spawner
    {
        public static void Update()
        {
            // Max 3 vapen på skärmen samtidigt
            if (!Player.Instance.isDead && EntityManager.weaponCount < 4)
            {
                // PISTOL SPAWNER
                if (random.Next((int)inverseSpawnChance) == 0)
                {
                    Pistol weapon = new Pistol(GetSpawnPosition(), 30, 10, 5, TimeSpan.FromMilliseconds(500), 13, Art.Player_Pistol);
                    EntityManager.Add(weapon);
                }

                // AK-47 / RIFLE SPAWNER
                if (random.Next((int)inverseSpawnChance) == 1)
                {
                    AK47 weapon = new AK47(GetSpawnPosition(), 10, 162, 27, TimeSpan.FromMilliseconds(2000), 4, Art.Player_Rifle);
                    EntityManager.Add(weapon);
                }

                // MINIGUN SPAWNER
                if (random.Next((int)inverseSpawnChance) == 2)
                {
                    Minigun weapon = new Minigun(GetSpawnPosition(), 5, 500, Art.Player_Rifle);
                    EntityManager.Add(weapon);
                }

                // SHOTGUN SPAWNER
                if (random.Next((int)inverseSpawnChance) == 3)
                {
                    Shotgun weapon = new Shotgun(GetSpawnPosition(), 10, 54, 6, TimeSpan.FromMilliseconds(2500), Art.Player_Shotgun);
                    EntityManager.Add(weapon);
                }

                // ROCKET LAUNCHER SPANWER
                if (random.Next((int)inverseSpawnChance) == 4)
                {
                    RocketLauncher weapon = new RocketLauncher(GetSpawnPosition(), 5, 10, 1, TimeSpan.FromMilliseconds(1400), Art.Player_Rifle);
                    EntityManager.Add(weapon);
                }

                // GRENADE LAUNCHER SPAWNER
                if (random.Next((int)inverseSpawnChance) == 4)
                {
                    GrenadeLauncher weapon = new GrenadeLauncher(GetSpawnPosition(), 1, 16, 4, TimeSpan.FromMilliseconds(900), 13, Art.Player_Rifle);
                    EntityManager.Add(weapon);
                }
            }

            // öka hastigheten som vapen skapas med över tid
            if (inverseSpawnChance > 500)
                inverseSpawnChance -= 0.005f;
        }

        public static void Initialize(float initVal)
        {
            inverseSpawnChance = initVal;
        }
    }
}
