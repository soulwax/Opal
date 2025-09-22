
using Microsoft.Xna.Framework;

namespace Opal.Graphics
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; } = 1f;
        public float ViewWidth { get; private set; }
        public float ViewHeight { get; private set; }

        private Vector2 _targetPosition;
        private float _followSpeed = 5f;

        // Map boundaries for camera constraints
        private Rectangle _mapBounds;

        public Camera(float viewWidth, float viewHeight)
        {
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
        }

        public void SetMapBounds(int mapWidth, int mapHeight, int tileSize)
        {
            _mapBounds = new Rectangle(0, 0, mapWidth * tileSize, mapHeight * tileSize);
        }

        public void Follow(Vector2 target)
        {
            _targetPosition = target;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Smooth camera following
            Vector2 difference = _targetPosition - Position;
            Vector2 newPosition = Position + difference * _followSpeed * deltaTime;

            // Constrain camera to map bounds, allowing player to reach edges
            float halfViewWidth = ViewWidth / (2f * Zoom);
            float halfViewHeight = ViewHeight / (2f * Zoom);

            newPosition.X = MathHelper.Clamp(newPosition.X,
                _mapBounds.Left + halfViewWidth,
                _mapBounds.Right - halfViewWidth);

            newPosition.Y = MathHelper.Clamp(newPosition.Y,
                _mapBounds.Top + halfViewHeight,
                _mapBounds.Bottom - halfViewHeight);

            Position = newPosition;
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(ViewWidth / 2, ViewHeight / 2, 0);
        }
    }
}