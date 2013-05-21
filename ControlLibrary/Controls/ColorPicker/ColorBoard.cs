using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace ControlLibrary
{
    public class ColorBoard : Control
    {
        public ColorBoard()
        {
            this.DefaultStyleKey = typeof(ColorBoard);
        }

        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(Color), typeof(ColorBoard), new PropertyMetadata(null, new PropertyChangedCallback(ColorBoard.OnColorPropertyChanged)));
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorBoard control = d as ColorBoard;
            if (control != null && control.rootElement != null)
            {
                if (control.Updating)
                {
                    return;
                }

                Color color = (Color)e.NewValue;
                control.UpdateControls(color, true, true, true);
            }
        }

        //public delegate void RoutedEventHandler(object sender);
        public event RoutedEventHandler DoneClicked;
        private void OnDoneClicked()
        {
            if (DoneClicked != null)
            {
                this.DoneClicked(this, new RoutedEventArgs());
                //this.DoneClicked(this);
            }
        }

        private FrameworkElement rootElement = null;

        private Canvas canvasHSV;
        private Rectangle rectangleRootHSV;
        private GradientStop gradientStopHSVColor;
        private Rectangle rectangleHSV;
        private Rectangle ellipseHSV;

        private Slider sliderHSV;

        private Slider sliderA;
        private GradientStop gradientStopA0;
        private GradientStop gradientStopA1;

        private Slider sliderR;
        private GradientStop gradientStopR0;
        private GradientStop gradientStopR1;

        private Slider sliderG;
        private GradientStop gradientStopG0;
        private GradientStop gradientStopG1;

        private Slider sliderB;
        private GradientStop gradientStopB0;
        private GradientStop gradientStopB1;

        private TextBox textBoxA;
        private TextBox textBoxR;
        private TextBox textBoxG;
        private TextBox textBoxB;

        private ComboBox comboBoxColor;
        private Rectangle rectangleColor;
        private SolidColorBrush brushColor;
        private TextBox textBoxColor;

        private Button buttonDone;

        protected override void OnApplyTemplate()
        {            
            base.OnApplyTemplate();

            rootElement = (FrameworkElement)GetTemplateChild("RootElement");

            canvasHSV = (Canvas)GetTemplateChild("CanvasHSV");
            rectangleRootHSV = (Rectangle)GetTemplateChild("RectangleRootHSV");
            gradientStopHSVColor = (GradientStop)GetTemplateChild("GradientStopHSVColor");
            rectangleHSV = (Rectangle)GetTemplateChild("RectangleHSV");
            ellipseHSV = (Rectangle)GetTemplateChild("EllipseHSV");
            sliderHSV = (Slider)GetTemplateChild("SliderHSV");

            sliderA = (Slider)GetTemplateChild("SliderA");
            gradientStopA0 = (GradientStop)GetTemplateChild("GradientStopA0");
            gradientStopA1 = (GradientStop)GetTemplateChild("GradientStopA1");
            sliderR = (Slider)GetTemplateChild("SliderR");
            gradientStopR0 = (GradientStop)GetTemplateChild("GradientStopR0");
            gradientStopR1 = (GradientStop)GetTemplateChild("GradientStopR1");
            sliderG = (Slider)GetTemplateChild("SliderG");
            gradientStopG0 = (GradientStop)GetTemplateChild("GradientStopG0");
            gradientStopG1 = (GradientStop)GetTemplateChild("GradientStopG1");
            sliderB = (Slider)GetTemplateChild("SliderB");
            gradientStopB0 = (GradientStop)GetTemplateChild("GradientStopB0");
            gradientStopB1 = (GradientStop)GetTemplateChild("GradientStopB1");

            textBoxA = (TextBox)GetTemplateChild("TextBoxA");
            textBoxR = (TextBox)GetTemplateChild("TextBoxR");
            textBoxG = (TextBox)GetTemplateChild("TextBoxG");
            textBoxB = (TextBox)GetTemplateChild("TextBoxB");

            comboBoxColor = (ComboBox)GetTemplateChild("ComboBoxColor");
            rectangleColor = (Rectangle)GetTemplateChild("RectangleColor");
            brushColor = (SolidColorBrush)GetTemplateChild("BrushColor");
            textBoxColor = (TextBox)GetTemplateChild("TextBoxColor");
            buttonDone = (Button)GetTemplateChild("ButtonDone");

            rectangleHSV.PointerPressed += rectangleHSV_PointerPressed;
            rectangleHSV.PointerMoved += rectangleHSV_PointerMoved;
            rectangleHSV.PointerReleased += rectangleHSV_PointerReleased;
            rectangleHSV.PointerExited += rectangleHSV_PointerExited;

            sliderHSV.ValueChanged += sliderHSV_ValueChanged;

            sliderA.ValueChanged += sliderA_ValueChanged;
            sliderR.ValueChanged += sliderR_ValueChanged;
            sliderG.ValueChanged += sliderG_ValueChanged;
            sliderB.ValueChanged += sliderB_ValueChanged;

            textBoxA.LostFocus += new RoutedEventHandler(textBoxA_LostFocus);
            textBoxR.LostFocus += new RoutedEventHandler(textBoxR_LostFocus);
            textBoxG.LostFocus += new RoutedEventHandler(textBoxG_LostFocus);
            textBoxB.LostFocus += new RoutedEventHandler(textBoxB_LostFocus);

            comboBoxColor.SelectionChanged += new SelectionChangedEventHandler(comboBoxColor_SelectionChanged);
            textBoxColor.GotFocus += new RoutedEventHandler(textBoxColor_GotFocus);
            textBoxColor.LostFocus += new RoutedEventHandler(textBoxColor_LostFocus);
            buttonDone.Click += new RoutedEventHandler(buttonDone_Click);

            InitializePredefined();
            UpdateControls(Color, true, true, true);
        }

        void sliderHSV_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            gradientStopHSVColor.Color = ColorHelper.HSV2RGB(e.NewValue, 1d, 1d);

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }

        void rectangleHSV_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            trackingHSV = false;
            rectangleHSV.ReleasePointerCapture(e.Pointer);
        }

        void rectangleHSV_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            trackingHSV = false;
            rectangleHSV.ReleasePointerCapture(e.Pointer);
        }

        private bool trackingHSV;
        void rectangleHSV_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            trackingHSV = rectangleHSV.CapturePointer(e.Pointer);

            Point point = e.GetCurrentPoint(rectangleHSV).Position;

            Size size = ellipseHSV.RenderSize;

            ellipseHSV.SetValue(Canvas.LeftProperty, point.X - ellipseHSV.ActualWidth / 2);
            ellipseHSV.SetValue(Canvas.TopProperty, point.Y - ellipseHSV.ActualHeight / 2);

            if (Updating)
            {
                return;
            }

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }

        void rectangleHSV_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (trackingHSV)
            {
                Point point = e.GetCurrentPoint(rectangleHSV).Position;
                Size size = ellipseHSV.RenderSize;

                ellipseHSV.SetValue(Canvas.LeftProperty, point.X - ellipseHSV.ActualWidth / 2);
                ellipseHSV.SetValue(Canvas.TopProperty, point.Y - ellipseHSV.ActualHeight / 2);

                if (Updating)
                {
                    return;
                }

                Color color = GetHSVColor();
                UpdateControls(color, false, true, true);
            }
        }

        private Dictionary<Color, PredefinedColorItem> dictionaryColor;
        private void InitializePredefined()
        {
            if (dictionaryColor != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.All;
            dictionaryColor = new Dictionary<Color, PredefinedColorItem>();
            foreach (PredefinedColor color in list)
            {
                PredefinedColorItem item = new PredefinedColorItem(color.Value, color.Name);
                comboBoxColor.Items.Add(item);

                if (!dictionaryColor.ContainsKey(color.Value))
                {
                    dictionaryColor.Add(color.Value, item);
                }
            }
        }

        private Color GetHSVColor()
        {
            double h = sliderHSV.Value;

            double x = (double)ellipseHSV.GetValue(Canvas.LeftProperty) + ellipseHSV.ActualWidth / 2;
            double y = (double)ellipseHSV.GetValue(Canvas.TopProperty) + ellipseHSV.ActualHeight / 2;

            double s = x / (rectangleHSV.ActualWidth - 1);
            if (s < 0d)
                s = 0d;
            else if (s > 1d)
                s = 1d;

            double v = 1 - y / (rectangleHSV.ActualHeight - 1);
            if (v < 0d)
                v = 0d;
            else if (v > 1d)
                v = 1d;

            return ColorHelper.HSV2RGB(h, s, v);
        }

        private void sliderA_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderR_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderG_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderB_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        private Color GetRGBColor()
        {
            byte a = (byte)sliderA.Value;
            byte r = (byte)sliderR.Value;
            byte g = (byte)sliderG.Value;
            byte b = (byte)sliderB.Value;

            return Color.FromArgb(a, r, g, b);
        }

        private void textBoxA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxA.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderA.Value = value;
            }
        }
        private void textBoxR_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderR.Value = value;
            }
        }
        private void textBoxG_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxG.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderG.Value = value;
            }
        }
        private void textBoxB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxB.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderB.Value = value;
            }
        }

        private void UpdateControls(Color color, bool hsv, bool rgb, bool predifined)
        {
            if (Updating)
            {
                return;
            }

            try
            {
                BeginUpdate();

                // HSV
                if (hsv)
                {
                    double h = ColorHelper.GetHSV_H(color);
                    double s = ColorHelper.GetHSV_S(color);
                    double v = ColorHelper.GetHSV_V(color);

                    sliderHSV.Value = h;
                    gradientStopHSVColor.Color = ColorHelper.HSV2RGB(h, 1d, 1d);

                    double x = s * (rectangleHSV.ActualWidth - 1);
                    double y = (1 - v) * (rectangleHSV.ActualHeight - 1);

                    ellipseHSV.SetValue(Canvas.LeftProperty, x - ellipseHSV.ActualWidth / 2);
                    ellipseHSV.SetValue(Canvas.TopProperty, y - ellipseHSV.ActualHeight / 2);
                }

                if (rgb)
                {
                    byte a = color.A;
                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;

                    sliderA.Value = a;
                    gradientStopA0.Color = Color.FromArgb(0, r, g, b);
                    gradientStopA1.Color = Color.FromArgb(255, r, g, b);
                    textBoxA.Text = a.ToString("X2");

                    sliderR.Value = r;
                    gradientStopR0.Color = Color.FromArgb(255, 0, g, b);
                    gradientStopR1.Color = Color.FromArgb(255, 255, g, b);
                    textBoxR.Text = r.ToString("X2");

                    sliderG.Value = g;
                    gradientStopG0.Color = Color.FromArgb(255, r, 0, b);
                    gradientStopG1.Color = Color.FromArgb(255, r, 255, b);
                    textBoxG.Text = g.ToString("X2");

                    sliderB.Value = b;
                    gradientStopB0.Color = Color.FromArgb(255, r, g, 0);
                    gradientStopB1.Color = Color.FromArgb(255, r, g, 255);
                    textBoxB.Text = b.ToString("X2");
                }

                if (predifined)
                {
                    brushColor.Color = color;
                    if (dictionaryColor.ContainsKey(color))
                    {
                        comboBoxColor.SelectedItem = dictionaryColor[color];
                        textBoxColor.Text = "";
                    }
                    else
                    {
                        comboBoxColor.SelectedItem = null;
                        textBoxColor.Text = color.ToString();
                    }
                }

                Color = color;
            }
            finally
            {
                EndUpdate();
            }
        }

        private void comboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            PredefinedColorItem coloritem = comboBoxColor.SelectedItem as PredefinedColorItem;
            if (coloritem != null)
            {
                Color = coloritem.Color;
            }
        }
        private void textBoxColor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            try
            {
                BeginUpdate();

                comboBoxColor.SelectedItem = null;
                textBoxColor.Text = Color.ToString();
            }
            finally
            {
                EndUpdate();
            }
        }
        private void textBoxColor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            string text = textBoxColor.Text.TrimStart('#');
            uint value = 0;
            if (uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                byte b = (byte)(value & 0xFF);
                value >>= 8;
                byte g = (byte)(value & 0xFF);
                value >>= 8;
                byte r = (byte)(value & 0xFF);
                value >>= 8;
                byte a = (byte)(value & 0xFF);

                if (text.Length <= 6)
                {
                    a = 0xFF;
                }

                Color color = Color.FromArgb(a, r, g, b);
                Color = color;
            }
            else
            {
                Color = Colors.White;
            }
        }
        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            OnDoneClicked();
        }

        private int isUpdating;
        private bool Updating
        {
            get
            {
                return isUpdating != 0;
            }
        }
        private void BeginUpdate()
        {
            isUpdating++;
        }
        private void EndUpdate()
        {
            isUpdating--;
        }
    }
}
