using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using NightClub.Models;
using NightClub.ViewModels;
using static CommunityToolkit.Maui.Converters.CompareConverter<object>;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace NightClub.Views;
public class MusicPlayerView : ContentPage
{
    #region Properties

    bool mustResumePlayback;
    double savedVolumeBeforeGoingMute;

    #endregion

    public MusicPlayerView()
    {
        Console.WriteLine("[NightClub] MusicPlayerView - Constructor");

        BindingContext = new MusicPlayerViewModel();

        NavigationPage.SetHasNavigationBar(this, false);
        BackgroundColor = Colors.DimGray;

        InitMusicPlayer();
        InitTimeTracker();
        InitMediaControlPanel();
        InitMuteButton();
        InitVolumeTracker();

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                Stars(60),
                Stars(40)),
            RowSpacing = 0,
            Children =
            {
                MusicPlayer,
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
                Stars(20),
                Stars(20),
                Stars(20),
                Stars(20),
                Stars(20)),
        ColumnSpacing = 0,
        Children =
        {
            ElapsedTime.Row(0).Column(0).End(),
            TimeTracker.Row(0).Column(1).ColumnSpan(3),
            TotalTime.Row(0).Column(4).Start(),

            RepeatOnceButton.Row(1).Column(0).End(),
            MediaControlLayout.Row(1).Column(1).ColumnSpan(3),
            DownloadButton.Row(1).Column(4).Start(),

            MuteButton.Row(2).Column(0).End(),
            VolumeTracker.Row(2).Column(1).ColumnSpan(3),
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

    #region MusicPlayer

    MediaElement MusicPlayer = new MediaElement();

    void InitMusicPlayer()
    {
        MusicPlayer.ShouldAutoPlay = true;

        MusicPlayer.Bind(
            MediaElement.SourceProperty,
            nameof(MusicPlayerViewModel.CurrentTrack),
            convert: (MusicTrack musicTrack) => MediaSource.FromUri(musicTrack.AudioURL)
            );
    }

    #endregion

    #region Time Tracker

    Label ElapsedTime = new Label
    {
        FontSize = 14,
        TextColor = Colors.White
    } .TextCenter();

    Slider TimeTracker = new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.LightSalmon,
        MaximumTrackColor = Colors.Black
    };

    Label TotalTime = new Label
    {
        FontSize = 14,
        TextColor = Colors.White
    } .TextCenter();

    void InitTimeTracker()
    {
        TimeTracker.Bind(
            Slider.ValueProperty,
            nameof(MusicPlayer.Position),
            source: MusicPlayer,
            convert: (TimeSpan currentPosition) => currentPosition.TotalSeconds);

        TimeTracker.Bind(
            Slider.MaximumProperty,
            nameof(MusicPlayer.Duration),
            source: MusicPlayer,
            convert: (TimeSpan duration) => duration.TotalSeconds);

        ElapsedTime.Bind(
            Label.TextProperty,
            nameof(MusicPlayer.Position),
            source: MusicPlayer,
            stringFormat: "{0:mm\\:ss}");

        TotalTime.Bind(
            Label.TextProperty,
            nameof(MusicPlayer.Duration),
            source: MusicPlayer,
            stringFormat: "{0:mm\\:ss}");

        TimeTracker.DragStarted += TimeTracker_DragStarted;
        TimeTracker.DragCompleted += TimeTracker_DragCompleted;
    }

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

    ImageButton PlayButton = new ImageButton
    {
        CornerRadius = 50,
        HeightRequest = 100,
        WidthRequest = 100,
        Source = "play.png",
        BackgroundColor = Colors.Black
    };

    void InitMediaControlPanel()
    {
        PlayButton.Bind(
            targetProperty: ImageButton.SourceProperty,
            source: MusicPlayer,
            path: nameof(MusicPlayer.CurrentState),
            convert: (MediaElementState currentState)
                => currentState != MediaElementState.Playing ? "play.png" : "pause.png");

        PlayButton.Clicked += PlayButton_Clicked;
    }

    ImageButton SkipNextButton => new ImageButton
    {
        HeightRequest = 75,
        WidthRequest = 75,
        Source = "skip_next.png"
    };

    ImageButton DownloadButton = new ImageButton
    {
        CornerRadius = 5,
        HeightRequest = 25,
        WidthRequest = 25,
        Source = "download.png",
        BackgroundColor = Colors.Black
    } .BindCommand("DownloadCurrentTrackCommand");

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

        MuteButton.Clicked += MuteButton_Clicked;
    }

    #endregion

    Slider VolumeTracker = new Slider
    {
        Minimum = 0,
        MinimumTrackColor = Colors.Black,
        Maximum = 100,
        MaximumTrackColor = Colors.Gray
    };

    void InitVolumeTracker()
    {
        VolumeTracker.DragCompleted += VolumeTracker_DragCompleted;

        VolumeTracker.Bind(
            Slider.ValueProperty,
            nameof(MusicPlayer.Volume),
            source: MusicPlayer,
            convert: (double mediaElementVolume) => mediaElementVolume * 100,
            convertBack: (double sliderValue) => sliderValue / 100);
    }

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

    void PlayButton_Clicked(object sender, EventArgs e)
    {
        if (MusicPlayer.CurrentState != MediaElementState.Playing)
        {
            MusicPlayer.Play();
        }
        else
        {
            MusicPlayer.Pause();
        }
    }
    void TimeTracker_DragStarted(object sender, EventArgs e)
    {
        if (MusicPlayer.CurrentState == MediaElementState.Playing)
        {
            mustResumePlayback = true;
            MusicPlayer.Pause();
        }
    }

    void TimeTracker_DragCompleted(object sender, EventArgs e)
    {
        // WARN: always update the control that is source of the event
        // through the sender object to not introduce any conflict updates
        if (sender is Slider timeTrackerControl)
        {
            // Update the position based on where the User finger ended
            MusicPlayer.SeekTo(TimeSpan.FromSeconds(timeTrackerControl.Value));

            if (mustResumePlayback)
            {
                // Resume playback if it was playing prior dragging the cursor
                MusicPlayer.Play();
                mustResumePlayback = false;
            }
        }
    }

    void MuteButton_Clicked(object sender, EventArgs e)
    {
        if (!MusicPlayer.ShouldMute)
        {
            // We must save the current volume before we mute...
            savedVolumeBeforeGoingMute = MusicPlayer.Volume;
            MusicPlayer.Volume = 0;
        }
        else
        {
            // ... and we set it back when we unmute!
            MusicPlayer.Volume = savedVolumeBeforeGoingMute;
        }

        MusicPlayer.ShouldMute = !MusicPlayer.ShouldMute;
    }

    void VolumeTracker_DragCompleted(object sender, EventArgs e)
    {
        // WARN: always update the control that is source of the event
        // through the sender object to not introduce any conflict updates
        if (sender is Slider volumeTrackerControl)
        {
            if (volumeTrackerControl.Value == 0)
            {
                // To improve the user experience, we must always turn
                // back the volume to a positive level when he unmutes
                savedVolumeBeforeGoingMute = 0.2;
                MusicPlayer.ShouldMute = true;
            }
            else if (MusicPlayer.ShouldMute)
            {
                // User can unmute after having moved the cursor
                // to a positive value
                MusicPlayer.ShouldMute = false;
            }
        }
    }

    #endregion
}
