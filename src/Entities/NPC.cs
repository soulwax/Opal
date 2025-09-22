// File: src/Entities/NPC.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Entities;

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
