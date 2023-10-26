namespace maui_mapsdemo;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;


public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

        //Location location = new Location(36.9628066, -122.0194722);
        //MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
        //myMapView.MoveToRegion(mapSpan);
        //SetupPins();
        SetupCustomPins();
    }

    void SetupCustomPins() {
        var img2 = ImageSource.FromResource("maui_mapsdemo.Resources.Images.pin.png");
        var img3 = ImageSource.FromResource("maui_mapsdemo.Resources.Images.destination.png");
        var img4 = ImageSource.FromResource("maui_mapsdemo.Resources.Images.de.png");
        var img5 = ImageSource.FromResource("maui_mapsdemo.Resources.Images.act.png");
        var imgUrl = ImageSource.FromUri(new Uri("https://picsum.photos/50"));

        var customPinFromUri = new CustomPin()
        {
            Label = "From Uri",
            Location = new Location(36.9641949, -122.0177232),
            Address = "Address",
            ImageSource = img2,
            Map = myMapView,
            Name = "1"
        };
      

        var customPinFromResource = new CustomPin()
        {
            Label = "From Resource",
            Location = new Location(36.961511651812785, -122.02185380422233),
            Address = "Address3",
            ImageSource = img3,
            Map = myMapView,
            Name = "2"
        };

        var customPinFromResource2 = new CustomPin()
        {
            Label = "From Resource 2",
            Location = new Location(36.9571571, -122.0173544),
            Address = "Address4",
            ImageSource = img4,
            Map = myMapView,
            Name = "3"
        };

        var customPinFromResource3 = new CustomPin()
        {
            Label = "From Resource 3",
            Location = new Location(36.96630236862692, -122.02103245740875),
            Address = "Address5",
            ImageSource = img5,
            Map = myMapView,
            Name = "4"
        };

        //36.96630236862692, -122.02103245740875

        myMapView.Pins.Add(customPinFromUri);
        myMapView.Pins.Add(customPinFromResource);
        myMapView.Pins.Add(customPinFromResource2);
        myMapView.Pins.Add(customPinFromResource3);


        customPinFromUri.InfoWindowClicked += async delegate
        {
            //await Toast.Make("Info Window is clicked").Show();
        };
        customPinFromUri.MarkerClicked += async delegate
        {
            //await Toast.Make("Marker is clicked").Show();
        };
        //myMapView.MoveToRegion(new MapSpan(new Location(10, 10), 10, 10));

        Location position = new Location(36.9641949, -122.0177232);
        MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
        myMapView.MoveToRegion(mapSpan);
    }


    void SetupPins()
    {
        Pin boardwalkPin = new Pin
        {
            Location = new Location(36.9641949, -122.0177232),
            Label = "Boardwalk",
            Address = "Santa Cruz",
            Type = PinType.Place
        };
        myMapView.Pins.Add(boardwalkPin);

        Location position = new Location(36.9641949, -122.0177232);
        MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
        myMapView.MoveToRegion(mapSpan);

        boardwalkPin.MarkerClicked += async (s, args) =>
        {
            //args.HideInfoWindow = true;
            string pinName = ((Pin)s).Label;
            await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
        };

        Pin wharfPin = new Pin
        {
            Location = new Location(36.9571571, -122.0173544),
            Label = "Wharf",
            Address = "Santa Cruz",
            Type = PinType.Place
        };
        myMapView.Pins.Add(wharfPin);

        wharfPin.InfoWindowClicked += async (s, args) =>
        {
            string pinName = ((Pin)s).Label;
            await DisplayAlert("Info Window Clicked", $"The info window was clicked for {pinName}.", "Ok");
        };
    }


    void myMapView_MapClicked(System.Object sender, Microsoft.Maui.Controls.Maps.MapClickedEventArgs e)
    {
        Console.WriteLine($"Map clicked position lat:-{e.Location.Latitude} long:-{e.Location.Longitude}");
    }

    void showDirectionsBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        String show = "Show Directions";
        String hide = "Hide Directions";
        if (showDirectionsBtn.Text == show)
        {
            setupPolylines();
            showDirectionsBtn.Text = hide;
        }
        else
        {
            showDirectionsBtn.Text = show;
            myMapView.MapElements.Clear();
        }
    }

    void setupPolylines()
    {

        Polyline polyline = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5,
            Geopath =
    {
        new Location(36.9641949, -122.0177232),
        new Location(36.961511651812785, -122.02185380422233),
        new Location(36.9571571, -122.0173544),
        new Location(36.96630236862692, -122.02103245740875),
            }
        };
        myMapView.MapElements.Clear();
        myMapView.MapElements.Add(polyline);
    }
}


