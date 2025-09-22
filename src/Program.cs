// File: src/Program.cs

﻿using Microsoft.Xna.Framework;

namespace Opal
{
    public class Program
    {
        private bool isRunning { get; set; } = false;
        private static readonly object _lock = new object();

        private const string title = "Opal Game Engine";
        private const int width = 800;
        private const int height = 600;

        public static void Main(string[] args)
        {

            using var game = new OpalMono.Opal(title, width, height);
            game.Window.AllowUserResizing = false;
            game.Window.AllowAltF4 = true;
    
            game.Run();
        }
    }
};
