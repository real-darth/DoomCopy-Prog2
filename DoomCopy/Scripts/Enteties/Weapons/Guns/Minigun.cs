using DoomCopy.Scripts.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties.Weapons
{
    class Minigun : Weapon, ShootingSound
    {
        private float revupTimer = 20f;
        private float shootSoundTimer = 5f;

        public Minigun(Vector2 position, int damage, int clipCapacity, Texture2D weaponSprite) : base(weaponSprite, position, damage, clipCapacity)
        {
            name = "MINIGUN";
            sprite = Art.Minigun;

            radius = sprite.Width / 2f;
            reloadingTimer = this.reloadTime;
        }

        public override void Shoot(Vector2 position, Vector2 velocity)
        {
            // vänta med att skjuta
            if (revupTimer <= 0)
            {
                EntityManager.Add(new Bullet(position, velocity * 1.4f));
                currentClip--;

                ShootSoundeffect();

                if (currentClip <= 0)
                    reloading = true;
            }
            else
            {
                revupTimer--;
                if (Input.WasShootingButtonPressed())
                {
                    Sound.Minigun["Start"].Play();
                }
            }

        }

        public void ShootSoundeffect()
        {
            if (shootSoundTimer <= 0)
            {
                Sound.Minigun["Shoot"].Play();
                shootSoundTimer = 5;
            }
            else
            {
                shootSoundTimer--;
            }

        }

        // ljudeffekt timer
        private float emptySoundTimer = 8f;
        public override void Update()
        {
            // sakta ned player movement när den skjuter
            if (Input.IsShootingButtonHeld() && pickedup)
            {
                PlayerManager.SpeedModifier = 0.5f;

                // om magasinet är tomt spela annan ljudeffekt
                if(currentClip <= 0)
                {
                    if (emptySoundTimer >= 0)
                    {
                        emptySoundTimer--;
                    }
                    else
                    {
                        Sound.Minigun["Empty"].Play();
                        emptySoundTimer = 8f;
                    }

                }
            }
            else if (!Input.IsShootingButtonHeld() && pickedup)
            {
                // reset plater movement speed och spela ljudeffekt
                if (!Input.WasShootingButtonPressed())
                {
                    if (revupTimer <= 0)
                    {
                        Extensions.DelayAction(80, delegate 
                        {
                            Sound.Minigun["Stop"].Play();
                        });
                        revupTimer = 20f;
                    }
                }

                PlayerManager.SpeedModifier = 1f;
            }

            if(Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Q) && pickedup)
                PlayerManager.SpeedModifier = 1f;

            /*
            if (!Input.WasShootingButtonPressed())
            {
                Sound.Minigun["Stop"].Play();
            }
            */

            base.Update();
        }
    }
}
