using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Converters.CompareConverter<object>;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NightClub.Views;
public class MusicPlayerView : ContentPage
{
	public MusicPlayerView()
    {
        Console.WriteLine("[NightClub] MusicPlayerView - Constructor");

        NavigationPage.SetHasNavigationBar(this, false);
        BackgroundColor = Colors.DimGray;

        InitMuteButton();

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                Stars(60),
                Stars(40)),
            RowSpacing = 0,
            Children =
            {
                TopLayout.Row(0),
                BottomLayout.Row(1),
            }
        };
    }

    #region Controls

    #region Main Layouts

    Grid TopLayout => new Grid
    {
        BackgroundColor = Colors.Black
    };

    Grid BottomLayout => new Grid
    {
        BackgroundColor = Colors.DimGray,
        RowDefinitions = Rows.Define(
                Stars(1),
                Stars(1),
                Stars(1)),
        RowSpacing = 0,
        ColumnDefinitions = Columns.Define(
                Stars(10),
                Stars(10),
                Stars(20),
                Stars(20),
                Stars(20),
                Stars(10),
                Stars(10)),
        ColumnSpacing = 0,
        Children =
        {
            ElapsedTime.Row(0).Column(1),
            TimeTracker.Row(0).Column(2).ColumnSpan(3),
            TotalTime.Row(0).Column(5),

            RepeatOnceButton.Row(1).Column(1),
            MediaControlLayout.Row(1).Column(2).ColumnSpan(3),
            DownloadButton.Row(1).Column(5),

            MuteButton.Row(2).Column(1),
            VolumeTracker.Row(2).Column(2).ColumnSpan(3),
        }
    };

    Grid MediaControlLayout => new Grid
    {
        ColumnDefinitions = Columns.Define(
                Stars(30),
                Stars(40),
                Stars(30)),
        ColumnSpacing = 0,
        Children =
        {
            SkipPreviousButton.Column(0),
            PlayButton.Column(1),
            SkipNextButton.Column(2)
        }
    };

    #endregion

    #region Time Tracker

    Label ElapsedTime => new Label
    {
        FontSize = 14,
        Text = "0:36",
        TextColor = Colors.White
    }.TextCenter();

    Slider TimeTracker => new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.LightSalmon,
        Maximum = 100,
        MaximumTrackColor = Colors.Black,
        Value = 20
    };

    Label TotalTime => new Label
    {
        FontSize = 14,
        Text = "2:57",
        TextColor = Colors.White
    }.TextCenter();

    #endregion

    #region Media Control Panel

    ImageButton RepeatOnceButton => new ImageButton
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "repeat_once.png",
        BackgroundColor = Colors.Black
    };

    ImageButton SkipPreviousButton => new ImageButton
    {
        HeightRequest = 75,
        WidthRequest = 75,
        Source = "skip_previous.png"
    };

    ImageButton PlayButton => new ImageButton
    {
        CornerRadius = 50,
        HeightRequest = 100,
        WidthRequest = 100,
        Source = "play.png",
        BackgroundColor = Colors.Black
    };

    ImageButton SkipNextButton => new ImageButton
    {
        HeightRequest = 75,
        WidthRequest = 75,
        Source = "skip_next.png"
    };

    ImageButton DownloadButton => new ImageButton
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "download.png",
        BackgroundColor = Colors.Black
    };

    #endregion

    #region Volume Tracker

    ImageButton MuteButton = new ImageButton
    {
        HeightRequest = 25,
        WidthRequest = 25
    };

    #region Mute Button Visual States

    DataTrigger VolumeOffTrigger => new DataTrigger(typeof(ImageButton))
    {
        Binding = new Binding(nameof(Slider.Value), source: VolumeTracker),
        Value = 0d,
        Setters = {
                new Setter { Property = ImageButton.SourceProperty, Value = "volume_off.png" }
            }
    };

    MultiTrigger VolumeLowTrigger = new MultiTrigger(typeof(ImageButton))
    {
        Setters = {
                new Setter { Property = ImageButton.SourceProperty, Value = "volume_low.png" }
            }
    };

    MultiTrigger VolumeMediumTrigger = new MultiTrigger(typeof(ImageButton))
    {
        Setters = {
                new Setter { Property = ImageButton.SourceProperty, Value = "volume_medium.png" }
            }
    };

    MultiTrigger VolumeHighTrigger = new MultiTrigger(typeof(ImageButton))
    {
        Setters = {
                new Setter { Property = ImageButton.SourceProperty, Value = "volume_high.png" }
            }
    };

    void InitMuteButton()
    {
        BindingCondition CreateRangeCondition(OperatorType comparison, double value) => new BindingCondition
        {
            Binding = new Binding(
                        nameof(Slider.Value),
                        source: VolumeTracker,
                        converter: new CompareConverter
                        {
                            ComparisonOperator = comparison,
                            ComparingValue = value
                        }),
            Value = true
        };

        BindingCondition CreateMinRangeCondition(double value) => CreateRangeCondition(OperatorType.GreaterOrEqual, value);
        BindingCondition CreateMaxRangeCondition(double value) => CreateRangeCondition(OperatorType.SmallerOrEqual, value);

        VolumeLowTrigger.Conditions.Add(CreateMinRangeCondition(1d));
        VolumeLowTrigger.Conditions.Add(CreateMaxRangeCondition(15d));
        VolumeMediumTrigger.Conditions.Add(CreateMinRangeCondition(16d));
        VolumeMediumTrigger.Conditions.Add(CreateMaxRangeCondition(50d));
        VolumeHighTrigger.Conditions.Add(CreateMinRangeCondition(51d));
        VolumeHighTrigger.Conditions.Add(CreateMaxRangeCondition(100d));

        MuteButton.Triggers.Add(VolumeOffTrigger);
        MuteButton.Triggers.Add(VolumeLowTrigger);
        MuteButton.Triggers.Add(VolumeMediumTrigger);
        MuteButton.Triggers.Add(VolumeHighTrigger);
    }

    #endregion

    Slider VolumeTracker = new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.Black,
        Maximum = 100,
        MaximumTrackColor = Colors.Gray,
        Value = 60
    };

    #endregion

    #endregion

    #region Events

    /// <summary>
    /// Event raised when MusicPlayerView appears on screen
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Console.WriteLine("[NightClub] MusicPlayerView - OnAppearing");
    }

    /// <summary>
    /// Event raised when MusicPlayerView disappears from screen
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Console.WriteLine("[NightClub] MusicPlayerView - OnDisappearing");
    }

    #endregion
}
