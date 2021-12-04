using Android.App;
using Android.Content;
using Luna.Renderers;
using Luna.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ExportRenderer(typeof(OverflowLabel), typeof(OverflowLabelFastRenderer))]
namespace Luna.Renderers
{
    class OverflowLabelFastRenderer : Xamarin.Forms.Platform.Android.FastRenderers.LabelRenderer, IOnLayoutChangeListener
    {
        public OverflowLabelFastRenderer(Context context) : base(context)
        {
            AddOnLayoutChangeListener(this);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                ((OverflowLabel)e.NewElement).GetLineEnd = null;
            }
            if (e.NewElement != null)
            {
                ((OverflowLabel)e.NewElement).GetLineEnd = (line) => Layout.GetLineEnd(line);
            }
            
        }

        public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            v.Post(() => ((OverflowLabel)Element).FireLayoutChangeEvent());
        }
    }
}