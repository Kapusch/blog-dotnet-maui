using Microsoft.Maui.Layouts;

namespace NightClub.Views.Components;
public class Spotlight : BoxView
{
	public Spotlight(Color color, double size, double positionX, double positionY, uint animationLength = 0, double animationDelay = 0)
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

        if (animationLength > 0) this.Animate(animationLength, animationDelay);
    }

    void Animate(uint length, double delay)
    {
        var fadeInAnimation = new Animation(v => this.Opacity = v, start: 0, end: 1, Easing.CubicOut);
        var fadeOutAnimation = new Animation(v => this.Opacity = v, start: 1, end: 0, Easing.CubicOut);

        var spotlightAnimation = new Animation
        {
            { 0, 0.5, fadeInAnimation },
            { 0.5, 1, fadeOutAnimation }
        };
        spotlightAnimation.StartDelay = delay;
        spotlightAnimation.Commit(this, "fadeInAndOut", length: length, repeat: () => true);
    }
}