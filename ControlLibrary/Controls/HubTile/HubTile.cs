using System;
using System.Collections.Generic;
using System.Linq;
using ControlLibrary.Helper;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    /// <summary>
    /// Represents an animated tile that supports an image and a title.
    /// Furthermore, it can also be associated with a message or a notification.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = Expanded, GroupName = ImageStates)]
    [TemplateVisualState(Name = Semiexpanded, GroupName = ImageStates)]
    [TemplateVisualState(Name = Collapsed, GroupName = ImageStates)]
    [TemplateVisualState(Name = Flipped, GroupName = ImageStates)]
    [TemplatePart(Name = NotificationBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = MessageBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = BackTitleBlock, Type = typeof(TextBlock))]
    public class HubTile : Control
    {
        /// <summary>
        /// Common visual states.
        /// </summary>
        private const string ImageStates = "ImageState";

        /// <summary>
        /// Expanded visual state.
        /// </summary>
        private const string Expanded = "Expanded";

        /// <summary>
        /// Semiexpanded visual state.
        /// </summary>
        private const string Semiexpanded = "Semiexpanded";

        /// <summary>
        /// Collapsed visual state.
        /// </summary>
        private const string Collapsed = "Collapsed";

        /// <summary>
        /// Flipped visual state.
        /// </summary>
        private const string Flipped = "Flipped";

        /// <summary>
        /// Nofitication Block template part name.
        /// </summary>
        private const string NotificationBlock = "NotificationBlock";

        /// <summary>
        /// Message Block template part name.
        /// </summary>
        private const string MessageBlock = "MessageBlock";

        /// <summary>
        /// Back Title Block template part name.
        /// </summary>
        private const string BackTitleBlock = "BackTitleBlock";

        /// <summary>
        /// 反转图片的常量名
        /// </summary>
        private const string Image = "Image";

        /// <summary>
        /// 默认Load的图片常量名
        /// </summary>
        private const string DefaultImage = "defaultImage";

        /// <summary>
        /// 反面图片的常量名
        /// </summary>
        private const string OppositeImage = "ImageOpposite";

        /// <summary>
        /// Notification Block template part.
        /// </summary>
        private TextBlock _notificationBlock;

        /// <summary>
        /// Message Block template part.
        /// </summary>
        private TextBlock _messageBlock;

        /// <summary>
        /// Back Title Block template part.
        /// </summary>
        private TextBlock _backTitleBlock;

        /// <summary>
        /// 默认头像
        /// </summary>
        private Image imageDefault;

        /// <summary>
        /// 反面头像
        /// </summary>
        private Image imageOpposite;

        /// <summary>
        /// 反转图片
        /// </summary>
        //private Image image;
        private ClipImage image;

        /// <summary>
        /// Represents the number of steps inside the pipeline of stalled images
        /// </summary>
        internal int _stallingCounter;

        /// <summary>
        /// Flag that determines if the hub tile has a primary text string associated to it.
        /// If it does not, the hub tile will not drop.
        /// </summary>
        internal bool _canDrop;

        /// <summary>
        /// Flag that determines if the hub tile has a secondary text string associated to it.
        /// If it does not, the hub tile will not flip.
        /// </summary>
        internal bool _canFlip;

        #region Source DependencyProperty

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(HubTile), new PropertyMetadata(null));

        #endregion

        #region SourceDefault DependencyProperty

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource SourceDefault
        {
            get { return (ImageSource)GetValue(SourceDefaultProperty); }
            set { SetValue(SourceDefaultProperty, value); }
        }

        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceDefaultProperty =
            DependencyProperty.Register("SourceDefault", typeof(ImageSource), typeof(HubTile), new PropertyMetadata(null));

        #endregion

        #region SourceDefault DependencyProperty

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        public ImageSource SourceOpposite
        {
            get { return (ImageSource)GetValue(SourceOppositeProperty); }
            set { SetValue(SourceOppositeProperty, value); }
        }

        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceOppositeProperty =
            DependencyProperty.Register("SourceOpposite", typeof(ImageSource), typeof(HubTile), new PropertyMetadata(null));

        #endregion

        #region Title DependencyProperty

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Identifies the Title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(HubTile), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTitleChanged)));

        /// <summary>
        /// Prevents the hub tile from transitioning into a Semiexpanded or Collapsed visual state if the title is not set.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnTitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HubTile tile = (HubTile)obj;

            if (string.IsNullOrEmpty((string)e.NewValue))
            {
                tile._canDrop = false;
                tile.State = ImageState.Expanded;
            }
            else
            {
                tile._canDrop = true;
            }
        }

        #endregion

        #region Notification DependencyProperty

        /// <summary>
        /// Gets or sets the notification alert.
        /// </summary>
        public string Notification
        {
            get { return (string)GetValue(NotificationProperty); }
            set { SetValue(NotificationProperty, value); }
        }

        /// <summary>
        /// Identifies the Notification dependency property.
        /// </summary>
        public static readonly DependencyProperty NotificationProperty =
            DependencyProperty.Register("Notification", typeof(string), typeof(HubTile), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBackContentChanged)));

        /// <summary>
        /// Prevents the hub tile from transitioning into a Flipped visual state if neither the notification alert nor the message are set.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnBackContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HubTile tile = (HubTile)obj;

            // If there is a new notification or a message, the hub tile can flip.
            if ((!(string.IsNullOrEmpty(tile.Notification)) && tile.DisplayNotification)
                || (!(string.IsNullOrEmpty(tile.Message)) && !tile.DisplayNotification))
            {
                tile._canFlip = true;
            }
            else
            {
                tile._canFlip = false;
                tile.State = ImageState.Expanded;
            }
        }

        #endregion

        #region Message DependencyProperty

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Identifies the Message dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(HubTile), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBackContentChanged)));

        #endregion

        #region DisplayNotification DependencyProperty

        /// <summary>
        /// Gets or sets the flag for new notifications.
        /// </summary>
        public bool DisplayNotification
        {
            get { return (bool)GetValue(DisplayNotificationProperty); }
            set { SetValue(DisplayNotificationProperty, value); }
        }

        /// <summary>
        /// Identifies the DisplayNotification dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNotificationProperty =
            DependencyProperty.Register("DisplayNotification", typeof(bool), typeof(HubTile), new PropertyMetadata(false, new PropertyChangedCallback(OnBackContentChanged)));

        #endregion

        #region IsFrozen DependencyProperty

        /// <summary>
        /// Gets or sets the flag for images that do not animate.
        /// </summary>
        public bool IsFrozen
        {
            get { return (bool)GetValue(IsFrozenProperty); }
            set { SetValue(IsFrozenProperty, value); }
        }

        /// <summary>
        /// Identifies the IsFrozen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFrozenProperty =
            DependencyProperty.Register("IsFrozen", typeof(bool), typeof(HubTile), new PropertyMetadata(false, new PropertyChangedCallback(OnIsFrozenChanged)));

        /// <summary>
        /// Removes the frozen image from the enabled image pool or the stalled image pipeline.
        /// Adds the non-frozen image to the enabled image pool.  
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnIsFrozenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            HubTile tile = (HubTile)obj;

            if ((bool)e.NewValue)
            {
                HubTileService.FreezeHubTile(tile);
            }
            else
            {
                HubTileService.UnfreezeHubTile(tile);
            }
        }

        #endregion

        #region GroupTag DependencyProperty

        /// <summary>
        /// Gets or sets the group tag.
        /// </summary>
        public string GroupTag
        {
            get { return (string)GetValue(GroupTagProperty); }
            set { SetValue(GroupTagProperty, value); }
        }

        /// <summary>
        /// Identifies the GroupTag dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupTagProperty =
            DependencyProperty.Register("GroupTag", typeof(string), typeof(HubTile), new PropertyMetadata(string.Empty));

        #endregion

        #region State DependencyProperty

        /// <summary>
        /// Gets or sets the visual state.
        /// </summary>
        internal ImageState State
        {
            get { return (ImageState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        /// <summary>
        /// Identifies the State dependency property.
        /// </summary>
        private static readonly DependencyProperty StateProperty =
                DependencyProperty.Register("State", typeof(ImageState), typeof(HubTile), new PropertyMetadata(ImageState.Expanded, OnImageStateChanged));

        /// <summary>
        /// Triggers the transition between visual states.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnImageStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((HubTile)obj).UpdateVisualState();
        }

        #endregion

        //推送的字体大小
        public double NotificationFontSize
        {
            get { return (double)GetValue(NotificationFontSizeProperty); }
            set { SetValue(NotificationFontSizeProperty, value); }
        }

        public static readonly DependencyProperty NotificationFontSizeProperty = DependencyProperty.Register("NotificationFontSize", typeof(double), typeof(HubTile), new PropertyMetadata(28.0));

        //是否开始图片的延时加载动画
        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register("IsAnimation", typeof(bool), typeof(HubTile), new PropertyMetadata(false));

        //动画时间
        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register("AnimationTime", typeof(TimeSpan), typeof(HubTile), new PropertyMetadata(TimeSpan.FromSeconds(2)));

        //缓存类型
        public CacheImageDateType CacheType
        {
            get { return (CacheImageDateType)GetValue(CacheTypeProperty); }
            set { SetValue(CacheTypeProperty, value); }
        }

        public static readonly DependencyProperty CacheTypeProperty = DependencyProperty.Register("CacheType", typeof(CacheImageDateType), typeof(HubTile), new PropertyMetadata(CacheImageDateType.CacheHead));

        //是否缓存
        public bool IsCacheImage
        {
            get { return (bool)GetValue(IsCacheImageProperty); }
            set { SetValue(IsCacheImageProperty, value); }
        }

        public static readonly DependencyProperty IsCacheImageProperty = DependencyProperty.Register("IsCacheImage", typeof(bool), typeof(HubTile), new PropertyMetadata(true));

        /// <summary>
        /// Updates the visual state.
        /// </summary>
        private void UpdateVisualState()
        {
            string state;
            //if (image != null)
            //{
            //    image.Width = 173;
            //    image.Height = 173;
            //}
            switch (State)
            {
                case ImageState.Expanded:
                    state = Expanded;
                    break;
                case ImageState.Semiexpanded:
                    state = Semiexpanded;
                    break;
                case ImageState.Collapsed:
                    state = Collapsed;
                    break;
                case ImageState.Flipped:
                    state = Flipped;
                    break;
                default:
                    state = Expanded;
                    break;
            }

            VisualStateManager.GoToState(this, state, true);
        }

        /// <summary>
        /// Gets the template parts and sets binding.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _notificationBlock = base.GetTemplateChild(NotificationBlock) as TextBlock;
            _messageBlock = base.GetTemplateChild(MessageBlock) as TextBlock;
            _backTitleBlock = base.GetTemplateChild(BackTitleBlock) as TextBlock;
            imageDefault = base.GetTemplateChild(DefaultImage) as Image;
            imageOpposite = base.GetTemplateChild(OppositeImage) as Image;
            //image = base.GetTemplateChild(Image) as Image;
            image = base.GetTemplateChild(Image) as ClipImage;

            if (image != null)
            {
                image.ImageOpened += image_ImageOpened;
            }

            //Do binding in code to avoid exposing unnecessary value converters.
            if (_notificationBlock != null)
            {
                Binding bindVisible = new Binding();
                bindVisible.Source = this;
                bindVisible.Path = new PropertyPath("DisplayNotification");
                bindVisible.Converter = new VisibilityConverter();
                bindVisible.ConverterParameter = false;
                _notificationBlock.SetBinding(TextBlock.VisibilityProperty, bindVisible);
            }

            if (_messageBlock != null)
            {
                Binding bindCollapsed = new Binding();
                bindCollapsed.Source = this;
                bindCollapsed.Path = new PropertyPath("DisplayNotification");
                bindCollapsed.Converter = new VisibilityConverter();
                bindCollapsed.ConverterParameter = true;
                _messageBlock.SetBinding(TextBlock.VisibilityProperty, bindCollapsed);
            }

            if (_backTitleBlock != null)
            {
                Binding bindTitle = new Binding();
                bindTitle.Source = this;
                bindTitle.Path = new PropertyPath("Title");
                bindTitle.Converter = new MultipleToSingleLineStringConverter();
                _backTitleBlock.SetBinding(TextBlock.TextProperty, bindTitle);
            }

            UpdateVisualState();
            this.SizeChanged += HubTile_SizeChanged;
        }

        void HubTile_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (image != null)
            //{
            //    image.Width = 173;
            //    image.Height = 173;
            //}
        }

        void image_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (imageDefault != null)
            {
                imageDefault.Opacity = 0;
            }
        }

        //void image_ImageOpened(object sender, RoutedEventArgs e)
        //{
        //    if (imageDefault != null)
        //    {
        //        imageDefault.Opacity = 0;
        //    }
        //}

        /// <summary>
        /// Initializes a new instance of the HubTile class.
        /// </summary>
        public HubTile()
        {
            DefaultStyleKey = typeof(HubTile);
            Loaded += HubTile_Loaded;
            Unloaded += HubTile_Unloaded;
        }

        /// <summary>
        /// This event handler gets called as soon as a hub tile is added to the visual tree.
        /// A reference of this hub tile is passed on to the service singleton.
        /// </summary>
        /// <param name="sender">The hub tile.</param>
        /// <param name="e">The event information.</param>
        void HubTile_Loaded(object sender, RoutedEventArgs e)
        {
            HubTileService.InitializeReference(this);
        }

        /// <summary>
        /// This event handler gets called as soon as a hub tile is removed from the visual tree.
        /// Any existing reference of this hub tile is eliminated from the service singleton.
        /// </summary>
        /// <param name="sender">The hub tile.</param>
        /// <param name="e">The event information.</param>
        void HubTile_Unloaded(object sender, RoutedEventArgs e)
        {
            HubTileService.FinalizeReference(this);
        }
    }

    /// <summary>
    /// Represents the visual states of a Hub tile.
    /// </summary>
    internal enum ImageState
    {
        /// <summary>
        /// Expanded visual state value.
        /// </summary>
        Expanded = 0,

        /// <summary>
        /// Semiexpanded visual state value.
        /// </summary>
        Semiexpanded = 1,

        /// <summary>
        /// Collapsed visual state value.
        /// </summary>
        Collapsed = 2,

        /// <summary>
        /// Flipped visual state value.
        /// </summary>
        Flipped = 3,
    };
}