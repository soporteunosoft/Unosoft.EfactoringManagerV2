using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.FTP
{
    public class FtpFile
    {
        
        public string FileName { get; set; }
        public string FullName { get; set; }
        public Stream FileStream { get; set; }
    }
}
