// File: src/Core/GameState.cs

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Opal.Input;

namespace Opal.Core
{
    public enum GameStateType
    {
        Menu,
        Playing,
        Inventory,
        Dialogue,
        Paused,
        Loading
    }

    public abstract class GameState
    {
        public abstract void Update(GameTime gameTime, InputHandler input);
        public abstract void Draw(GameTime gameTime);
        public abstract void Enter();
        public abstract void Exit();
    }

    public class GameStateManager
    {
        private Stack<GameState> _states = new();
        private List<GameState> _statesToAdd = new();
        private List<GameState> _statesToRemove = new();

        public void PushState(GameState state)
        {
            _statesToAdd.Add(state);
        }

        public void PopState()
        {
            if (_states.Count > 0)
                _statesToRemove.Add(_states.Peek());
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            // Process state changes
            foreach (var state in _statesToRemove)
            {
                state.Exit();
                _states.Pop();
            }
            _statesToRemove.Clear();

            foreach (var state in _statesToAdd)
            {
                state.Enter();
                _states.Push(state);
            }
            _statesToAdd.Clear();

            // Update current state
            if (_states.Count > 0)
                _states.Peek().Update(gameTime, input);
        }

        public void Draw(GameTime gameTime)
        {
            if (_states.Count > 0)
                _states.Peek().Draw(gameTime);
        }
    }
}
