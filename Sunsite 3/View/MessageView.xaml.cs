using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Sunsite_3.ViewModel;

namespace Sunsite_3.View
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    public partial class MessageView : UserControl
    {
        public MessageView()
        {
            InitializeComponent();
            var he = App.Sharedata.Content;
            
            Resultat.Text = he;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((App)App.Current).ChangeUserControl(typeof(HomeViewModel));
        }
    }
}
