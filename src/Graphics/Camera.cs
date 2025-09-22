// File: src/Graphics/Camera.cs
using Microsoft.Xna.Framework;

namespace OpalMono.Graphics
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Zoom { get; set; } = 1f;
        public float ViewWidth { get; private set; }
        public float ViewHeight { get; private set; }

        private Vector2 _targetPosition;
        private float _followSpeed = 5f;

        public Camera(float viewWidth, float viewHeight)
        {
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
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
            Position += difference * _followSpeed * deltaTime;
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(ViewWidth / 2, ViewHeight / 2, 0);
        }
    }
}