using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Titulos
{
    public class TitulosFileInfo
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }

        public TitulosFileInfo(string FileName, byte[] content)
        {
            this.FileName = FileName;
            this.Content = content;
            this.Extension = Path.GetExtension(FileName).ToLower(); 

        }
    }
}
