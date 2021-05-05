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

namespace Player
{
    /// <summary>
    /// Логика взаимодействия для reklama.xaml
    /// </summary>
    public partial class reklama : Window
    {
        bool isclosing = true;
        public int ka = 0;
        public reklama()
        {
            InitializeComponent();

            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;

            this.MinHeight = this.Height;
            this.MaxHeight = this.Height;

            this.MinWidth = this.MinWidth;
            this.MaxWidth = this.MaxWidth;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = isclosing;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void me_reklama_MediaEnded(object sender, RoutedEventArgs e)
        {
            isclosing = false;
            me_reklama.Close();
            this.Close();
            App.Current.MainWindow.Show();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if(ka == 0)
                isclosing = true;
            else
            {
                isclosing = false;
                this.Close();
            }    
        }
    }
}
