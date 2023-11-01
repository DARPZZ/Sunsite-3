using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
using Sunsite_3.Model;
using Sunsite_3.ViewModel;
using IniParser;
using IniParser.Model;
using System.Windows.Threading;

namespace Sunsite_3.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private TcpClient client;

        private StreamReader reader;
        private StreamWriter writer;
        Connection con = new Connection();
        string server;
        IniFile infFil = new IniFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");
        public HomeView()
        {
            InitializeComponent();
            initData();
            ReadFavorits();

        }
        public async void initData()
        {
            server = infFil.Read("server_name", "login");

            int port = 119;

            try
            {
                App.Sharedata.Client = new TcpClient(server, port);
                App.Sharedata.Reader = new StreamReader(App.Sharedata.Client.GetStream(), Encoding.Default);
                App.Sharedata.Writer = new StreamWriter(App.Sharedata.Client.GetStream(), Encoding.Default);
                client = App.Sharedata.Client;
                writer = App.Sharedata.Writer;
                reader = App.Sharedata.Reader;

                con.Authenticate(OutputBox, reader, writer);
                StartFlowNews();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            initData();
        }

        private async Task StartFlowNews()
        {
           
            ListboxList.IsEnabled = false;
            favouritsbox.IsEnabled = false;
            await Task.Run(() => ReceiveNewsAsync());
        }

        private void ReceiveNewsAsync()
        {
            
            Dispatcher.Invoke(() =>
            {
                ListboxList.Items.Clear();
            });

            writer.WriteLine("LIST");
            writer.Flush();

            while (true)
            {
                string response = reader.ReadLine();
                if (response == ".")
                    break;

                string[] parts = response.Split(' ');
                if (parts.Length > 0)
                {
                    string item = parts[0];

                    
                    Dispatcher.Invoke(() =>
                    {
                        ListboxList.Items.Add(item + Environment.NewLine);
                    });
                }
            }

            
            Dispatcher.Invoke(() =>
            {
                ListboxList.IsEnabled = true;
                favouritsbox.IsEnabled = true;
            });
        }


        public void GetSpecializedInfo(ListBox genListBox)
        {
            string selectedItem = genListBox.SelectedItem.ToString();
            App.Sharedata.ForPosting = selectedItem;
            Debug.WriteLine(selectedItem);
            GetArticlesInNewsgroup(selectedItem);
        }


        private void ListboxList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            GetSpecializedInfo(ListboxList);
          

        }
        public void GetArticlesInNewsgroup(string groupName)
        {
            if (Each.Items.Count > 0)
            {
                Each.Items.Clear();
            }

            writer.WriteLine($"group {groupName}");
            writer.Flush();
            string groupResponse = reader.ReadLine();
            writer.WriteLine("XOVER 1-");
            writer.Flush();

            string[] articleInfo;
            string fullInfo;
            string response;
            string name;
            while (!(response = reader.ReadLine()).Equals("."))
            {
                articleInfo = response.Split();

                if (articleInfo[1] == "Re:")
                {
                    name = articleInfo[2];
                }
                else
                {
                    name = articleInfo[1];
                }

                fullInfo = articleInfo[0] + " " + name;
                Each.Items.Add(fullInfo);
            }
        }
        public string line { get; set; }
        StringBuilder articleContent = new StringBuilder();
        public void ReadArticle(string articleNumber)
        {
            writer.WriteLine($"ARTICLE {articleNumber}");

            writer.Flush();

            string[] parts = articleNumber.Trim().Split();
            while ((line = reader.ReadLine()) != null)
            {
                if (line == ".")

                    break;
                articleContent.AppendLine(line);
                App.Sharedata.Content = articleContent.ToString();
            }
        }
        private void Each_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = Each.SelectedItem.ToString();

            ReadArticle(selectedItem);
            ((App)App.Current).ChangeUserControl(typeof(MessageViewModel));
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).ChangeUserControl(typeof(SettingsViewModel));
        }

        private void PostingArticals(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).ChangeUserControl(typeof(PostViewModel));
        }
        Search search = new Search();
        private void searchWordInBox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var userWord = Searching.Text;
                Debug.WriteLine(userWord + " whdahwdhwhadhadhwahhwd");
                if (userWord == "")
                {
                    StartFlowNews();
                }
                else
                {
                    search.search(writer, reader, ListboxList, userWord);
                }

            }
        }

        public async  void ReadFavorits()
        {
            favouritsbox.Items.Clear();

            
           await Task.Run(() =>
            {
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

                Debug.WriteLine($"Thread {threadId} reeeeeeeeeeeeeeeeeeeeeeeeee");

                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");

                SectionData favorSection = data.Sections.GetSectionData("favor");

                foreach (KeyData key in favorSection.Keys)
                {
                    string value = key.Value;

                    
                    Dispatcher.Invoke(() =>
                    {
                        favouritsbox.Items.Add(value);
                    });
                }
            });
        }


        private void saveFavourit(object sender, RoutedEventArgs e)
        {
            string iteam = ListboxList.SelectedItem.ToString();

            infFil.Write(iteam.Trim(), iteam, "favor");
            ReadFavorits();
        }

        private void favouritsbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetSpecializedInfo(favouritsbox);
        }
    }
}

