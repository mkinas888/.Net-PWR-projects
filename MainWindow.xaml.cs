using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity;

namespace Platformy_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JObject jsonMeme;
        private JObject jsonWeather;
        platformyEntities1 db = new platformyEntities1();
        System.Windows.Data.CollectionViewSource memyViewSource;
        System.Windows.Data.CollectionViewSource pogodaViewSource;
        private bool loaded;

        public MainWindow()
        {
            InitializeComponent();
			memyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("memyViewSource")));
            pogodaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pogodaViewSource")));
			DataContext = this;
            loaded = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			var miastoTextBox = this.FindName("miastoTextBox") as TextBox;
			miastoTextBox.BorderBrush = new SolidColorBrush(Colors.Transparent);
            this.Width = Properties.Settings.Default.Width;
            this.Height = Properties.Settings.Default.Height;
			db.memy.Load();
            db.pogoda.Load();
            memyViewSource.Source = db.memy.Local;
            pogodaViewSource.Source = db.pogoda.Local;
			CreateListHistory();
			CheckWeather(Properties.Settings.Default.InitialCity);
        }

		private void CreateListHistory()
		{
			var myListViewVar = this.FindName("locationList") as ListView;
			foreach (var weatherElem in db.pogoda.Local)
			{
				var item = new ListViewItem();
				var panel = new StackPanel
				{
					Name = "stackPanel" + weatherElem.id,
					Orientation = Orientation.Horizontal,
				};
				var textPanel = new TextBlock
				{
					Name = "textPanel" + weatherElem.id,
					Text = weatherElem.condition + " " + weatherElem.temperatura + "°C" + " " + weatherElem.miasto,
					Width = 215
				};
				var closeButton = new Button
				{
					Name = "closeButton" + weatherElem.id,
					BorderBrush = new SolidColorBrush(Colors.Transparent),
					HorizontalAlignment = HorizontalAlignment.Right,
					Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("https://static.thenounproject.com/png/26894-200.png")) },
					Height = 20,
					Width = 20,
				};
				closeButton.Click += (sender2, e2) => DeleteRecordHandler(sender2, e2, weatherElem.id, item, myListViewVar);
				panel.Children.Add(textPanel);
				panel.Children.Add(closeButton);
				item.Content = panel;
				myListViewVar.Items.Add(item);
			}
		}

		private void DeleteRecordHandler(object sender, EventArgs e, int id, ListViewItem item , ListView list)
		{
			pogoda pg = db.pogoda.Local.First(x => x.id == id);
			db.pogoda.Local.Remove(pg);
			db.SaveChanges();
			list.Items.Remove(item);
		}

        private async void CheckWeather(string city)
        {
            string jsonInStringWeather = await WeatherConnection.DownloadAsJson(city);
            jsonWeather = JObject.Parse(jsonInStringWeather);
            var dataToDatabse = ProcessJson.Weather(jsonWeather);
            AddWeatherToDatabase(dataToDatabse);
            
        }

        private async void DownloadMeme(object sender, RoutedEventArgs e)
        {
            string jsonInStringMeme = await MemeConnection.DownloadJsonAsync();
            jsonMeme = JObject.Parse(jsonInStringMeme);
            var dataToDatabase = ProcessJson.NextMeme(jsonMeme);
            ChangePicture(dataToDatabase["url"]);
            AddMemeToDatabase(dataToDatabase);
            loaded = true;
            MemeChangerAsync();
        }

        private async void MemeChangerAsync()
        {
            if (loaded)
            {
                while (true)
                {
                    Task delay = Task.Delay(TimeSpan.FromSeconds(30));
                    await delay;
                    ChangePicture(ProcessJson.NextMeme(jsonMeme)["url"]);
                }
            }


        }

        private void NextMeme(object sender, RoutedEventArgs e)
        {   if (loaded) ChangePicture(ProcessJson.NextMeme(jsonMeme)["url"]);
        }

        private void ChangePicture(string image)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(image);
            bi3.EndInit();
            //this.Meme.Source = bi3;
        }

        private void AddMemeToDatabase(Dictionary<string,string> data)
        {

            //var query = (from memy in db.memy where memy.meme_url == data["url"] select memy);

            var newEntry = new memy()
            {
                //id = ++i,
                meme_url = data["url"],
                upvotes = int.Parse(data["score"]),
                uzytkownik = data["author"],
                data_top = DateTime.Parse(DateTime.Today.ToShortDateString()),
            };
            db.memy.Local.Add(newEntry);
           
            try
            {
                
                db.SaveChanges();
                memyViewSource.View.Refresh();
                Debug.WriteLine("OK!");
            }
            catch (Exception ex)
            {
                db.memy.Local.Remove(newEntry);
                Debug.WriteLine("Error, id is not unique!: " + ex);
            }
        }

        private void AddWeatherToDatabase(Dictionary<string, string> data)
        {
            switch(data["cod"])
            {
                case "200":
                    {
						var newEntry = new pogoda()
						{
							miasto = data["city"],
							temperatura = (float)Math.Round((double.Parse(data["temp"]) - 273.15), 2),
							data_pomiaru = DateTime.Now,
							pressure = float.Parse(data["pressure"]),
							speed = float.Parse(data["speed"]),
							condition = data["condition"]

						};
                        db.pogoda.Local.Add(newEntry);

                        try
                        {

                            db.SaveChanges();
                            pogodaViewSource.View.MoveCurrentToLast();
							var myListViewVar = this.FindName("locationList") as ListView;
							var item = new ListViewItem();
							var panel = new StackPanel
							{
								Name = "stackPanel" + newEntry.id,
								Orientation = Orientation.Horizontal,
							};
							var textPanel = new TextBlock
							{
								Name = "textPanel" + newEntry.id,
								Text = newEntry.condition + " " + newEntry.temperatura + "°C" + " " + newEntry.miasto,
								Width = 215
							};
							var closeButton = new Button
							{
								Name = "closeButton" + newEntry.id,
								BorderBrush = new SolidColorBrush(Colors.Transparent),
								HorizontalAlignment = HorizontalAlignment.Right,
								Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("https://static.thenounproject.com/png/26894-200.png")) },
								Height = 20,
								Width = 20,
							};
							closeButton.Click += (sender2, e2) => DeleteRecordHandler(sender2, e2, newEntry.id, item, myListViewVar);
							panel.Children.Add(textPanel);
							panel.Children.Add(closeButton);
							item.Content = panel;
							myListViewVar.Items.Add(item);
							Debug.WriteLine("OK!");
                        }
                        catch (Exception ex)
                        {
                            db.pogoda.Local.Remove(newEntry);
                            Debug.WriteLine("Error, id is not unique!: " + ex);
                        }
                        break;
                    }
                case "404":
                    {
                        MessageBox.Show("City not foudn.\n Check spelling.");
                        break;
                    }
                    
                default:
                    {
                        MessageBox.Show("Somthing gone wrong!");
                        Debug.Write(data["cod"]);
                        break;
                    }
            }
            }
            

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.Show();
            
        }

		private void MiastoTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				CheckWeather(miastoTextBox.Text);
				e.Handled = true;
			}
		}
	}
}
