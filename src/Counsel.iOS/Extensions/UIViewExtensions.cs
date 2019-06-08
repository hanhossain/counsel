using System;
using UIKit;

namespace Counsel.iOS.Extensions
{
    public static class UIViewExtensions
    {
        /// <summary>
        /// Pins the sourceView to the targetView.
        /// </summary>
        /// <param name="sourceView">View to pin.</param>
        /// <param name="targetView">View to pin to.</param>
        public static UIView PinToCenterOf(this UIView sourceView, UIView targetView)
        {
            sourceView.TranslatesAutoresizingMaskIntoConstraints = false;
            sourceView.CenterXAnchor.ConstraintEqualTo(targetView.CenterXAnchor).Active = true;
            sourceView.CenterYAnchor.ConstraintEqualTo(targetView.CenterYAnchor).Active = true;
            return sourceView;
        }

        public static UIView PinSize(this UIView view, nfloat size)
        {
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            view.WidthAnchor.ConstraintEqualTo(view.HeightAnchor).Active = true;
            view.HeightAnchor.ConstraintEqualTo(size).Active = true;
            return view;
        }
    }
}
