using Abp.Dependency;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.EBillServices.DTO;

namespace Unosoft.EfactoringManager.EBillServices
{

    public static class TipoArchivosTipoC
    {
        public const string ENDOSABLE = "endosables";
        public const string NOENDOSABLE = "noendosables";
    }


    public class FileEBillManager : ITransientDependency
    {

        public FileEBillManager()
        {

        }


        public List<T> ProcesarArchivo<T>(Stream fileStream) where T : class
        {
            var reader = new StreamReader(fileStream);
            reader.BaseStream.Position = 0;
            reader.DiscardBufferedData();
            var engine = new FileHelperEngine<T>();
            return engine.ReadStream(reader).ToList();
        }


    }
}
