using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary
{
    public sealed class MoveOutControl : Control
    {
        private Grid gridRoot = null;
        private Border borderBack = null;
        private Border borderPositive = null;
        private double w, h = double.NaN;
        private Storyboard risingOutOfSb = null;
        private Storyboard moveDownSb = null;

        public MoveOutControl()
        {
            this.DefaultStyleKey = typeof(MoveOutControl);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.borderBack = this.GetTemplateChild("borderBack") as Border;
            this.borderPositive = this.GetTemplateChild("borderPositive") as Border;
            this.gridRoot = this.GetTemplateChild("gridRoot") as Grid;
            this.gridRoot.Background = new SolidColorBrush(Colors.Transparent);
            this.risingOutOfSb = new Storyboard();
            this.moveDownSb = new Storyboard();
            if (this.gridRoot != null)
            {
                this.gridRoot.PointerEntered += gridRoot_PointerEntered;
                this.gridRoot.PointerExited += gridRoot_PointerExited;
            }

            if (this != null && this.borderPositive != null)
            {
                this.borderPositive.Child = this.PositiveContent;
            }
            if (this != null && this.borderBack != null)
            {
                this.borderBack.Child = this.BackContent;
            }
        }

        private void gridRoot_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (PositiveContent != null)
            {
                risingOutOfSb.Stop();
                moveDownSb.Begin();
            }
        }

        private void gridRoot_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (PositiveContent != null)
            {
                moveDownSb.Stop();
                risingOutOfSb.Begin();
            }
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            this.w = this.gridRoot.Width = this.borderBack.Width = this.borderPositive.Width = finalSize.Width;
            this.h = this.gridRoot.Height = this.borderBack.Height = this.borderPositive.Height = finalSize.Height;
            if (this.gridRoot != null)
            {
                BorderPositiveCompositeTransform();
                AddMask();
                CreateAnimations();
            }
            return base.ArrangeOverride(finalSize);
        }

        public UIElement BackContent
        {
            get { return (UIElement)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register("BackContent", typeof(UIElement), typeof(MoveOutControl), new PropertyMetadata(null,
            new PropertyChangedCallback(OnBackContentPropertyChanged)));

        private static void OnBackContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var moveOutControl = d as MoveOutControl;
            if (moveOutControl != null && moveOutControl.borderBack != null)
            {
                moveOutControl.borderBack.Child = moveOutControl.BackContent;
            }
        }

        public UIElement PositiveContent
        {
            get { return (UIElement)GetValue(PositiveContentProperty); }
            set { SetValue(PositiveContentProperty, value); }
        }

        public static readonly DependencyProperty PositiveContentProperty = DependencyProperty.Register("PositiveContent", typeof(UIElement), typeof(MoveOutControl), new PropertyMetadata(null, 
            new PropertyChangedCallback(OnPositiveContentPropertyChanged)));

        private static void OnPositiveContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var moveOutControl = d as MoveOutControl;
            if (moveOutControl != null && moveOutControl.borderPositive != null)
            {
                moveOutControl.borderPositive.Child = moveOutControl.PositiveContent;
            }
        }

        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register("AnimationTime", typeof(TimeSpan), typeof(MoveOutControl), new PropertyMetadata(TimeSpan.FromSeconds(1), new PropertyChangedCallback(onAnimationTimePropertyChanged)));

        private static void onAnimationTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var moveOutControl = sender as MoveOutControl;
            if (moveOutControl != null && moveOutControl.borderPositive != null)
            {
                moveOutControl.CreateAnimations();
            }
        }

        //添加CompositeTransform
        private void AddCompositeTransform(UIElement control)
        {
            CompositeTransform ctf = new CompositeTransform();
            control.RenderTransformOrigin = new Point(0.5, 0.5);
            control.RenderTransform = ctf;
        }

        private void AddTransformYValue(CompositeTransform ctf, double value)
        {
            ctf.TranslateY = value;
        }

        private void BorderPositiveCompositeTransform()
        {
            this.AddCompositeTransform(borderPositive);
            this.AddTransformYValue(borderPositive.RenderTransform as CompositeTransform, -this.h);
        }

        private void CreateMoveDownSbAnimations(Border border)
        {
            this.moveDownSb = new Storyboard();
            DoubleAnimationUsingKeyFrames keyFramesMoveDown = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesMoveDown, border);
            Storyboard.SetTargetProperty(keyFramesMoveDown, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            KeyTime kt1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesMoveDown.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = kt1, Value = -this.h });
            KeyTime kt2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesMoveDown.KeyFrames.Add(new EasingDoubleKeyFrame()
            {
                KeyTime = kt2,
                Value = 0,
                EasingFunction = new BounceEase() { EasingMode = EasingMode.EaseOut }
            });
            moveDownSb.Children.Add(keyFramesMoveDown);
        }

        private void CreateRisingOutOfSbAnimations(Border border)
        {
            this.risingOutOfSb = new Storyboard();
            DoubleAnimationUsingKeyFrames keyFramesRisingOutOf = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesRisingOutOf, border);
            Storyboard.SetTargetProperty(keyFramesRisingOutOf, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            KeyTime kt1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesRisingOutOf.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = kt1, Value = 0 });
            KeyTime kt2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesRisingOutOf.KeyFrames.Add(new EasingDoubleKeyFrame() 
            { 
                KeyTime = kt2, 
                Value = -this.h, 
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } 
            });
            risingOutOfSb.Children.Add(keyFramesRisingOutOf);
        }

        //创建动画
        private void CreateAnimations()
        {
            CreateMoveDownSbAnimations(this.borderPositive);
            CreateRisingOutOfSbAnimations(this.borderPositive);
        }

        //剪裁显示大小
        private void AddMask()
        {
            if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height))
            {
                gridRoot.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), new Size(this.Width, this.Height)) };
            }
            else
            {
                gridRoot.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, this.w, this.h) };
            }
        }
    }
}
