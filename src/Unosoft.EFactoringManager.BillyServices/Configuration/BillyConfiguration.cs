using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.BillyServices.Configuration
{
    public class BillyConfiguration
    {
        public string UrlBase { get; set; }
        public string Token { get; set; }
        public string Market { get; set; }
        public int TimerPeriod { get; set; }
        public int TimeSearchBill { get; set; }
        public int TimeValidateEndoso { get; set; }
        public List<string> ScheduleValidEndorsementList { get; set; }
    }
}
