using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary
{
    /// <summary>
    /// Enumeration containing the possible drop behaviours on element removal
    /// </summary>
    public enum RemoveElementDropBehaviour
    {
        /// <summary>
        /// Enumeration option stating the current child should be replaced on drop
        /// </summary>
        Replace,
        /// <summary>
        /// Enumeration option stating a drop should be disallowed when the current child
        /// can't be moved to the position of the parent of the dragsource that's being dropped
        /// </summary>
        Disallow
    }

    /// <summary>
    /// Class defining a droptarget
    /// </summary>
    public class DropTarget : Control, IDisposable
    {


        #region Ghost (DependencyProperty)

        /// <summary>
        /// Ghost control, shown when nothing has been dropped
        /// </summary>
        public FrameworkElement Ghost
        {
            get { return (FrameworkElement)GetValue(GhostProperty); }
            set { SetValue(GhostProperty, value); }
        }
        /// <summary>
        /// Ghost control, shown when nothing has been dropped
        /// </summary>
        public static readonly DependencyProperty GhostProperty =
            DependencyProperty.Register("Ghost", typeof(FrameworkElement), typeof(DropTarget),
              new PropertyMetadata(null));

        #endregion

        #region GhostVisibility (DependencyProperty)

        /// <summary>
        /// Determines wether or not the ghost control should be shown
        /// </summary>
        public Visibility GhostVisibility
        {
            get { return (Visibility)GetValue(ShowGhostProperty); }
            set { SetValue(ShowGhostProperty, value); }
        }
        /// <summary>
        /// Determines wether or not the ghost control should be shown
        /// </summary>
        public static readonly DependencyProperty ShowGhostProperty =
            DependencyProperty.Register("GhostVisibility", typeof(Visibility), typeof(DropTarget),
              new PropertyMetadata(Visibility.Visible));

        #endregion

        #region ShowHover (DependencyProperty)

        /// <summary>
        /// Determines wether or not a hover effect should be visible 
        /// when hovering a dragsource over this droptarget
        /// </summary>
        public bool ShowHover
        {
            get { return (bool)GetValue(ShowHoverProperty); }
            set { SetValue(ShowHoverProperty, value); }
        }
        /// <summary>
        /// Determines wether or not a hover effect should be visible 
        /// when hovering a dragsource over this droptarget
        /// </summary>
        public static readonly DependencyProperty ShowHoverProperty =
            DependencyProperty.Register("ShowHover", typeof(bool), typeof(DropTarget),
              new PropertyMetadata(true));

        #endregion

        #region AllowPositionSave (DependencyProperty)

        /// <summary>
        /// Determines wether or not the position of a droptarget should be saved.  Setting this option
        /// to true results in a significant increase in performance: in regular mode (when this property is
        /// false), droptarget positioning is calculated on the fly while dragging a dragsource.  When this
        /// is set to true, it's only calculated when the droptarget is loaded.  Do keep in mind that setting
        /// this to true will result in strange behaviour if the droptarget hasn't got a fixed position on screen
        /// (eg: when the droptarget itself can be moved around).  
        /// However, you can recalculate this position by hand by calling the "RecalculatePosition"-method on 
        /// the droptarget!.  
        /// This option is advised when you have lots and lots of droptargets on one screen - in other cases,
        /// it shouldn't be necessary.
        /// </summary>
        public bool AllowPositionSave
        {
            get { return (bool)GetValue(AllowPositionSaveProperty); }
            set { SetValue(AllowPositionSaveProperty, value); }
        }
        /// <summary>
        /// Determines wether or not the position of a droptarget should be saved.  Setting this option
        /// to true results in a significant increase in performance: in regular mode (when this property is
        /// false), droptarget positioning is calculated on the fly while dragging a dragsource.  When this
        /// is set to true, it's only calculated when the droptarget is loaded.  Do keep in mind that setting
        /// this to true will result in strange behaviour if the droptarget hasn't got a fixed position on screen
        /// (eg: when the droptarget itself can be moved around).  
        /// However, you can recalculate this position by hand by calling the "RecalculatePosition"-method on 
        /// the droptarget!.  
        /// This option is advised when you have lots and lots of droptargets on one screen - in other cases,
        /// it shouldn't be necessary.
        /// </summary>
        public static readonly DependencyProperty AllowPositionSaveProperty =
            DependencyProperty.Register("AllowPositionSave", typeof(bool), typeof(DropTarget),
              new PropertyMetadata(false));

        #endregion

        #region RemoveElementDropBehaviour (DependencyProperty)

        /// <summary>
        /// RemoveElementDropBehaviour: if you drop an item on a droptarget which already has a
        /// dragsource, and the existing dragsource hasn't got the permission to be
        /// dropped where the item you're dropping was residing (or the item you're 
        /// dropping isn't the child of a droptarget at all), what should happen?  
        /// Replace the existing dragsource (and thus removing it altogether) or 
        /// disallow the drop?
        /// </summary>
        public RemoveElementDropBehaviour RemoveElementDropBehaviour
        {
            get { return (RemoveElementDropBehaviour)GetValue(RemoveElementDropBehaviourProperty); }
            set { SetValue(RemoveElementDropBehaviourProperty, value); }
        }
        /// <summary>
        /// RemoveElementDropBehaviour: if you drop an item on a droptarget which already has a
        /// dragsource, and the existing dragsource hasn't got the permission to be
        /// dropped where the item you're dropping was residing (or the item you're 
        /// dropping isn't the child of a droptarget at all), what should happen?  
        /// Replace the existing dragsource (and thus removing it altogether) or 
        /// disallow the drop?
        /// </summary>
        public static readonly DependencyProperty RemoveElementDropBehaviourProperty =
            DependencyProperty.Register("RemoveElementDropBehaviour", typeof(RemoveElementDropBehaviour), typeof(DropTarget),
              new PropertyMetadata(RemoveElementDropBehaviour.Disallow));

        #endregion

        #region DropBorderBrush (DependencyProperty)

        /// <summary>
        /// The brush for the drop border
        /// </summary>
        public Brush DropBorderBrush
        {
            get { return (Brush)GetValue(DropBorderBrushProperty); }
            set { SetValue(DropBorderBrushProperty, value); }
        }
        /// <summary>
        /// The brush for the drop border
        /// </summary>
        public static readonly DependencyProperty DropBorderBrushProperty =
            DependencyProperty.Register("DropBorderBrush", typeof(Brush), typeof(DropTarget),
            new PropertyMetadata(new SolidColorBrush()));

        #endregion

        #region DropBorderThickness (DependencyProperty)

        /// <summary>
        /// The thickness of the drop border
        /// </summary>
        public Thickness DropBorderThickness
        {
            get { return (Thickness)GetValue(DropBorderThicknessProperty); }
            set { SetValue(DropBorderThicknessProperty, value); }
        }
        /// <summary>
        /// The thickness of the drop border
        /// </summary>
        public static readonly DependencyProperty DropBorderThicknessProperty =
            DependencyProperty.Register("DropBorderThickness", typeof(Thickness), typeof(DropTarget),
              new PropertyMetadata(new Thickness()));

        #endregion

        #region DropBorderCornerRadius (DependencyProperty)

        /// <summary>
        /// The cornerradius of the drop border
        /// </summary>
        public CornerRadius DropBorderCornerRadius
        {
            get { return (CornerRadius)GetValue(DropBorderCornerRadiusProperty); }
            set { SetValue(DropBorderCornerRadiusProperty, value); }
        }
        /// <summary>
        /// The cornerradius of the drop border
        /// </summary>
        public static readonly DependencyProperty DropBorderCornerRadiusProperty =
            DependencyProperty.Register("DropBorderCornerRadius", typeof(CornerRadius), typeof(DropTarget),
              new PropertyMetadata(new CornerRadius()));

        #endregion


        private Grid plstContent = new Grid();

        /// <summary>
        /// Contains the content control for a dragtarget
        /// </summary>
        public DragSource Content
        {
            get
            {

                if (MainContentControl != null)
                {
                    if (MainContentControl.Children.Count > 0)
                    {
                        return (DragSource)MainContentControl.Children[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {

                    if (MainContentControl != null)
                    {
                        MainContentControl.Children.Clear();
                    }
                }
                else
                {
                    if (MainContentControl != null)
                    {
                        MainContentControl.Children.Clear();
                        MainContentControl.Children.Add(value);
                    }
                    else
                    {
                        // temporary save
                        plstContent.Children.Add(value);
                    }

                    if (value.DropTargets == null)
                    {
                        value.DropTargets = new List<DropTarget>();
                        value.DropTargets.Add(this);
                    }
                }
            }
        }


        private Grid MainControlHost;
        private Grid GhostContentControl;
        private Grid MainContentControl;
        private Border BoundingBorder;

        internal bool PositionCalculated = false;

        /// <summary>
        /// Is a drag source hovering this droptarget?
        /// </summary>
        private bool DragSourceIsHovering { get; set; }

        /// <summary>
        /// The drop target entered event
        /// </summary>
        public event DropEventHandler DropTargetEntered;

        /// <summary>
        /// The drop target left event
        /// </summary>
        public event DropEventHandler DropTargetLeft;


        /// <summary>
        /// The drag source dropped event
        /// </summary>
        public event DropEventHandler DragSourceDropped;

        internal event DropEventHandler InternalDragSourceDropped;

        internal Point internalOffset;


        /// <summary>
        /// Constructor
        /// </summary>
        public DropTarget()
        {
            this.DefaultStyleKey = typeof(DropTarget);

            // default values
            DragSourceIsHovering = false;
            PositionCalculated = false;
        }


        /// <summary>
        /// Method used to manually recalculate the droptarget's position.  Only to be 
        /// when AllowPositionSave = true
        /// </summary>
        /// <returns></returns>
        public bool RecalculatePosition()
        {
            try
            {
                internalOffset = this.TransformToVisual(Window.Current.Content as UIElement).TransformPoint(new Point(0, 0));
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        /// <summary>
        /// Method overrides OnApplyTemplate to add handlers / get references to control in the template
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // all our controls are inside of a canvas control.  Because of this, it doesn't 
            // automatically resize.  We need to make sure the parent control is resized properly

            // get the main control host
            MainControlHost = (Grid)this.GetTemplateChild("MainControlHost");
            // get the ghost control host
            GhostContentControl = (Grid)this.GetTemplateChild("GhostContentControl");
            // get the main content host
            MainContentControl = (Grid)this.GetTemplateChild("MainContentControl");

            // add the content
            if (plstContent.Children.Count > 0)
            {
                DragSource tmp = (DragSource)plstContent.Children[0];
                plstContent.Children.Remove(tmp);

                Content = tmp;
                //ResetContent();
            }

            // add the ghost?
            if (GhostVisibility == Visibility.Visible)
            {
                if (Ghost != null)
                {
                    GhostContentControl.Children.Clear();
                    GhostContentControl.Children.Add(Ghost);
                }
            }

            // get bounding border for hover-effects
            BoundingBorder = (Border)this.GetTemplateChild("BoundingBorder");

            // add handler for droptargetentered
            DropTargetEntered += new DropEventHandler(DropTargetBase_DropTargetEntered);

            // add handler for droptargetleft
            DropTargetLeft += new DropEventHandler(DropTargetBase_DropTargetLeft);

            // add handler for dragsourcedropped
            //DragSourceDropped += new DropEventHandler(DropTargetBase_DragSourceDropped);

            InternalDragSourceDropped += new DropEventHandler(DropTargetBase_InternalDragSourceDropped);

            if (AllowPositionSave)
            {
                (Window.Current.Content as FrameworkElement).SizeChanged += new SizeChangedEventHandler(DropTarget_SizeChanged);
            }

        }

        void DropTarget_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionCalculated = false;
        }


        /// <summary>
        /// Resets the content of the droptarget to the "Content"-dragsource
        /// </summary>
        private void ResetContent()
        {
            if (plstContent != null)
            {
                if (MainContentControl != null)
                {
                    MainContentControl.Children.Clear();

                    if (plstContent.Children.Count > 0)
                        MainContentControl.Children.Add(plstContent.Children[0]);
                }

                if (plstContent.Children.Count > 0)
                {
                    if (((DragSource)plstContent.Children[0]).DropTargets == null)
                    {
                        ((DragSource)plstContent.Children[0]).DropTargets = new System.Collections.Generic.List<DropTarget>();
                        ((DragSource)plstContent.Children[0]).DropTargets.Add(this);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the dropping of a dragsource in this droptarget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void DropTargetBase_InternalDragSourceDropped(object sender, DropEventArgs args)
        {
            if (ShowHover)
            {
                // after this, start the hover-out animation on the droptarget
                Animation.CreateDropTargetHoverOut(BoundingBorder).Begin();
            }

            // what if there are children? 
            if (MainContentControl.Children.Count > 0)
            {
                // get the current child (which is a dragsource by definition) 
                // and either switch it with the new child (if the parent of the new child
                // is a valid droptarget for the current child - so it must have rights to 
                // be used as a droptarget to be able to make the switch!) or replace it

                DragSource currentChild = (DragSource)MainContentControl.Children[0];

                // if currentchild <> child you're dragging (else, we're just dropping our
                // dragsource onto its own parent (droptarget)
                if (currentChild != args.DragSource)
                {
                    // is the new childs' parent (parent of parent of parent) a droptarget?
                    Panel firstParent = (Panel)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(args.DragSource));

                    if (firstParent != null)
                    {
                        // droptarget?
                        if (VisualTreeHelper.GetParent(firstParent) is DropTarget)
                        {
                            DropTarget newChildParentDropTarget = (DropTarget)VisualTreeHelper.GetParent(firstParent);
                            if (currentChild.DropTargets.Contains(newChildParentDropTarget)
                                || currentChild.AllDropTargetsValid == true) // check for valid droptarget, or check if all droptargets are valid
                            {


                                // point needed for animation of the current child
                                Point from = new Point();
                                from = args.DragSource.getCurrentPosition();

                                Point offsetCurrentChild = new Point();
                                if (currentChild.ShowSwitchReplaceAnimation)
                                {
                                    offsetCurrentChild = currentChild.MainDraggableControl.TransformToVisual(InitialValues.ContainingLayoutPanel).TransformPoint(new Point(0, 0));
                                }

                                Point oriOffset = new Point(args.DragSource.OriginalOffset.X, args.DragSource.OriginalOffset.Y);


                                // reset position of dragsource, so control is on top of ghost, right
                                // before actually moving it.
                                args.DragSource.ResetMyPosition();

                                // remove from current parent
                                ((Panel)VisualTreeHelper.GetParent(args.DragSource)).Children.Remove(args.DragSource);

                                MainContentControl.Children.Clear();
                                MainContentControl.Children.Add(args.DragSource);



                                // move the current child, with or without an animation
                                if (currentChild.ShowSwitchReplaceAnimation)
                                {
                                    // animation, from the current position to the new position
                                    // current position = where the new child is now
                                    // new position = where the new child was

                                    // add the current child to its new position, then move it from the
                                    // "current position" to 0, 0 (the new position)
                                    newChildParentDropTarget.MainContentControl.Children.Add(currentChild);

                                    //currentChild.AnimateOnSwitch(from);

                                    Storyboard sb = currentChild.ReturnAnimateOnSwitch(offsetCurrentChild, oriOffset);

                                    EventHandler<object> handler = null;
                                    handler = (send, arg) =>
                                    {
                                        sb.Completed -= handler;
                                        currentChild.ResetMyPosition();
                                        // trigger external dragsourcedropped-event
                                        TriggerDragSourceDropped(args.DragSource);
                                    };
                                    sb.Completed += handler;
                                    sb.Begin();


                                }
                                else
                                {
                                    // no animation
                                    newChildParentDropTarget.MainContentControl.Children.Add(currentChild);

                                    // trigger external dragsourcedropped-event
                                    TriggerDragSourceDropped(args.DragSource);
                                }

                            }
                            else
                            {
                                // parent of the new child isn't a VALID droptarget.  Depending on DropBehaviour, remove 
                                // current child & set new one (replace-behaviour) or return the new child to 
                                // its original position (disallow-behaviour)

                                if (RemoveElementDropBehaviour == RemoveElementDropBehaviour.Replace)
                                {
                                    // reset position of dragsource, so control is on top of ghost, right
                                    // before actually moving it.
                                    args.DragSource.ResetMyPosition();

                                    // remove from current parent
                                    ((Panel)VisualTreeHelper.GetParent(args.DragSource)).Children.Remove(args.DragSource);

                                    MainContentControl.Children.Clear();
                                    MainContentControl.Children.Add(args.DragSource);

                                    // trigger external dragsourcedropped-event
                                    TriggerDragSourceDropped(args.DragSource);
                                }
                                else
                                {
                                    // drop is disallowed, return dragsource to original position

                                    args.DragSource.ReturnToOriginalPosition();

                                    // trigger external dragsourcedropped-event
                                    TriggerDragSourceDropped(args.DragSource);
                                }
                            }
                        }
                        else
                        {
                            // parent of the new child isn't a droptarget.  Depending on DropBehaviour, remove 
                            // current child & set new one (replace-behaviour) or return the new child to 
                            // its original position (disallow-behaviour)

                            if (RemoveElementDropBehaviour == RemoveElementDropBehaviour.Replace)
                            {
                                // reset position of dragsource, so control is on top of ghost, right
                                // before actually moving it.
                                args.DragSource.ResetMyPosition();

                                // remove from current parent
                                ((Panel)VisualTreeHelper.GetParent(args.DragSource)).Children.Remove(args.DragSource);

                                MainContentControl.Children.Clear();
                                MainContentControl.Children.Add(args.DragSource);

                                // trigger external dragsourcedropped-event
                                TriggerDragSourceDropped(args.DragSource);
                            }
                            else
                            {
                                // drop is disallowed, return dragsource to original position

                                args.DragSource.ReturnToOriginalPosition();

                                // trigger external dragsourcedropped-event
                                TriggerDragSourceDropped(args.DragSource);
                            }
                        }
                    }
                    else
                    {
                        // reset position of dragsource, so control is on top of ghost, right
                        // before actually moving it.
                        args.DragSource.ResetMyPosition();

                        // remove from current parent
                        ((Panel)VisualTreeHelper.GetParent(args.DragSource)).Children.Remove(args.DragSource);

                        MainContentControl.Children.Clear();
                        MainContentControl.Children.Add(args.DragSource);

                        // trigger external dragsourcedropped-event
                        TriggerDragSourceDropped(args.DragSource);

                    }
                }
                else
                {
                    // reset position of dragsource, so control is on top of ghost, right
                    // before actually moving it.
                    args.DragSource.ResetMyPosition();

                    // trigger external dragsourcedropped-event
                    TriggerDragSourceDropped(args.DragSource);
                }

            }
            else
            {
                // reset position of dragsource, so control is on top of ghost, right
                // before actually moving it.
                args.DragSource.ResetMyPosition();

                // remove from current parent
                ((Panel)VisualTreeHelper.GetParent(args.DragSource)).Children.Remove(args.DragSource);

                MainContentControl.Children.Clear();
                MainContentControl.Children.Add(args.DragSource);

                ((DropTarget)VisualTreeHelper.GetParent((Panel)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(args.DragSource)))).Content = null;
                this.Content = args.DragSource;

                // trigger external dragsourcedropped-event
                TriggerDragSourceDropped(args.DragSource);

            }



        }


        void DropTargetBase_DropTargetLeft(object sender, DropEventArgs args)
        {
            if (DragSourceIsHovering)
            {
                // set value of boolean
                DragSourceIsHovering = false;

                if (ShowHover)
                {
                    // hide control
                    Animation.CreateDropTargetHoverOut(BoundingBorder).Begin();
                }
            }
        }

        void DropTargetBase_DropTargetEntered(object sender, DropEventArgs args)
        {
            if (!DragSourceIsHovering)
            {
                // set value of boolean
                DragSourceIsHovering = true;

                if (ShowHover)
                {
                    // show control
                    Animation.CreateDropTargetHoverIn(BoundingBorder).Begin();
                }
            }
        }

        internal void TriggerDropTargetEntered(DragSource source)
        {
            // Fire the drop target entered event
            if (DropTargetEntered != null)
            {
                this.DropTargetEntered(this, new DropEventArgs(source));
            }
        }

        internal void RemoveBorder()
        {
            // procedure is only used when double-checking the removal of all borders, just in case.
            if (DragSourceIsHovering)
            {
                DragSourceIsHovering = false;
                Animation.CreateDropTargetHoverOutFromAnyAnimationPosition(BoundingBorder).Begin();
            }
        }

        internal void TriggerDragSourceDropped(DragSource source)
        {
            // double-check: remove all borders

            source.RemoveAllDropBorders();

            // Fire the drop target dropped event
            if (DragSourceDropped != null)
            {
                this.DragSourceDropped(this, new DropEventArgs(source));
            }
        }

        internal void TriggerDropTargetLeft(DragSource source)
        {
            // Fire the drop target left event
            if (DropTargetLeft != null)
            {
                this.DropTargetLeft(this, new DropEventArgs(source));
            }
        }

        internal void TriggerInternalDragSourceDropped(DragSource source)
        {
            // Fire the drop target entered event
            if (InternalDragSourceDropped != null)
            {
                this.InternalDragSourceDropped(this, new DropEventArgs(source));
            }
        }

        #region IDisposable Members

        // reference IDisposable Pattern: http://msdn.microsoft.com/en-us/magazine/cc163392.aspx

        /// <summary>
        /// Destructor.
        /// </summary>
        ~DropTarget()
        {
            Dispose(false);
        }

        /// <summary>
        /// DPerforms application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                MainControlHost = null;
                GhostContentControl = null;
                MainContentControl = null;
                BoundingBorder = null;

                // remove handlers
                DropTargetEntered -= new DropEventHandler(DropTargetBase_DropTargetEntered);
                DropTargetLeft -= new DropEventHandler(DropTargetBase_DropTargetLeft);
                InternalDragSourceDropped -= new DropEventHandler(DropTargetBase_InternalDragSourceDropped);

                //if (AllowPositionSave)
                //{
                (Window.Current.Content as FrameworkElement).SizeChanged -= new SizeChangedEventHandler(DropTarget_SizeChanged);
                //}

            }

            // Clean up all native resources
        }
        #endregion
    }
}
