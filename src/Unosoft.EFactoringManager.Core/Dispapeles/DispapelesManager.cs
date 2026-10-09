using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.RadianDispapeles;
using Unosoft.EfactoringManager.RadianDispapeles.Dto;
using Unosoft.EFactoringManager.Dispapeles.Dto;
using Unosoft.EFactoringManager.Oficina;
using Unosoft.EFactoringManager.TiposEventosRadian;
using Unosoft.EFactoringManager.ValidarEventosRadian;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.Dispapeles
{
    public class DispapelesManager : IDomainService
    {
        private readonly DispapelesService _dispapelesService;
        private readonly OficinaManager _oficinaManager;
        private readonly ValidarEventosRadianManager _validarEventosRadianManager;
        private readonly TiposEventosRadianManager _tiposEventosManager;

        public DispapelesManager(DispapelesService dispapelesService,
                                 OficinaManager oficinaManager,
                                 ValidarEventosRadianManager validarEventosRadianManager,
                                 TiposEventosRadianManager tiposEventosManager
            )
        {
            _dispapelesService = dispapelesService;
            _oficinaManager = oficinaManager;
            _validarEventosRadianManager = validarEventosRadianManager;
            _tiposEventosManager = tiposEventosManager;
        }

        public string Login(int idEmpresa, string usuario, string password)
        {
            var result = _dispapelesService.Login(idEmpresa, usuario, password);
            return result.Token;
        }

        public async Task<ConsultaEventoDianResponseDto> ConsultarFactura(string cufe)
        {

            var oficina = await _oficinaManager.Get(1);


            if (oficina.FecTokenDispapeles < DateTime.Now)
            {
                oficina.TokenDispapeles = Login((int)oficina.IdEmpresaDispapeles, oficina.UsuarioDispapeles, oficina.PasswordDispapeles);
                oficina.FecTokenDispapeles = DateTime.Now.AddDays(1);
                //Actualizar token en BD
                await _oficinaManager.Update(oficina);
            }

            return _dispapelesService.ConsultarDian((int)oficina.IdEmpresaDispapeles, oficina.TokenDispapeles, cufe);

        }


        public async Task CrearEvento(string tipoEvento, string tipoOperacion, Identity.Titulos titulo)
        {

            var oficina = await _oficinaManager.Get(1);


            if (oficina.FecTokenDispapeles < DateTime.Now)
            {
                oficina.TokenDispapeles = Login((int)oficina.IdEmpresaDispapeles, oficina.UsuarioDispapeles, oficina.PasswordDispapeles);
                oficina.FecTokenDispapeles = DateTime.Now.AddDays(1);
                //Actualizar token en BD
                await _oficinaManager.Update(oficina);
            }

            var tipoEventoRadian = await _tiposEventosManager.ObtenerTiposEventosRadianPorTipo(tipoEvento);
                      

            CrearEventoRequestDto evento = new CrearEventoRequestDto()
            {
                IdEmpresa = (int)oficina.IdEmpresaDispapeles,
                Token = oficina.TokenDispapeles,
                CodigoEvento = tipoEvento,
                TipoOperacion = tipoOperacion
            };


            #region Asignación campos FEL_EVENTO
            foreach (string campo in tipoEventoRadian.CamposObligatorios.Where(x => x.TipoObjeto == TipoObjetosEventosDian.Fel_Evento)
                                                  .Select(x=> x.NombreAtributo))
            {

                switch (campo.Trim())
                {
                    case CamposEventosFelEvento.ReferenciaTipoOperacion:
                        evento.ReferenciaTipoOperacion = "";
                        break;
                    case CamposEventosFelEvento.Moneda:
                        evento.Mondeda = "COP";
                        break;
                    case CamposEventosFelEvento.CodigoCantidadDocumentos:
                        evento.CodigoCantidadDocumentos = "";
                        break;
                    case CamposEventosFelEvento.Facultades:
                        evento.Facultades = "";
                        break;
                    case CamposEventosFelEvento.DescripcionFacultades:
                        evento.DescripcionFacultades = "";
                        break;
                    case CamposEventosFelEvento.Notas:
                        evento.Notas = tipoEventoRadian.Notas.Select(x => x.TiposNotasRadian.NomNota).ToList();
                        break;
                    case CamposEventosFelEvento.FechaActuarMandatario:

                        break;

                }

            }

            #endregion


            #region Asignación Documento de referencia
            if (tipoEventoRadian.CamposObligatorios.Any(x => x.TipoObjeto == TipoObjetosEventosDian.Fact_DocumentoReferenciado))
            {
                var documentoReferenciado = new Fact_DocumentoReferenciado();


                foreach (string campo in tipoEventoRadian.CamposObligatorios.Where(x => x.TipoObjeto == TipoObjetosEventosDian.Fact_DocumentoReferenciado)
                                                 .Select(x => x.NombreAtributo))
                {
                    switch (campo.Trim())
                    {
                        case CamposEventosDocumentoReferenciado.Consecutivo:
                            documentoReferenciado.Consecutivo  = titulo.IdTitulo;
                            break;
                        case CamposEventosDocumentoReferenciado.CufeCude:
                            documentoReferenciado.CufeCude = titulo.CufeTitulo;
                            break;
                        case CamposEventosDocumentoReferenciado.TipoDocumento:
                            documentoReferenciado.TipoDocumento = "01";
                            break;
                        case CamposEventosDocumentoReferenciado.CodigoTiempoMandato:
                            documentoReferenciado.CodigoTiempoMandato = "";
                            break;
                        case CamposEventosDocumentoReferenciado.DescripcionEvento:
                            documentoReferenciado.DescripcionEvento = "";
                            break;

                    }
                }
                evento.DocumentosReferenciado = new List<Fact_DocumentoReferenciado>() { documentoReferenciado };

            }

            #endregion

            #region Asignación Generador del Evento
            if (tipoEventoRadian.CamposObligatorios.Any(x => x.TipoObjeto == TipoObjetosEventosDian.Fel_GeneradorEvento))
            {
                var generadorEvento = new Fact_PersonaNaturalJuridica();


                foreach (string campo in tipoEventoRadian.CamposObligatorios.Where(x => x.TipoObjeto == TipoObjetosEventosDian.Fel_GeneradorEvento)
                                                 .Select(x => x.NombreAtributo))
                {
                    switch (campo)
                    {
                        case CamposEventosPersonaNaturalJuridica.TipoDocumento:
                            generadorEvento.TipoDocumento = "31";
                            break;
                        case CamposEventosPersonaNaturalJuridica.NumeroDocumento:
                            generadorEvento.NumeroDocumento = "860028580";
                            break;
                        case CamposEventosPersonaNaturalJuridica.NombreCompleto:
                            generadorEvento.NombreCompleto = "Dispapeles sas";
                            break;
                        case CamposEventosPersonaNaturalJuridica.TipoPersona:
                            generadorEvento.TipoPersona = "1";
                            break;
                        case CamposEventosPersonaNaturalJuridica.TipoFactor:
                            generadorEvento.TipoFactor = "";
                            break;

                    }
                }
                evento.GeneradorEvento = new List<Fact_PersonaNaturalJuridica>() { generadorEvento };

            }

            #endregion

            #region Asignación información campo valor

            if (tipoEventoRadian.CamposObligatorios.Any(x => x.TipoObjeto == TipoObjetosEventosDian.InformacionCampoValor))
            {
                evento.InformacionCampoValor = new List<Fact_ObjetoComun>();


                foreach (string campo in tipoEventoRadian.CamposObligatorios.Where(x => x.TipoObjeto == TipoObjetosEventosDian.InformacionCampoValor)
                                                 .Select(x => x.NombreAtributo))
                {
                    switch (campo)
                    {
                        case CamposEventosInformacionCampoValor.ValorFEVaval:
                            evento.InformacionCampoValor.Add(new Fact_ObjetoComun()
                            {
                                Nombre = campo,
                                Valor="0" 
                            });
                            break;
                        case CamposEventosInformacionCampoValor.ValorPagado:
                            evento.InformacionCampoValor.Add(new Fact_ObjetoComun()
                            {
                                Nombre = campo,
                                Valor = "0"
                            });
                            break;
                        case CamposEventosInformacionCampoValor.NuevoValorTV:
                            evento.InformacionCampoValor.Add(new Fact_ObjetoComun()
                            {
                                Nombre = campo,
                                Valor = "0"
                            });
                            break;
                    }
                }              

            }

            #endregion

            var result = _dispapelesService.CrearEvento(evento);
        }

    }
}
