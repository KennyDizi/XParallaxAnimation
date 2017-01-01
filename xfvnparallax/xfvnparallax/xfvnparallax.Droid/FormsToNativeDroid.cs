using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace xfvnparallax.Droid
{
    public static class FormsToNativeDroid
    {
        public static ViewGroup ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
			if (Platform.GetRenderer(view) == null)
				Platform.SetRenderer(view, Platform.CreateRenderer(view));
			var vRenderer = Platform.GetRenderer(view);

            
            var viewGroup = vRenderer.ViewGroup;
            vRenderer.Tracker.UpdateLayout ();
            var layoutParams = new ViewGroup.LayoutParams ((int)size.Width, (int)size.Height);
            viewGroup.LayoutParameters = layoutParams;
            view.Layout (size);
            viewGroup.Layout (0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup; 
        }
    }
}

