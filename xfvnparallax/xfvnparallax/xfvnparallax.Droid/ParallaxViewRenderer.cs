using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using xfvnparallax.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFMiniMobileApplication.SourceCode.Controls.XWellcare;

[assembly: ExportRenderer(typeof(ParallaxView), typeof(ParallaxViewRenderer))]

namespace xfvnparallax.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class ParallaxViewRenderer : ViewRenderer<ParallaxView, Android.Views.View>
    {
        public ParallaxViewRenderer()
        {
            _context = Forms.Context;
            var metrics = _context.Resources.DisplayMetrics;
            _screenSize = new Size(ConvertPixelsToDp(metrics.WidthPixels), ConvertPixelsToDp(metrics.HeightPixels));
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)(pixelValue / Resources.DisplayMetrics.Density);
            return dp;
        }

        private readonly Size _screenSize;
        private readonly Context _context;
        private ImageView _imageView;
        private ExtendedScrollView _scrollView;
        private LinearLayout _contentView;
        private Android.Views.View _rendererView;

        protected override async void OnElementChanged(ElementChangedEventArgs<ParallaxView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                //init our control
                var inflatorservice = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
                _rendererView = inflatorservice.Inflate(Resource.Layout.xfvnparallaxlayout, null);                

                _imageView = _rendererView.FindViewById<ImageView>(Resource.Id.parallax_main_image);
                _scrollView = _rendererView.FindViewById<ExtendedScrollView>(Resource.Id.parallax_scroll_view);
                _contentView = _rendererView.FindViewById<LinearLayout>(Resource.Id.parallax_content_view);

                SetupContent(needClearlayout:false);
                SetNativeControl(_rendererView);

                await SetUpImage();
            }

            if (e.NewElement != null)
            {
                //register handler
                _scrollView.Scrolled += ScrollViewScrolled;
            }

            if (e.OldElement != null)
            {
                //remove handlder
                _scrollView.Scrolled -= ScrollViewScrolled;
            }

            System.Diagnostics.Debug.Write("Init all success!");
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            SetParallax();
            SetImageAlpha();
        }

        private void SetupContent(bool needClearlayout)
        {
            var content = Element.ContentView;

            if (needClearlayout) _contentView.RemoveAllViews();
            if (content == null) return;

            var nativeConverted = FormsToNativeDroid.ConvertFormsToNative(content,
                new Rectangle(0, 0, _screenSize.Width - 170, _screenSize.Height - 170));
            _contentView.AddView(nativeConverted, LayoutParams.MatchParent, LayoutParams.WrapContent);

            _contentView.SetBackgroundColor(Element.BackgroundColor == Xamarin.Forms.Color.Transparent
                ? Xamarin.Forms.Color.Default.ToAndroid(Xamarin.Forms.Color.Default)
                : Element.BackgroundColor.ToAndroid(Xamarin.Forms.Color.Default));            
        }

        private void SetParallax()
        {
            if (Element.ParallaxRate <= 1)
                _imageView.TranslationY = 0;
            else
                _imageView.TranslationY = -(int)(_scrollView.ScrollY / Element.ParallaxRate);
        }

        private void SetImageAlpha()
        {
            if (Element.Fade)
                _imageView.Alpha = ((float)_contentView.MeasuredHeight - _scrollView.ScrollY) / _contentView.MeasuredHeight;
            else
                _imageView.Alpha = 1;
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ParallaxView.ContentViewProperty.PropertyName)
            {
                SetupContent(needClearlayout:true);
                RequestLayout();
            }

            if (e.PropertyName == ParallaxView.FadeProperty.PropertyName)
                SetImageAlpha();

            if (e.PropertyName == ParallaxView.ParallaxRateProperty.PropertyName)
                SetParallax();

            if (e.PropertyName == ParallaxView.ImageSourceProperty.PropertyName)
                await SetUpImage();
        }

        private async Task SetUpImage()
        {
            if (Element?.ImageSource != null)
            {
                var bitmap = await GetImageFromImageSourceAsync(Element.ImageSource);
                _imageView.SetImageBitmap(bitmap);
            }
        }

        private Task<Bitmap> GetImageFromImageSourceAsync(ImageSource imageSource)
        {
            IImageSourceHandler handler = null;

            if (imageSource is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (imageSource is StreamImageSource)
            {
                handler = new StreamImagesourceHandler();
            }
            else if (imageSource is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler();
            }

            return handler?.LoadImageAsync(imageSource, _context);
        }
    }
}