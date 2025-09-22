using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace OpalMono
{
    public class InputHandler
    {
        #region Private Fields
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private readonly HashSet<Keys> _pressedKeys;
        private readonly HashSet<Keys> _releasedKeys;
        #endregion

        #region Public Properties
        
        // Mouse Properties
        public Vector2 MousePosition => new Vector2(_currentMouseState.X, _currentMouseState.Y);
        public Vector2 PreviousMousePosition => new Vector2(_previousMouseState.X, _previousMouseState.Y);
        public Vector2 MouseDelta => new Vector2(_currentMouseState.X - _previousMouseState.X, _currentMouseState.Y - _previousMouseState.Y);
        public int ScrollWheelValue => _currentMouseState.ScrollWheelValue;
        public int ScrollWheelDelta => _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        
        // Keyboard Properties
        public Keys[] CurrentPressedKeys => _currentKeyboardState.GetPressedKeys();
        public IReadOnlyCollection<Keys> JustPressedKeys => _pressedKeys;
        public IReadOnlyCollection<Keys> JustReleasedKeys => _releasedKeys;
        
        #endregion

        #region Constructor
        public InputHandler()
        {
            _pressedKeys = new HashSet<Keys>();
            _releasedKeys = new HashSet<Keys>();
        }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Updates the input states. Call this once per frame in your Game's Update method.
        /// </summary>
        public void Update()
        {
            // Store previous states
            _previousKeyboardState = _currentKeyboardState;
            _previousMouseState = _currentMouseState;
            
            // Get current states
            _currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
            
            // Update pressed/released key collections
            UpdateKeyCollections();
        }

        #region Keyboard Methods
        
        /// <summary>
        /// Checks if a key is currently being held down.
        /// </summary>
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key is currently up (not pressed).
        /// </summary>
        public bool IsKeyUp(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a key was just pressed this frame.
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks if a key was just released this frame.
        /// </summary>
        public bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if any of the specified keys are currently down.
        /// </summary>
        public bool IsAnyKeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (IsKeyDown(key)) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the specified keys were just pressed.
        /// </summary>
        public bool IsAnyKeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (IsKeyPressed(key)) return true;
            }
            return false;
        }

        #endregion

        #region Mouse Methods
        
        /// <summary>
        /// Checks if a mouse button is currently being held down.
        /// </summary>
        public bool IsMouseButtonDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        /// <summary>
        /// Checks if a mouse button was just pressed this frame.
        /// </summary>
        public bool IsMouseButtonPressed(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed && 
                                   _previousMouseState.LeftButton == ButtonState.Released,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed && 
                                    _previousMouseState.RightButton == ButtonState.Released,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed && 
                                     _previousMouseState.MiddleButton == ButtonState.Released,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Pressed && 
                                       _previousMouseState.XButton1 == ButtonState.Released,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Pressed && 
                                       _previousMouseState.XButton2 == ButtonState.Released,
                _ => false
            };
        }

        /// <summary>
        /// Checks if a mouse button was just released this frame.
        /// </summary>
        public bool IsMouseButtonReleased(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Released && 
                                   _previousMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Released && 
                                    _previousMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Released && 
                                     _previousMouseState.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Released && 
                                       _previousMouseState.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Released && 
                                       _previousMouseState.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        /// <summary>
        /// Checks if the mouse is within the specified rectangular bounds.
        /// </summary>
        public bool IsMouseInBounds(Rectangle bounds)
        {
            return bounds.Contains(MousePosition);
        }

        /// <summary>
        /// Checks if the mouse is within the specified circular bounds.
        /// </summary>
        public bool IsMouseInBounds(Vector2 center, float radius)
        {
            return Vector2.Distance(MousePosition, center) <= radius;
        }

        /// <summary>
        /// Gets the distance the mouse has moved since last frame.
        /// </summary>
        public float GetMouseMovementDistance()
        {
            return MouseDelta.Length();
        }

        /// <summary>
        /// Checks if the mouse has moved since the last frame.
        /// </summary>
        public bool HasMouseMoved()
        {
            return MouseDelta != Vector2.Zero;
        }

        #endregion

        #region Utility Methods
        
        /// <summary>
        /// Gets input as a movement vector based on WASD or arrow keys.
        /// </summary>
        public Vector2 GetMovementVector(bool useWASD = true, bool useArrows = true)
        {
            Vector2 movement = Vector2.Zero;

            if (useWASD)
            {
                if (IsKeyDown(Keys.W)) movement.Y -= 1;
                if (IsKeyDown(Keys.S)) movement.Y += 1;
                if (IsKeyDown(Keys.A)) movement.X -= 1;
                if (IsKeyDown(Keys.D)) movement.X += 1;
            }

            if (useArrows)
            {
                if (IsKeyDown(Keys.Up)) movement.Y -= 1;
                if (IsKeyDown(Keys.Down)) movement.Y += 1;
                if (IsKeyDown(Keys.Left)) movement.X -= 1;
                if (IsKeyDown(Keys.Right)) movement.X += 1;
            }

            // Normalize diagonal movement
            if (movement != Vector2.Zero)
                movement.Normalize();

            return movement;
        }

        /// <summary>
        /// Resets the input handler state. Useful for scene transitions.
        /// </summary>
        public void Reset()
        {
            _previousKeyboardState = new KeyboardState();
            _currentKeyboardState = new KeyboardState();
            _previousMouseState = new MouseState();
            _currentMouseState = new MouseState();
            _pressedKeys.Clear();
            _releasedKeys.Clear();
        }

        #endregion

        #endregion

        #region Private Methods
        
        private void UpdateKeyCollections()
        {
            _pressedKeys.Clear();
            _releasedKeys.Clear();

            Keys[] currentKeys = _currentKeyboardState.GetPressedKeys();
            Keys[] previousKeys = _previousKeyboardState.GetPressedKeys();

            // Find newly pressed keys
            foreach (Keys key in currentKeys)
            {
                if (_previousKeyboardState.IsKeyUp(key))
                {
                    _pressedKeys.Add(key);
                }
            }

            // Find newly released keys
            foreach (Keys key in previousKeys)
            {
                if (_currentKeyboardState.IsKeyUp(key))
                {
                    _releasedKeys.Add(key);
                }
            }
        }

        #endregion
    }

    #region Enums
    
    public enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }

    #endregion

    #region Usage Example
    
    /*
    ? Usage example inside a Game class:

    protected override void LoadContent()
    {
        _inputHandler = new InputHandler();
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _inputHandler.Update();

        // Example usage:
        if (_inputHandler.IsKeyPressed(Keys.Space))
        {
            // Jump logic
        }

        if (_inputHandler.IsMouseButtonDown(MouseButton.Left))
        {
            // Shoot logic
        }

        Vector2 movement = _inputHandler.GetMovementVector();
        playerPosition += movement * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_inputHandler.HasMouseMoved())
        {
            // Handle mouse look or UI hover
        }

        base.Update(gameTime);
    }
    */
    
    #endregion
}