// File: src/Entities/Entity.cs

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Opal.Input;

namespace Opal.Entities
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Rotation { get; set; }
        public bool IsActive { get; set; } = true;
        public Rectangle BoundingBox { get; protected set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected void UpdateBoundingBox(int width, int height)
        {
            BoundingBox = new Rectangle(
                (int)Position.X - width / 2,
                (int)Position.Y - height / 2,
                width,
                height
            );
        }

        public bool CollidesWith(Entity other)
        {
            return BoundingBox.Intersects(other.BoundingBox);
        }
    }
}
