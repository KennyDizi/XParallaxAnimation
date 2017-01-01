using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace xfvnparallax.Droid
{
    public class ExtendedScrollView : ScrollView
    {
        protected ExtendedScrollView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public ExtendedScrollView(Context context)
            : base(context)
        {
        }

        public ExtendedScrollView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public ExtendedScrollView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public event EventHandler<ScrolledEventArgs> Scrolled;

        //This method attribute allows us to register the inbuilt OnScrollChanged event that fires when scrolling a ScrollView
        [Register("onScrollChanged", "(IIII)V", "GetOnScrollChanged_IIIIHandler")]
        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            base.OnScrollChanged(l, t, oldl, oldt);

            Scrolled?.Invoke(this, new ScrolledEventArgs
            {
                X = l,
                Y = t,
                OldX = oldl,
                OldY = oldt
            });
        }
    }

    /*//Set an event listener
    public class ScrollViewChangedListener
    {
        private readonly FormsAppCompatActivity _activity;
        private readonly ActionBar _actionBar;
        private readonly Drawable _actionBarDrawable;
        private readonly Drawable _blackDraw;
        private readonly ExtendedScrollView _extendedScrollView;

        //Pass the Activity and the NotifyingScrollView instance so they can be changed
        public ScrollViewChangedListener(FormsAppCompatActivity a, ExtendedScrollView n)
        {
            _extendedScrollView = n;            
            _activity = a;
            _actionBar = a.ActionBar;
            _blackDraw = a.Resources.GetDrawable(Resource.Drawable.black, _activity.Theme);
            _actionBarDrawable = a.Resources.GetDrawable(Resource.Drawable.actionbar_background, _activity.Theme);
            _actionBarDrawable.SetAlpha(0);
            _blackDraw.SetAlpha(15);
            _actionBar.SetBackgroundDrawable(_blackDraw);
        }

        public void StartListening()
        {
            _extendedScrollView.Scrolled += ScrollChangedTarget;
        }

        //Handle the changing of the scroll
        public void ScrollChangedTarget(object sender, ScrolledEventArgs e)
        {
            //You set the View you want to be your header as a header height, and then get it's height
            var headerHeight = _activity.FindViewById<FrameLayout>(Resource.Id.framePictureLayoutUpper).Height -
                               _actionBar.Height;
            var ratio = (float) Math.Min(Math.Max(e.X, 0), headerHeight) / headerHeight;
            var newAlpha = (int) (ratio * 255);
            if (newAlpha < 15)
            {
                /*
                We can't always know in advance what our background will be. That's why we aren't making a fully transparent ActionBar, but instead giving it
                a fully black background when it's opacity should be under 15. That way, out ActionBar icons will always be readable. Note, if you have black
                icons in your application, you can always make your blackDraw white (blackDraw is defined in res/drawable as an .xml file)
                #1#
                _blackDraw.SetAlpha(15);
                _actionBar.SetBackgroundDrawable(_blackDraw);
            }
            else
            {
                _actionBarDrawable.SetAlpha(newAlpha);
                _actionBar.SetBackgroundDrawable(_actionBarDrawable);
            }            
        }
    }*/
}