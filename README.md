# Blade.MonoGame.Core

MonoGame Helper Library used by the BladeSoftware.MonoGame.* packages.

[![NuGet](https://img.shields.io/nuget/v/BladeSoftware.MonoGame.Core.svg)](https://www.nuget.org/packages/BladeSoftware.MonoGame.Core/)
[![License: MPL 2.0](https://img.shields.io/badge/License-MPL%202.0-brightgreen.svg)](https://opensource.org/licenses/MPL-2.0)

Blade.MG.Core is a collection of helper classes, extension methods and utilities for [MonoGame](https://www.monogame.net/) projects. It provides commonly needed building blocks - 2D primitive drawing, SVG-style vector graphics, colour management, input handling, splines, sequencing/tweening and math extensions - so you can spend more time building your game and less time re-writing boilerplate.

## Features

- **2D Primitives** (`Blade.MG.Primitives.Primitives2D`) - Draw and fill Lines, Rectangles, Rounded Rectangles, Circles, Ellipses, Arcs, Triangles, Quads and Pixels directly onto a `SpriteBatch`.
- **SVG-style Vector Graphics** (`Blade.MG.SVG`) - Build vector shapes (Path, Line, Circle, Ellipse, Rectangle, Polygon, Polyline) in code, including a parser for SVG path (`d` attribute) markup syntax, and render/cache them to a `RenderTarget2D`.
- **Colour Management** (`Blade.MG.ColorManagement` / `ColorHelper`) - Convert Hex (`#RRGGBB` / `#RRGGBBAA`) and JSON style (`{R:0 G:128 B:0 A:255}`) strings to `Color`, plus a full `HSBColor` (Hue/Saturation/Brightness) implementation with conversion to/from `Color`.
- **Input Handling** (`Blade.MG.Input`) - Unified `InputManager` for Keyboard, Mouse, Touch and up to 4 GamePads, with pressed/released/held state tracking and localized keyboard mappings (US, French).
- **MonoGame Extensions** (`Vector2Ex`, `Vector3Ex`, `MatrixEx`, `RayEx`, `GameExtensions`) - Extension methods that fill in the gaps of the base MonoGame math and `Game` types (e.g. `Matrix.GetPosition()`, `Matrix.GetScale()`, `Vector2.Cross()`, `Ray.Point(t)`, `Game.Viewport()`).
- **Splines** - Bézier, B-Spline, Catmull-Rom, Hermite and Kochanek-Bartels spline/patch interpolation.
- **Sequencing** (`Blade.MG.Sequencing`) - Chainable, time based action sequences (e.g. `DelaySequence`) driven from your game's `Update` loop.
- **Geometry Helpers** - `RectangleF`, `Quad2D` and `Edge2D` for floating point bounds, quads and scan-line edges.
- **GameEntity** - A lightweight abstract base class for game objects with `Initialize`, `LoadContent`, `Update`, `Draw` and tagging support.

## Installation

Install the [BladeSoftware.MonoGame.Core](https://www.nuget.org/packages/BladeSoftware.MonoGame.Core/) NuGet package:

```powershell
dotnet add package BladeSoftware.MonoGame.Core
```

or via the Package Manager Console:

```powershell
Install-Package BladeSoftware.MonoGame.Core
```

## Requirements

- .NET 10
- [MonoGame.Framework.DesktopGL](https://www.nuget.org/packages/MonoGame.Framework.DesktopGL/) 3.8.4.1 (or compatible)

## Getting Started

### Drawing 2D Primitives

```csharp
using Blade.MG.Primitives;

protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    spriteBatch.Begin();

    Primitives2D.DrawLine(spriteBatch, new Vector2(10, 10), new Vector2(200, 10), Color.White, 2f);
    Primitives2D.DrawRect(spriteBatch, new Rectangle(10, 30, 100, 60), Color.Red, 2f);
    Primitives2D.FillCircle(spriteBatch, new Vector2(150, 120), 40, Color.Yellow);

    spriteBatch.End();

    base.Draw(gameTime);
}
```

### Colour Conversion

```csharp
using Microsoft.Xna.Framework;

Color red = ColorHelper.FromString("#FF0000");
Color translucentGreen = ColorHelper.FromString("{R:0 G:128 B:0 A:128}");
```

### Input Handling

```csharp
using Blade.MG.Input;

protected override void Update(GameTime gameTime)
{
    InputManager.Update();

    if (InputManager.Keyboard.KeyPressed(Keys.Space))
    {
        Jump();
    }

    if (InputManager.GamePad(PlayerIndex.One).IsConnected)
    {
        var move = InputManager.GamePad(PlayerIndex.One).ThumbStickLeft;
    }

    base.Update(gameTime);
}
```

### Sequencing

```csharp
using Blade.MG.Sequencing;

var sequence = new DelaySequence(TimeSpan.FromSeconds(1), _ => Console.WriteLine("First"));
sequence.Then(new DelaySequence(TimeSpan.FromSeconds(2), _ => Console.WriteLine("Second")));

SequenceManager.Add(sequence);

// In your game's Update loop:
SequenceManager.Update(gameTime);
```

### SVG-style Vector Graphics

```csharp
using Blade.MG.SVG;

var svg = new SVGDocument(stroke: Color.Black, strokeWidth: 2f, fill: new SVGFill(Color.CornflowerBlue));
svg.AddPath("M10 10 H 90 V 90 H 10 Z");

svg.Draw(spriteBatch, Matrix.CreateTranslation(100, 100, 0));
```

### GameEntity

```csharp
using Blade.MG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Player : GameEntity
{
    public override void LoadContent()
    {
        // Load textures, etc.
    }

    public override void Update(GameTime gameTime)
    {
        // Update logic
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime, RenderTarget2D renderTarget)
    {
        // Draw logic
    }
}
```

## Project Structure

| Namespace | Description |
|---|---|
| `Blade.MG` | Core types: `GameEntity`, `RectangleF`, `Quad2D`, `Edge2D`, `Splines` |
| `Blade.MG.ColorManagement` | `HSBColor` and colour helpers |
| `Blade.MG.Input` | Keyboard, Mouse, Touch, GamePad and the `InputManager` |
| `Blade.MG.Input.Keyboards` | Localized keyboard mappings (US, French) |
| `Blade.MG.Primitives` | `Primitives2D` drawing helpers |
| `Blade.MG.Sequencing` | Time based action sequencing/tweening |
| `Blade.MG.SVG` | SVG-style vector graphics geometry and rendering |
| `Microsoft.Xna.Framework` (extensions) | `Vector2Ex`, `Vector3Ex`, `MatrixEx`, `RayEx`, `GameExtensions`, `ColorHelper` |

## Contributing

Issues and pull requests are welcome at the [GitHub repository](https://github.com/BladeSoftware/Blade.MonoGame.Core).

## License

This project is licensed under the [Mozilla Public License 2.0](LICENSE).
