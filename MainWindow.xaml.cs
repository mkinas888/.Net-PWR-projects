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

namespace Platformy_projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JObject jsonMeme;
        platformyEntities db = new platformyEntities();
        System.Windows.Data.CollectionViewSource platformyEntryViewSource;
        System.Windows.Data.CollectionViewSource platformyEntitiesViewSource;

        public MainWindow()
        {
            InitializeComponent();
            //    platformyEntryViewSource =
            //        ((System.Windows.Data.CollectionViewSource)(this.FindResource("platformyEntryViewSource")));
            //    platformyEntitiesViewSource =
            //        ((System.Windows.Data.CollectionViewSource)(this.FindResource("platformyEntitiesViewSource")));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            db.memy.Local.Concat(db.memy.ToList());
            platformyEntryViewSource.Source = db.memy.Local;
            platformyEntitiesViewSource.Source = db.memy.Local;
            //System.Windows.Data.CollectionViewSource personViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("personViewSource")));
            //// Load data by setting the CollectionViewSource.Source property:
            //// personViewSource.Source = [generic data source]
            //System.Windows.Data.CollectionViewSource productViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("productViewSource")));
            //// Load data by setting the CollectionViewSource.Source property:
            //// productViewSource.Source = [generic data source]
        }


        private async void DownloadMeme(object sender, RoutedEventArgs e)
        {
            string jsonInString = await MemeConnection.DownloadJsonAsync();
            jsonMeme = JObject.Parse(jsonInString);
            var dataToDatabase = ProcessJson.NextMeme(jsonMeme);
            ChangePicture(dataToDatabase["url"]);
            AddToDatabase(dataToDatabase);
        }

        private void NextMeme(object sender, RoutedEventArgs e)
        {
            ChangePicture(ProcessJson.NextMeme(jsonMeme)["url"]);
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
                id = 0,
                meme_url = data["url"],
                upvotes = int.Parse(data["score"]),
                uzytkownik = data["author"],
                data_top = DateTime.Now
            };
            db.memy.Local.Add(newEntry);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.memy.Local.Remove(newEntry);
                Debug.WriteLine("Error, id is not unique!: " + ex);
            }
        }

    }
}
