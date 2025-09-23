# Opal - A dark RPG Adventure

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![MonoGame](https://img.shields.io/badge/MonoGame-3.8-orange)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Linux%20%7C%20macOS-green)
![License](https://img.shields.io/badge/License-MIT-yellow)

*A haunting journey through a post-punk future metropolis where augmentation blurs the line between humanity and machine.*

## 🌆 Story

In a city without a name, within industrial districts marked by buildings reaching towards the stars, **Eve** moves like a wraith. Her implants beneath artificial flesh vibrate with a frequency that once promised transcendence but now attest only to emptiness.

Navigate through a world where cobblestone streets remember a bygone era, where gothic spires now house server farms, and where the search for wholeness becomes a perpetual journey through a metropolis that beats with a synthetic heart.

## 🎮 Features

### Core Gameplay

- To be implemented
- **Features**:
- - Top Down 2.5D RPG
- - Narrative+Gameplay driven
- - Philosophical and Metaphysical themes
- - Possibly rogue lite **or** expansive narrative and world

## 🛠️ Technologies

- **Engine**: MonoGame Framework 3.8 (DesktopGL)
- **Language**: C# (.NET 8.0)
- **Platform**: Cross-platform (Windows, Linux, macOS)
- **Content Pipeline**: MonoGame Content Builder
- **Font**: Renogare

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MonoGame SDK](https://www.monogame.net/downloads/)
- [Visual Studio Code](https://code.visualstudio.com/) (recommended) or Visual Studio

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/Opal.git
   cd Opal
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the project**

   ```bash
   dotnet build
   ```

4. **Run the game**

   ```bash
   dotnet run
   ```

### Development Setup

For development with VS Code:

1. Install the **C# Dev Kit** extension
2. Install the **MonoGame for VSCode** extension (optional)
3. Open the project folder in VS Code
4. Press `F5` to run with debugging

## 🎯 Controls

| Key | Action |
|-----|--------|
| `WASD` or `Arrow Keys` | Movement |
| `Left Shift` | Sprint (consumes energy) |
| `E` or `F` | Interact with objects |
| `H` | Take damage (debug) |
| `X` | Gain experience (debug) |
| `P` | Print position info (debug mode) |
| `F1` | Toggle debug information |
| `Escape` | Exit game |

### Dialogue Controls

| Key | Action |
|-----|--------|
| `1`, `2`, `3` | Select dialogue choice |
| `Enter` or `Space` | Continue/End dialogue |

## 🏗️ Architecture

### Project Structure

```plaintext
src/
├── Core/               # Core game systems
│   └── GameState.cs    # Game state management
├── Entities/           # Game objects
│   ├── Entity.cs       # Base entity class
│   ├── Player.cs       # Player character
│   └── NPC.cs          # Non-player characters
├── Graphics/           # Rendering systems
│   └── Camera.cs       # Camera management
├── Systems/            # Game systems
│   └── DialogueSystem.cs # Dialogue management
├── World/              # World and level systems
│   └── Map.cs          # Tile-based map system
├── InputHandler.cs     # Input management
├── OpalGame.cs         # Main game class
└── Program.cs          # Entry point
```

### Key Systems

#### Player System

- Health, energy, and experience management
- Augmentation vs humanity balance system
- Smooth movement with collision detection
- Sprint mechanics with energy consumption

#### World System

- Tile-based map with special tile types:
  - `TerminalAccess` - Data terminals for dialogue
  - `AugmentationStation` - Enhancement interfaces
  - `DataPoint` - Experience gathering points

#### Camera System

- Smooth following with configurable offset
- Map boundary constraints
- Transform matrix for world rendering

## 🎨 Assets

### Content Pipeline

- **Fonts**: Renogare.spritefont
- **Graphics**: Procedurally generated textures for prototyping
- **Content**: MonoGame Content Builder pipeline

### Asset Creation

```bash
# Build content
dotnet mgcb-editor Content/Content.mgcb
```

## 🔧 Configuration

### Graphics Settings

- **Resolution**: 800x600 (configurable in Program.cs)
- **Windowed Mode**: Fixed window, no resizing
- **Rendering**: 2D sprite-based with camera transforms

### Game Balance

Modify player stats in `Player.cs`:

```csharp
private float _speed = 200f;              // Base movement speed
private float _sprintMultiplier = 1.5f;   // Sprint speed multiplier
public int MaxHealth { get; set; } = 100; // Starting health
public int MaxEnergy { get; set; } = 50;  // Starting energy
```

## 🐛 Debug Features

Enable debug mode with `F1` to access:

- Player position and camera information
- Health, energy, and experience display
- Augmentation and humanity levels
- Tile interaction debugging
- Performance monitoring

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines

- Follow C# naming conventions
- Add XML documentation for public APIs
- Include unit tests for new features
- Maintain the future gothic aesthetic and narrative tone

## 📋 TODO

- [ ] Add audio system with a fitting soundtrack
- [ ] Implement inventory and item systems
- [ ] Create multiple map areas and transitions
- [ ] Add save/load functionality
- [ ] Implement combat mechanics
- [ ] Create NPC interaction system
- [ ] Add visual effects and particle systems
- [ ] Implement skill trees and augmentation choices

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🎭 Inspiration

*Opal* draws inspiration from classic narratives exploring themes of:

- The erosion of humanity through technological augmentation
- The search for meaning in a digitized world
- Gothic architecture as a metaphor for forgotten spirituality
- The tension between progress and human essence

---

*"She does not recall giving herself the name Eve. It simply arose in the neural static on some half-lit moment when the city lights crept into her optical input in digital tears."*
