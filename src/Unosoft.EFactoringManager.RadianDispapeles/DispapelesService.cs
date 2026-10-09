using System;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.RadianDispapeles.Dto;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Abp.Dependency;

namespace Unosoft.EfactoringManager.RadianDispapeles
{
    public class DispapelesService : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;
        public DispapelesService(IConfigurationRoot configuration )
        {
            _appConfiguration = configuration;
        }

        public string Login(int idEmpresa, string usuario, string password)
        {
            var requestDto = new LoginRequestDto()
            {
                IdEmpresa = idEmpresa,
                Usuario = usuario,
                Password = password
            };

                
            string urlLogin = "https://ria8x3qvge.execute-api.us-east-1.amazonaws.com/TransversalLogin/login";
            //urlLogin = urlLogin + "?idEmpresa=" + idEmpresa + "&usuario=" + usuario+ "&password="+ password;
            var client = new RestClient(urlLogin);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(requestDto);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            content = content.TrimStart('"');
            var respuesta = JsonConvert.DeserializeObject<LoginResponseDto>(content);

            if (respuesta.CodigoRespuesta == 200)
                return respuesta.Token;

            return content.TrimEnd('"');

        }

        public string ConsultarDian()
        {
            return null;
        }
    }
}
