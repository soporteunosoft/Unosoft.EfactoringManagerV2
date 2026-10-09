using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Unosoft.EFactoringManager.Dto;
using Unosoft.EFactoringManager.Terceros.Dto;
using Unosoft.EFactoringManager.Authorization.Users;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Certificado.Dto;
using Unosoft.EfactoringManager.Certificados.Dto;

namespace Unosoft.EFactoringManager.Certificado
{


    public class CertificadoService : ApplicationService, ICertificadoService
    {
        #region Atributos
        CertificadosDigitales.CertificadoDigitalManager _certificadoDigitalManager;
        private readonly UserManager _userManager;

        #endregion

        public CertificadoService(CertificadosDigitales.CertificadoDigitalManager certificadoDigitalManager, UserManager userManager)
        {
            _certificadoDigitalManager = certificadoDigitalManager;
            _userManager = userManager;
        }

        //[AbpAuthorize]

        [HttpPost]
        public async Task<GetSingleRespuesta> CertificadosVinculacion(TerceroPerfilDto tercero)
        {
            try
            {
                var respuesta  = new RespuestasDto();


                switch (tercero.TipoCertificado)

                {
                    case (int)EFactoringManagerConsts.TiposCertificados.gRepresentanteLegal:
                    case (int)EFactoringManagerConsts.TiposCertificados.gPerteneceEmpresa:
                    case (int)EFactoringManagerConsts.TiposCertificados.gPerteneceJuridica:
                        respuesta = await _certificadoDigitalManager.CertificateVinculacionEmpresa(tercero.Id, tercero.VigenciaCert);
                        break;

                    case (int)EFactoringManagerConsts.TiposCertificados.gPersonaNatual:
                        respuesta = await _certificadoDigitalManager.CertificateVinculacionCertificados(tercero.Id, tercero.VigenciaCert);
                        break;

                    default:
                        respuesta.CodigoRespuesta = -1;
                        respuesta.MensajeRespuesta = "No existe el método para solicitar este tipo de certificado";
                        break;

                }


                return new GetSingleRespuesta(DateTime.Now, respuesta.CodigoRespuesta, respuesta.MensajeRespuesta);

            }

            catch (Exception e)
            {
                return new GetSingleRespuesta(DateTime.Now, -1, e.Message);
            }
        }


        [AbpAuthorize]
        [HttpGet]
        public async Task<GetSingleRespuesta> ValidarDocumentosCertificado(string idTercero)
        {
            try
            {
                GetSingleRespuesta _respuesta;
                DateTime localDate = DateTime.Now;
                if (await _certificadoDigitalManager.ValidarDocumentosCertificado(idTercero))
                {
                   return  _respuesta = new GetSingleRespuesta(localDate, 0, "Documentos Validos");
                }
                else
                {
                    return _respuesta = new GetSingleRespuesta(localDate, 1, "Documentos No Validos");
                }

            }
            catch(Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<bool> ValidarCertificado(string idTercero)
        {
            return await _certificadoDigitalManager.ValidarCertificado(idTercero);
        }

        //[AbpAuthorize]
        [HttpPost]

        //17/01/2022 Revisar la llamada de este metodo desde el front debe pasar el id del hijo
        public async Task<GetSingleRespuesta> FirmarDocumento(DocumentoFirmarInput documentoFirmar)
        {
            try
            {
                GetSingleRespuesta _respuesta;
                DateTime localDate = DateTime.Now;
                var respuesta = await _certificadoDigitalManager.FirmarDocumento(documentoFirmar.IdTercero, documentoFirmar.IdDetDoc, documentoFirmar.PinFirma, documentoFirmar.Formatoentrada, documentoFirmar.Formatosalida);

                return _respuesta = new GetSingleRespuesta(localDate, respuesta.CodigoRespuesta, respuesta.MensajeRespuesta);


            }
            catch (Exception)
            {
                throw;
            }
        }

        [AbpAuthorize]
        [HttpPost]
        public async Task<GetSingleRespuesta> FirmarDocumentoAutorizar(DocumentoFirmarInput documentoFirmar)
        {
            try
            {
                GetSingleRespuesta _respuesta;
                DateTime localDate = DateTime.Now;
                var respuesta = await _certificadoDigitalManager.FirmarDocumentoAutorizar(documentoFirmar.IdTercero, documentoFirmar.IdDetDoc, documentoFirmar.PinFirma, documentoFirmar.Formatoentrada, documentoFirmar.Formatosalida);

                return _respuesta = new GetSingleRespuesta(localDate, respuesta.CodigoRespuesta, respuesta.MensajeRespuesta);


            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<TerceroCertificadoDto> ConsultarCertificado(string idTercero, string idTerceroPadre)
        {
            return await _certificadoDigitalManager.ConsultarCertificadoTercero(idTercero, idTerceroPadre);
        }

    }
}
