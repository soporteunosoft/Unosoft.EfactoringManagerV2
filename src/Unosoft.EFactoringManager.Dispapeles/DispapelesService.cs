using RestSharp;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Unosoft.EFactoringManager.Dispapeles.Configuration;
using Unosoft.EFactoringManager.Dispapeles.Dto;
using Unosoft.EfactoringManager.RadianDispapeles.Dto;

namespace Unosoft.EfactoringManager.RadianDispapeles
{
    public class DispapelesService
    {
        private readonly DispapelesConfiguration settings;

        public DispapelesService(IOptions<DispapelesConfiguration> settings)
        {
            this.settings = settings.Value;
        }

        private string RequestPOST(string url, string data)
        {
            var client = new RestClient(settings.UrlBase);
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(data);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception("Error conectando con el servicio");

            return response.Content?.TrimStart('"');
        }

        public LoginResponseDto Login(int idEmpresa, string usuario, string password)
        {
            var requestDto = new LoginRequestDto
            {
                IdEmpresa = idEmpresa,
                Usuario = usuario,
                Password = password
            };

            return JsonConvert.DeserializeObject<LoginResponseDto>(
                RequestPOST("TransversalLogin/login", JsonConvert.SerializeObject(requestDto))
            );
        }

        public ConsultaEventoDianResponseDto ConsultarDian(int idEmpresa, string token, string cufe)
        {
            var requestDto = new EventosDianRequestDto
            {
                IdEmpresa = idEmpresa,
                Token = token,
                Cufe = cufe
            };

            var url = "https://a31wkyf2w0.execute-api.us-east-1.amazonaws.com/2021_01_22_INIT/fact-e/consultarEventoDian";

            return JsonConvert.DeserializeObject<ConsultaEventoDianResponseDto>(
                RequestPOST(url, JsonConvert.SerializeObject(requestDto))
            );
        }

        public CrearEventoResponseDto CrearEvento(CrearEventoRequestDto evento)
        {
            var url = "https://a31wkyf2w0.execute-api.us-east-1.amazonaws.com/2021_01_22_INIT/fact-e/crearEvento";

            return JsonConvert.DeserializeObject<CrearEventoResponseDto>(
                RequestPOST(url, JsonConvert.SerializeObject(evento))
            );
        }
    }
}
