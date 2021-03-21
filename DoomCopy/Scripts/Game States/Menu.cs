using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    class MenuItem
    {
        #region FIELDS
        // PRIVATE VALUES
        private Texture2D texture;
        private Vector2 position;
        private int currentState;

        // GET VALUES
        public Texture2D Texture { get { return texture; } }
        public Vector2 Position { get { return position; } }
        public int State { get { return currentState; } }
        #endregion

        public MenuItem(Texture2D texture, Vector2 position, int currentState)
        {
            this.texture = texture;
            this.position = position;
            this.currentState = currentState;
        }
    }

    class Menu
    {
        #region FIELDS
        private List<MenuItem> menu;
        private int selected = 0;

        private float curHeight = 0;
        private int defaultMenuState;
        #endregion

        #region METHODS
        public Menu(int defaultMenuState)
        {
            // Create menu list
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
        }

        public void AddItem(Texture2D itemTexture, int state)
        {
            // Fix height on menu item
            float x = 0;
            float y = 0 + curHeight;

            // Change current height after item is added
            // marginal 20 pixels
            curHeight += itemTexture.Height + 20;

            MenuItem temp = new MenuItem(itemTexture, new Vector2(x, y), state);
            menu.Add(temp);
        }

        public int Update()
        {
            if (Input.WasKeyPressed(Keys.Down))
            {
                selected++;

                // If selected menu item reach past the limit
                // Reset it to the first value
                if (selected > menu.Count - 1)
                {
                    selected = 0;
                }
            }

            if (Input.WasKeyPressed(Keys.Up))
            {
                selected--;

                // If selected menu item reach past the limit
                // Reset it to the last value
                if (selected < 0)
                {
                    selected = menu.Count - 1;
                }
            }

            // Return selected menu item
            if (Input.WasKeyPressed(Keys.Enter))
            {
                return menu[selected].State;
            }

            // if no value was selected, return defualt
            return defaultMenuState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menu.Count; i++)
            {
                if (i == selected)
                {
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.RosyBrown);
                }
                else
                {
                    spriteBatch.Draw(menu[i].Texture, menu[i].Position, Color.White);
                }
            }
        }
        #endregion
    }
}
