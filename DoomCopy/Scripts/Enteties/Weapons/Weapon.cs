using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Weapon : Entity
    {
        protected string name;
        protected int damage;
        protected int ammunition = 0;
        protected int capacity;
        protected int currentClip;

        // Används i Players Update funktion
        protected int shootingCooldown;

        protected Texture2D playerWeaponSprite;

        protected TimeSpan reloadTime;
        protected TimeSpan reloadingTimer;
        protected bool reloading = false;

        protected bool melee;
        protected bool pickedup = false;
        private TimeSpan dissepearingTimer = TimeSpan.FromMilliseconds(10000);

        #region KONSTRUKTORS
        public Weapon(Texture2D sprite, Vector2 position, int damage, int ammunition, int clipCapacity)
        {
            playerWeaponSprite = sprite;
            this.position = position;

            this.damage = damage;
            this.ammunition = ammunition;

            capacity = clipCapacity;
            currentClip = clipCapacity;
        }

        // UTAN AMMUNITION
        public Weapon(Texture2D sprite, Vector2 position, int damage, int clipCapacity)
        {
            playerWeaponSprite = sprite;
            this.position = position;

            this.damage = damage;

            capacity = clipCapacity;
            currentClip = clipCapacity;
        }

        // KONSTRUKTOR FÖR FISTS
        public Weapon(Texture2D sprite) 
        {
            playerWeaponSprite = sprite;
        }
        #endregion

        public override void Update()
        {
            // kolla om vapnet är plockat upp
            if (!pickedup)
            {
                // kolla om vapnet har legat 10 sekunder på marken, om det är sant förstör det
                dissepearingTimer -= GameRoot.GameTime.ElapsedGameTime;
                if(dissepearingTimer < TimeSpan.Zero)
                {
                    isDestroyed = true;
                    return;
                }
            }

            // kolla ammunition, reloada om magasinet är tomt och man har ammunition kvar
            if (reloading == true && ammunition > 0)
            {
                reloadingTimer -= GameRoot.GameTime.ElapsedGameTime;
                if (reloadingTimer < TimeSpan.Zero)
                {
                    if(ammunition >= capacity)
                    {
                        ammunition -= (capacity - currentClip);
                        currentClip = capacity;
                    }
                    else if(ammunition < capacity)
                    {
                        int kulorSomSaknas = capacity - currentClip;
                        ammunition -= kulorSomSaknas;
                        currentClip += kulorSomSaknas;

                        if(ammunition < 0)
                        {
                            ammunition = 0;
                        }
                    }

                    reloadingTimer = reloadTime;
                    reloading = false;
                }
            }

            // klickar man på R så reloadar den manuellt
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.R) && !melee && ammunition > 0 && currentClip < capacity)
            {
                reloading = true;
            }
        }

        public virtual void Shoot(Vector2 position, Vector2 velocity)
        {
            EntityManager.Add(new Bullet(position, velocity * 2.2f));
            currentClip--;

            if(currentClip <= 0)
            {
                reloading = true;
            }
        }

        public virtual void MuzzleFlash(Texture2D muzzle)
        {

        }

        #region GET VALUES
        public bool IsReloading()
        {
            return reloading;
        }

        public string GetBullets()
        {
            return currentClip.ToString();
        }

        public string GetAmmunition()
        {
            return ammunition.ToString();
        }

        public string GetReloadTime()
        {
            double second = reloadingTimer.TotalSeconds;
            return second.ToString("F1");
        }

        public int GetDamage()
        {
            return damage;
        }

        public int GetCooldown()
        {
            return shootingCooldown;
        }

        public Texture2D GetPlayerSprite()
        {
            return playerWeaponSprite;
        }

        public override string ToString()
        {
            return name;
        }
        #endregion

        public void SetPickedUp(bool pickup)
        {
            pickedup = pickup;
            hideSprite = pickup;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!hideSprite) {

                spriteBatch.Draw(Art.Shadow, position, null, Color.Black, rotation, new Vector2(Art.Shadow.Width, Art.Shadow.Height) / 2f, 1.3f, 0, 0);
                spriteBatch.Draw(sprite, position, null, color, rotation, size / 2f, 1.2f, 0, 0);
            }
        }
    }
}
