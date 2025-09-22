// File: src/Entities/Player.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Opal.Input;
using System;
namespace Opal.Entities;

public class Player : Entity
{
    private float _speed = 200f;
    private float _sprintMultiplier = 1.5f;
    private Texture2D _texture;
    private const int PLAYER_WIDTH = 32;
    private const int PLAYER_HEIGHT = 32;

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
        UpdateBoundingBox(PLAYER_WIDTH, PLAYER_HEIGHT);
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
        UpdateBoundingBox(PLAYER_WIDTH, PLAYER_HEIGHT);
    }

    // Method to check if a new position would be valid
    public bool CanMoveTo(Vector2 newPosition, World.Map map)
    {
        return map.IsPositionWalkable(newPosition, PLAYER_WIDTH, PLAYER_HEIGHT);
    }

    // Method to apply movement with collision checking
    public void ApplyMovement(float deltaTime, World.Map map)
    {
        if (Velocity == Vector2.Zero) return;

        Vector2 newPosition = Position;

        // Try horizontal movement first
        newPosition.X += Velocity.X * deltaTime;
        if (CanMoveTo(newPosition, map))
        {
            Position = newPosition;
        }
        else
        {
            newPosition.X = Position.X; // Revert X movement
        }

        // Then try vertical movement
        newPosition.Y += Velocity.Y * deltaTime;
        if (CanMoveTo(newPosition, map))
        {
            Position = newPosition;
        }

        UpdateBoundingBox(PLAYER_WIDTH, PLAYER_HEIGHT);
    }

    public override void Update(GameTime gameTime)
    {
        // Player-specific updates (status effects, etc.)
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_texture != null)
        {
            // Draw player texture centered on the player's position
            Rectangle destinationRect = new Rectangle(
                (int)(Position.X - PLAYER_WIDTH / 2),
                (int)(Position.Y - PLAYER_HEIGHT / 2),
                PLAYER_WIDTH,
                PLAYER_HEIGHT
            );
            spriteBatch.Draw(_texture, destinationRect, Color.White);
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
