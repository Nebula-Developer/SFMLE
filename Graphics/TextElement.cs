using SFML.Graphics;
using SFML.System;

namespace SFMLE;

public class TextElement : TransformableElement {
    public Text Text { get; set; }
    public TextElement() : base() {
        Text = new Text("", new Font("/Library/Fonts/Arial Unicode.ttf"), 16) {
            FillColor = Color.White
        };
    }
    
    public new void Update() {
        base.Update();
        Text.Position = WorldPosition;
        Text.Scale = Size;
        Text.Origin = new Vector2f(0, 0);
    }

    public void Draw(RenderTarget target, RenderStates states) {
        target.Draw(Text, states);
    }
}
