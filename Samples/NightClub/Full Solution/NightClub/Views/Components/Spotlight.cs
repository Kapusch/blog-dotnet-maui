using Microsoft.Maui.Layouts;

namespace NightClub.Views.Components;
public class Spotlight : BoxView
{
    const string AnimationName = "fadeInAndOut";
    public uint AnimationLength { get; set; }
    public Animation SpotlightAnimation { get; set; }

    public Spotlight(Color color, double size, double positionX, double positionY, uint animationLength = 0)
    {
        Opacity = 0;
        Background = new RadialGradientBrush()
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop(color, 0),
                new GradientStop(Colors.Transparent, 1)
            }
        };

        Shadow = new Shadow()
        {
            Radius = (float)(size / 2),
            Brush = new SolidColorBrush(color)
        };

        CornerRadius = size / 2;

        AbsoluteLayout.SetLayoutBounds(this, new Rect(positionX, positionY, size, size));
        AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.PositionProportional);

        this.AnimationLength = animationLength;
        SetAnimation();
    }

    void SetAnimation()
    {
        if (AnimationLength <= 0) return;

        var fadeInAnimation = new Animation(v => Opacity = v, start: 0, end: 1, Easing.CubicOut);
        var fadeOutAnimation = new Animation(v => Opacity = v, start: 1, end: 0, Easing.CubicOut);

        SpotlightAnimation = new Animation
        {
            { 0, 0.5, fadeInAnimation },
            { 0.5, 1, fadeOutAnimation }
        };
    }

    public void StartAnimation()
    {
        if (AnimationLength <= 0) return;

        SpotlightAnimation.Commit(this, AnimationName, length: AnimationLength, repeat: () => true);
    }

    public void StopAnimation()
    {
        this.AbortAnimation(AnimationName);
        this.Opacity = 0;
    }
}