using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

public enum Anchor {
    TopLeft, TopCenter, TopRight,
    CenterLeft, Center, CenterRight,
    BottomLeft, BottomCenter, BottomRight
}

public class Anchored {
    public Vector2f Position { get; set; }
    public Vector2f WorldPosition { get; private set; }
    public Vector2f Size { get; set; }
    public Anchor Anchor { get; set; }

    private static readonly Dictionary<Anchor, Vector2f> AnchorPositions = new Dictionary<Anchor, Vector2f> {
        { Anchor.TopLeft, new Vector2f(0, 0) },
        { Anchor.TopCenter, new Vector2f(0.5f, 0) },
        { Anchor.TopRight, new Vector2f(1, 0) },
        { Anchor.CenterLeft, new Vector2f(0, 0.5f) },
        { Anchor.Center, new Vector2f(0.5f, 0.5f) },
        { Anchor.CenterRight, new Vector2f(1, 0.5f) },
        { Anchor.BottomLeft, new Vector2f(0, 1) },
        { Anchor.BottomCenter, new Vector2f(0.5f, 1) },
        { Anchor.BottomRight, new Vector2f(1, 1) },
    };

    public void CalcWorldPosition(Vector2f parentPosition, Vector2f parentSize) {
        Vector2f anchorPositionDict = AnchorPositions[Anchor];
        Vector2f anchorPosition = new Vector2f(parentSize.X * anchorPositionDict.X, parentSize.Y * anchorPositionDict.Y);

        Vector2f position = new Vector2f(anchorPosition.X, anchorPosition.Y);

        bool isXCentered = Anchor == Anchor.Center || Anchor == Anchor.TopCenter || Anchor == Anchor.BottomCenter;
        bool isYCentered = Anchor == Anchor.Center || Anchor == Anchor.CenterLeft || Anchor == Anchor.CenterRight;

        bool isBottom = Anchor == Anchor.BottomLeft || Anchor == Anchor.BottomCenter || Anchor == Anchor.BottomRight;
        bool isRight = Anchor == Anchor.TopRight || Anchor == Anchor.CenterRight || Anchor == Anchor.BottomRight;
        
        if (isXCentered) position.X -= Size.X / 2;
        if (isYCentered) position.Y -= Size.Y / 2;

        if (isBottom) position.Y -= Size.Y;
        if (isRight) position.X -= Size.X;

        position += parentPosition;
        WorldPosition = position;
    }
}

public static class Program {
    public static void Main(String[] args) {
        Anchored testAnchor = new Anchored() {
            Position = new Vector2f(0, 0),
            Size = new Vector2f(50, 50),
            Anchor = Anchor.BottomRight
        };

        Vector2f parentPosition = new Vector2f(0, 0);
        Vector2f parentSize = new Vector2f(200, 200);
        float parentPadding = 0;

        void Calculate() => testAnchor.CalcWorldPosition(parentPosition + new Vector2f(parentPadding, parentPadding), parentSize - new Vector2f(parentPadding * 2, parentPadding * 2));
        Calculate();

        RenderWindow displayWindow = new RenderWindow(new VideoMode(200, 200), "Test");
        displayWindow.Closed += (sender, e) => displayWindow.Close();

        RectangleShape testShape = new RectangleShape(new Vector2f(50, 50)) {
            FillColor = Color.White,
            Position = testAnchor.WorldPosition
        };

        RectangleShape backgroundShape = new RectangleShape(parentSize - new Vector2f(parentPadding * 2, parentPadding * 2)) {
            FillColor = Color.Red,
            Position = parentPosition + new Vector2f(parentPadding, parentPadding)
        };

        void CalculateBackgroundShape() {
            backgroundShape.Position = parentPosition + new Vector2f(parentPadding, parentPadding);
            backgroundShape.Size = parentSize - new Vector2f(parentPadding * 2, parentPadding * 2);
        }

        CalculateBackgroundShape();

        displayWindow.KeyPressed += (sender, e) => {
            if (e.Code == Keyboard.Key.Right) {
                parentPadding += 1;
            } else if (e.Code == Keyboard.Key.Left) {
                parentPadding -= 1;
            } else {
                testAnchor.Anchor = (Anchor)(((int) testAnchor.Anchor + 1) % 9);
            }
            
            Calculate();
            testShape.Position = testAnchor.WorldPosition;
        };

        displayWindow.Resized += (sender, e) => {
            parentSize = new Vector2f(e.Width, e.Height);
            displayWindow.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
            Calculate();
            testShape.Position = testAnchor.WorldPosition;
        };

        while (displayWindow.IsOpen) {
            displayWindow.DispatchEvents();
            displayWindow.Clear(Color.Black);

            CalculateBackgroundShape();
            displayWindow.Draw(backgroundShape);
            displayWindow.Draw(testShape);

            displayWindow.Display();
        }
    }
}