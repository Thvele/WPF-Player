using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using Microsoft.VisualBasic;


namespace Player
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ListView curr_lv = new ListView();
        public TabItem curr_ti = new TabItem();
        ListView nemix = new ListView();
        Random rnd = new Random();
        int tCount = 0;

        int ka = 0;

        bool p = false;
        ListViewItem lv_copy = new ListViewItem();

        public string Hpath;

        //ListViewItem curr_lvi = new ListViewItem();

        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            App.Current.StartupUri = new Uri("/Player;component/MainWindow.xaml", UriKind.Relative);
            App.Current.MainWindow = this;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);
            curr_ti = tc_main.Items[0] as TabItem;
            curr_lv = curr_ti.Content as ListView;
            curr_lv.SelectionChanged += Mpl_SelectionChanged;
            lv_copy.Content = null;
            lv_copy.Tag = null;
            me_main.Volume = 1;

            string[] MPLfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + $"\\mp3\\");
            foreach (string f in MPLfiles)
            {
                string[] ff = f.Split('\u005C');
                curr_lv.Items.Add(new ListViewItem { Content = ff[ff.Length - 1].Replace('_', ' '), Tag = f.ToString() });
            }

            curr_lv = (tc_main.Items[0] as TabItem).Content as ListView;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            s_duration.Value = me_main.Position.TotalSeconds;
        }

        private void Mpl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                tBox_author.Content = "";
                tBox_song.Content = "";


                me_main.Source = new Uri(((curr_lv).SelectedItem as ListViewItem).Tag.ToString());
                var taglib = TagLib.File.Create((curr_lv.SelectedItem as ListViewItem).Tag.ToString());
                TagLib.File FD = TagLib.File.Create((curr_lv.SelectedItem as ListViewItem).Tag.ToString());
                TimeSpan Duration = taglib.Properties.Duration;
                me_videos.Position = TimeSpan.FromSeconds(rnd.Next(0, 3000));
                me_main.Pause();

                try
                {
                    tBox_song.Content = taglib.Tag.Title;
                    tBox_author.Content = taglib.Tag.FirstPerformer;

                }
                catch { }

                if((curr_lv.SelectedItem as ListViewItem).Content.ToString().Contains(".mp4"))
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Hidden;
                }
                else
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            g_main.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#272727");
            tc_main.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#272727");
            if(curr_lv != null)
                curr_lv.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
            tBox_author.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
            tBox_song.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");

            lv_find.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
            cb_his.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
            c_user.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#272727");
            c_user.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFFFFF");
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            g_main.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A4A4A4");
            tc_main.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A4A4A4");
            if (curr_lv != null)
                curr_lv.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
            tBox_author.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
            tBox_song.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");

            lv_find.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
            cb_his.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
            c_user.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A4A4A4");
            c_user.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        }

        private void tc_main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tc_main.SelectedItem != null)
                    curr_ti = tc_main.SelectedItem as TabItem;
                if (curr_ti.Content != null)
                {
                    curr_lv = curr_ti.Content as ListView;
                    curr_lv.SelectionChanged += Mpl_SelectionChanged;
                }
            }
            catch { }
        }

        private void b_back_Click(object sender, RoutedEventArgs e)
        {
            me_videos.Close();
            int temp = curr_lv.Items.Count;
            temp--;
            int temp1 = curr_lv.SelectedIndex;
            temp1--;
            tb_repeat.IsChecked = false;

            if (temp1 < 0)
            {
                curr_lv.SelectedIndex = temp;
            }
            else if (temp1 >= 0)
            {
                curr_lv.SelectedIndex--;
            }

            me_main.Play();
        }

        private void b_tenback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                me_main.Position = TimeSpan.FromSeconds(me_main.Position.TotalSeconds - 10);
                me_videos.Position = TimeSpan.FromSeconds(me_videos.Position.TotalSeconds - 10);
            }
            catch { }
        }

        private void b_pause_Click(object sender, RoutedEventArgs e)
        {
            me_main.Pause();
            me_videos.Pause();
            p = true;

            //ozh();
            //reklama();
        }

        private void b_play_Click(object sender, RoutedEventArgs e)
        {
            me_main.Play();
            me_videos.Play();
            p = false;
        }

        private void b_tenforward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                me_main.Position = TimeSpan.FromSeconds(me_main.Position.TotalSeconds + 10);
                me_videos.Position = TimeSpan.FromSeconds(me_videos.Position.TotalSeconds + 10);
            }
            catch { }
        }

        private void b_forward_Click(object sender, RoutedEventArgs e)
        {
            me_videos.Close();

            int temp = curr_lv.Items.Count;
            temp--;
            tb_repeat.IsChecked = false;

            if (curr_lv.SelectedIndex++ < temp)
            {
                curr_lv.SelectedIndex = curr_lv.SelectedIndex;
            }
            else
            {
                curr_lv.SelectedIndex = 0;
            }

            me_main.Play();
        }

        private void tb_mix_Checked(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            ListView lvtemp = new ListView();

            string[] a = new string[curr_lv.Items.Count];
            string[] atag = new string[curr_lv.Items.Count];

            for (int i = 0; i < curr_lv.Items.Count; i++)
            {
                a[i] = (curr_lv.Items[i] as ListViewItem).Content.ToString();
                atag[i] = (curr_lv.Items[i] as ListViewItem).Tag.ToString();
            }

            nemix.Items.Clear();

            for (int i = 0; i < curr_lv.Items.Count; i++)
            {
                nemix.Items.Add(new ListViewItem {Content = a[i], Tag = atag[i]});
            }

            string[] randomize = (from object item in curr_lv.Items select item.ToString()).ToArray<string>();

            Random rand = new Random();
            for (int i = randomize.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i);
                string tmp = randomize[i];
                randomize[i] = randomize[j];
                randomize[j] = tmp;
            }

            curr_lv.Items.Clear();

            for (int i = 0; i < randomize.Length; i++)
            {
                string temp = "";
                string[] temp1 = randomize[i].Split(':');
                string temp2 = "";

                for (int k = 1; k < temp1.Length; k++)
                {
                    temp2 += temp1[k].Trim();
                }

                temp2.Remove(0, 1);

                for (int j = 0; j < a.Length; j++)
                {
                    if(temp2 == a[j])
                    {
                        temp = atag[j];
                    }
                }

                curr_lv.Items.Add(new ListViewItem { Content = temp2, Tag = temp });
            }

            curr_lv.SelectedIndex = 0;
            me_main.Pause();
        }

        private void tb_mix_Unchecked(object sender, RoutedEventArgs e)
        {
            string[] a = new string[nemix.Items.Count];
            string[] atag = new string[nemix.Items.Count];

            for (int i = 0; i < nemix.Items.Count; i++)
            {
                nemix.SelectedIndex = i;
                a[i] = (nemix.SelectedItem as ListViewItem).Content.ToString();
                atag[i] = (nemix.SelectedItem as ListViewItem).Tag.ToString();
            }

            while (curr_lv.Items.Count != 0)
                curr_lv.Items.Clear();

            for (int i = 0; i < nemix.Items.Count; i ++)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = a[i];
                lv.Tag = atag[i];
                curr_lv.Items.Add(lv);
            }

            curr_lv.SelectedIndex = 0;
            me_main.Pause();
            nemix.Items.Clear();
        }

        private void me_main_MediaEnded(object sender, RoutedEventArgs e)
        {
            int temp = curr_lv.Items.Count;
            temp--;
            int temp1 = curr_lv.SelectedIndex;
            temp1++;
            me_videos.Close();

            if (tb_repeat.IsChecked == false && (me_main.Position > TimeSpan.FromSeconds(60) || (curr_lv.SelectedItem as ListViewItem).Content.ToString().Contains(".mp4")))
            {

                if (temp1 <= temp)
                {
                    curr_lv.SelectedIndex++;
                }
                else
                {
                    curr_lv.SelectedIndex = 0;
                }

                me_main.Source = new Uri(((curr_lv.SelectedItem as ListViewItem).Tag.ToString()));
                var taglib = TagLib.File.Create(((curr_lv).SelectedItem as ListViewItem).Tag.ToString());
                TimeSpan Duration = taglib.Properties.Duration;
                me_main.Position = TimeSpan.FromSeconds(0);
                me_videos.Position = TimeSpan.FromSeconds(rnd.Next(0, 3000));
                me_main.Play();

                try
                {
                    tBox_song.Content = taglib.Tag.Title;
                    tBox_author.Content = taglib.Tag.FirstPerformer;
                }
                catch { }

                if ((curr_lv.SelectedItem as ListViewItem).Content.ToString().Contains(".mp4"))
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Hidden;
                }
                else
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Visible;
                }
            }
            else if(tb_repeat.IsChecked == true)
            {
                me_main.Position = TimeSpan.FromSeconds(0);
            }
        }

        private void s_duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(!p)
                me_videos.Play();
            me_main.Position = TimeSpan.FromSeconds(s_duration.Value);
        }

        private void me_main_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                history((curr_lv.SelectedItem as ListViewItem).Content.ToString());

                TimeSpan ts = me_main.NaturalDuration.TimeSpan;
                s_duration.Maximum = ts.TotalSeconds;
                timer.Start();
            }
            catch { }

            me_videos.Source = new Uri(Directory.GetCurrentDirectory() + "\\vid\\def.mp4");
            me_videos.Position = TimeSpan.FromSeconds(rnd.Next(0, 3000));
            me_videos.Pause();
        }

        private void s_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                me_main.Volume = s_volume.Value / 100;
            }
            catch { }
        }

        private void b_Madd_Click(object sender, RoutedEventArgs e)
        {
            bool error = false;
            tb_mix.IsChecked = false;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Медиа файлы (*.aif, *.m3u, *.m4a, *.mid, *.mp3, *.mpa, *.wav, *.wma, *.mp4)|*.aif; *.m3u; *.m4a; *.mid; *.mp3; *.mpa; *.wav; *.wma; *.mp4"; ;

            if(ofd.ShowDialog() == true)
            {
                string[] ff = ofd.FileName.Split('\u005C');
                if(!error)
                curr_lv.Items.Add((new ListViewItem { Content = ff[ff.Length - 1], Tag = ofd.FileName.ToString() }));
                
            }
                nemix.Items.Clear();

                string[] a = new string[curr_lv.Items.Count];
                string[] atag = new string[curr_lv.Items.Count];

                for (int i = 0; i < curr_lv.Items.Count; i++)
                {
                    curr_lv.SelectedIndex = i;
                    a[i] = (curr_lv.SelectedItem as ListViewItem).Content.ToString().Replace('_', ' ');
                    atag[i] = (curr_lv.SelectedItem as ListViewItem).Tag.ToString();
                }

                for (int i = 0; i < curr_lv.Items.Count; i++)
                {
                    ListViewItem lv = new ListViewItem();
                    lv.Content = a[i];
                    lv.Tag = atag[i];
                    nemix.Items.Add(lv);
                }


                int temp = curr_lv.Items.Count;
                curr_lv.SelectedIndex = temp--;
        }

        private void b_Mdelete_Click(object sender, RoutedEventArgs e)
        {
            curr_lv.Items.RemoveAt(curr_lv.SelectedIndex);
            curr_lv.SelectedIndex = 0;
        }

        private void b_Pladd_Click(object sender, RoutedEventArgs e)
        {
            tb_mix.IsChecked = false;
            me_videos.Source = null;
            me_main.Close();
            tBox_author.Content = "";
            tBox_song.Content = "";

            try
            {
                TabItem ti = new TabItem();
                ListView lv = new ListView();

                ti.Content = lv;

                try
                {

                    string res = Interaction.InputBox("Введите название альбома: ");

                    if (res != null)
                    {
                        ti.Header = res;
                        ti.Name = res + tCount.ToString();
                        tc_main.Items.Add(ti);

                        tc_main.SelectedItem = ti;
                        tCount++;
                    }
                }
                catch
                {
                    MessageBox.Show("Недопустимое название альбома!");
                }

                nemix.Items.Clear();

                string[] a = new string[curr_lv.Items.Count];
                string[] atag = new string[curr_lv.Items.Count];

                for (int i = 0; i < curr_lv.Items.Count; i++)
                {
                    curr_lv.SelectedIndex = i;
                    a[i] = (curr_lv.SelectedItem as ListViewItem).Content.ToString().Replace('_', ' ');
                    atag[i] = (curr_lv.SelectedItem as ListViewItem).Tag.ToString();
                }

                for (int i = 0; i < curr_lv.Items.Count; i++)
                {
                    ListViewItem lv1 = new ListViewItem();
                    lv1.Content = a[i];
                    lv1.Tag = atag[i];
                    nemix.Items.Add(lv1);
                }
            }
            catch 
            {
                
            }
        }

        private void b_PLdelete_Click(object sender, RoutedEventArgs e)
        {
            if ((tc_main.SelectedItem as TabItem).Name.ToString() != "ti_main")
                tc_main.Items.Remove(tc_main.SelectedItem);
            else
                MessageBox.Show("Закрыть основной плейлист нельзя!");
        }

        private void b_PLrename_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((tc_main.SelectedItem as TabItem).Header.ToString() != "mainPL")
                {
                    string res = Interaction.InputBox("Введите название альбома: ");

                    if (res != null)
                    {
                        curr_ti.Header = res;
                        curr_ti.Name = res;
                    }
                }
                else
                {
                    MessageBox.Show("Нельзя переименовать основной альбом!");
                }
            }
            catch { }
        }

        private void s_duration_Drop(object sender, DragEventArgs e)
        {
            me_videos.Position = TimeSpan.FromSeconds(me_videos.Position.TotalSeconds) + TimeSpan.FromSeconds(s_duration.Value);
        }

        private void b_find_Click(object sender, RoutedEventArgs e)
        {
            lv_find.Items.Clear();

            string temp = tb_find.Text;

            string[] a = new string[curr_lv.Items.Count];
            string[] atag = new string[curr_lv.Items.Count];

            for (int i = 0; i < curr_lv.Items.Count; i++)
            {
                curr_lv.SelectedIndex = i;
                a[i] = (curr_lv.SelectedItem as ListViewItem).Content.ToString().Replace('_',' ');
                atag[i] = (curr_lv.SelectedItem as ListViewItem).Tag.ToString();
            }

            for (int i = 0; i < curr_lv.Items.Count; i++)
            {
                string[] tempai = tb_find.Text.Split(' ');
                int k = 0;

                for (int j = 0; j < tempai.Length; j++)
                {
                    if (a[i].ToLower().Contains(tempai[j].ToLower()))
                    {
                        k++;
                    }
                    if(k == tempai.Length)
                    {
                        lv_find.Items.Add(new ListViewItem { Content = a[i], Tag = atag[i] });
                    }
                }
            }
        }

        private void lv_find_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                tBox_author.Content = "";
                tBox_song.Content = "";


                me_main.Source = new Uri(((lv_find).SelectedItem as ListViewItem).Tag.ToString());
                var taglib = TagLib.File.Create((lv_find.SelectedItem as ListViewItem).Tag.ToString());
                TagLib.File FD = TagLib.File.Create((lv_find.SelectedItem as ListViewItem).Tag.ToString());
                TimeSpan Duration = taglib.Properties.Duration;
                me_videos.Position = TimeSpan.FromSeconds(rnd.Next(0, 3000));
                me_main.Pause();

                try
                {
                    tBox_song.Content = taglib.Tag.Title;
                    tBox_author.Content = taglib.Tag.FirstPerformer;
                }
                catch { }

                if ((lv_find.SelectedItem as ListViewItem).Content.ToString().Contains(".mp4"))
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Hidden;
                }
                else
                {
                    b_tenback_Click(sender, e);
                    me_videos.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }

        private void b_Tcopy_Click(object sender, RoutedEventArgs e)
        {
            if (curr_lv.SelectedValue != null)
            {
                lv_copy.Content = (curr_lv.SelectedItem as ListViewItem).Content.ToString();
                lv_copy.Tag = (curr_lv.SelectedItem as ListViewItem).Tag.ToString();
            }
            else
            {
                MessageBox.Show("Для начала выделите элемент");
            }
        }

        private void b_Tpaste_Click(object sender, RoutedEventArgs e)
        {
            if(lv_copy.Content != null)
            {
                curr_lv.Items.Add(new ListViewItem{ Content = lv_copy.Content, Tag = lv_copy.Tag });
            }
            else
            {
                MessageBox.Show("Для начала скопируйте элемент");
            }
        }

        private void s_balance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                me_main.Balance = s_balance.Value / 100;
            }
            catch { }
        }

        async private void reklama()
        {
            await Task.Run(() => System.Threading.Thread.Sleep(30000));
            me_main.Pause();
            me_videos.Pause();
            p = true;
            this.Hide();


            Random r = new Random();
            int rand = r.Next(1, 4);


            await Task.Run(() => odin());

            void odin()
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    reklama w = new reklama();
                    if (w.IsActive == false)
                    {
                        w.ka = 0;
                        w.Show();

                        w.me_reklama.Source = new Uri(Directory.GetCurrentDirectory() + $"\\rek\\{rand}.mp4");
                        var taglib = TagLib.File.Create(Directory.GetCurrentDirectory() + $"\\rek\\{rand}.mp4");
                        TimeSpan Duration = taglib.Properties.Duration;

                        w.me_reklama.Play();
                    }
                    else
                    {
                        this.Show();
                    }
                });
            }

            var taglibtemp = TagLib.File.Create(Directory.GetCurrentDirectory() + $"\\rek\\{rand}.mp4");
            await Task.Run(() => System.Threading.Thread.Sleep(Convert.ToInt32(taglibtemp.Properties.Duration.TotalMilliseconds)));

        }

        private void MW_Activated(object sender, EventArgs e)
        {
            if (tc_main.Items.Count == 1)
            {
                try
                {
                    string[] MPLfiles = Directory.GetFiles($@"{Hpath}\PaL\");

                    if (MPLfiles.Length == 1)
                    {
                        TabItem ti = new TabItem();
                        ListView lv = new ListView();

                        string[] Tplname = MPLfiles[0].Split('\u005C');
                        string plname = Tplname[Tplname.Length - 1];

                        ti.Name = plname.Split('.')[0];
                        ti.Header = plname.Split('.')[0];

                        string[] temp = File.ReadAllLines(MPLfiles[0]);

                        foreach (string f in temp)
                        {
                            string[] ff = f.Split('\u005C');
                            lv.Items.Add(new ListViewItem { Content = ff[ff.Length - 1].Replace('_', ' '), Tag = f.ToString() });
                        }
                        ti.Content = lv;

                        tc_main.Items.Add(ti);
                    }
                    else
                    {
                        for (int k = 0; k < MPLfiles.Length; k++)
                        {

                            string[] Tplname = MPLfiles[k].Split('\u005C');
                            string plname = Tplname[Tplname.Length - 1];

                            for (int i = 0; i < MPLfiles.Length - 1; i++)
                            {
                                TabItem ti = new TabItem();
                                ListView lv = new ListView();

                                string[] tf = File.ReadAllLines($@"{Hpath}\PaL\{plname}");

                                for (int j = 0; j < tf.Length; j++)
                                {
                                    ti.Name = plname.Split('.')[0];
                                    ti.Header = plname.Split('.')[0];

                                    string[] ff = tf[j].Split('\u005C');
                                    lv.Items.Add(new ListViewItem { Content = ff[ff.Length - 1].Replace('_', ' '), Tag = tf[j].ToString() });
                                }

                                ti.Content = lv;

                                tc_main.Items.Add(ti);
                            }

                            curr_ti = tc_main.SelectedItem as TabItem;
                            curr_lv = curr_ti.Content as ListView;
                            curr_lv.SelectionChanged += Mpl_SelectionChanged;

                            curr_lv = curr_ti.Content as ListView;
                        }
                        string[] tpath = File.ReadAllLines($@"{Hpath}History.txt");

                        foreach (string tempH in tpath)
                        {
                            cb_his.Items.Add(tempH);
                        }
                    }
                }
                catch { }
            }
        }

        private void history(string address)
        {
            StreamWriter sw = File.AppendText($@"{Hpath}History.txt");

            sw.WriteLine(address.ToString() + "\t\t\t" + DateTime.Now);

            sw.Close();

            cb_his.Items.Clear();

            string[] tpath = File.ReadAllLines($@"{Hpath}History.txt");

            foreach (string tempH in tpath)
            {
                cb_his.Items.Add(tempH);
            }
        }

        private void MW_Closed(object sender, EventArgs e)
        {
            int kv_pl = 0;

            if(Directory.Exists(Directory.GetCurrentDirectory() + $"\\accs\\{c_user.Content}\\PaL"))
            {
                string[] MPLfiles = Directory.GetFiles(Directory.GetCurrentDirectory() + $"\\accs\\{c_user.Content}\\PaL");

                foreach (string f in MPLfiles)
                {
                    File.Delete(f);
                }
            }

            Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"\\accs\\{c_user.Content}\\PaL");

            var path = Directory.GetCurrentDirectory() + $"\\accs\\{c_user.Content}\\PaL";

            for(int i = 1; i < tc_main.Items.Count; i++)
            {
                File.Delete($@"{path}\{(tc_main.Items[i] as TabItem).Name}.txt");
                StreamWriter sw = File.AppendText($@"{path}\{(tc_main.Items[i] as TabItem).Header}.txt");

                ListView lv = ((tc_main.Items[i] as TabItem).Content as ListView);

                for(int j = 0; j < lv.Items.Count; j++)
                {
                    sw.WriteLine((lv.Items[j] as ListViewItem).Tag);
                }

                sw.Close();
                kv_pl++;
            }

            Environment.Exit(0);
        }

        private void MW_Activated_1(object sender, EventArgs e)
        {
            if(ka == 0)
                reklama();
            ka++;
        }

        private void MW_Deactivated(object sender, EventArgs e)
        {
            ka = 0;
        }
    }
}