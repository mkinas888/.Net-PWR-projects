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
        public MainWindow()
        {
            InitializeComponent();
            
            
        }


        private async void DownloadMeme(object sender, RoutedEventArgs e)
        {
            string jsonInString = await MemeConnection.DownloadJsonAsync();
            jsonMeme = JObject.Parse(jsonInString);
            ChangePicture(ProcessJson.NextMeme(jsonMeme));
            
        }

        private void NextMeme(object sender, RoutedEventArgs e)
        {
            ChangePicture(ProcessJson.NextMeme(jsonMeme));
        }

        private void ChangePicture(string image)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(image);
            bi3.EndInit();
            this.Meme.Source = bi3;
        }
    }
}
