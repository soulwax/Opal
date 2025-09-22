// File: src/Opal.cs - UPDATED
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpalMono.Core;
using OpalMono.Entities;
using OpalMono.World;
using OpalMono.Graphics;
using OpalMono.Systems;
using OpalMono.Input;

namespace OpalMono
{
    public class Opal : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private InputHandler _inputHandler;
        private GameStateManager _stateManager;

        // Core systems
        private Camera _camera;
        private DialogueSystem _dialogueSystem;

        // Game objects
        private Player _player;
        private Map _currentMap;

        // Textures (temporary placeholders)
        private Texture2D _playerTexture;
        private Texture2D _tileTexture;
        private SpriteFont _font;

        // Game state
        private bool _showDebugInfo = false;
        private bool _isInDialogue = false;

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
            _inputHandler = new InputHandler();
            _stateManager = new GameStateManager();
            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _dialogueSystem = new DialogueSystem();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create placeholder textures
            _playerTexture = CreateColorTexture(Color.Lime, 32, 32);
            _tileTexture = CreateColorTexture(Color.Gray, 32, 32);

            // Load font (you'll need to add this to Content.mgcb)
            // _font = Content.Load<SpriteFont>("DefaultFont");

            // Initialize game objects
            _player = new Player(_playerTexture, new Vector2(400, 300));
            _currentMap = new Map(50, 50, _tileTexture);

            // Setup camera constraints based on map size
            _camera.SetMapBounds(_currentMap.Width, _currentMap.Height, _currentMap.TileSize);

            // Setup dialogue system
            _dialogueSystem.LoadDialogues();

            // Set camera to follow player
            _camera.Follow(_player.Position);
        }

        protected override void Update(GameTime gameTime)
        {
            _inputHandler.Update();

            // Exit conditions
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                _inputHandler.IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Debug toggle
            if (_inputHandler.IsKeyPressed(Keys.F1))
                _showDebugInfo = !_showDebugInfo;

            // Handle dialogue system
            if (_dialogueSystem.IsActive)
            {
                HandleDialogue();
            }
            else
            {
                HandleGameplay(deltaTime, gameTime);
            }

            // Update camera
            _camera.Follow(_player.Position);
            _camera.Update(gameTime);

            base.Update(gameTime);
        }

        private void HandleDialogue()
        {
            // Handle dialogue input
            if (_inputHandler.IsKeyPressed(Keys.D1))
                _dialogueSystem.SelectChoice(0);
            else if (_inputHandler.IsKeyPressed(Keys.D2))
                _dialogueSystem.SelectChoice(1);
            else if (_inputHandler.IsKeyPressed(Keys.D3))
                _dialogueSystem.SelectChoice(2);
            else if (_inputHandler.IsKeyPressed(Keys.Enter) || _inputHandler.IsKeyPressed(Keys.Space))
                _dialogueSystem.EndDialogue();
        }

        private void HandleGameplay(float deltaTime, GameTime gameTime)
        {
            // Handle input first
            _player.HandleInput(_inputHandler, deltaTime);

            // Apply movement with proper collision detection
            _player.ApplyMovement(deltaTime, _currentMap);

            // Interaction system
            if (_inputHandler.IsKeyPressed(Keys.E) || _inputHandler.IsKeyPressed(Keys.F))
            {
                HandleInteraction();
            }

            // Player updates
            _player.Update(gameTime);

            // Check for special tiles
            CheckTileInteractions();

            // Example: Debug commands
            if (_inputHandler.IsKeyPressed(Keys.H))
            {
                _player.TakeDamage(10);
                Console.WriteLine($"Player health: {_player.Health}/{_player.MaxHealth}");
            }

            if (_inputHandler.IsKeyPressed(Keys.X))
            {
                _player.GainExperience(25);
                Console.WriteLine($"Player XP: {_player.Experience}, Level: {_player.Level}");
            }

            // Debug info about player position
            if (_showDebugInfo && _inputHandler.IsKeyPressed(Keys.P))
            {
                Console.WriteLine($"Player Position: {_player.Position}");
                Console.WriteLine($"Camera Position: {_camera.Position}");
            }
        }

        private void HandleInteraction()
        {
            var currentTile = _currentMap.GetTileAt(_player.Position);

            switch (currentTile.Type)
            {
                case TileType.TerminalAccess:
                    Console.WriteLine("Accessing data terminal...");
                    _dialogueSystem.StartDialogue("start");
                    break;

                case TileType.AugmentationStation:
                    Console.WriteLine("Augmentation interface detected...");
                    // Handle augmentation mechanics
                    _player.AugmentationLevel = Math.Min(1.0f, _player.AugmentationLevel + 0.1f);
                    _player.HumanityLevel = Math.Max(0.0f, _player.HumanityLevel - 0.05f);
                    Console.WriteLine($"Augmentation: {_player.AugmentationLevel:P0}, Humanity: {_player.HumanityLevel:P0}");
                    break;

                default:
                    Console.WriteLine("Nothing to interact with here.");
                    break;
            }
        }

        private void CheckTileInteractions()
        {
            var currentTile = _currentMap.GetTileAt(_player.Position);

            // Passive effects from standing on certain tiles
            if (currentTile.Type == TileType.DataPoint)
            {
                // Slow data absorption
                _player.GainExperience(1);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // World rendering with camera transform
            _spriteBatch.Begin(transformMatrix: _camera.GetTransform());

            // Draw map
            _currentMap.Draw(_spriteBatch, _camera);

            // Draw player
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            // UI rendering (no camera transform)
            _spriteBatch.Begin();

            DrawUI();

            if (_dialogueSystem.IsActive)
                DrawDialogue();

            if (_showDebugInfo)
                DrawDebugInfo();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawUI()
        {
            // Health bar
            Rectangle healthBarBg = new Rectangle(20, 20, 200, 20);
            Rectangle healthBar = new Rectangle(20, 20,
                (int)(200 * ((float)_player.Health / _player.MaxHealth)), 20);

            DrawRectangle(_spriteBatch, healthBarBg, Color.DarkRed);
            DrawRectangle(_spriteBatch, healthBar, Color.Red);

            // Energy bar
            Rectangle energyBarBg = new Rectangle(20, 45, 150, 15);
            Rectangle energyBar = new Rectangle(20, 45,
                (int)(150 * ((float)_player.Energy / _player.MaxEnergy)), 15);

            DrawRectangle(_spriteBatch, energyBarBg, Color.DarkBlue);
            DrawRectangle(_spriteBatch, energyBar, Color.Cyan);

            // Augmentation/Humanity indicators
            Rectangle augBar = new Rectangle(20, 65, 100, 10);
            Rectangle humanityBar = new Rectangle(125, 65, 100, 10);

            DrawRectangle(_spriteBatch, augBar, Color.Orange * _player.AugmentationLevel);
            DrawRectangle(_spriteBatch, humanityBar, Color.White * _player.HumanityLevel);

            // Level indicator
            if (_font != null)
            {
                _spriteBatch.DrawString(_font, $"Level {_player.Level}", new Vector2(20, 80), Color.White);
            }
        }

        private void DrawDialogue()
        {
            var node = _dialogueSystem.CurrentNode;

            // Dialogue background
            Rectangle dialogueBg = new Rectangle(50, _graphics.PreferredBackBufferHeight - 200,
                _graphics.PreferredBackBufferWidth - 100, 150);
            DrawRectangle(_spriteBatch, dialogueBg, Color.Black * 0.8f);

            // For now, draw text placeholders since we don't have font loaded
            // In a real implementation, you'd draw the dialogue text and choices here
            DrawRectangle(_spriteBatch, new Rectangle(60, dialogueBg.Y + 10, 300, 20), Color.Gray);
            DrawRectangle(_spriteBatch, new Rectangle(60, dialogueBg.Y + 40, 250, 15), Color.DarkGray);
            DrawRectangle(_spriteBatch, new Rectangle(60, dialogueBg.Y + 60, 280, 15), Color.DarkGray);
        }

        private void DrawDebugInfo()
        {
            // Debug information overlay
            var debugBg = new Rectangle(300, 20, 250, 140);
            DrawRectangle(_spriteBatch, debugBg, Color.Black * 0.7f);

            // Draw debug rectangles as placeholders for text
            DrawRectangle(_spriteBatch, new Rectangle(310, 30, 230, 15), Color.Green);  // Position
            DrawRectangle(_spriteBatch, new Rectangle(310, 50, 180, 15), Color.Yellow); // Health
            DrawRectangle(_spriteBatch, new Rectangle(310, 70, 150, 15), Color.Cyan);   // Energy
            DrawRectangle(_spriteBatch, new Rectangle(310, 90, 200, 15), Color.Orange); // Augmentation
            DrawRectangle(_spriteBatch, new Rectangle(310, 110, 170, 15), Color.White); // Humanity
            DrawRectangle(_spriteBatch, new Rectangle(310, 130, 190, 15), Color.Pink);  // Camera pos

            // Add instructions for debug mode
            DrawRectangle(_spriteBatch, new Rectangle(20, 100, 200, 15), Color.Gray);   // "Press P for pos info"
        }

        private Texture2D CreateColorTexture(Color color, int width, int height)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

        private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            var pixel = CreateColorTexture(Color.White, 1, 1);
            spriteBatch.Draw(pixel, rectangle, color);
        }
    }
}