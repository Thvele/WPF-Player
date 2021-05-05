using System;
using System.Collections.Generic;
using System.IO;
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
//using bool.cs;

namespace Player
{
    /// <summary>
    /// Логика взаимодействия для log_reg.xaml
    /// </summary>
    public partial class log_reg : Window
    {
        Image imag = new Image();
        public string Hpath;
        public bool log = false;
        public log_reg()
        {
            InitializeComponent();

            tb_log.Text = "";
            pb_pass.Password = "";
        }

        private void b_reg_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            reg r = new reg();
            r.Show();
        }

        private void b_log_Click(object sender, RoutedEventArgs e)
        {
            if (tb_log.Text.Length > 0)
            {
                if (pb_pass.Password.Length > 0)
                {
                    if (Directory.Exists(Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}"))
                    {
                        Hpath = Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}" + "\u005C";
                        var path = Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}";
                        string pass = "";

                        using(StreamReader sr = new StreamReader(path + "\u005Cpassword.txt"))
                        {
                            pass += sr.ReadLine();
                        }

                        if (pb_pass.Password == pass)
                        {
                            var imgpath = Directory.GetFiles(path);
                            string Pimg = imgpath[imgpath.Length - 1];

                            try
                            {

                                imag.Source = new BitmapImage(new Uri(Pimg));
                            }
                            catch 
                            {
                                imag.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\\img\\1.png"));
                            }

                            log = true;

                            MainWindow me = new MainWindow();

                            this.Hide();
                            me.c_user.Content = tb_log.Text;
                            me.c_user_img.Source = imag.Source;
                            me.Hpath = Hpath;

                            me.Show();
                        }
                        else
                            MessageBox.Show("Неверный пароль");
                    }
                    else
                        MessageBox.Show("Такого пользователя не существует");
                }
                else
                    MessageBox.Show("пароль не может быть пустым");
            }
            else
                MessageBox.Show("Логин не может быть пустым");

        }
    }
}
