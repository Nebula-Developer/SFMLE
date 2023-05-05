using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLE;

public static class Program {
    public static void Main(String[] args) {
        RenderWindow window = new RenderWindow(new VideoMode(300, 300), "SFML.NET");

        TransformableElement windowElement = new TransformableElement() {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(300, 300),
            Anchor = Anchor.TopLeft
        };

        RectangleElement rectangle = new RectangleElement() {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(100, 100),
            Anchor = Anchor.TopCenter,
            Parent = windowElement
        };

        window.Resized += (sender, e) => {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            windowElement.Size = new Vector2f(e.Width, e.Height);
        };

        window.KeyPressed += (sender, e) => {
            rectangle.Anchor = (Anchor) (((int)rectangle.Anchor + 1) % 9);
        };

        while (window.IsOpen) {
            window.DispatchEvents();

            window.Clear(Color.Black);

            rectangle.Update();
            rectangle.Draw(window, RenderStates.Default);

            window.Display();
        }


    }
}
