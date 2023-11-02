namespace maui_mapsdemo;

using System;
using CoreGraphics;
using CoreLocation;
using MapKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Maps.Platform;
using UIKit;


public class CustomMKAnnotationView : MKAnnotationView
{
    public string Name { get; set; }

    public CustomMKAnnotationView(IMKAnnotation annotation, string id)
        : base(annotation, id)
    {
    }
}

public static partial class MapExtensions
{
	private static UIView customPinView;
    static CustomMKAnnotationView customView;
    static int i = 0;

    public static async Task AddAnnotation(this CustomPin pin)
	{
		var imageSourceHandler = new ImageLoaderSourceHandler();
		var image = await imageSourceHandler.LoadImageAsync(pin.ImageSource);
		var annotation = new CustomAnnotation()
		{
			Identifier = pin.Id,
			Image = image,
			Title = pin.Label,
			Subtitle = pin.Address,
			Coordinate = new CLLocationCoordinate2D(pin.Location.Latitude, pin.Location.Longitude),
			Pin = pin,
			Name = pin.Name
		};
		pin.MarkerId = annotation;

		var nativeMap = (MauiMKMapView?)pin.Map?.Handler?.PlatformView;
		if (nativeMap is not null)
		{
			var customAnnotations = nativeMap.Annotations.OfType<CustomAnnotation>().Where(x => x.Identifier == annotation.Identifier).ToArray();
            nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
            nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            nativeMap.RemoveAnnotations(customAnnotations);
			nativeMap.GetViewForAnnotation += GetViewForAnnotations;
			nativeMap.AddAnnotation(annotation);
		}
	}


	static void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
	{
        CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;

		if (e.View is CustomMKAnnotationView)
		{
            customPinView = new UIView();
            customPinView.Frame = new CGRect(0, 0, 200, 284);
        customPinView.BackgroundColor = UIColor.Red;
        var image = new UIImageView(new CGRect(0, 0, 200, 84));
        image.Image = UIImage.FromFile("xamarin.png");
        customPinView.AddSubview(image);
        UILabel label = new UILabel();
            label.Frame = new CGRect(0, 70, 200, 70);
            label.BackgroundColor = UIColor.Green;
        label.Text = "Custom View";

            UIButton button = new UIButton();
            button.Frame = new CGRect(0, 160, 200, 70);
            button.BackgroundColor = UIColor.Yellow;
            button.SetTitle("Custom Button", UIControlState.Normal);
            button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            button.UserInteractionEnabled = true;

            button.TouchUpInside += async (sender, ea) =>
            {
                System.Console.WriteLine("Custom button pressed");
            };

            button.TouchUpOutside += async (sender, ea) =>
            {
                System.Console.WriteLine(" TouchUpOutside Custom button pressed");
            };

            customPinView.AddSubview(label);
            customPinView.AddSubview(button);


            customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 125));
            customView.AddSubview(customPinView);
            MapExtensions.customView = customView;

        }
	}

	static void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
	{
		if (!e.View.Selected) { 
			Console.WriteLine($"e.View -> {e.View}");
		Console.WriteLine($"e.View.Selected -> {e.View.Selected}");

		if (e.View is CustomMKAnnotationView)
		{
			if (customPinView != null)
			{
					foreach (UIView view in customView.Subviews)
					{
						view.RemoveFromSuperview();
					}
                    }
		}
	}
	}

	private static void OnCalloutClicked(IMKAnnotation annotation)
	{
		var pin = GetPinForAnnotation(annotation);
		if (customPinView is MKAnnotationView)
			return;
		pin?.SendInfoWindowClick();
	}

	private static MKAnnotationView GetViewForAnnotations(MKMapView mapView, IMKAnnotation annotation)
	{
		MKAnnotationView? annotationView = null;

        if (annotation is CustomAnnotation customAnnotation)
		{
            annotationView = new CustomMKAnnotationView(annotation, customAnnotation.Name);
            //annotationView = mapView.DequeueReusableAnnotation(customAnnotation.Identifier.ToString()) ??
							 //new MKAnnotationView(annotation, customAnnotation.Identifier.ToString());
			annotationView.Image = customAnnotation.Image;
            annotationView.LeftCalloutAccessoryView = new UIImageView(customAnnotation.Image);
            annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
            annotationView.CanShowCallout = false;
            ((CustomMKAnnotationView)annotationView).Name = customAnnotation.Name;
        }

        var result = annotationView ?? new MKAnnotationView(annotation, null);
		//AttachGestureToPin(result, annotation);
		return result;
	}

	static void AttachGestureToPin(MKAnnotationView mapPin, IMKAnnotation annotation)
	{
		var recognizers = mapPin.GestureRecognizers;

		if (recognizers != null)
		{
			foreach (var r in recognizers)
			{
				mapPin.RemoveGestureRecognizer(r);
			}
		}

		var recognizer = new UITapGestureRecognizer(g => OnCalloutClicked(annotation))
		{
			ShouldReceiveTouch = (gestureRecognizer, touch) =>
			{
                customPinView = touch.View;
				return true;
			}
		};

		//mapPin.AddGestureRecognizer(recognizer);
	}

	static IMapPin? GetPinForAnnotation(IMKAnnotation? annotation)
	{
		if (annotation is CustomAnnotation customAnnotation)
		{
			return customAnnotation.Pin;
		}

		return null;
	}
}