using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.EBillServices.Configuration;
using Unosoft.EFactoringManager.BillyServices.DTO;
using Unosoft.EFactoringManager.BillyServices.Helper.Rest;

namespace Unosoft.EFactoringManager.BillyServices
{
    public class BillyProxy
    {
        string apiUrl;
        private readonly RestClientHelper _restClientHelper;
        readonly string authorization;
        readonly string _market;

        #region Constructor de la clase
        public BillyProxy(string url, string token, string market)
        {
            apiUrl = url;
            _restClientHelper = new RestClientHelper();
            authorization = token;
            _market = market;
        }
        #endregion

        #region Metodos de la clase
        public async Task<BillyResponse<CargaRespondeDTO>> CargarFacturaCufe(string cufe)
        {
            var url = $"{apiUrl}invoices/uploadByCufe";

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            var body = new CargaFacturaRequest
            {
               Cufe = cufe
            };

            return await _restClientHelper.PostAsync<CargaFacturaRequest, CargaRespondeDTO>(url, body, header);
        }

        public async Task<BillyResponse<EncontrarFacturaResponseDTO>> EncontrarFacturaCufe(string cufe)
        {
            var url = $"{apiUrl}invoices?cufe=" + cufe;

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            return await _restClientHelper.GetAsync<EncontrarFacturaResponseDTO>(url, header);
        }

        public async Task<BillyResponse<CargaRespondeDTO>> EndosoFacturaCufe(string cufe, double annualPercentageRate)
        {
            var url = $"{apiUrl}transactions";

            var header = new Dictionary<string, string>
            {
                {"Authorization",  $"Bearer {authorization}" }
            };

            var body = new EndosoFacturaRequest
            {
                Cufe = cufe,
                Market = _market,
                AnnualPercentageRate = annualPercentageRate
            };

            return await _restClientHelper.PostAsync<EndosoFacturaRequest, CargaRespondeDTO>(url, body, header);
        }
        #endregion
    }
}