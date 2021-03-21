using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoomCopy.Components;
using DoomCopy.Components.StatusEffect;

namespace DoomCopy
{
    public abstract class Entity
    {
        #region FIELDS

        // Entity Sprite
        protected Texture2D sprite;
        public bool hideSprite = false;

        // Entity färg är vit by default
        protected Color color = Color.White;

        // Position
        public Vector2 position;
        public bool freezeMovement = false;

        // Rörelseriktining / Hastighet
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return freezeMovement == false ? velocity : Vector2.Zero; }
            set { velocity = value; }
        }

        // Rotation
        public float rotation;

        // Collisions radie
        public float radius = 20;

        // Check för death
        public bool isDestroyed;

        // Component för status effecter
        protected StatusEffects statusEffects = new StatusEffects();

        // Sprite storlek
        public Vector2 size
        {
            get { return sprite == null ? Vector2.Zero : new Vector2(sprite.Width, sprite.Height); }
        }

        #endregion

        #region FIELDS

        public Entity() { }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(!hideSprite)
                spriteBatch.Draw(sprite, position, null, color, rotation, size / 2f, 1f, 0, 0);
        }

        #region PYSHICS FUNCTION
        public virtual void PushBackwards(float magnitutde)
        {
            position -= Velocity * magnitutde;
        }

        /*
        public virtual void ForceForward(float magnitutde)
        {
            position += position * magnitutde;
        }
        */
        #endregion

        #region EFFECT FUNCTIONS
        public void AddEffect(IStatusEffect component)
        {
            statusEffects.StatusEffectList.Add(component);
        }

        /*
        public void RemoveEffect(IStatusEffect component)
        {
            statusEffects.StatusEffectList.Remove(component);
        }
        */
        #endregion

        #endregion
    }
}
