using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChapterSeven
{

	public class Role
	{
		public string category { get; set; }
	}

	public class App : Application
	{



		Label ResultEditText = new Label();
		Label ResultLabel = new Label();
		Label wawa = new Label();
		Entry lll = new Entry();
		string xxx;

		Button xa = new Button();


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
			var intResult = await DownloadHomepage(e.NewTextValue);

			// when the Task<int> returns, the value is available and we can display on the UI
			ResultLabel.Text = "Result: " + intResult;

			if (intResult == "Null")
			{
				
			}
			else
			{
				List<Role> roles = new List<Role>();

				JsonTextReader reader = new JsonTextReader(new StringReader(intResult));
				reader.SupportMultipleContent = true;

				while (true)
				{
					if (!reader.Read())
					{
						break;
					}

					JsonSerializer serializer = new JsonSerializer();
					Role role = serializer.Deserialize<Role>(reader);

					roles.Add(role);
				}

				foreach (Role role in roles)
				{
					 xxx = role.category + " ";
				}
			}

		}

		public App()
		{


			xa.Text = xxx;

			// The root page of your application
			var content = new ContentPage
			{
				Title = xxx,
				Content = new StackLayout
				{
					VerticalOptions = LayoutOptions.Center,
					Children = {lll,wawa,
						new ScrollView{Content=new StackLayout{BackgroundColor=Color.Lime,Children ={ResultLabel,ResultEditText}}
						},xa
					}
				}
			};

			lll.TextChanged += watda;
			CarouselPage main = new CarouselPage
			{
				Children = { content,new MyPage(),new Second()}

			};
			//xa.Clicked += HandleTouchUpInside;

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

