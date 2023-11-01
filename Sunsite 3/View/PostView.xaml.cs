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

namespace Sunsite_3.View
{
    /// <summary>
    /// Interaction logic for PostView.xaml
    /// </summary>
    public partial class PostView : UserControl
    {

        StreamWriter writer = App.Sharedata.Writer;
        StreamReader reader = App.Sharedata.Reader;
        string ko;
        public PostView()
        {
            InitializeComponent();
             ko = App.Sharedata.ForPosting;
            Debug.WriteLine(ko + "   SUIIIIIII");
            group.Text = ko;
        }
        IniFile infFil = new IniFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");

  
        public void PostToNewsgroup()
        {
            var userNameInf = infFil.Read("username", "login");
            var pawwordInf = infFil.Read("password", "login");
            string server = infFil.Read("server_name", "login");
            int port = 119;
            string newsgroup = ko;
            string username = userNameInf;
            string password = pawwordInf;
            string subject = SubjectTextbox.Text;
            string articleText = BodyBox.Text;

            try
            {
                using (TcpClient client = new TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
                {
                   
                    string response = reader.ReadLine();
                    Debug.WriteLine(response);

                  
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        writer.WriteLine($"AUTHINFO USER {username}");
                        writer.Flush();
                        response = reader.ReadLine();
                        Debug.WriteLine(response);

                        writer.WriteLine($"AUTHINFO PASS {password}");
                        writer.Flush();
                        response = reader.ReadLine();
                        Debug.WriteLine(response);
                    }


                    writer.WriteLine($"GROUP {newsgroup}");
                    writer.Flush();
                    response = reader.ReadLine();
                    Debug.WriteLine(response);

                    writer.WriteLine($"POST");
                    writer.Flush();
                    response = reader.ReadLine();
                    Debug.WriteLine(response);

                    
                    writer.WriteLine($"Subject: {subject}");
                    writer.WriteLine($"From: {username}");
                    writer.WriteLine($"Newsgroups: {newsgroup}");
                    writer.WriteLine("Content-Type: text/plain; charset=ISO-8859-1"); 
                    writer.WriteLine(""); 
                    writer.Flush();

                    writer.WriteLine(articleText);
                    writer.Flush();

                   
                    writer.WriteLine(".");
                    writer.Flush();
                    response = reader.ReadLine();
                    Debug.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }








        private void Post(object sender, RoutedEventArgs e)
        {
            PostToNewsgroup();
        }

    }
}
