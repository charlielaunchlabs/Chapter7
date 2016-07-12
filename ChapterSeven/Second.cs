using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ChapterSeven
{

	public class TitleViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		String title;
		public string Title
		{
			set
			{
				if (!value.Equals(title, StringComparison.Ordinal))
				{
					title = value;
					OnPropertyChanged("Title");
				}
			}
			get
			{
				return title;
			}
		}
		void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}


	public class Second : ContentPage
	{
		public Second()
		{
			var titleViewModel = new TitleViewModel();
			titleViewModel.Title = "First";
			this.BindingContext = titleViewModel;
			var titleEntry = new Entry()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			titleEntry.SetBinding(Entry.TextProperty, new Binding("Title"));
			Button buttonDisplay = new Button
			{
				Text = "Display Item Value",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Fill
			};
			buttonDisplay.Clicked += async (sender, args) =>
			{
				await DisplayAlert("Item Object", "Title property:" + titleViewModel.Title.
								   ToString(), "OK");
			};
			Button buttonUpdate = new Button
			{
				Text = "Update the Data Model",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Fill
			};
			buttonUpdate.Clicked += async (sender, args) =>
			{
				titleViewModel.Title = "Data Model Updated";
				await DisplayAlert("Item Object", "Title property:" + titleViewModel.Title.
								   ToString(), "OK");
			};
			Content = new StackLayout
			{
				Children = { titleEntry, buttonDisplay, buttonUpdate }
			};
		}



		}
	}



