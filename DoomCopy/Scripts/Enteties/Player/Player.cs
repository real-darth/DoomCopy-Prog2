using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy.Enteties
{
    class Player : Entity
    {
        #region FIELDS

        // Global referens till player
        private static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player();

                return instance;
            }
        }

        // shooting cooldown
        private int cooldownRemaining = 0;
        static Random rand = new Random();

        // respawn timer
        private int framesUntilRespawn = 0;
        
        // Check för death
        public bool isDead { get { return framesUntilRespawn > 0; } }

        // Konstruktor
        private Player()
        {
            sprite = Art.Player_Melee;
            position = GameRoot.ScreenSize / 2;
            radius = 10;

            color = Color.LightYellow;
        }

        #endregion

        #region METHODS 

        public void ResetRespawn()
        {
            framesUntilRespawn = 0;
            position = GameRoot.ScreenSize / 2;
        }

        public override void Update()
        {
            // Om spelare är död
            // Påbörja respawn timern
            if (isDead)
            {
                framesUntilRespawn--;
                return;
            }

            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            position += Velocity * PlayerManager.SpeedModifier;

            // Stoppa spelaren från att gå utanför skärmen
            // Fixa senare till en större bana
            position = Vector2.Clamp(position, size / 2, GameRoot.ScreenSize - size / 2);

            // Gör så spelaren roterar efter muspekaren
            var rotation = Input.GetAimDirection();
            base.rotation = rotation.VectorToAngle();

            //if (velocity.LengthSquared() > 0)


            // Shooting cooldown
            if (rotation.LengthSquared() > 0 && cooldownRemaining <= 0 && Input.WasShootingButtonPressed() && !PlayerManager.EquipedWeapon.IsReloading())
            {
                cooldownRemaining = PlayerManager.EquipedWeapon.GetCooldown();

                // Aim angles
                float aimAngle = rotation.VectorToAngle();

                // Offset Angle (Quaternion)
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                // Bullet hastighet
                Vector2 vel = Extensions.FromPolar(aimAngle, 11f);

                // Bullet spawn offset (position och rotation)
                Vector2 offset = Vector2.Transform(new Vector2(-25, -13), aimQuat);

                PlayerManager.EquipedWeapon.Shoot(position - offset, vel);

                // Spela ljud effekt
                //Sound.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
            }
            else if (cooldownRemaining <= 0 && Input.IsShootingButtonHeld() && !PlayerManager.EquipedWeapon.IsReloading())
            {
                cooldownRemaining = PlayerManager.EquipedWeapon.GetCooldown();

                // Aim angles
                float aimAngle = rotation.VectorToAngle();

                // Offset Angle (Quaternion)
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                // Random shooting spread (tas från wapens recoil)
                float randomSpread = (float)(rand.NextDouble() * 0.1f);

                // Bullet hastighet
                Vector2 vel = Extensions.FromPolar(aimAngle + randomSpread, 11f);

                // Bullet spawn offset (position och rotation)
                Vector2 offset = Vector2.Transform(new Vector2(-25, -13), aimQuat);

                PlayerManager.EquipedWeapon.Shoot(position - offset, vel);
            }

            // Kolla om spelaren släppt vapnet
            if (Input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Q))
            {
                PlayerManager.DropWeapon();
            }

            // If cooldown true, subtrahera tills noll
            if (cooldownRemaining > 0)
                cooldownRemaining--;
        }

        public void Kill()
        {
            framesUntilRespawn = 60;

            // Play SoundEffect
            Sound.Player.Play();

            // Reset alla spawners
            Spawner.Reset();

            PlayerManager.RemoveLife();

            // Om spelare har slut på liv, så sätts respawn time till nästan oändlighet
            framesUntilRespawn = PlayerManager.isGameOver ? int.MaxValue : 120;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                // rita skugga, sedan spelaren
                spriteBatch.Draw(Art.Shadow, position, null, Color.Black, rotation, new Vector2(Art.Shadow.Width, Art.Shadow.Height) / 2f, 1.4f, 0, 0);
                spriteBatch.Draw(PlayerManager.EquipedWeapon.GetPlayerSprite(), position, null, color, rotation, size / 2f, 1f, 0, 0);
            }
        }

        #endregion
    }
}