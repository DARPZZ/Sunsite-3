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
        public PostView()
        {
            InitializeComponent();
            var ko = App.Sharedata.ForPosting;
            Debug.WriteLine(ko + "   SUIIIIIII");
        }
        IniFile infFil = new IniFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");

  
        public void PostToNewsgroup()
        {
            var userNameInf = infFil.Read("username", "login");
            var pawwordInf = infFil.Read("password", "login");
            string server = infFil.Read("server_name", "login");
            int port = 119;
            string newsgroup = "dk.test";
            string username = userNameInf;
            string password = pawwordInf;
            string subject = "Test";
            string articleText = "Your article content. ";

            try
            {
                using (TcpClient client = new TcpClient(server, port))
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1")))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("ISO-8859-1")))
                {
                    // Read the server's initial response
                    string response = reader.ReadLine();
                    Debug.WriteLine(response);

                    // Send a command to authenticate (if needed)
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

                    // Send a command to set the newsgroup
                    writer.WriteLine($"GROUP {newsgroup}");
                    writer.Flush();
                    response = reader.ReadLine();
                    Debug.WriteLine(response);

                    // Create and send the article headers and body
                    writer.WriteLine($"POST");
                    writer.Flush();
                    response = reader.ReadLine();
                    Debug.WriteLine(response);

                    // Send the article headers
                    writer.WriteLine($"Subject: {subject}");
                    writer.WriteLine($"From: {username}");
                    writer.WriteLine($"Newsgroups: {newsgroup}");
                    writer.WriteLine("Content-Type: text/plain; charset=ISO-8859-1"); // Adjust content type if needed
                    writer.WriteLine(""); // Empty line to separate headers from the body
                    writer.Flush();

                    // Send the article body
                    writer.WriteLine(articleText);
                    writer.Flush();

                    // Send a period to indicate the end of the article
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
