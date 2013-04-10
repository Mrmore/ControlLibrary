using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools
{
    public class UIUtil
    {
        public static T GetTransform<T>(UIElement uIElement) where T : Transform
        {
            var transform = uIElement.RenderTransform as T;
            if (transform != null) return transform;

            var tg = uIElement.RenderTransform as TransformGroup;
            if (tg != null)
            {
                foreach (var t in tg.Children)
                {
                    if (t.GetType() == typeof(T)) return (T)t;
                }
            }

            return null;
        }

        public static bool TrySetImageSource(Image img, string path)
        {
            if (img == null) return false;
            if (path != null && Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                img.Source = new BitmapImage(new Uri(path));
                return true;
            }
            return false;
        }
    }
}
