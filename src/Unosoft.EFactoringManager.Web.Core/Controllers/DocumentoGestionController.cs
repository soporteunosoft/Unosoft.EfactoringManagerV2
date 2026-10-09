using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.GestionesTerceros;
using Unosoft.EFactoringManager.Web;

namespace Unosoft.EFactoringManager.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DocumentoGestionController : EFactoringManagerControllerBase
    {
        private readonly GestionesTercerosManager _gestionesTercerosManager;
        private readonly UserManager _userManager;

        public DocumentoGestionController(
            GestionesTercerosManager gestionesTercerosManager,
            UserManager userManager)
        {
            _gestionesTercerosManager = gestionesTercerosManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                long idUsuario = AbpSession.UserId.Value;
                var httpRequest = HttpContext.Request;

                var parser = await MultipartParser.CreateAsync(httpRequest.Body);

                if (!parser.Success)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var user = await _userManager.GetUserByIdAsync(idUsuario);

                string numeroTercero = Encoding.UTF8
                    .GetString(parser.MyContents[0].Data)
                    .Replace("\r", "");

                string idOficina = Encoding.UTF8
                    .GetString(parser.MyContents[1].Data)
                    ?.Replace("\r", "");

                string idGestion = Encoding.UTF8
                    .GetString(parser.MyContents[2].Data)
                    ?.Replace("\r", "");

                string idDocumentoGestion = Encoding.UTF8
                    .GetString(parser.MyContents[3].Data)
                    ?.Replace("\r", "");

                var gestion =
                    await _gestionesTercerosManager
                        .ObtenerGestionesTercerosPorId(
                            int.Parse(idGestion),
                            int.Parse(idOficina));

                if (gestion == null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var fileName =
                    $"{Guid.NewGuid()}{Path.GetExtension(parser.Filename).ToLower()}";

                bool result =
                    _gestionesTercerosManager.SubirDocumento(
                        int.Parse(idOficina),
                        int.Parse(idGestion),
                        int.Parse(numeroTercero),
                        int.Parse(idDocumentoGestion),
                        parser.FileContents,
                        fileName);

                return result
                    ? new HttpResponseMessage(HttpStatusCode.Created)
                    : new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch (IOException ex)
            {
                throw new Exception(
                    "A generic error occurred. Please try again later. " + ex.Message);
            }
        }
    }
}