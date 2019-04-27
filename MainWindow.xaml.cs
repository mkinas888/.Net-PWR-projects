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
        platformyEntities1 db = new platformyEntities1();
        System.Windows.Data.CollectionViewSource memyViewSource;
        private bool loaded;

        public MainWindow()
        {
            InitializeComponent();
            memyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("memyViewSource")));
            DataContext = this;
            loaded = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = Properties.Settings.Default.Width;
            this.Height= Properties.Settings.Default.Height;
            db.memy.Load();
            memyViewSource.Source = db.memy.Local;
        }


        private async void DownloadMeme(object sender, RoutedEventArgs e)
        {
            string jsonInString = await MemeConnection.DownloadJsonAsync();
            jsonMeme = JObject.Parse(jsonInString);
            var dataToDatabase = ProcessJson.NextMeme(jsonMeme);
            ChangePicture(dataToDatabase["url"]);
            AddToDatabase(dataToDatabase);
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
            this.Meme.Source = bi3;
        }

        private void AddToDatabase(Dictionary<string,string> data)
        {
            var newEntry = new memy()
            {
                //id = ++i,
                meme_url = data["url"],
                upvotes = int.Parse(data["score"]),
                uzytkownik = data["author"],
                data_top = DateTime.Now
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow win = new SettingsWindow();
            win.Show();
            
        }
    }
}
