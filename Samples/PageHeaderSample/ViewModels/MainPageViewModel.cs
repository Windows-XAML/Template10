using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.PageHeaderSample.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
	{
		public MainPageViewModel()
		{
			ResetItems();	
		}

		public void ResetItems()
		{
			var items = new ObservableCollection<string>();
			for(int i=0; i<1000; i++)
			{
				items.Add("Item " + i);
			}

			Items = items;
		}

		#region Items property

		private ObservableCollection<string> _Items;

		public ObservableCollection<string> Items
		{
			get
			{
				return _Items;
			}
			set
			{
				Set(ref _Items, value);
			}
		}

		#endregion

		#region SelectionMode property

		private ListViewSelectionMode _SelectionMode = ListViewSelectionMode.None;

		public ListViewSelectionMode SelectionMode
		{
			get
			{
				return _SelectionMode;
			}
			set
			{
				Set(ref _SelectionMode, value);
			}
		}

		#endregion

		#region PageHeaderVisibility property

		private Visibility _PageHeaderVisibility = Visibility.Visible;

		public Visibility PageHeaderVisibility
		{
			get
			{
				return _PageHeaderVisibility;
			}
			set
			{
				if (Set(ref _PageHeaderVisibility, value))
					RaisePropertyChanged(nameof(SelectionPageHeaderVisibility));
			}
		}

		#endregion

		#region SelectionPageHeaderVisibility property

		public Visibility SelectionPageHeaderVisibility
		{
			get
			{
				if (PageHeaderVisibility == Visibility.Collapsed)
					return Visibility.Visible;

				return Visibility.Collapsed;
			}
		}

		#endregion

		#region SelectionCount property

		private int _SelectionCount;

		public int SelectionCount
		{
			get
			{
				return _SelectionCount;
			}
			set
			{
				if (Set(ref _SelectionCount, value))
				{
					RaisePropertyChanged(nameof(SelectionText));
					RaisePropertyChanged(nameof(DeleteIsEnabled));
				}
			}
		}

		#endregion

		#region SelectionText property

		public string SelectionText
		{
			get
			{
				if (SelectionCount == 0)
					return "No items selected";
				if (SelectionCount == 1)
					return "1 item selected";

				return $"{SelectionCount} items selected";
			}
		}

		#endregion

		#region DeleteIsEnabled property

		public bool DeleteIsEnabled => SelectionCount > 0;

		#endregion

		internal void DeleteSelectedItems(IList<object> selectedItems)
		{
			foreach (string obj in selectedItems.ToArray())
				Items.Remove(obj);

			StopSelectionMode();
		}

		public void StartSelectionMode()
		{
			PageHeaderVisibility = Visibility.Collapsed;
			SelectionMode = ListViewSelectionMode.Multiple;
		}
		
		public void StopSelectionMode()
		{
			PageHeaderVisibility = Visibility.Visible;
			SelectionMode = ListViewSelectionMode.None;
		}
	}
}

