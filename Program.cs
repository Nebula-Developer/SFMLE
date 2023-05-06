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

        TextElement rectangle = new TextElement() {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(10, 10),
            Anchor = Anchor.TopCenter,
            Parent = windowElement
        };

        rectangle.Text = new Text("Hello World", new Font("/Library/Fonts/Arial Unicode.ttf"), 16) {

        };

        window.Resized += (sender, e) => {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            windowElement.Size = new Vector2f(e.Width, e.Height);
            rectangle.Size = new Vector2f(e.Width, e.Height);
        };

        window.KeyPressed += (sender, e) => {
            rectangle.Anchor = (Anchor) (((int)rectangle.Anchor + 1) % 9);
        };

        var shader = new Shader(null, null, "fragment.glsl");

        while (window.IsOpen) {
            window.DispatchEvents();

            window.Clear(Color.Black);

            windowElement.Update();
            rectangle.Update();

            // Set the shader uniforms
            shader.SetUniform("time", (float) DateTime.Now.TimeOfDay.TotalSeconds);
            shader.SetUniform("resolution", new Vector2f(window.Size.X * 10, window.Size.Y * 10));

            // Draw the rectangle with the shader
            rectangle.Draw(window, RenderStates.Default);

            window.Display();
        }
    }
}
