using System;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Drawing;

namespace PRAMotionZoom
{
	public class PRAZoomScrollBarView : UIView
	{
		public CAShapeLayer scrollBarLayer=new CAShapeLayer();

		public PRAZoomScrollBarView (RectangleF rect, UIEdgeInsets edgeInsets):base(rect)
		{
			var scrollBarPath = new UIBezierPath ();
			scrollBarPath.MoveTo (new PointF (edgeInsets.Left, Bounds.Height - edgeInsets.Bottom));
			scrollBarPath.AddLineTo (new PointF (Bounds.Width - edgeInsets.Right, Bounds.Height - edgeInsets.Bottom));

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

