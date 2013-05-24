using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using ControlLibrary.Extensions;

namespace ControlLibrary
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(PickerSelectorItem))]
    public class PickerSelector : ItemsControl
    {  
        // Type of listbox (Day, Month, Year)
        public DataSourceType DataSourceType { get; set; }
        public YearDataSource YearDataSource { get; set; }
        public MonthDataSource MonthDataSource { get; set; }
        public DayDataSource DayDataSource { get; set; }
        public Rect RectPosition { get; set; }

        // Parent date picker 
        internal DatePicker DatePicker { get; set; }

        // Items Panel
        private LoopItemsPickerPanel itemsPanel;

        /// <summary>
        /// If Tapped on selected item, prevent animation and visual states
        /// </summary>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {

            if (!this.IsFocused && DatePicker.EnableFirstTapHasFocusOnly)
                return;

            FrameworkElement fxElement = e.OriginalSource as FrameworkElement;

            if (fxElement == null) return;

            var dtw = fxElement.DataContext as DateTimeWrapper;

            if (dtw == null) return;

            if (this.SelectedItem != null && this.SelectedItem.DateTime == dtw.DateTime)
                return;

            this.SelectedItem = dtw;
        }

        public Boolean IsFocused
        {
            get { return (Boolean)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for isFocused.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register("IsFocused", typeof(Boolean), typeof(PickerSelector), new PropertyMetadata(false, OnIsFocusedChanged));

        /// <summary>
        /// When focus is set, just set IsFocused on the itemsPanel
        /// </summary>
        private static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || (e.NewValue == e.OldValue))
                return;

            PickerSelector pickerSelector = (PickerSelector)d;

            bool isFocused = (bool)e.NewValue;

            pickerSelector.UpdateIsFocusedItems(isFocused);

        }


        /// <summary>
        /// Ctor. Register for first LayoutUpdate to set initial DateTime
        /// </summary>
        public PickerSelector()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            // Set style
            this.DefaultStyleKey = typeof(PickerSelector);

        }

        private DateTimeWrapper selectedItem;
        /// <summary>
        /// Current DateTime selected
        /// </summary>
        public DateTimeWrapper SelectedItem
        {
            get
            {
                return selectedItem;

            }
            set
            {
                selectedItem = value;
                // update the IsSelected items state
                this.UpdateIsSelectedItems(value);
            }
        }


        /// <summary>
        /// Update is focused or not on items state
        /// </summary>
        private void UpdateIsFocusedItems(bool isFocused)
        {
            if (this.Items == null || this.Items.Count <= 0)
                return;

            if (this.itemsPanel == null)
                return;

            foreach (PickerSelectorItem child in this.itemsPanel.Children)
                child.IsFocused = isFocused;

        }

        /// <summary>
        /// Update selected or not selected items state
        /// </summary>
        private void UpdateIsSelectedItems(DateTimeWrapper selectedValue)
        {
            if (this.Items == null || this.Items.Count <= 0)
                return;

            if (this.itemsPanel == null)
                return;

            if (selectedValue == null)
                return;

            foreach (PickerSelectorItem pickerSelectorItem in this.itemsPanel.Children)
            {
                DateTimeWrapper currentValue = (DateTimeWrapper)pickerSelectorItem.DataContext;
                pickerSelectorItem.IsSelected = selectedValue.DateTime == currentValue.DateTime;
                if (pickerSelectorItem.IsSelected)
                    selectedPickerSelectorItem = pickerSelectorItem;
            }
        }

        private PickerSelectorItem selectedPickerSelectorItem;

        internal PickerSelectorItem GetSelectedPickerSelectorItem()
        {
            return selectedPickerSelectorItem;
        }

        /// <summary>
        /// Overridden. Creates or identifies the element that is used to display the given item.
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new PickerSelectorItem { PickerSelector = this };
        }

     
        /// <summary>
        /// Return ItemsPanel
        /// </summary>
        internal LoopItemsPickerPanel GetItemsPanel()
        {
            return this.itemsPanel;
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (DesignMode.DesignModeEnabled)
                return;

            itemsPanel = this.GetVisualDescendent<LoopItemsPickerPanel>();

            // get the item
            PickerSelectorItem loopListItem = element as PickerSelectorItem;
            DateTimeWrapper dateTimeWrapper = item as DateTimeWrapper;

            if (loopListItem == null || dateTimeWrapper == null)
                return;

            if (this.ItemTemplate == null) return;

            // load data templated
            var contentElement = this.ItemTemplate.LoadContent() as FrameworkElement;

            if (contentElement == null)
                return;
            // attach DataContext and Context to the item
            loopListItem.Style = ItemContainerStyle;
            loopListItem.DataContext = item;
            loopListItem.Content = contentElement;
            loopListItem.IsSelected = dateTimeWrapper == this.SelectedItem;
            loopListItem.IsFocused = this.IsFocused;

        }

        /// <summary>
        /// Maybe this method is Obsolet : TODO : Test obsoletence
        /// </summary>
        /// <param name="e"></param>
        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {

            PickerSelectorItem middleItem = this.itemsPanel.GetMiddleItem();

            if (middleItem == null) return;

            this.SelectedItem = middleItem.DataContext as DateTimeWrapper;

            base.OnManipulationCompleted(e);
        }

        /// <summary>
        /// Check item type
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is PickerSelectorItem;

        }


        /// <summary>
        /// Get the first available date and the max (28, 29 30 or 31 for Day per example)
        /// </summary>
        private DateTime GetFirstAvailable(DateTime dateTime, out int newMax)
        {
            DateTime firstAvailableDate = dateTime;
            newMax = 0;
            switch (this.DataSourceType)
            {
                case DataSourceType.Year:
                    firstAvailableDate = this.YearDataSource.GetFirstAvailableDate(dateTime);
                    newMax = this.YearDataSource.GetNumberOfItems(dateTime);
                    break;
                case DataSourceType.Month:
                    firstAvailableDate = this.MonthDataSource.GetFirstAvailableDate(dateTime);
                    newMax = this.MonthDataSource.GetNumberOfItems(dateTime);
                    break;
                case DataSourceType.Day:
                    firstAvailableDate = this.DayDataSource.GetFirstAvailableDate(dateTime);
                    newMax = this.DayDataSource.GetNumberOfItems(dateTime);
                    break;
            }

            return firstAvailableDate;
        }

        /// <summary>
        /// Update Items. Used for days
        /// </summary>
        internal void CreateOrUpdateItems(DateTime dateTime)
        {
            if (this.Items == null)
                return;

            DateTimeWrapper selectedDateTimeWrapper = null;
            int newMax;
            DateTime firstAvailableDate = GetFirstAvailable(dateTime, out newMax);

            // Make a copy without any minutes / seconds ...
            DateTimeWrapper newData =
                new DateTimeWrapper(new DateTime(firstAvailableDate.Year,
                                                 firstAvailableDate.Month,
                                                 firstAvailableDate.Day));

            // One item is deleted but was selected..
            // Don't forget to reactivate a selected item
            Boolean oneItemMustBeDeletedAndIsSelected = false;

            // If new month have less day than last month
            if (newMax < this.Items.Count)
            {
                int numberOfLastDaysToDelete = this.Items.Count - newMax;
                for (int cpt = 0; cpt < numberOfLastDaysToDelete; cpt++)
                {
                    PickerSelectorItem item =
                       this.ItemContainerGenerator.ContainerFromItem(this.Items[this.Items.Count - 1]) as PickerSelectorItem;

                    if (item == null)
                        continue;

                    if (item.IsSelected)
                        oneItemMustBeDeletedAndIsSelected = true;

                    this.Items.RemoveAt(this.Items.Count - 1);

                }
            }


            for (int i = 0; i < newMax; i++)
            {

                // -----------------------------------------------------------------------------
                // Add or Update Items
                // -----------------------------------------------------------------------------
                if (this.Items.Count <= i)
                {
                    this.Items.Add(newData);
                }
                else
                {
                    // Verify the item already exists
                    var itemDate = ((DateTimeWrapper)this.Items[i]).DateTime;

                    if (itemDate != newData.DateTime)
                        this.Items[i] = newData;
                }

                // -----------------------------------------------------------------------------
                // Get the good selected itm
                // -----------------------------------------------------------------------------
                if (newData.DateTime == dateTime)
                    selectedDateTimeWrapper = newData;

                // -----------------------------------------------------------------------------
                // Get the next data, relative to original wrapper, then relative to firstWrapper
                // -----------------------------------------------------------------------------
                DateTime? nextData = null;

                // Get nex date
                switch (this.DataSourceType)
                {
                    case DataSourceType.Year:
                        nextData = this.YearDataSource.GetNext(dateTime, firstAvailableDate, i + 1);
                        break;
                    case DataSourceType.Month:
                        nextData = this.MonthDataSource.GetNext(dateTime, firstAvailableDate, i + 1);
                        break;
                    case DataSourceType.Day:
                        nextData = this.DayDataSource.GetNext(dateTime, firstAvailableDate, i + 1);
                        break;
                }
                if (nextData == null)
                    break;

                newData = nextData.Value.ToDateTimeWrapper();
            }

            // Set the correct Selected Item
            if (selectedDateTimeWrapper != null)
                this.SelectedItem = selectedDateTimeWrapper;
            else if (oneItemMustBeDeletedAndIsSelected) // When 31 was selected and we are on a Month < 31 days (February, April ...)
                this.SelectedItem = (DateTimeWrapper)this.Items[this.Items.Count - 1];
            else
                this.SelectedItem = (DateTimeWrapper)this.Items[0];
        }

        //private void DebugWriteLine(string text)
        //{
        //    string datasource = null;
        //    switch (this.DataSourceType)
        //    {
        //        case DataSourceType.Day:
        //            datasource = "Day : ";
        //            break;
        //        case DataSourceType.Month:
        //            datasource = "Month : ";
        //            break;
        //        case DataSourceType.Year:
        //            datasource = "Year : ";
        //            break;
        //    }

        //    Debug.WriteLine(datasource + text);
        //}

    }
}
