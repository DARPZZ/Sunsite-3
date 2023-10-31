using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sunsite_3
{
    public class Share
    {
        public string Content { get; set; }
        public string ForPosting { get; set; }

        public StreamWriter Writer { get; set; }
        public StreamReader Reader { get; set; }
        public TcpClient Client { get; set; }


    }
}
