using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChapterSeven
{


	public class CategoryList
	{
		public string id { get; set; }
		public string name { get; set; }
	}

	public class Location
	{
		public string street { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public string zip { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string located { get; set; }
	}

	public class Datum
	{
		public string category { get; set; }
		public List<CategoryList> category_list { get; set; }
		public Location location { get; set; }
		public string name { get; set; }
		public string id { get; set; }
	}

	public class Paging
	{
		public string next { get; set; }
	}

	public class RootObject
	{
		public List<Datum> data { get; set; }
		public Paging paging { get; set; }
	}

	public class ListContent
	{
		public string Name { get; set; }
		public string Locations { get; set; }
		public string Category { get; set; }
		public string ID { get; set; }
	}



	public class App : Application
	{

		Label ResultEditText = new Label();
		Label ResultLabel = new Label();
		Label wawa = new Label();
		Entry lll = new Entry();
		// xxx;
		ListView list = new ListView()
		{
			ItemTemplate = new DataTemplate(() =>
			{
				var textCell = new TextCell();
				textCell.SetBinding(TextCell.TextProperty, "Name");
				textCell.SetBinding(TextCell.DetailProperty, "Category");
				return textCell;
			})
		};
		WebView web = new WebView()
		{

			Source = new UrlWebViewSource {Url= "http://www.facebook.com" },
			HeightRequest = 1000,
			WidthRequest = 1000
		};

		
		Button xa = new Button();
		ObservableCollection<ListContent> item = new ObservableCollection<ListContent>();


		public async Task<string> DownloadHomepage(string q)
		{
			try
			{
				string url = "https://graph.facebook.com/search?q=" + q + "&type=place&access_token=296505937365008|QpUUBZQ5215gYnTvDgdwK4r3OI0";

				var httpClient = new HttpClient(); // Xamarin supports HttpClient!

				Task<string> contentsTask = httpClient.GetStringAsync(url); // async method!

				// await! control returns to the caller and the task continues to run on another thread
				string contents = await contentsTask;

				ResultEditText.Text += "DownloadHomepage method continues after async call. . . . .\n";

				// After contentTask completes, you can calculate the length of the string.
				//int exampleInt = contents.Length;

				ResultEditText.Text += "Downloaded the html and found out the length.\n\n\n";

				ResultEditText.Text += contents; // just dump the entire HTML

				return contents; }
			catch
			{
				return "Null";
			}// Task<TResult> returns an object of type TResult, in this case int
		}

		//async void HandleTouchUpInside(object sender, EventArgs e)
		//{
		//	ResultLabel.Text = "loading...";
		//	lll.Text = "loading...\n";

		//	// await! control returns to the caller
		//	var intResult = await DownloadHomepage(e);

		//	// when the Task<int> returns, the value is available and we can display on the UI
		//	ResultLabel.Text = "Length: " + intResult;
		//}

		async void watda(object sender, TextChangedEventArgs e)
		{
			
			ResultLabel.Text = "loading...";

			// await! control returns to the caller
			var Result = await DownloadHomepage(e.NewTextValue);

			// when the Task<int> returns, the value is available and we can display on the UI
			//ResultLabel.Text = "Result: " + Result;

			if (Result == "Null")
			{
				item.Clear();
			}
			else
			{
				
				List<RootObject> roles = new List<RootObject>();

				JsonTextReader reader = new JsonTextReader(new StringReader(Result));
				reader.SupportMultipleContent = true;

				while (true)
				{
					if (!reader.Read())
					{
						break;
					}

					JsonSerializer serializer = new JsonSerializer();
					RootObject role = serializer.Deserialize<RootObject>(reader);

					roles.Add(role);

				}
				item.Clear();
				foreach (RootObject role in roles)
				{
					
					for (int i = 0; i < role.data.Count; i++)
					{
						item.Add(new ListContent() { Name = role.data[i].name, Category = role.data[i].category, Locations = role.data[i].location.city + "," + role.data[i].location.state, ID = role.data[i].id});
						list.ItemsSource =  item;
						list.BindingContext = item;
					}

				}
			}

		}

		public App()
		{

			xa.Text = "ambot";

			// The root page of your application
			var content = new ContentPage
			{
				Title ="Title",
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = 
					{
						new ScrollView{
							Content=new StackLayout{
								Children={
									lll,wawa,list,xa,web,
								}
							}
						}
					}
				}
			};

			lll.TextChanged += watda;
			CarouselPage main = new CarouselPage
			{
				Children = { content,new MyPage(),new Second()}

			};
			//xa.Clicked += HandleTouchUpInside;
			list.ItemTapped += async (sender, e) =>
			{
				ListContent items = (ListContent)e.Item;
				await content.DisplayAlert("Tapped", items.Name + " was selected.", "OK"); 
				((ListView)sender).SelectedItem = null;

				//web.Source = new UrlWebViewSource { Url="http://wwww.facebook.com/" + items.ID};
				if (Device.OS != TargetPlatform.WinPhone)
				{
					Device.OpenUri(new Uri("http://wwww.facebook.com/" + items.ID));
				}

			};
			MainPage = new NavigationPage(main);
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

