using Microsoft.Maui.Layouts;

namespace NightClub.Views.Components;
public class Spotlight : BoxView
{
	public Spotlight(Color color, double size, double positionX, double positionY)
    {
        this.Background = new RadialGradientBrush()
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop(color, 0),
                new GradientStop(Colors.Transparent, 1)
            }
        };

        this.Shadow = new Shadow()
        {
            Radius = (float)(size / 2),
            Brush = new SolidColorBrush(color)
        };

        this.CornerRadius = size / 2;

        AbsoluteLayout.SetLayoutBounds(this, new Rect(positionX, positionY, size, size));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);
    }
}