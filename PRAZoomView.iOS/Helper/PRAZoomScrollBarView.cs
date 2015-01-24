using System;
using UIKit;
using CoreAnimation;
using System.Drawing;
using CoreGraphics;
using CoreMotion;
using Foundation;

namespace PRAMotionZoom
{
	public class PRAZoomScrollBarView : UIView
	{
		public CAShapeLayer scrollBarLayer=new CAShapeLayer();

		public PRAZoomScrollBarView (CGRect rect, UIEdgeInsets edgeInsets):base(rect)
		{
			var scrollBarPath = new UIBezierPath ();
			scrollBarPath.MoveTo (new CGPoint (edgeInsets.Left, Bounds.Height - edgeInsets.Bottom));
			scrollBarPath.AddLineTo (new CGPoint (Bounds.Width - edgeInsets.Right, Bounds.Height - edgeInsets.Bottom));

			var scrollBarBackgroundLayer = new CAShapeLayer ();
			scrollBarBackgroundLayer.Path = scrollBarPath.CGPath;
			scrollBarBackgroundLayer.LineWidth = 1.0f;
			scrollBarBackgroundLayer.StrokeColor = UIColor.White.ColorWithAlpha (0.1f).CGColor;
			scrollBarBackgroundLayer.FillColor = UIColor.Clear.CGColor;
			Layer.AddSublayer (scrollBarBackgroundLayer);

			scrollBarLayer.Path = scrollBarPath.CGPath;
			scrollBarLayer.LineWidth = 1.0f;
			scrollBarLayer.StrokeColor = UIColor.White.CGColor;
			scrollBarLayer.FillColor = UIColor.Clear.CGColor;
			//scrollBarLayer.Actions= //["strokeStart": NSNull(), "strokeEnd": NSNull()]
			scrollBarLayer.ActionForKey ("strokeStart");
			scrollBarLayer.ActionForKey ("strokeEnd");
			Layer.AddSublayer (scrollBarLayer);

		}
		public void UpdateWithScrollAmount(float scrollAmount, float scrollableWidth, float scrollableArea)
		{
			scrollBarLayer.StrokeStart = scrollAmount * scrollableArea;
			scrollBarLayer.StrokeEnd = scrollAmount * scrollableArea + scrollableWidth;
		}
	}
}

