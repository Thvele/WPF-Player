using Microsoft.Win32;
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

namespace Player
{
    /// <summary>
    /// Логика взаимодействия для reg.xaml
    /// </summary>
    public partial class reg : Window
    {
        string imgpath = "";
        public reg()
        {
            InitializeComponent();
        }

        private void b_reg_Click(object sender, RoutedEventArgs e)
        {
            var cyrillic = Enumerable.Range(1024, 256).Select(ch => (char)ch);

            if (tb_log.Text.Length > 0)
            {
                if(pb_pass.Password.Length > 0)
                {

                    if(!tb_log.Text.Any(cyrillic.Contains))
                    {

                        if (!pb_pass.Password.Any(cyrillic.Contains))
                        {
                            if (!Directory.Exists(Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}"))
                            {
                                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}");

                                var path = Directory.GetCurrentDirectory() + $"\\accs\\{tb_log.Text}";

                                using (StreamWriter sw = File.CreateText(path + "\u005Cpassword.txt"))
                                {
                                    sw.WriteLine(pb_pass.Password);
                                }

                                if (p_img.Source != null)
                                {
                                    string[] ff = imgpath.Split('.');
                                    string f = "." + ff[ff.Length - 1];

                                    File.Copy($@"{imgpath}", path + $"\u005CPimg{f}");
                                }

                                File.Create(path + "\u005CHistory.txt");

                                MessageBox.Show("Регистрация успешна");

                                this.Hide();
                                log_reg lg = new log_reg();

                                lg.tb_log.Text = this.tb_log.Text;

                                lg.Show();
                            }
                            else
                            {
                                MessageBox.Show("Данный пользователь существует");
                            }
                        }
                        else
                            MessageBox.Show("Пароль не должен содержать кириллицу");
                    }
                    else
                        MessageBox.Show("Логин не должен содержать кириллицу");
                }
                else
                    MessageBox.Show("Введите пароль");
            }
            else
                MessageBox.Show("Введите логин");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Изображения (*.jpeg, *.jpg, *.png, *.bmp)|*.jpeg; *.jpg; *.png; *.bmp"; ;

            if (ofd.ShowDialog() == true)
            {
                p_img.Source = new BitmapImage(new Uri(ofd.FileName));
                imgpath = ofd.FileName;
            }
        }
    }
}
