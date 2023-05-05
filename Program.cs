using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLE;

public static class Program {
    public static void Main(String[] args) {
        RenderWindow window = new RenderWindow(new VideoMode(800, 600), "SFML Window");

        window.Resized += (sender, e) => {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            window.Display();
        };

        window.Closed += (sender, e) => window.Close();

        Anchored anchored = new Anchored {
            Position = new Vector2f(0, 0), Size = new Vector2f(100, 100), Anchor = Anchor.BottomRight
        };

        RectangleShape shape = new RectangleShape(anchored.Size) { FillColor = Color.Red };

        while (window.IsOpen) {
            window.DispatchEvents();

            anchored.CalcWorldPosition(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y));
            shape.Position = anchored.WorldPosition;

            window.Clear();
            window.Draw(shape);
            window.Display();
        }
    }
}
