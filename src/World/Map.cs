using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Graphics;
using System;

namespace Opal.World
{
    public enum TileType
    {
        Floor,
        Wall,
        Door,
        TerminalAccess,
        AugmentationStation,
        DataPoint
    }

    public struct Tile
    {
        public TileType Type;
        public bool IsWalkable;
        public Color Color;
        public string InteractionText;

        public Tile(TileType type, bool walkable = true, Color? color = null, string interaction = "")
        {
            Type = type;
            IsWalkable = walkable;
            Color = color ?? Color.Gray;
            InteractionText = interaction;
        }
    }

    public class Map
    {
        private Tile[,] _tiles;
        private int _width, _height;
        private int _tileSize = 32;
        private Texture2D _tileTexture;

        public int Width => _width;
        public int Height => _height;
        public int TileSize => _tileSize;

        public Map(int width, int height, Texture2D tileTexture)
        {
            _width = width;
            _height = height;
            _tileTexture = tileTexture;
            _tiles = new Tile[width, height];
            GenerateMap();
        }

        private void GenerateMap()
        {
            // Create a simple cyberpunk city district
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                    {
                        _tiles[x, y] = new Tile(TileType.Wall, false, Color.DarkGray);
                    }
                    else if ((x + y) % 10 == 0) // Scattered terminals
                    {
                        _tiles[x, y] = new Tile(TileType.TerminalAccess, true, Color.Cyan, "Access Terminal");
                    }
                    else if ((x * y) % 50 == 0) // Rare augmentation stations
                    {
                        _tiles[x, y] = new Tile(TileType.AugmentationStation, true, Color.Red, "Augmentation Interface");
                    }
                    else
                    {
                        _tiles[x, y] = new Tile(TileType.Floor, true, Color.DimGray);
                    }
                }
            }
        }

        public bool IsWalkable(Vector2 position)
        {
            int tileX = (int)(position.X / _tileSize);
            int tileY = (int)(position.Y / _tileSize);

            if (tileX < 0 || tileX >= _width || tileY < 0 || tileY >= _height)
                return false;

            return _tiles[tileX, tileY].IsWalkable;
        }

        // Enhanced collision checking that considers entity size
        public bool IsPositionWalkable(Vector2 position, int entityWidth, int entityHeight)
        {
            // Calculate the bounding box of the entity at this position
            int left = (int)((position.X - entityWidth / 2) / _tileSize);
            int right = (int)((position.X + entityWidth / 2) / _tileSize);
            int top = (int)((position.Y - entityHeight / 2) / _tileSize);
            int bottom = (int)((position.Y + entityHeight / 2) / _tileSize);

            // Check all tiles that the entity would occupy
            for (int x = left; x <= right; x++)
            {
                for (int y = top; y <= bottom; y++)
                {
                    if (x < 0 || x >= _width || y < 0 || y >= _height)
                        return false;

                    if (!_tiles[x, y].IsWalkable)
                        return false;
                }
            }

            return true;
        }

        public Tile GetTileAt(Vector2 position)
        {
            int tileX = (int)(position.X / _tileSize);
            int tileY = (int)(position.Y / _tileSize);

            if (tileX < 0 || tileX >= _width || tileY < 0 || tileY >= _height)
                return new Tile(TileType.Wall, false);

            return _tiles[tileX, tileY];
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            // Calculate visible tile range
            int startX = Math.Max(0, (int)(camera.Position.X / _tileSize) - 1);
            int endX = Math.Min(_width, startX + (int)(camera.ViewWidth / _tileSize) + 2);
            int startY = Math.Max(0, (int)(camera.Position.Y / _tileSize) - 1);
            int endY = Math.Min(_height, startY + (int)(camera.ViewHeight / _tileSize) + 2);

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Vector2 drawPosition = new Vector2(x * _tileSize, y * _tileSize);
                    spriteBatch.Draw(_tileTexture, drawPosition, _tiles[x, y].Color);
                }
            }
        }
    }
}