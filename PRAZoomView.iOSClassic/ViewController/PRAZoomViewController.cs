
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using PRAMotionZoom;

namespace PRAZoom.iOSClassic
{
    public partial class PRAZoomViewController : UIViewController
    {
        public PRAZoomViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            var zoomScrollView = new PRAZoomView(View.Bounds, UIImage.FromFile("iwatchCUT_2497756b.jpg"));
            zoomScrollView.StartZooming();
            View.AddSubview(zoomScrollView);
        }
    }
}

