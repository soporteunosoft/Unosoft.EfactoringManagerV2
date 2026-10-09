using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unosoft.EfactoringManager.RadianDispapeles
{
    public class EFactoringManagerRadianDispapelesModule : AbpModule
    {

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EFactoringManagerRadianDispapelesModule).GetAssembly());
        }
    }
}
