# PRAZoomView #
PRAZoomView library that enables you to add zoom functionality to zoom an image in your app. Why always that old way to zoom an image `"Tap to Zoom"`. Using this library will be more cool way to zoom an image in different way. *`jakeloo jakealoo jakerloo :)`*

![alt tag](https://raw.githubusercontent.com/ndmeiri/Panorific/master/preview.gif)

## PRAZoomView ##
Just add PRAZoomView library reference into your app and use following code snippet.

		public override void ViewDidLoad ()
        {
            var	zoomScrollView = new PRAZoomView(View.Bounds, UIImage.FromFile("iwatchCUT_2497756b.jpg"));
            zoomScrollView.StartZooming();
            View.AddSubview(zoomScrollView);
        }
Here we go...

Thank you to [Naji Dmeiri](http://najidmeiri.com). Original code ported from [Panorific](https://github.com/ndmeiri/Panorific)
