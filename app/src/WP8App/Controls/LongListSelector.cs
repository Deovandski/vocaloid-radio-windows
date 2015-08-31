using WPAppStudio.ViewModel.Base;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WPAppStudio.Controls
{
    public class LongListSelector : Microsoft.Phone.Controls.LongListSelector
    {
        private const int OffsetKnob = 10;
        private int _pageNumber = 0;

        public LongListSelector()
        {
            SelectionChanged += LongListSelector_SelectionChanged;
            ItemRealized += LongListSelector_ItemRealized;     
        }

        void LongListSelector_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var selector = (LongListSelector)sender;
            var viewModel = DataContext;
            if (PagingEnabled && viewModel != null && RefreshCommand!=null &&
			(selector.ItemsSource != null && selector.ItemsSource.Count >= OffsetKnob))
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    object element = e.Container.Content;
                    if (element != null &&
                        element.Equals(
                            selector.ItemsSource[selector.ItemsSource.Count - OffsetKnob]))
                    {
                        ((DelegateCommand<int>)RefreshCommand).Execute(++_pageNumber);
                    }
                }
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItem = base.SelectedItem;
        }

        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public object RefreshCommand
        {
            get { return GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }		
		
        public bool PagingEnabled
        {
            get { return (bool)GetValue(PagingEnabledProperty); }
            set { SetValue(PagingEnabledProperty, value); }
        }

		public int PageNumber
        {
            get { return (int)GetValue(PageNumberProperty); }
            set{SetValue(PageNumberProperty, value);}
        }
		
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof(object),
                typeof(LongListSelector),
                new PropertyMetadata(null, OnSelectedItemChanged)
            );

        public static readonly DependencyProperty RefreshCommandProperty =
            DependencyProperty.Register(
                "RefreshCommand",
                typeof(object),
                typeof(LongListSelector),
                new PropertyMetadata(null, OnRefreshCommandProperty)
            );
			
        public static readonly DependencyProperty PagingEnabledProperty =
            DependencyProperty.Register(
                "PagingEnabled",
                typeof(bool),
                typeof(LongListSelector),
                new PropertyMetadata(false)
            );

		public static readonly DependencyProperty PageNumberProperty =
            DependencyProperty.Register(
                "PageNumber",
                typeof(int),
                typeof(LongListSelector),
                new PropertyMetadata(0, OnPageNumberChanged)
            );
			
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SetSelectedItem(e);
        }

        private static void OnRefreshCommandProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SetRefreshCommand(e);
        }

		private static void OnPageNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (LongListSelector)d;
            selector.SetPageNumber(e);
        }
		
        private void SetSelectedItem(DependencyPropertyChangedEventArgs e)
        {
            base.SelectedItem = e.NewValue;
        }

        private void SetRefreshCommand(DependencyPropertyChangedEventArgs e)
        {
            RefreshCommand = e.NewValue;
        }
		
		private void SetPageNumber(DependencyPropertyChangedEventArgs e)
        {
            _pageNumber = Convert.ToInt32(e.NewValue);
        }
    }
}
