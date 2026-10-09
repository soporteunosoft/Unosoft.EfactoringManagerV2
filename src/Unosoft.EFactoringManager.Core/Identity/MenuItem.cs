using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity
{
    public class MenuItem
    {
        public string name { get; set; }
        public string permissionName { get; set; }
        public string icon { get; set; }
        public string route { get; set; }
        public List<MenuItem> Items { get; set; }
       
    }
}
