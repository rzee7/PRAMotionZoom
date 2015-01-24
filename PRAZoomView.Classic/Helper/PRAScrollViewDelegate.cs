using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace PRAMotionZoom
{
		public class PRAScrollViewDelegate : UIScrollViewDelegate
		{

		readonly PRAZoomView _view;

		public PRAScrollViewDelegate (PRAZoomView view)
		{
			this._view = view;
		}
			public override UIView ViewForZoomingInScrollView (UIScrollView scrollView)
			{
			return _view._panningImageView;
			}
			public override void ZoomingEnded (UIScrollView scrollView, UIView withView, float atScale)
			{
			scrollView.SetContentOffset (_view.clampedContentOffsetForHorizontalOffset ((float)scrollView.ContentOffset.X), true);
			}
			public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
			{
				if (!willDecelerate) {
				scrollView.SetContentOffset (_view.clampedContentOffsetForHorizontalOffset ((float)scrollView.ContentOffset.X), true);
				}
			}
			public override void DecelerationStarted (UIScrollView scrollView)
			{
			scrollView.SetContentOffset (_view.clampedContentOffsetForHorizontalOffset ((float)scrollView.ContentOffset.X), true);
			}
	}
}

