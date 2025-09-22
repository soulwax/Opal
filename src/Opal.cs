// File: src/Opal.cs

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OpalMono;


public class Opal : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private InputHandler _inputHandler;

    private Vector2 mousePosition;
    // TODO: dummy player variables here
    private Vector2 playerPosition = new Vector2(400, 300);
    private float playerSpeed = 200f; // pixels per second

    float mouseDrag = 0f;

    public Opal(string title, int width = 800, int height = 600)
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        Window.Title = title;
    }


    protected override void Initialize()
    {
        // TODO: Add more initialization logic here
        base.Initialize();

    }

    protected override void LoadContent()
    {
        _inputHandler = new InputHandler();
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: Use this.Content to load game content here
    }

protected override void Update(GameTime gameTime)
{
    _inputHandler.Update();
    
    // Exit conditions
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
        _inputHandler.IsKeyDown(Keys.Escape))
        Exit();

    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

    // === KEYBOARD INPUT ===
    
    // Single key press detection
    if (_inputHandler.IsKeyPressed(Keys.Space))
    {
        Console.WriteLine("Space pressed - Jump or action triggered!");
    }

    // Key release detection
    if (_inputHandler.IsKeyReleased(Keys.Space))
    {
        Console.WriteLine("Space released - Stop jump or action!");
    }

    // Hold detection for continuous actions
    if (_inputHandler.IsKeyDown(Keys.LeftShift))
    {
        playerSpeed = 400f; // Sprint speed
    }
    else
    {
        playerSpeed = 200f; // Normal speed
    }

    // Multiple key alternatives
    if (_inputHandler.IsAnyKeyPressed(Keys.Enter, Keys.E, Keys.F))
    {
        Console.WriteLine("Interact key pressed!");
    }

    // Check for any input this frame
    if (_inputHandler.JustPressedKeys.Count > 0)
    {
        Console.WriteLine($"Keys pressed this frame: {string.Join(", ", _inputHandler.JustPressedKeys)}");
    }

    // === MOUSE INPUT ===
    
    // Basic mouse button detection
    if (_inputHandler.IsMouseButtonPressed(MouseButton.Left))
    {
        mousePosition = _inputHandler.MousePosition;
        Console.WriteLine($"LMB pressed at {mousePosition.X}, {mousePosition.Y}");
    }

    if (_inputHandler.IsMouseButtonReleased(MouseButton.Left))
    {
        mousePosition = _inputHandler.MousePosition;
        Console.WriteLine($"LMB released at {mousePosition.X}, {mousePosition.Y}");
    }

    // Right-click for context menu or secondary action
    if (_inputHandler.IsMouseButtonPressed(MouseButton.Right))
    {
        Console.WriteLine($"Right-click context menu at {_inputHandler.MousePosition}");
    }

    // Middle mouse button for camera or special actions
    if (_inputHandler.IsMouseButtonDown(MouseButton.Middle))
    {
        Console.WriteLine("Middle mouse held - camera pan mode");
    }

    // Mouse wheel scrolling
    if (_inputHandler.ScrollWheelDelta != 0)
    {
        float zoomChange = _inputHandler.ScrollWheelDelta * 0.001f;
        Console.WriteLine($"Zoom changed by {zoomChange}");
    }

    // === DRAG SYSTEM ===
    
    // Drag started (only triggers once when drag begins)
    if (_inputHandler.DragStarted)
    {
        Console.WriteLine($"Drag started at {_inputHandler.DragStartPosition} with {_inputHandler.DragButton}");
    }

    // Currently dragging
    if (_inputHandler.IsDragging)
    {
        Vector2 dragDelta = _inputHandler.DragDelta;
        float dragDistance = _inputHandler.DragDistance;
        
        Console.WriteLine($"Dragging: Delta({dragDelta.X:F1}, {dragDelta.Y:F1}), Distance: {dragDistance:F1}");
        
        // Example: Move player with drag
        // playerPosition = _inputHandler.DragStartPosition + dragDelta;
        
        // Example: Camera panning
        // cameraOffset = baseCameraOffset + dragDelta;
        
        // Check if drag exceeds certain distance
        if (_inputHandler.IsDragDistanceGreaterThan(50f))
        {
            Console.WriteLine("Long drag detected!");
        }
    }

    // Drag completed (only triggers when drag ends)
    if (_inputHandler.IsDragCompleted())
    {
        Console.WriteLine($"Drag completed! Total distance: {_inputHandler.DragDistance:F1}");
        
        // Example: Drop object if dragged to specific area
        Rectangle dropZone = new Rectangle(100, 100, 200, 200);
        if (_inputHandler.IsDragInBounds(dropZone))
        {
            Console.WriteLine("Object dropped in valid zone!");
        }
    }

    // === MOVEMENT SYSTEM ===
    
    // Get movement vector from WASD/Arrow keys
    Vector2 movement = _inputHandler.GetMovementVector(useWASD: true, useArrows: true);
    playerPosition += movement * playerSpeed * deltaTime;
    
    // Alternative: Custom movement with different keys
    Vector2 customMovement = Vector2.Zero;
    if (_inputHandler.IsKeyDown(Keys.Up)) customMovement.Y -= 1;
    if (_inputHandler.IsKeyDown(Keys.Down)) customMovement.Y += 1;
    if (_inputHandler.IsKeyDown(Keys.Left)) customMovement.X -= 1;
    if (_inputHandler.IsKeyDown(Keys.Right)) customMovement.X += 1;
    
    if (customMovement != Vector2.Zero)
    {
        customMovement.Normalize();
        // Use customMovement for something else
    }

    // === BOUNDS CHECKING ===
    
    // Keep player within screen bounds
    playerPosition.X = MathHelper.Clamp(playerPosition.X, 0, _graphics.PreferredBackBufferWidth);
    playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0, _graphics.PreferredBackBufferHeight);

    // Check if mouse is in specific areas
    Rectangle uiArea = new Rectangle(50, 50, 150, 100);
    if (_inputHandler.IsMouseInBounds(uiArea))
    {
        if (_inputHandler.IsMouseButtonPressed(MouseButton.Left))
        {
            Console.WriteLine("UI element clicked!");
        }
    }

    // Circular bounds checking
    Vector2 circleCenter = new Vector2(400, 300);
    float circleRadius = 100f;
    if (_inputHandler.IsMouseInBounds(circleCenter, circleRadius))
    {
        Console.WriteLine("Mouse is inside circular area");
    }

    // === UTILITY FEATURES ===
    
    // Mouse movement detection
    if (_inputHandler.HasMouseMoved())
    {
        Vector2 mouseDelta = _inputHandler.MouseDelta;
        float mouseSpeed = _inputHandler.GetMouseMovementDistance();
        
        // Only log significant mouse movement to avoid spam
        if (mouseSpeed > 5f)
        {
            Console.WriteLine($"Mouse moved {mouseSpeed:F1} pixels");
        }
    }

    // Debug: Show all currently held keys
    if (_inputHandler.CurrentPressedKeys.Length > 0)
    {
        // Uncomment for debugging
        // Console.WriteLine($"Held keys: {string.Join(", ", _inputHandler.CurrentPressedKeys)}");
    }

    // === ADVANCED EXAMPLES ===
    
    // Combo detection example
    if (_inputHandler.IsKeyDown(Keys.LeftControl) && _inputHandler.IsKeyPressed(Keys.S))
    {
        Console.WriteLine("Ctrl+S pressed - Save game!");
    }

    // Toggle example
    if (_inputHandler.IsKeyPressed(Keys.Tab))
    {
        Console.WriteLine("Toggle inventory/menu");
    }

    // Charge-up action example
    if (_inputHandler.IsKeyDown(Keys.C))
    {
        // Charge up power while key is held
        Console.WriteLine("Charging power...");
    }
    if (_inputHandler.IsKeyReleased(Keys.C))
    {
        Console.WriteLine("Power released!");
    }

    // Double-click simulation (you'd need to add timing logic)
    // This is just showing the concept
    if (_inputHandler.IsMouseButtonPressed(MouseButton.Left))
    {
        // Check if this is a double-click by tracking time between clicks
        // Implementation would require additional timing variables
    }

    // === DRAG CONFIGURATION ===
    
    // Adjust drag sensitivity dynamically
    if (_inputHandler.IsKeyDown(Keys.LeftAlt))
    {
        _inputHandler.DragThreshold = 20f; // Require more movement to start drag
    }
    else
    {
        _inputHandler.DragThreshold = 5f; // Default sensitivity
    }

    // Important: Call base.Update last
    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add drawing code here

        base.Draw(gameTime);
    }
}
