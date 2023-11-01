using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sunsite_3.Model
{
    public class Search
    {
        public void search(StreamWriter writer, StreamReader reader,ListBox ListboxList,string searchWord)
        {
            ListboxList.Items.Clear();
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
                    if (parts.Contains(searchWord))
                    {
                        ListboxList.Items.Add(parts[0] + Environment.NewLine);
                    }
                }
            }

        }
    }
}
