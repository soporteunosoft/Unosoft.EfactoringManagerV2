using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.Certificados.Dto.FirmaDigital;
using Unosoft.EfactoringManager.Certificados.Dto.FirmaElectronica;
using Unosoft.EfactoringManager.Certificados.Enum;
using Unosoft.EfactoringManager.Certificados.Helper.Rest;

namespace Unosoft.EfactoringManager.Certificados
{
    public class FirmaElectronicaProxy
    {

        string apiUrl;
        private readonly RestClientHelper _restClientHelper;
        private readonly Task<string> _authorizationTask;

        #region Constructor de la clase
        public FirmaElectronicaProxy(string url, string username, string password)
        {
            apiUrl = url;
            _restClientHelper = new RestClientHelper();
            _authorizationTask = AuthenticateAsync(username, password);
        }
        #endregion

        #region Metodos de la clase

        private async Task<string> AuthenticateAsync(string username, string password)
        {
            var url = $"{apiUrl}/api/users/authenticate";

            AuthentiqueRequest request = new AuthentiqueRequest()
            {
                UserName = username,
                Password = password
            };

            var response = await _restClientHelper.PostAsync<AuthentiqueRequest, AuthentiqueResponse>(url, request);

            return response.Token;
        }


        public async Task<FEResponseDTO> Upload(string filePath)
        {
            var authorization = await _authorizationTask;
            var url = $"{apiUrl}/api/Transaccion/upload/doc";

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            return await _restClientHelper.Upload<FEResponseDTO>(url, filePath, header);
        }

        public async Task<FEResponseDTO> Create(TipoFirmaElectronica tipoFirmaElectronica, FirmaDigitalRequest request)
        {
            switch (tipoFirmaElectronica)
            {
                case TipoFirmaElectronica.Simple:
                    return await SimpleCreate(request);

                case TipoFirmaElectronica.Avanzada:
                    return await AvanzadaCreate(request);

                default:
                    return await StandardCreate(request);
            }
        }

        private async Task<FEResponseDTO> StandardCreate(FirmaDigitalRequest request)
        {
            var url = $"{apiUrl}/api/Transaccion/create";
            return await CreateInternal(url, request);
        }

        private async Task<FEResponseDTO> SimpleCreate(FirmaDigitalRequest request)
        {
            var url = $"{apiUrl}/api/Transaccion/create/firma/simple";
            return await CreateInternal(url, request);
        }

        private async Task<FEResponseDTO> AvanzadaCreate(FirmaDigitalRequest request)
        {
            var url = $"{apiUrl}/api/Transaccion/create/firma/avanzada";
            return await CreateInternal(url, request);
        }

        private async Task<FEResponseDTO> CreateInternal(string url, FirmaDigitalRequest request)
        {
            var authorization = await _authorizationTask;

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            return await _restClientHelper.PostAsync<FirmaDigitalRequest, FEResponseDTO>(url, request, header);
        }


        public async Task<bool> Anular(string id)
        {
            var authorization = await _authorizationTask;
            var url = $"{apiUrl}/api/informe/get/proceso/anular";
            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            var result = await _restClientHelper.PostAsync<AnularProcesoDto, FEResponseDTO>(url, new AnularProcesoDto() { Id = id, Nota = "Anulación desde el sistema Oficina Virtual" }, header);

            return result.Estado == true;
        }

        public async Task<FEResponseDTO> Consultar(string id)
        {
            var authorization = await _authorizationTask;
            var url = $"{apiUrl}/api/Transaccion/{id}";

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            return await _restClientHelper.GetAsync<FEResponseDTO>(url, header);
        }
        #endregion
    }
}
