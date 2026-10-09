using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Unosoft.EfactoringManager.EBillServices
{
    public  class EFactoringManagerEBillServiceModule : AbpModule
    {


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerEBillServiceModule).GetAssembly());
        }

    }
}
