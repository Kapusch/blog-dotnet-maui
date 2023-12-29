using Microsoft.Maui.Layouts;

namespace NightClub.Views.Components;
public class Spotlight : BoxView
{
	public Spotlight(Color color, double size, double positionX, double positionY)
	{
        this.Color = color;
        this.CornerRadius = size / 2;

        AbsoluteLayout.SetLayoutBounds(this, new Rect(positionX, positionY, size, size));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
    }
}