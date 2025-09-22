// File: src/Entities/Entity.cs

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpalMono.Input;
namespace OpalMono.Entities
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

    public class Player : Entity
    {
        private float _speed = 200f;
        private float _sprintMultiplier = 1.5f;
        private Texture2D _texture;
        
        // RPG Stats
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public int Energy { get; set; } = 50;
        public int MaxEnergy { get; set; } = 50;
        public int Experience { get; set; } = 0;
        public int Level { get; set; } = 1;

        // Cyberpunk-specific
        public float AugmentationLevel { get; set; } = 0.3f; // 30% augmented initially
        public float HumanityLevel { get; set; } = 0.7f;   // 70% humanity remaining

        public Player(Texture2D texture, Vector2 startPosition)
        {
            _texture = texture;
            Position = startPosition;
            UpdateBoundingBox(32, 32);
        }

        public void HandleInput(InputHandler input, float deltaTime)
        {
            Vector2 movement = input.GetMovementVector();
            
            // Sprint mechanics
            float currentSpeed = _speed;
            if (input.IsKeyDown(Keys.LeftShift) && Energy > 0)
            {
                currentSpeed *= _sprintMultiplier;
                Energy = Math.Max(0, Energy - (int)(30 * deltaTime)); // Drain energy
            }
            else if (Energy < MaxEnergy)
            {
                Energy = Math.Min(MaxEnergy, Energy + (int)(10 * deltaTime)); // Regenerate
            }

            Velocity = movement * currentSpeed;
            Position += Velocity * deltaTime;
            
            UpdateBoundingBox(32, 32);
        }

        public override void Update(GameTime gameTime)
        {
            // Player-specific updates (status effects, etc.)
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, Position, Color.White);
            }
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            
            // Humanity loss from damage in cyberpunk setting
            HumanityLevel = Math.Max(0, HumanityLevel - damage * 0.001f);
        }

        public void GainExperience(int xp)
        {
            Experience += xp;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            int xpNeeded = Level * 100; // Simple formula
            if (Experience >= xpNeeded)
            {
                Level++;
                Experience -= xpNeeded;
                MaxHealth += 20;
                Health = MaxHealth;
                MaxEnergy += 10;
                Energy = MaxEnergy;
            }
        }
    }

    public class NPC : Entity
    {
        public string Name { get; set; }
        public string DialogueKey { get; set; }
        public bool IsInteractable { get; set; } = true;
        private Texture2D _texture;

        public NPC(string name, Texture2D texture, Vector2 position)
        {
            Name = name;
            _texture = texture;
            Position = position;
            UpdateBoundingBox(32, 32);
        }

        public override void Update(GameTime gameTime)
        {
            // NPC AI logic here
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, Position, Color.White);
            }
        }
    }
}
