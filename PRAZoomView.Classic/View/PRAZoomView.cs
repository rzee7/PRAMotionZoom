using MonoTouch.UIKit;
using MonoTouch.CoreMotion;
using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using System;
using System.Drawing;
using MonoTouch.CoreGraphics;


namespace PRAMotionZoom
{
	/// <summary>
	/// PRA zoom view.
	/// </summary>
	public sealed class PRAZoomView : UIScrollView
	{
		#region private Declaration

		internal UIImageView _panningImageView;
		internal PRAZoomScrollBarView _scrollBarView;

		//Motion Manager
		CMMotionManager _motionManager;
		bool motionBasedPanEnabled=true;

		//Display link
		internal CADisplayLink _displayLink;

		//Const
		float MovementSmoothing = 0.3f;
		float AnimationDuration = 0.3f;
		float RotationMultiplier = 5.0f;


		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="PRAMotionZoom.PRAZoomView"/> class.
		/// </summary>
		/// <param name="rect">Rect. view frame</param>
		/// <param name="imageToZoom">Image to zoom.</param>
		public PRAZoomView (RectangleF rect, UIImage imageToZoom):base(rect)
		{
			_motionManager = _motionManager ?? new CMMotionManager ();
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			BackgroundColor = UIColor.Black;
			Delegate = new PRAScrollViewDelegate (this);
			ScrollEnabled = false;
			AlwaysBounceVertical = false;
			MaximumZoomScale = 2.0f;
			PinchGestureRecognizer.AddTarget (this, new MonoTouch.ObjCRuntime.Selector ("pinchGestureRecognized"));

			//2
			_panningImageView =new UIImageView (rect);
			_panningImageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			_panningImageView.BackgroundColor = UIColor.Black;
			_panningImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			AddSubview (_panningImageView);

			//bar view
			_scrollBarView = new PRAZoomScrollBarView (rect, new UIEdgeInsets (0, 10, 50, 10));
			_scrollBarView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			_scrollBarView.UserInteractionEnabled = false;
			//AddSubview (_scrollBarView);

			//3
			_displayLink = UIScreen.MainScreen.CreateDisplayLink (DisplayLinkUpdate);//new CADisplayLink ();
			_displayLink.AddToRunLoop (NSRunLoop.Main, NSRunLoopMode.Common);

			var tapGestureRecognizer = new UITapGestureRecognizer (ToggleMotionBasedPan);//this.View, new ObjCRuntime.Selector ("toggleMotionBasedPan:"));
			AddGestureRecognizer (tapGestureRecognizer);

			ConfigureWithImage (imageToZoom);
		}

		internal void ConfigureWithImage(UIImage image) 
		{
			_panningImageView.Image = image;
			UpdateScrollViewZoomToMaximumForImage (image);
		}
		void calculateRotationBasedOnDeviceMotionRotationRate(CMDeviceMotion motion) {
			if (motionBasedPanEnabled) {
				float xRotationRate = (float)motion.RotationRate.x;
				float yRotationRate = (float)motion.RotationRate.y;
				float zRotationRate = (float)motion.RotationRate.z;

				if (Math.Abs (yRotationRate) > (Math.Abs (xRotationRate) + Math.Abs (zRotationRate))) {
					var invertedYRotationRate = yRotationRate * (-1.0f);

					float zoomScale = MaximumZoomScaleForImage (_panningImageView.Image);
					float interpretedXOffset = (float)(ContentOffset.X + invertedYRotationRate * zoomScale * RotationMultiplier);

					var contentOffset = clampedContentOffsetForHorizontalOffset (interpretedXOffset);

					UIView.Animate (MovementSmoothing, 0.0, UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.AllowUserInteraction | UIViewAnimationOptions.CurveEaseOut,
						() => SetContentOffset (contentOffset, false),
						null);
				}
			}
		}
		private void ToggleMotionBasedPan() {
			bool motionBasedPanWasEnabled = motionBasedPanEnabled;

			if (motionBasedPanWasEnabled) {
				motionBasedPanEnabled = false;
			}

			UIView.Animate (AnimationDuration, () =>
				UpdateViewsForMotionBasedPanEnabled (!motionBasedPanWasEnabled)
				, () => {
					motionBasedPanEnabled |= !motionBasedPanWasEnabled;
				});
		}

		/// <summary>
		/// Starts the zooming.
		/// </summary>
		public void StartZooming()
		{
			ContentOffset = new PointF ((ContentSize.Width / 2) - (Bounds.Width / 2), (ContentSize.Height / 2) - (Bounds.Height / 2));
			_motionManager.StartDeviceMotionUpdates (NSOperationQueue.MainQueue, (motion, error) => calculateRotationBasedOnDeviceMotionRotationRate (motion));
		}

		internal void DisplayLinkUpdate() {
			var panningImageViewPresentationLayer = _panningImageView.Layer.PresentationLayer ;
			var panningScrollViewPresentationLayer = Layer.PresentationLayer;

			var horizontalContentOffset = panningScrollViewPresentationLayer.Bounds.GetMaxX ();

			var contentWidth = panningImageViewPresentationLayer.Frame.Width;
			var visibleWidth = Bounds.Width;

			float	clampedXOffsetAsPercentage = (float)Math.Max (0, Math.Min (1, horizontalContentOffset / (contentWidth - visibleWidth)));

			float scrollBarWidthPercentage = (float)(visibleWidth / contentWidth);
			float scrollableAreaPercentage = 1 - scrollBarWidthPercentage;

			_scrollBarView.UpdateWithScrollAmount (clampedXOffsetAsPercentage, scrollableAreaPercentage, scrollableAreaPercentage);

		}

		//new
		void UpdateViewsForMotionBasedPanEnabled(bool motionBasedPanEnabled) {
			if (motionBasedPanEnabled) {
				UpdateScrollViewZoomToMaximumForImage (_panningImageView.Image);
				ScrollEnabled = false;
			} else {
				ZoomScale = 1.0f;
				ScrollEnabled = true;
			}
		}
		//New 

		//new
		float MaximumZoomScaleForImage(UIImage image) {
			return (float)((Bounds.Height / Bounds.Width) * (image.Size.Width / image.Size.Height));
		}

		//New
		void UpdateScrollViewZoomToMaximumForImage(UIImage image) {
			var zoomScale = MaximumZoomScaleForImage (image);

			MaximumZoomScale = zoomScale;
			ZoomScale = zoomScale;
		}

		//new
		internal PointF clampedContentOffsetForHorizontalOffset(float horizontalOffset) {
			float maximumXOffset = (float)(ContentSize.Width - Bounds.Width);
			float minimumXOffset = 0.0f;

			float	clampedXOffset = Math.Max (minimumXOffset, Math.Min (horizontalOffset, maximumXOffset));
			float centeredY = (float)((ContentSize.Height / 2.0) - (Bounds.Height / 2.0));

			return new PointF (clampedXOffset, centeredY);
		}

		//new
		[Export("pinchGestureRecognized")]
		void pinchGestureRecognized() {
			motionBasedPanEnabled = false;
			ScrollEnabled = true;
		}
	}
}

