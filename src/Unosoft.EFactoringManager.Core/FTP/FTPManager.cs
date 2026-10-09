using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using CsvHelper;
using FluentFTP;
using Microsoft.Extensions.Options;
using Unosoft.EfactoringManager.EBillServices.Configuration;

namespace Unosoft.EFactoringManager.FTP
{
    public class FTPManager : IDomainService
    {
        readonly EBillFtpConfiguration settings;

        public FTPManager(IOptions<EBillFtpConfiguration> _settings)
        {
            settings = _settings.Value;
        }


        private FtpClient ConfigClient()
        {
            var client = new FtpClient(settings.Host, settings.UserName, settings.Password, settings.Port);
            client.Config.EncryptionMode = FtpEncryptionMode.None;// FtpEncryptionMode.Implicit; 
            client.Config.ConnectTimeout = 600000;
            client.Config.ReadTimeout = 60000;
            client.Config.DataConnectionType = FtpDataConnectionType.AutoActive;
            client.Config.DataConnectionEncryption = true;
            client.Config.DataConnectionType = FtpDataConnectionType.PASV;
            client.Config.ValidateAnyCertificate = true;

            return client;

        }

        public void UploadFile(Stream fileStream, string uploadFileName, string directory)
        {
            using (var client = ConfigClient())
            {
                client.Connect();
                client.UploadStream(fileStream, $"{directory}/{uploadFileName}.temp");
                client.MoveFile($"{directory}/{uploadFileName}.temp", $"{directory}/{uploadFileName}");
                client.Disconnect();
            }
        }


        public List<FtpFile> DownloadFile(string directory)
        {

            var result = new List<FtpFile>();

            using (var client = ConfigClient())
            {
                client.Connect();

                foreach (FtpListItem item in
                    client.GetListing(directory))
                {
                    var extension = Path.GetExtension(item.Name).ToLower();

                    Stream outStream = new MemoryStream();
                    // if this is a file
                    if (item.Type == FtpObjectType.File &&
                        (extension == ".csv" || extension == ".txt" || extension == ".zip")
                        && client.DownloadStream(outStream, item.FullName))
                    {
                        outStream.Position = 0;
                        result.Add(new FtpFile() { FileStream = outStream, FileName = item.Name, FullName = item.FullName });
                        //client.MoveFile(item.FullName, $"{item.FullName}.rev");
                    }
                }
                client.Disconnect();
                return result;

            }
        }

        public void UpdateFile(FtpFile ftpfile)
        {
            using (var client = ConfigClient())
            {
                client.Connect();

                client.MoveFile(ftpfile.FullName, $"{ftpfile.FullName}.rev");

                client.Disconnect();


            }
        }

        public EBillFtpConfiguration GetSettings()
        {
            return settings;
        }
    }
}
