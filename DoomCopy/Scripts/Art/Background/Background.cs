using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoomCopy
{
    static class Background
    {
        #region FIELDS

        // gridsystem
        private static Tile[,] grid;
        private static int width;
        private static int height;

        // class för varje tile
        public class Tile
        {
            public Texture2D sprite;
            public int x;
            public int y;

            public static int tileSize
            {
                get
                {
                    return Art.BackgroundTile.Height;
                }
            }

            // konstruktor
            public Tile(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private static Random rand = new Random();

        #endregion

        #region METHODS

        public static void Initialize()
        {
            width = (GameRoot.Viewport.Width / Tile.tileSize) + 1;
            height = (GameRoot.Viewport.Height / Tile.tileSize) + 1;

            grid = new Tile[width, height];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    grid[x, y] = new Tile(x, y);

                    if (GlowCheck())
                    {
                        grid[x, y].sprite = Art.BackgroundTile;
                    }
                    else
                    {
                        grid[x, y].sprite = Art.TransparetnBackgroundTile;
                    }
                }
            }
        }

        private static bool GlowCheck()
        {
            return rand.Next(0, 100) > 50;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    spriteBatch.Draw(grid[x,y].sprite, new Rectangle(x * Tile.tileSize, y * Tile.tileSize, Tile.tileSize, Tile.tileSize), Color.DarkRed);
                }
            }
        }

        #endregion
    }
}
