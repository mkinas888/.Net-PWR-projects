using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace PersonAdder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int skok_upvote;
        private int skok_nick;
        private int skok_meme;
        private string[] url;
        private int X;
        private int[] skoki_pocz;

        ObservableCollection<Person> people = new ObservableCollection<Person>
		{
			new Person { Name = "Janusz", Age = 57, PhotoReference="C:\\Users\\Lukas\\Pictures\\SHU_X2G0aG.png" },
			new Person { Name = "Bożydar", Age = 96, PhotoReference="C:\\Users\\Lukas\\Pictures\\SHU_M1JDad.png" }
		};

        public ObservableCollection<Person> Items
        {
            get => people;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
			addNewPersonButton.IsEnabled = false;
            
            skok_nick = 1;
            skok_meme = 1;
            X = 0;
            url = new string[3];
            url[0] = "https://www.reddit.com/r/ProgrammerHumor/top/?t=month";
            url[1] = "https://www.reddit.com/r/gaming/top/?t=month";
            url[2] = "https://www.reddit.com/r/dankmemes/top/?t=month";
            skoki_pocz = new int[3];
            skoki_pocz[0] = 6;
            skoki_pocz[1] = 6;
            skoki_pocz[2] = 9;
            skok_upvote = skoki_pocz[X];

            Rob();
        }
        
        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
			people.Add(new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text, PhotoReference = ((BitmapImage)personPhoto.Source).UriSource.AbsolutePath});
			ageTextBox.Text = "";
			nameTextBox.Text = "";
			personPhoto.Source = null;
			addNewPersonButton.IsEnabled = false;
        }

		private void UploadPhoto_Click(object sender, RoutedEventArgs e)
		{
			String imagePath = "";
			try
			{
				Microsoft.Win32.OpenFileDialog imageDialog = new Microsoft.Win32.OpenFileDialog();
				imageDialog.InitialDirectory = "c:\\Pictures";
				imageDialog.Filter = "jpg files (*.jpg)|*.jpg| png files (*.png)|*png| bmp files (*.bmp)|*.bmp";
				imageDialog.FilterIndex = 2;
				imageDialog.RestoreDirectory = true;
				if (imageDialog.ShowDialog().GetValueOrDefault())
				{
					imagePath = imageDialog.FileName;
					BitmapImage bitmap = new BitmapImage();
					bitmap.BeginInit();
					bitmap.UriSource = new Uri(imagePath);
					bitmap.EndInit();
                    
					personPhoto.Source = bitmap;
					addNewPersonButton.IsEnabled = true;
				}

			}
			catch (Exception)
			{
				MessageBox.Show("An error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		
		private void AgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (System.Text.RegularExpressions.Regex.IsMatch(ageTextBox.Text, "[^0-9]"))
			{
				MessageBox.Show("Only accepted format is number!");
				ageTextBox.Text = ageTextBox.Text.Remove(ageTextBox.Text.Length - 1);
			}
		}


        private async Task<string> AccessTheWebAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                 
                Task<string> getStringTask = client.GetStringAsync(url[X]);
                
                string urlContents = await getStringTask;
                
                return urlContents;
            }
        }

        private async Task<int> GetUpvotesAsync(string urlContent)
        {
            Random rnd = new Random();

            int temp = 0;
            int x;
            StringBuilder temp_string = new StringBuilder(128);
            string upvotes = string.Empty;

            await Task.Run(() =>
            {
                try { 
                // ILOSC UPVOTES
                for (int i = 0; i < skok_upvote; i++)
                {
                    x = urlContent.IndexOf("_1rZYMD_4xY3gRcSS3p8ODO", temp);
                    temp = x + 1;
                }
                x = urlContent.IndexOf(">", temp);
                temp = x + 1;

                while (urlContent[temp] != '<') temp_string.Append(urlContent[temp++]);

                upvotes = temp_string.ToString();

                    if (upvotes.IndexOf('k') > 0)
                    {
                        bool kropka = false;
                        char[] vs = upvotes.ToCharArray();
                        StringBuilder temp_string1 = new StringBuilder();
                        foreach (var item in vs)
                        {
                            if (char.IsDigit(item)) temp_string1.Append(item);
                            else if (item == '.') kropka = true;
                        }
                        if(kropka)
                            temp_string1.Append("00");
                        else
                            temp_string1.Append("000");
                        upvotes = temp_string1.ToString();
                    }

                }
                catch
                {
                    upvotes = "0";
                }
            });
            skok_upvote += 2;
            return int.Parse(upvotes);
         
        }

        private async Task<string> GetNameAsync(string urlContent)
        {
            Random rnd = new Random();

            int temp = 0;
            int x;
            StringBuilder temp_string = new StringBuilder(128);
            string nick = string.Empty;

            await Task.Run(() =>
            {
                // Nikc
                for (int i = 0; i < skok_nick; i++)
                {
                    x = urlContent.IndexOf("_2tbHP6ZydRpjI44J3syuqC s1461iz-1 gWXVVu", temp);
                    temp = x + 1;
                }

                x = urlContent.IndexOf(">", temp);
                temp = x + 1;

                while (urlContent[temp] != '<') temp_string.Append(urlContent[temp++]);

                nick = temp_string.ToString();
                

            });
            skok_nick++;
            return nick;

        }

        private async Task<string> GeMemeAsync(string urlContent)
        {
            Random rnd = new Random();

            int temp = 0;
            int x;
            StringBuilder temp_string = new StringBuilder(128);
            string meme = string.Empty;

            await Task.Run(() =>
            {
                // Nikc
                for (int i = 0; i < skok_meme; i++)
                {
                    x = urlContent.IndexOf("_2_tDEnGMLxpM6uOa2kaDB3 media-element", temp);
                    temp = x + 1;
                }

                x = urlContent.IndexOf("src=", temp);
                temp = x + 5;

                while (urlContent[temp] != '\"')
                {
                    if(urlContent[temp] == 'a')
                    {
                        if (urlContent.IndexOf("amp;",temp) == temp)
                        {
                            temp += 4;
                        }

                    }
                    temp_string.Append(urlContent[temp++]);
                }

                meme = temp_string.ToString();
                
            });
            skok_meme++;
            return meme;

        }


        private async void Rob()
        {
            int indeks = 0;
            while (true)
            {

                var getResultTask = AccessTheWebAsync();
                var test = await getResultTask;
                var getUpvotes = GetUpvotesAsync(test);
                var getMeme = GeMemeAsync(test);
                var getNick = GetNameAsync(test);
                int upvotes = await getUpvotes;
                string name = await getNick;
                string meme = await getMeme;
                
                BitmapImage bimage = new BitmapImage();
                bimage.BeginInit();
                bimage.UriSource = new Uri(meme);
                bimage.EndInit();
                personPhoto.Source = bimage;
                people.Add(new Person { Age = upvotes, Name = name, PhotoReference = meme });
                if (indeks++ == 7)
                {
                    X++;
                    if (X == 3) X = 0;
                    skok_upvote = skoki_pocz[X];
                    skok_nick = 1;
                    skok_meme = 1;
                    indeks = 0;
                    
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }


        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    int indeks = 0;
        //    while (true)
        //    {
                
        //        var getResultTask = AccessTheWebAsync();
        //        var test = await getResultTask;
        //        var getUpvotes = GetUpvotesAsync(test);
        //        var getMeme = GeMemeAsync(test);
        //        var getNick = GetNameAsync(test);
        //        int upvotes = await getUpvotes;
        //        string name = await getNick;
        //        string meme = await getMeme;
        //        //MessageBox.Show(name + " : " + upvotes.ToString());
        //        BitmapImage bimage = new BitmapImage();
        //        bimage.BeginInit();
        //        bimage.UriSource = new Uri(meme);
        //        bimage.EndInit();
        //        personPhoto.Source = bimage;
        //        people.Add(new Person { Age = upvotes, Name = name, PhotoReference = meme });
        //        if (indeks++ == 7)
        //        {
        //            X++;
        //            skok_upvote = skoki_pocz[X];
        //            skok_nick = 1;
        //            skok_meme = 1;
        //            if (X == 3) X = 0;
        //        }
                
        //        await Task.Delay(3000);
        //    }
            
        //}
    }
}