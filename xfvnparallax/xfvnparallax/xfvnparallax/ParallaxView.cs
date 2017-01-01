using Xamarin.Forms;

namespace XFMiniMobileApplication.SourceCode.Controls.XWellcare
{
    public class ParallaxView : View
    {
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ParallaxView));

        public ImageSource ImageSource
        {
            get { return (ImageSource) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly BindableProperty ParallaxRateProperty =
            BindableProperty.Create(nameof(ParallaxRate), typeof(float), typeof(ParallaxView), 2.5f);

        public float ParallaxRate
        {
            get { return (float)GetValue(ParallaxRateProperty); }
            set { SetValue(ParallaxRateProperty, value); }
        }

        public static readonly BindableProperty FadeProperty =
            BindableProperty.Create(nameof(Fade), typeof(bool), typeof(ParallaxView), true);

        public bool Fade
        {
            get { return (bool)GetValue(FadeProperty); }
            set { SetValue(FadeProperty, value); }
        }

        public static readonly BindableProperty ContentViewProperty =
            BindableProperty.Create(nameof(ContentView), typeof(View), typeof(ParallaxView));

        public View ContentView
        {
            get { return (View)GetValue(ContentViewProperty); }
            set { SetValue(ContentViewProperty, value); }
        }
    }
}