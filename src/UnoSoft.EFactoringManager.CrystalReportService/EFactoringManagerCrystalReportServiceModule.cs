using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoSoft.EFactoringManager.CrystalReportService
{
  

    public class EFactoringManagerCrystalReportServiceModule : AbpModule
    {


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerCrystalReportServiceModule).GetAssembly());
        }

    }
}
