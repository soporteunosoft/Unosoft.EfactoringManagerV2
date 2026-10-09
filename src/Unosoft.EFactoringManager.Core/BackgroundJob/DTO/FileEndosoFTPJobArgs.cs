using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BackgroundJob.DTO
{
    public class FileEndosoFTPJobArgs : FileFTPJobArgs, ICloneable
    {
        public List<TituloFTP> Titulos { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    } 
}
