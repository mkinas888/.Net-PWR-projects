using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Platformy_projekt
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            TxtHeight.Text = Properties.Settings.Default.Height.ToString();
            TxtWidth.Text = Properties.Settings.Default.Width.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Width = double.Parse(TxtWidth.Text);
                Properties.Settings.Default.Height = double.Parse(TxtHeight.Text);
            }
            catch(Exception /*ex*/)
            {
                MessageBox.Show("Must be number");
                return;
            }
            Properties.Settings.Default.Save();
            this.Close();
            MessageBox.Show("To see changes restart app!");
        }
    }
}
