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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (_inputHandler.IsKeyPressed(Keys.Space))
        {
            // TODO: Simple example for e.g. Jump logic
        }
        if (_inputHandler.IsMouseButtonReleased(MouseButton.Left))
        {
            // TODO: LMB click logic
            mousePosition = _inputHandler.MousePosition;
            Console.WriteLine($"LMB Released at {mousePosition.X}, {mousePosition.Y}");
        }

        // TODO: dummy player movement logic
        Vector2 movement = _inputHandler.GetMovementVector();
        playerPosition += movement * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


        // Mouse drag logic
        if (_inputHandler.IsMouseButtonDown(MouseButton.Left))
        {
            if (_inputHandler.HasMouseMoved())
            {
                Vector2 mouseDelta = _inputHandler.MouseDelta;
                mouseDrag += mouseDelta.Length();
                Console.WriteLine($"Mouse dragging. Total drag distance: {mouseDrag}");
            }
        }

        // TODO: Add more update logic here

        // ! Important call to base.Update last
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add drawing code here

        base.Draw(gameTime);
    }
}
