using DoomCopy.Components.StatusEffect;
using DoomCopy.Enteties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    static class EntityManager
    {
        // Entities (används för hantering av draw funktionen)
        static List<Entity> entities = new List<Entity>();

        // Enemies
        public static List<Enemy> enemies = new List<Enemy>();

        // Bullets
        static List<Bullet> bullets = new List<Bullet>();

        // Weapons
        static List<Weapon> weapons = new List<Weapon>();

        // Hantera enteties
        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int entityCount { get { return entities.Count; } }
        public static int weaponCount { get { return weapons.Count; } }
        public static int eyeCount { 
            get 
            {
                return enemies.Count(e => e.GetType() == typeof(Eye));
            } 
        }

        #region LÄGG TILL ENTETIES

        // Lägg till aktiv entity
        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is Weapon)
                weapons.Add(entity as Weapon);
        }

        #endregion

        public static void Update()
        {
            isUpdating = true;

            // Kolla kollisioner
            HandleCollisions();

            // Updatera alla enteties
            foreach (var entity in entities)
            {
                entity.Update();
            }

            isUpdating = false;

            foreach (var weapon in addedEntities)
            {
                AddEntity(weapon);
            }

            addedEntities.Clear();

            // Ta bort alla enteties som är förstörda / döda
            entities = entities.Where(x => !x.isDestroyed).ToList();
            bullets = bullets.Where(x => !x.isDestroyed).ToList();
            weapons = weapons.Where(x => !x.isDestroyed).ToList();

            enemies = enemies.Where(x => !x.isDestroyed).ToList();
        }

        // Hanterar all kollision mellan enteties
        static void HandleCollisions()
        {
            #region ANDRA ENTETIES COLLISION
            // hantera kollision mellan fiende ocj fiende
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int x = i + 1; x < enemies.Count; x++)
                {
                    if (IsColliding(enemies[i], enemies[x]))
                    {
                        enemies[i].HandleCollision(enemies[x]);
                        enemies[x].HandleCollision(enemies[i]);
                    }
                }
            }

            // hantera kollision mellan fiender och bullets
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int x = 0; x < bullets.Count; x++)
                {
                    if (IsColliding(enemies[i], bullets[x]))
                    {
                        // Kolla om bullet är en raket
                        if(bullets[x] is Rocket)
                        {
                            (bullets[x] as Rocket).Detoante();
                            return;
                        }

                        // Kolla om bullet är en grenade
                        if (bullets[x] is Grenade)
                        {
                            (bullets[x] as Grenade).Detoante();
                            return;
                        }

                        enemies[i].WasHit(PlayerManager.EquipedWeapon.GetDamage());
                        if (bullets[x].effectOnHit != null)
                        {
                            var effect = bullets[x].effectOnHit;
                            effect.entity = enemies[i];

                            enemies[i].AddEffect(effect);
                        }

                        bullets[x].isDestroyed = true;
                    }
                }
            }
            #endregion
            #region PLAYER KOLLISION
            // hantera kollision mellan spelaren och vapen
            for (int i = 0; i < weapons.Count; i++)
            {
                if (IsColliding(Player.Instance, weapons[i]))
                {
                    PlayerManager.EquipWeapon(weapons[i]);
                }
            }

            // hantera kollision mellan spelaren och kulor
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isDestroyed && IsColliding(Player.Instance, bullets[i]))
                {
                    Player.Instance.Kill();

                    // Kill all enemies
                    enemies.ForEach(x => x.WasHit(10000));
                    EnemySpawner.Reset();
                }
            }

            // hantera kollision mellan fiender och spelaren
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].isActive && IsColliding(Player.Instance, enemies[i]))
                {
                    Player.Instance.Kill();

                    // Kill all enemies
                    enemies.ForEach(x => x.WasHit(10000));
                    EnemySpawner.Reset();
                    break;
                }
            }
            #endregion
        }

        // Check för kollision mellan två enteties
        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.radius + b.radius;
            return !a.isDestroyed && !b.isDestroyed && Vector2.DistanceSquared(a.position, b.position) < radius * radius;
        }

        // Rita alla aktiva enteties
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var e in entities)
            {
                e.Draw(spriteBatch);
            }
        }

        #region HELPER FUNCTION
        public static IEnumerable GetNearbyEntities(Vector2 position, float radius)
        {
            return entities.Where(x => Vector2.DistanceSquared(position, x.position) < radius * radius);
        }
        #endregion
    }
}
