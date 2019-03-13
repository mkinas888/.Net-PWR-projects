using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PersonAdder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Person> people = new ObservableCollection<Person>
        {
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
        }
        
        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
			people.Add(new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text, PhotoReference = ((BitmapImage)personPhoto.Source).UriSource.AbsolutePath});
			ageTextBox.Text = "";
			nameTextBox.Text = "";
			personPhoto = new Image();
        }

		private void UploadPhoto_Click(object sender, RoutedEventArgs e)
		{
			String imagePath = "";
			try
			{
				Microsoft.Win32.OpenFileDialog imageDialog = new Microsoft.Win32.OpenFileDialog();
				imageDialog.InitialDirectory = "c:\\";
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
	}
}