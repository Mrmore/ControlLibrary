using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class OfflineTextBlock : Control
    {
        private Canvas canvas = null;
        private TextBlock tbTop = null;
        private TextBlock tbCenter = null;
        private TextBlock tbBottom = null;
        private Storyboard toTopSb = null;
        private Storyboard toBottomSb = null;
        private double tbHeight = double.NaN;
        private double tbWidth = double.NaN;
        private bool isLoad = true;
        private Grid grid = null;
        private Size size;
        private TextBlock tbTemp = null;

        private double oldText = 0;

        public OfflineTextBlock()
        {
            this.DefaultStyleKey = typeof(OfflineTextBlock);
            toTopSb = new Storyboard();
            toBottomSb = new Storyboard();
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            if (isLoad)
            {
                isLoad = false;
                tbHeight = this.tbCenter.ActualHeight;
                tbWidth = this.tbCenter.ActualWidth;
                if (!double.IsNaN(tbHeight) || !double.IsNaN(tbWidth))
                {
                    canvas.Width = tbWidth;
                    canvas.Height = tbHeight;
                    CreateTextBlock(tbHeight, tbWidth);
                    CreatetoTopSbAnimations(this.tbCenter, this.tbBottom);
                    CreatetoBottomSbAnimations(this.tbCenter, this.tbTop);
                    if (this.OfflineText != 0.0)
                    {
                        OfflineTextValue();
                    }
                    if (this.OfflineText == 0.0)
                    {
                        this.tbCenter.Opacity = 0;
                    }
                }
            }
            size = finalSize;
            AddMask();
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            canvas = this.GetTemplateChild("canvas") as Canvas;
            tbTop = this.GetTemplateChild("tbTop") as TextBlock;
            tbCenter = this.GetTemplateChild("tbCenter") as TextBlock;
            tbBottom = this.GetTemplateChild("tbBottom") as TextBlock;
            grid = this.GetTemplateChild("grid") as Grid;
            tbTemp = this.GetTemplateChild("tbTemp") as TextBlock;

            if (this.canvas != null)
            {
                if (!double.IsNaN(tbHeight) || !double.IsNaN(tbWidth))
                {
                    CreateTextBlock(tbHeight, tbWidth);
                    CreateAnimations();
                    if (this.OfflineText != 0.0)
                    {
                        OfflineTextValue();
                    }
                }
                this.SizeChanged -= OfflineTextBlock_SizeChanged;
                this.SizeChanged += OfflineTextBlock_SizeChanged;
                tbCenter.SizeChanged -= tbCenter_SizeChanged;
                tbCenter.SizeChanged += tbCenter_SizeChanged;
            }
        }

        void tbCenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //canvas.Width = e.NewSize.Width;
        }

        private void OfflineTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AddMask();
        }

        private void CreateTextBlock(double height, double width)
        {
            tbTop.SetValue(Canvas.TopProperty, -height);
            tbBottom.SetValue(Canvas.TopProperty, height);
        }

        //> 小于
        private void CreatetoTopSbAnimations(TextBlock tbCenter, TextBlock tbBottom)
        {
            //<Storyboard x:Name="ToTopSb">
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(Canvas.Top)" Storyboard.TargetName="tbCenter">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="0"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="-13.2"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="tbCenter">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="1"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="tbBottom">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="1"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="1"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(Canvas.Top)" Storyboard.TargetName="tbBottom">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="13.2"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                </Storyboard>
            DoubleAnimationUsingKeyFrames keyFramesCenterTop = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesCenterTop, tbCenter);
            Storyboard.SetTargetProperty(keyFramesCenterTop, "(Canvas.Top)");
            KeyTime ktCenterTop1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesCenterTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterTop1, Value = 0 });
            KeyTime ktCenterTop2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesCenterTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterTop2, Value = -tbHeight });
            toTopSb.Children.Add(keyFramesCenterTop);

            DoubleAnimationUsingKeyFrames keyFramesCenterOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesCenterOpacity, tbCenter);
            Storyboard.SetTargetProperty(keyFramesCenterOpacity, "(UIElement.Opacity)");
            KeyTime ktCenterOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesCenterOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterOpacity1, Value = 1 });
            KeyTime ktCenterOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesCenterOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterOpacity2, Value = 0 });
            toTopSb.Children.Add(keyFramesCenterOpacity);

            DoubleAnimationUsingKeyFrames keyFramesBottomOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesBottomOpacity, tbBottom);
            Storyboard.SetTargetProperty(keyFramesBottomOpacity, "(UIElement.Opacity)");
            KeyTime ktBottomOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesBottomOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktBottomOpacity1, Value = 1 });
            KeyTime ktBottomOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesBottomOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktBottomOpacity2, Value = 1 });
            toTopSb.Children.Add(keyFramesBottomOpacity);

            DoubleAnimationUsingKeyFrames keyFramesBottomTop = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesBottomTop, tbBottom);
            Storyboard.SetTargetProperty(keyFramesBottomTop, "(Canvas.Top)");
            KeyTime ktBottomTop1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesBottomTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktBottomTop1, Value = tbHeight });
            KeyTime ktBottomTop2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesBottomTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktBottomTop2, Value = 0 });
            toTopSb.Children.Add(keyFramesBottomTop);
        }

        //< 大于
        private void CreatetoBottomSbAnimations(TextBlock tbCenter, TextBlock tbTop)
        {
            //<Storyboard x:Name="ToBottomSb">
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(Canvas.Top)" Storyboard.TargetName="tbCenter">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="0"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="13.2"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="tbCenter">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="1"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(Canvas.Top)" Storyboard.TargetName="tbTop">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="-13.2"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="0"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                    <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(UIElement.Opacity)" Storyboard.TargetName="tbTop">
            //                        <SplineDoubleKeyFrame KeyTime="0" Value="1"/>
            //                        <SplineDoubleKeyFrame KeyTime="0:0:0.5" Value="1"/>
            //                    </DoubleAnimationUsingKeyFrames>
            //                </Storyboard>

            DoubleAnimationUsingKeyFrames keyFramesCenterTop = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesCenterTop, tbCenter);
            Storyboard.SetTargetProperty(keyFramesCenterTop, "(Canvas.Top)");
            KeyTime ktCenterTop1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesCenterTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterTop1, Value = 0 });
            KeyTime ktCenterTop2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesCenterTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterTop2, Value = tbHeight });
            toBottomSb.Children.Add(keyFramesCenterTop);

            DoubleAnimationUsingKeyFrames keyFramesCenterOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesCenterOpacity, tbCenter);
            Storyboard.SetTargetProperty(keyFramesCenterOpacity, "(UIElement.Opacity)");
            KeyTime ktCenterOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesCenterOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterOpacity1, Value = 1 });
            KeyTime ktCenterOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesCenterOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktCenterOpacity2, Value = 0 });
            toBottomSb.Children.Add(keyFramesCenterOpacity);

            DoubleAnimationUsingKeyFrames keyFramesTopOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesTopOpacity, tbTop);
            Storyboard.SetTargetProperty(keyFramesTopOpacity, "(UIElement.Opacity)");
            KeyTime ktTopOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesTopOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktTopOpacity1, Value = 1 });
            KeyTime ktTopOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesTopOpacity.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktTopOpacity2, Value = 1 });
            toBottomSb.Children.Add(keyFramesTopOpacity);

            DoubleAnimationUsingKeyFrames keyFramesTopTop = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesTopTop, tbTop);
            Storyboard.SetTargetProperty(keyFramesTopTop, "(Canvas.Top)");
            KeyTime ktTopTop1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesTopTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktTopTop1, Value = -tbHeight });
            KeyTime ktTopTop2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesTopTop.KeyFrames.Add(new SplineDoubleKeyFrame() { KeyTime = ktTopTop2, Value = 0 });
            toBottomSb.Children.Add(keyFramesTopTop);
        }

        //创建动画
        private void CreateAnimations()
        {
            CreatetoTopSbAnimations(this.tbCenter, this.tbBottom);
            CreatetoBottomSbAnimations(this.tbCenter, this.tbTop);
        }

        public double OfflineText
        {
            get { return (double)GetValue(OfflineTextProperty); }
            set { SetValue(OfflineTextProperty, value); }
        }

        public static readonly DependencyProperty OfflineTextProperty = DependencyProperty.Register("OfflineText", typeof(double), typeof(OfflineTextBlock), new PropertyMetadata(0.0, new PropertyChangedCallback(onOfflineTextBlockPropertyChanged)));

        private static void onOfflineTextBlockPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var offlineTextBlock = sender as OfflineTextBlock;
            if (offlineTextBlock != null && offlineTextBlock.tbTop != null && offlineTextBlock.tbCenter != null && offlineTextBlock.tbBottom != null)
            {
                offlineTextBlock.OfflineTextValue();
            }
        }

        public string OfflineMessageHeadText
        {
            get { return (string)GetValue(OfflineMessageHeadTextProperty); }
            set { SetValue(OfflineMessageHeadTextProperty, value); }
        }

        public static readonly DependencyProperty OfflineMessageHeadTextProperty = DependencyProperty.Register("OfflineMessageHeadText", typeof(string), typeof(OfflineTextBlock), new PropertyMetadata(string.Empty, new PropertyChangedCallback(onOfflineMessageHeadTextPropertyChanged)));

        private static void onOfflineMessageHeadTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var offlineTextBlock = sender as OfflineTextBlock;
            if (offlineTextBlock != null && offlineTextBlock.tbTop != null && offlineTextBlock.tbCenter != null && offlineTextBlock.tbBottom != null)
            {
                offlineTextBlock.OfflineTextValue();
            }
        }

        public string OfflineMessageTailText
        {
            get { return (string)GetValue(OfflineMessageTailTextProperty); }
            set { SetValue(OfflineMessageTailTextProperty, value); }
        }

        public static readonly DependencyProperty OfflineMessageTailTextProperty = DependencyProperty.Register("OfflineMessageTailText", typeof(string), typeof(OfflineTextBlock), new PropertyMetadata(string.Empty, new PropertyChangedCallback(onOfflineMessageTailTextPropertyChanged)));

        private static void onOfflineMessageTailTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var offlineTextBlock = sender as OfflineTextBlock;
            if (offlineTextBlock != null && offlineTextBlock.tbTop != null && offlineTextBlock.tbCenter != null && offlineTextBlock.tbBottom != null)
            {
                offlineTextBlock.OfflineTextValue();
            }
        }

        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register("AnimationTime", typeof(TimeSpan), typeof(OfflineTextBlock), new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 500), new PropertyChangedCallback(onAnimationTimePropertyChanged)));

        private static void onAnimationTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var offlineTextBlock = sender as OfflineTextBlock;
            if (offlineTextBlock != null && offlineTextBlock.tbTop != null && offlineTextBlock.tbCenter != null && offlineTextBlock.tbBottom != null)
            {
                offlineTextBlock.CreateAnimations();
            }
        }

        void toBottomSb_Completed(object sender, object e)
        {
            toBottomSb.Completed -= toBottomSb_Completed;
            if (OfflineText == 0)
            {
                this.tbCenter.Opacity = 0;
                this.tbTop.Opacity = 0;
                tbTemp.Text = "";
                canvas.Width = 0.0;
            }
            else
            {
                tbTemp.Text = this.tbTop.Text;
            }
        }

        private void OfflineTextValue()
        {
            if (OfflineText > -1)
            {
                if (OfflineText < oldText)
                {
                    toTopSb.Stop();
                    this.tbCenter.Text = this.OfflineMessageHeadText + oldText.ToString() + this.OfflineMessageTailText;
                    this.tbTop.Text = this.OfflineMessageHeadText + OfflineText.ToString() + this.OfflineMessageTailText;
                    toBottomSb.Completed -= toBottomSb_Completed;
                    toBottomSb.Completed += toBottomSb_Completed;
                    toBottomSb.Begin();
                }
                if (OfflineText > oldText)
                {
                    toBottomSb.Stop();
                    this.tbCenter.Text = this.OfflineMessageHeadText + oldText.ToString() + this.OfflineMessageTailText;
                    this.tbBottom.Text = this.OfflineMessageHeadText + OfflineText.ToString() + this.OfflineMessageTailText;
                    tbTemp.Text = this.tbBottom.Text;
                    toTopSb.Begin();
                }
                if (OfflineText == 0)
                {
                    if (OfflineText < oldText)
                    {
                        toBottomSb.Completed -= toBottomSb_Completed;
                        toBottomSb.Completed += toBottomSb_Completed;
                    }
                    //this.tbTop.Opacity = 0;
                }
                oldText = OfflineText;
            }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(OfflineTextBlock), new PropertyMetadata(TextAlignment.Left));


        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(OfflineTextBlock), new PropertyMetadata(TextTrimming.None));

        //剪裁显示大小
        private void AddMask()
        {
            if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height))
            {
                grid.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), new Size(this.Width, this.Height)) };
            }
            else
            {
                grid.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), size) };
            }
        }
    }
}
