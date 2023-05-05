using SFML.Graphics;
using SFML.System;

namespace SFMLE;

public class TransformableElement : Anchored, Element {
    public TransformableElement Parent { get; set; }

    public TransformableElement() {
        Position = new Vector2f(0, 0);
        Size = new Vector2f(0, 0);
        Parent = null;
        Anchor = Anchor.TopLeft;
    }
    
    public void Update() {
        CalcWorldPosition(Parent.WorldPosition, Parent.Size);
    }
}

public class RectangleElement : TransformableElement {
    public RectangleShape Shape { get; set; }
    public RectangleElement() {
        Position = new Vector2f(0, 0);
        Size = new Vector2f(0, 0);
        Parent = null;
        Anchor = Anchor.TopLeft;

        Shape = new RectangleShape(Size) {
            FillColor = Color.White
        };
    }
    
    public new void Update() {
        base.Update();
        Shape.Position = WorldPosition;
        Shape.Size = Size;
        Shape.Origin = new Vector2f(0, 0);
    }

    public void Draw(RenderTarget target, RenderStates states) {
        target.Draw(Shape, states);
    }
}