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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sunsite_3.Model;
using Sunsite_3.ViewModel;

namespace Sunsite_3.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        string username;
        string paswword;
        string port;
        string server_name;
        Connection con = new Connection();
        public SettingsView()
        {
            InitializeComponent();
        }

        private void GoBacktoHome(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).ChangeUserControl(typeof(HomeViewModel));
        }
        public void data(IniFile inf)
        {
            con.ReadIniFile();
        }

        private void saveSettings(object sender, RoutedEventArgs e)
        {
            string portNr = portBox.Text;
            string serverNr = serverBox.Text;
            string usernameNr = UsernameBox.Text;
            string passwordNr = passwordBoxText.Password.ToString();
            var myInf = new IniFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");
            myInf.Write("username", usernameNr, "login");
            myInf.Write("password", passwordNr, "login");
            myInf.Write("port", portNr, "login");
            myInf.Write("server_name", serverNr, "login");

        }
    }
}
