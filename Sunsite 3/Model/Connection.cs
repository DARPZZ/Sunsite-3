using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sunsite_3.Model
{
    public class Connection
    {
        string userName;
        string password;
        public void Authenticate(TextBox OutputBox, StreamReader reader, StreamWriter writer)
        {
            ReadIniFile();

            writer.WriteLine("AUTHINFO USER " + userName);
            writer.Flush();
            string response = reader.ReadLine();

            OutputBox.AppendText(response + Environment.NewLine);
            writer.WriteLine("AUTHINFO PASS " + password);
            writer.Flush();
            for (int i = 0; i < 2; i++)
            {
                response = reader.ReadLine();
                OutputBox.AppendText(response + Environment.NewLine);

            }
        }
        public void ReadIniFile()
        {
            IniFile infFil = new IniFile(@"C:\Users\rasmu\Documents\Sunsite webserver\Login.ini");
            userName = infFil.Read("username", "login");
            password = infFil.Read("password", "login");
        }
    }
}
