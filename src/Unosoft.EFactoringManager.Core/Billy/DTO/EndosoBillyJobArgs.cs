using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Billy.DTO
{
    public class EndosoBillyJobArgs : BillyJobArgs, ICloneable
    {
        public List<string> Titulos { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    
}
