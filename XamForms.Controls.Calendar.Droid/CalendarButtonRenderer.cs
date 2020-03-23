using Android.Graphics.Drawables;
using XamForms.Controls.Droid;
using XamForms.Controls;
using Android.Runtime;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Graphics;
using Xamarin.Forms;
using System;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(CalendarButton), typeof(CalendarButtonRenderer))]
namespace XamForms.Controls.Droid
{
    [Preserve(AllMembers = true)]
    public class CalendarButtonRenderer : ButtonRenderer
    {
        private Context context;

        public CalendarButtonRenderer(Android.Content.Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.TextChanged += (sender, a) =>
            {
                var element = Element as CalendarButton;
                if (Control.Text == element.TextWithoutMeasure || (string.IsNullOrEmpty(Control.Text) && string.IsNullOrEmpty(element.TextWithoutMeasure)))
                {
                    return;
                }
                Control.Text = element.TextWithoutMeasure;
            };

            Control.SetPadding(1, 1, 1, 1);
            Control.ViewTreeObserver.GlobalLayout += (sender, args) => ChangeBackgroundPattern();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if(!(Element is CalendarButton element))
            {
                return;
            }

            if (e.PropertyName == nameof(element.TextWithoutMeasure) || e.PropertyName == "Renderer")
            {
                Control.Text = element.TextWithoutMeasure;
            }

            if (e.PropertyName == nameof(Element.TextColor) || e.PropertyName == "Renderer")
            {
                Control.SetTextColor(Element.TextColor.ToAndroid());
            }

            if (e.PropertyName == nameof(Element.BorderWidth) || e.PropertyName == nameof(Element.BorderColor) || e.PropertyName == nameof(Element.BackgroundColor) || e.PropertyName == "Renderer")
            {
                if (element.BackgroundPattern == null)
                {
                    if (element.BackgroundImage == null)
                    {
                        var drawable = new GradientDrawable();
                        drawable.SetShape(ShapeType.Rectangle);
                        var borderWidth = (int)Math.Ceiling(Element.BorderWidth);
                        drawable.SetStroke(borderWidth > 0 ? borderWidth + 1 : borderWidth, Element.BorderColor.ToAndroid());
                        drawable.SetColor(Element.BackgroundColor.ToAndroid());
                        Control.SetBackground(drawable);
                    }
                    else
                    {
                        ChangeBackgroundImage();
                        return;
                    }
                }
                else
                {
                    ChangeBackgroundPattern();
                    return;
                }
            }

            if (e.PropertyName == nameof(element.BackgroundPattern))
            {
                ChangeBackgroundPattern();
                return;
            }

            if (e.PropertyName == nameof(element.BackgroundImage))
            {
                ChangeBackgroundImage();
                return;
            }
        }

        protected async void ChangeBackgroundImage()
        {
            if (!(Element is CalendarButton element) || element.BackgroundImage == null)
            {
                return;
            }

            var drawableList = new List<Drawable>();

            var image = await GetBitmap(element.BackgroundImage);
            drawableList.Add(new BitmapDrawable(image));
            var drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            var borderWidth = (int)Math.Ceiling(Element.BorderWidth);
            drawable.SetStroke(borderWidth > 0 ? borderWidth + 1 : borderWidth, Element.BorderColor.ToAndroid());
            drawable.SetColor(Android.Graphics.Color.Transparent);
            drawableList.Add(drawable);
            var layer = new LayerDrawable(drawableList.ToArray());
            layer.SetLayerInset(drawableList.Count - 1, 0, 0, 0, 0);

            Control.SetBackground(layer);
        }

        protected void ChangeBackgroundPattern()
        {
            if (!(Element is CalendarButton element) || element.BackgroundPattern == null || Control.Width == 0)
            {
                return;
            }

            var drawableList = new List<Drawable>();
            for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
            {
                var backgroundPattern = element.BackgroundPattern.Pattern[i];
                if (!string.IsNullOrEmpty(backgroundPattern.Text))
                {
                    drawableList.Add(new TextDrawable(context, backgroundPattern.Color.ToAndroid(), backgroundPattern));
                }
                else
                {
                    drawableList.Add(new ColorDrawable(backgroundPattern.Color.ToAndroid()));
                }
            }

            var drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            var borderWidth = (int)Math.Ceiling(Element.BorderWidth);
            drawable.SetStroke(borderWidth > 0 ? borderWidth + 1 : borderWidth, Element.BorderColor.ToAndroid());
            drawable.SetColor(Android.Graphics.Color.Transparent);
            drawableList.Add(drawable);
            var layer = new LayerDrawable(drawableList.ToArray());

            for (var i = 0; i < element.BackgroundPattern.Pattern.Count; i++)
            {
                var l = (int)Math.Ceiling(Control.Width * element.BackgroundPattern.GetLeft(i));
                var t = (int)Math.Ceiling(Control.Height * element.BackgroundPattern.GetTop(i));
                var r = (int)Math.Ceiling(Control.Width * (1 - element.BackgroundPattern.Pattern[i].WidthPercent)) - l;
                var b = (int)Math.Ceiling(Control.Height * (1 - element.BackgroundPattern.Pattern[i].HeightPercent)) - t;
                layer.SetLayerInset(i, l, t, r, b);
            }
            layer.SetLayerInset(drawableList.Count - 1, 0, 0, 0, 0);
            Control.SetBackground(layer);
        }

        private Task<Bitmap> GetBitmap(FileImageSource image)
        {
            var handler = new FileImageSourceHandler();
            return handler.LoadImageAsync(image, this.Control.Context);
        }
    }
}