using Abp.Domain.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.Certificados;
using Unosoft.EfactoringManager.Certificados.Dto;
using Unosoft.EFactoringManager.CertificadosDigitales.Configuration;
using Unosoft.EFactoringManager.FirmasDetalleDocumentos;
using Unosoft.EFactoringManager.OrdenantesTiposCertificados;
using static Unosoft.EFactoringManager.EFactoringManagerConsts;

namespace Unosoft.EFactoringManager.CertificadosDigitales
{

    public class ProcessResponse
    {
        [JsonProperty("estado")]
        public string Estado { get; set; }
    }

    public class ConsultarCertResponse
    {
        //[{"finvigencia":"2022-01-02 08:53:00","serial":"7F:86:1A:4F:41:0B:6F:FE","tipoCertificado":"8"}]
        [JsonProperty("finvigencia")]
        public string Finvigencia { get; set; }
        [JsonProperty("serial")]
        public string Serial { get; set; }
        [JsonProperty("tipoCertificado")]
        public string TipoCertificado { get; set; }
    }
    public class CertificadoDigitalManager : IDomainService
    {
        #region Atributos
        private readonly CertificadosProxy _certificadosProxy;
        private readonly Terceros.TercerosManager _tercerosManager;
        //private readonly OrdenantesTerceros.OrdenantesTercerosManager _ordenantesTercerosManager;
        private readonly Ciudades.CiudadesManager _ciudadesManager;
        private readonly DetalleDocumentos.DetalleDocumentosManager _detalleDocumentosManager;
        private readonly OrdenantesTiposCertificadosManager _ordenantesTiposCertificadosManager;
        private readonly AndesConfiguration _andesConfiguration;
        private readonly FirmasDetalleDocumentosManager _firmasDetalleDocumentosManager;

        #endregion

        public CertificadoDigitalManager(Terceros.TercerosManager tercerosManager,
                                         Ciudades.CiudadesManager ciudadesManager,
                                         DetalleDocumentos.DetalleDocumentosManager detalleDocumentosManager,
                                         OrdenantesTiposCertificadosManager ordenantesTiposCertificadosManager,
                                         IOptions<AndesConfiguration> _settings,
                                         FirmasDetalleDocumentosManager firmasDetalleDocumentosManager
                                         )
        {
            _certificadosProxy = new CertificadosProxy(_settings.Value);
            _tercerosManager = tercerosManager;
            _ciudadesManager = ciudadesManager;
            _detalleDocumentosManager = detalleDocumentosManager;
            _ordenantesTiposCertificadosManager = ordenantesTiposCertificadosManager;
            _firmasDetalleDocumentosManager = firmasDetalleDocumentosManager;
            _andesConfiguration = _settings.Value;
        }

        #region [Deprecated]
        /*
        public RespuestasDto CertificateVinculacionEmpresa(Identity.OrdenantesTerceros ordenante)
        {
            CertificateVinculacionEmpresaInput vinculacion = new CertificateVinculacionEmpresaInput();

            //Modificar Data Terceros Hijo y Padre
            var tercero = _tercerosManager.ObtenerTerceroPorId(ordenante.IdTercero);
            var ordenanteTercero = _ordenantesTercerosManager.ObtenerOrdenantePorId(ordenante.IdOrdenanteTercero);
            var ciudadEnt = _ciudadesManager.ObntenerCiudadPorId(tercero.IdCiudad.Value);


            if (ordenanteTercero.TipoOrdenante == 9)
            {
                vinculacion.tipoCert = 8;
            }
            else
            {
            }
            //vinculacion.tipoDoc = 1;
            vinculacion.tipoDoc = ordenanteTercero.TipoIdentificacionOrd;
            vinculacion.documento = ordenanteTercero.IdOrdenanteTercero;
            vinculacion.nombres = ordenanteTercero.NomOrdenanteCliente;
            vinculacion.apellidos = ordenanteTercero.ApeOrdenanteCliente;
            vinculacion.municipio = Convert.ToInt32(ordenanteTercero.Ciudad.CodDaneCiudad);
            vinculacion.direccion = ordenanteTercero.DirOficinaOrdCliente;
            vinculacion.email = ordenanteTercero.EmailOrdCliente;
            vinculacion.emailEnt = tercero.Email;
            //vinculacion.telefono = "";
            vinculacion.telefono = ordenanteTercero.TelOficinaOrdCliente;
            vinculacion.celular = ordenanteTercero.TelMovilOrdCliente;
            //vinculacion.ocupacion = "";
            vinculacion.ocupacion = ordenanteTercero.ProfesionOrdCliente;
            vinculacion.tipoDocEnt = 2;//Tipo Documento de la Empresa
            vinculacion.documentoEnt = tercero.IdTercero;
            vinculacion.razonsocial = tercero.RazonSocial;
            vinculacion.municipioEnt = Convert.ToInt32(ciudadEnt.CodDaneCiudad);
            vinculacion.direccionEnt = tercero.DirOficina;
            vinculacion.cargo = ordenanteTercero.ProfesionOrdCliente;
            vinculacion.unidadOrganizacional = ordenanteTercero.AreaOrdCliente;

            vinculacion.fechaCert = DateTime.Now.ToString("yyyy-MM-dd");
            vinculacion.formato = 4;
            //vinculacion.vigenciaCert = 15;
            vinculacion.vigenciaCert = 19;//30 DIAS
            vinculacion.testigo = "";
            vinculacion.foto = "";
            vinculacion.pin = "";
            vinculacion.pkcs10 = "";

            //Mejorar dinámica por base de datos tabla:OrdenantesTiposCertificados
            vinculacion.soporte = CargarDocumentosSoporte(tercero);
            vinculacion.verific_doc = "";
            vinculacion.plantillaHuella = "";

            var repuesta = _certificadosProxy.CertificateVinculacionEmpresaService(vinculacion);

            if (repuesta.CodigoRespuesta == 0)
            {
                ordenante.Certificado = true;
                //ordenante.FecVigenciaCert = DateTime.Now.AddYears(2);
                ordenante.FecVigenciaCert = DateTime.Now.AddDays(30);//ADICIONA 30 DIAS
                ordenante.NumSolicitudCert = repuesta.MensajeRespuesta;

                _ordenantesTercerosManager.ActualizarOrdenantePorId(ordenante);
            }

            return repuesta;
        }

        */
        #endregion


        public async Task<RespuestasDto> CertificateVinculacionCertificados(int numeroTercero, int vigenciaCert)
        {

            CertificadosInput vinculacion = new CertificadosInput();

            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);


            if (tercero.VigenciaCert == null && tercero.FechaCert != null
                && tercero.FechaCert.Value.AddDays(2) > DateTime.Now)
            {
                return new RespuestasDto()
                {
                    CodigoRespuesta = 1000,
                    MensajeRespuesta = "Existe una solicitud de  certificado digital vigente"
                };
            }


            vinculacion.tipoCert = tercero.TiposOrdenantes.TipoCertificado.Value;
            vinculacion.tipoDoc = tercero.TiposIdentificacion.TipoIdentificacionAndes.Value;
            vinculacion.documento = tercero.IdTercero;
            vinculacion.nombres = tercero.NomTercero;
            vinculacion.apellidos = tercero.ApeTercero;
            vinculacion.municipio = Convert.ToInt32(tercero.Ciudades.CodDaneCiudad);
            vinculacion.direccion = tercero.DirOficina;
            vinculacion.email = tercero.Email;
            vinculacion.telefono = tercero.TelOficina;
            vinculacion.celular = tercero.TelMovil;
            vinculacion.ocupacion = tercero.Profesion;
            vinculacion.fechaCert = DateTime.Now.ToString("yyyy-MM-dd");

            //11/02/2022: WIGELO: Los datos formato y vigenciaCer están trocados en el manual tecnico se confirmo con ANDES
            vinculacion.formato = 4;//30 DIAS
            //vinculacion.vigenciaCert = 15;
            //vinculacion.vigenciaCert = 19;
            vinculacion.vigenciaCert = vigenciaCert; // tercero.VigenciaCert ?? 19;
            vinculacion.testigo = "";
            vinculacion.foto = "";
            vinculacion.pin = "";
            vinculacion.pkcs10 = "";

            //Mejorar dinámica por base de datos tabla:OrdenantesTiposCertificados
            vinculacion.soporte = await CargarDocumentosSoporte(tercero);
            vinculacion.verific_doc = "";
            vinculacion.plantillaHuella = "";

            var repuesta = _certificadosProxy.CertificadosService(vinculacion);


            if (repuesta.CodigoRespuesta == 0)
            {
                //Obtener los datos reales del certificado
                tercero.Certificado = true;
                tercero.FechaCert = DateTime.Now;
                tercero.FecVigenciaCert = DateTime.Now.AddDays(30);//ADICIONA 30 DIAS
                tercero.NumSolicitudCert = repuesta.NumeroSolicitud;
                await _tercerosManager.Actualizar(tercero);
            }

            return repuesta;
        }



        public async Task<RespuestasDto> CertificateVinculacionEmpresa(int numeroTercero, int vigenciaCert)
        {
            CertificateVinculacionEmpresaInput vinculacion = new CertificateVinculacionEmpresaInput();

            //Modificar Data Terceros Hijo y Padre
            var tercero = await _tercerosManager.ObtenerTerceroPorId(numeroTercero);

            //Preguntar a Arley si sobra esto
            if (await ValidarCertificado(tercero))
            {
                return new RespuestasDto()
                {
                    CodigoRespuesta = 1000,
                    MensajeRespuesta = "El Ordenante ya cuenta con un certificado digital vigente"
                };
            };
            

            if (!string.IsNullOrEmpty(tercero.NumSolicitudCert) && tercero.FechaCert != null )
            {
                TimeSpan difFechas = DateTime.Now - tercero.FechaCert.Value;
                int dias = difFechas.Days;

                if (dias < 3)
                {
                    return new RespuestasDto()
                    {
                        CodigoRespuesta = 1000,
                        MensajeRespuesta = "El Ordenante cuenta con una solicitud de certificado pendiente"
                    };
                }
            }

            vinculacion.tipoCert = tercero.TiposOrdenantes.TipoCertificado.Value;
            vinculacion.tipoDoc = tercero.TiposIdentificacion.TipoIdentificacionAndes.Value;
            vinculacion.documento = tercero.IdTercero;
            vinculacion.nombres = tercero.NomTercero;
            vinculacion.apellidos = tercero.ApeTercero;
            vinculacion.municipio = Convert.ToInt32(tercero.Ciudades.CodDaneCiudad);
            vinculacion.direccion = tercero.DirOficina;
            vinculacion.email = tercero.Email;
            vinculacion.emailEnt = tercero.Email;

            //vinculacion.telefono = "";
            vinculacion.telefono = tercero.TelOficina;
            vinculacion.celular = tercero.TelMovil;
            //vinculacion.ocupacion = "";
            vinculacion.ocupacion = tercero.Profesion;

            vinculacion.tipoDocEnt = tercero.TerceroPadre.TiposIdentificacion.TipoIdentificacion; //  2;//Tipo Documento de la Empresa
            vinculacion.documentoEnt = tercero.TerceroPadre.IdTercero;
            //vinculacion.razonsocial = tercero.RazonSocial;
            vinculacion.razonsocial = tercero.TerceroPadre.RazonSocial;
            vinculacion.municipioEnt = Convert.ToInt32(tercero.TerceroPadre.Ciudades.CodDaneCiudad);
            vinculacion.direccionEnt = tercero.TerceroPadre.DirOficina;
            vinculacion.cargo = tercero.Profesion;
            vinculacion.unidadOrganizacional = tercero.Area;

            vinculacion.fechaCert = DateTime.Now.ToString("yyyy-MM-dd");

            //Obtener de parametros
            //11/02/2022: WIGELO: Los datos formato y vigenciaCer están trocados en el manual tecnico se confirmo con ANDES
            vinculacion.formato = 4;
            //vinculacion.vigenciaCert = 15;
            // vinculacion.vigenciaCert = 19;//30 DIAS
            vinculacion.vigenciaCert = vigenciaCert; //  tercero.VigenciaCert ?? 19;
            vinculacion.testigo = "";
            vinculacion.foto = "";
            vinculacion.pin = "";
            vinculacion.pkcs10 = "";

            //Mejorar dinámica por base de datos tabla:OrdenantesTiposCertificados
            vinculacion.soporte = await CargarDocumentosSoporte(tercero);
            vinculacion.verific_doc = "";
            vinculacion.plantillaHuella = "";

            var repuesta = _certificadosProxy.CertificateVinculacionEmpresaService(vinculacion);


            if (repuesta.CodigoRespuesta == 0)
            {
                //Obtener los datos reales del certificado
                tercero.Certificado = true;
                tercero.FechaCert = DateTime.Now;
                //tercero.FecVigenciaCert = DateTime.Now.AddDays(30);//ADICIONA 30 DIAS
                tercero.NumSolicitudCert = repuesta.MensajeRespuesta;

                //Revisar actualización
                //_ordenantesTercerosManager.ActualizarOrdenantePorId(ordenante);
                await _tercerosManager.Actualizar(tercero);
            }

            return repuesta;
        }


        public async Task<bool> ValidarCertificado(string idTercero)
        {

            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero);
            if (tercero != null)
            {
                var documentos = await _detalleDocumentosManager.ObtenerDetalleDocumentosPorTercero(idTercero);

                //TO DO: Crear constante para manejo de tipo firma
                //Pregunta porque se llaman igual
                if (!documentos.Any(doc => doc.Documento.Documentos.TipoFirma == (int)TipoFirma.Digital))
                    return true;

                //Obtiene los diferente padres
                var lstTercerosPadre = documentos.Select(x => x.Documento.Terceros.IdTercero).Distinct().ToList();

                var result = true;
                foreach (var idterceroPadre in lstTercerosPadre)
                {
                    result = result && await ValidarCertificado(tercero, idterceroPadre);
                }

                return result;
            }
            else
            {
                return false;
            }

        }


        public async Task<bool> ValidarCertificado(Identity.Terceros tercero, string idTerceroPadre = null)
        {

            if (!tercero.Firma )
                return true;

            ConsultarCertInput consultarCertInput = new ConsultarCertInput();

            consultarCertInput.TipoDoc = tercero.TiposIdentificacion.TipoIdentificacionAndes.Value;
            consultarCertInput.Documento = tercero.IdTercero;
            consultarCertInput.TipoDocEmp = 2;
            consultarCertInput.DocumentoEmp = idTerceroPadre ?? tercero.TerceroPadre.IdTercero;


            var respuesta = _certificadosProxy.ConsultarCertificadoXEmpresaService(consultarCertInput);


            if (respuesta.CodigoRespuesta == 0)
            {
                var response = JsonConvert.DeserializeObject<List<ConsultarCertResponse>>(respuesta.MensajeRespuesta);
                DateTime fecha = DateTime.Parse(response.Select(x => x.Finvigencia).FirstOrDefault());

                if (tercero.FecVigenciaCert == null || tercero.FecVigenciaCert < fecha)
                {
                    tercero.FecVigenciaCert = fecha;
                    await _tercerosManager.Actualizar(tercero);
                }

                return fecha > DateTime.Now;
            }
            else return false;


        }

        public  Tuple<string, int> ValidarSolicitud(Identity.Terceros tercero, string idTerceroPadre = null )
        {

                       

            ConsultarCertInput consultarCertInput = new ConsultarCertInput();

            consultarCertInput.TipoDoc = tercero.TiposIdentificacion.TipoIdentificacionAndes.Value;
            consultarCertInput.Documento = tercero.IdTercero;
            consultarCertInput.TipoDocEmp = 2;
            consultarCertInput.DocumentoEmp = idTerceroPadre ?? tercero.TerceroPadre?.IdTercero;
            consultarCertInput.NumeroSolicitud = Convert.ToInt32(tercero.NumSolicitudCert);

            var respuesta = _certificadosProxy.ConsultarSolicitudCertificado(consultarCertInput);

            return Tuple.Create(respuesta.MensajeRespuesta, respuesta.CodigoRespuesta);


        }



        public async Task<bool> ValidarDocumentosCertificado(string idTercero, int tipoOrdenante = 0)
        {
            List<Identity.DetalleDocumentos> documentos = new List<Identity.DetalleDocumentos>
            {
                await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipo(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gRut),
                await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipo(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gCedulaRepresentante),
                await _detalleDocumentosManager.ObtenerDetalleDocumentoPorTipo(idTercero, (int)EFactoringManagerConsts.DocumentoSoporte.gCamaraComercioRepresentante)
            };

            if (documentos.Exists(x => x == null))
            {
                return false;
            }

            if (documentos.Exists(x => x.DirDocumento == null || x.FecRenovacion < DateTime.Now))
            {
                return false;
            }

            return true;

        }

        public async Task<string> CargarDocumentosSoporte(Identity.Terceros tercero)
        {

            var listDocumentos = await _ordenantesTiposCertificadosManager.GetTiposDocumentos(tercero.TipoOrdenante.Value);

            //documentos del ordenante
            var detDocumentos = await _detalleDocumentosManager.GetDetalleDocumentoPorTipo(tercero.Id, listDocumentos.Where(x => x.IdentificaPersona).Select(x => x.TipoDoc).ToList());

            //documentos de la empresa
            detDocumentos.AddRange(await _detalleDocumentosManager.GetDetalleDocumentoPorTipo(tercero.TerceroPadre.Id, listDocumentos.Where(x => !x.IdentificaPersona).Select(x => x.TipoDoc).ToList()));


            if (detDocumentos.Exists(doc => doc.FecRenovacion < DateTime.Now))
            {
                throw new Exception("Existen documentos que se deben renovar");
            }

            if (detDocumentos.Exists(doc => doc.DirDocumento == null))
            {
                throw new Exception("No se pude solicitar el cerficado porque los documentos obligatorios no han sido adjuntos");
            }

            using (var compressedStream = new MemoryStream())
            {
                // Build ZIP in-memory with System.IO.Compression
                using (var archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, true))
                {
                    foreach (var detalle in detDocumentos)
                    {
                        var entryName = $"{detalle.Documentos.TipoDocumento.NomDocumento}.pdf";
                        var entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);

                        using (var entryStream = entry.Open())
                        using (var fileStream = File.OpenRead(detalle.DirDocumento))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                return Convert.ToBase64String(compressedStream.ToArray());
            }
        }
        //Adicionar parametro idTerceroOrdenante
        public async Task<RespuestasDto> FirmarDocumento(string idTercero, int idDetDoc, string pinFirma, int formatoentrada, int formatosalida)
        {
            int tipoDocumentoOrd;
            string parametrosFirma;
            string userAndes;
            string passAndes;
            string userAndesEstampa;
            string passAndesEstampa;
            string strPathJar;
            string mensajeRespuesta;
            int codigoRespuesta;
            int firmaPagina;

            string[] posicionFirmas = { "420,590,180,60", "420,500,180,60", "420,410,180,60", "420,327,180,60", "420,245,180,60", "420,165,180,60" };


            //El formato de las coordenadas es: X,Y,W,H

            var detalleDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(idDetDoc);


            var terceros = await _tercerosManager.ObtenerTerceroId(idTercero);

            var detalleFirma = detalleDocumento.FirmasDetalleDocumentos
                            .FirstOrDefault(x => terceros.Select(t => t.Id).ToList().Contains( x.NumTerceroFirma ));

          
            if (detalleFirma == null)
                throw new Exception("No se econtró información del firmante");

            var tercero = terceros.FirstOrDefault(x => x.Id == detalleFirma.NumTerceroFirma);

            var posicion = posicionFirmas[detalleFirma.OrdenFirma-1];

            var locaDocumento = detalleDocumento.DirDocumento;

            firmaPagina = CountPage(locaDocumento);

            tipoDocumentoOrd = tercero.TiposIdentificacion.TipoIdentificacionAndes.Value;

           
            userAndes = _andesConfiguration.UserName;
            passAndes = _andesConfiguration.Password;
            userAndesEstampa = _andesConfiguration.UserNameEstampa; 
            passAndesEstampa = _andesConfiguration.PasswordEstampa;
            strPathJar = _andesConfiguration.RutaJar;


            parametrosFirma = $"--metodofirma ws --formatofirma pdf  --login {userAndes} --password {passAndes} --tipodocumento {tipoDocumentoOrd} ";
            parametrosFirma += $"--documento {tercero.IdTercero} --pinfirma " + pinFirma + " --entrada " + locaDocumento;
            parametrosFirma += " --salida " + locaDocumento + " --formatoentrada archivo --formatosalida archivo" + " --visible true --imagenFirma ";
            parametrosFirma += $"{strPathJar}LogoFirma.png --ubicacion {posicion} --tamanofuentefirma 6 --pagina {firmaPagina} ";
            parametrosFirma += $"--aplicatsa true --tsausuario {userAndesEstampa} --tsapass {passAndesEstampa} ";
            parametrosFirma += $"--aplicaqr false --test {_andesConfiguration.Test ?? "false"} --proteger false ";


            codigoRespuesta =  Firmar(strPathJar, parametrosFirma, out mensajeRespuesta);
         

            if (codigoRespuesta == 0)
            {

                mensajeRespuesta = "Documento Firmado";
              
                var firmaDetalle = detalleDocumento.FirmasDetalleDocumentos.Count == detalleDocumento.FirmasDetalleDocumentos.Count(x => x.Firma) + 1;

                await _detalleDocumentosManager.ActualizarFirmaDetalleDocumento(tercero.TerceroPadre?.IdTercero, idDetDoc, firmaDetalle);

                //10/07/2022:WIGELO SE INCORPORA POR NUEVO FLUJO
                await _firmasDetalleDocumentosManager.ActualizarFirmasDetalleDocumentos(tercero.TerceroPadre?.IdTercero, tercero.Id, idDetDoc, true);

            }
            RespuestasDto respuestaConsulta = new RespuestasDto();
            respuestaConsulta.CodigoRespuesta = codigoRespuesta;
            respuestaConsulta.MensajeRespuesta = mensajeRespuesta;
            return respuestaConsulta;
        }

        public async Task<RespuestasDto> FirmarDocumentoAutorizar(string idTercero, int idDetDoc, string pinFirma, int formatoentrada, int formatosalida)
        {
            int codigoRespuesta;
            var detalleDocumento = await _detalleDocumentosManager.ObtenerDetalleDocumentoId(idDetDoc);
            //var ordenanteTercero = _ordenantesTercerosManager.ObtenerRepresentantePorTercero(idTercero);
            //if (ordenanteTercero.TipoIdentificacionOrd == 9)
            //{
            //    tipoDocumentoOrd = 8;
            //}
            //else
            //{
            //    tipoDocumentoOrd = ordenanteTercero.TipoIdentificacionOrd;
            //}
            //strPathJar = "C:\\WEB\\AndesCD\\";

            //FIRMA IMAGEN  SIN ESTAMPADO
            //parametrosFirma = "--metodofirma ws --formatofirma pdf  --login " + "WILO" + " --password " + "a69rACtfhK" + " --tipodocumento " + tipoDocumentoOrd + " --documento " + ordenanteTercero.IdOrdenanteTercero + " --pinfirma " + pinFirma + " --entrada " + detalleDocumento.DirDocumento + " --salida " + detalleDocumento.DirDocumento + " --formatoentrada archivo --formatosalida archivo" + " --visible true --imagenFirma " + "C:\\WEB\\AndesCD\\LogoFirma.png" + " --ubicacion 50,50,180,60 --tamanofuentefirma 6 --pagina 1 --aplicatsa false --aplicaqr false --test true --proteger false ";

            //FIRMA IMAGEN  CON ESTAMPADO
            // parametrosFirma = "--metodofirma ws --formatofirma pdf  --login " + "WILO" + " --password " + "a69rACtfhK" + " --tipodocumento " + tipoDocumentoOrd + " --documento " + ordenanteTercero.IdOrdenanteTercero + " --pinfirma " + pinFirma + " --entrada " + detalleDocumento.DirDocumento + " --salida " + detalleDocumento.DirDocumento + " --formatoentrada archivo --formatosalida archivo" + " --visible true --imagenFirma " + "C:\\WEB\\AndesCD\\LogoFirma.png" + " --ubicacion 50,50,180,60 --tamanofuentefirma 6 --pagina 1 --aplicatsa true --tsausuario PRUEBA2 --tsapass lcVoxksw --aplicaqr false --test true --proteger false ";



            codigoRespuesta = 0; // firmar(strPathJar, parametrosFirma, out mensajeRespuesta);


            if (codigoRespuesta == 0)
            {
                //mensajeRespuesta = "Documento Firmado";
                await _detalleDocumentosManager.ActualizarFirmaDetalleDocumentoAutoriza(idTercero, idDetDoc, true);
                //_detalleDocumentosManager.ActualizarFirmaDetalleDocumentoAutoriza(idTercero, idDetDoc, true);
            }
            RespuestasDto respuestaConsulta = new RespuestasDto();
            respuestaConsulta.CodigoRespuesta = codigoRespuesta;
            respuestaConsulta.MensajeRespuesta = "0"; // mensajeRespuesta;
            return respuestaConsulta;
        }

        public int Firmar(string strPath, string parametrosFirma, out string mensajeRespuesta)
        {

            string json = "";
            int codigoRespuesta;

            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = "java.exe";

            //startinfo.Arguments = "-jar C:\\WEB\\AndesCD\\firmarLtvHash.jar " + parametrosFirma;
            //WIGELO:08/10/2021 Logica para obtener por base de datos
            //startinfo.Arguments = $"-jar {"C:\\WEB\\AndesCD\\"}AndesFirmador.jar " + parametrosFirma;
            startinfo.Arguments = $"-jar {strPath}AndesFirmador.jar " + parametrosFirma;
            Process process = new Process();

            if (!File.Exists($"{strPath}AndesFirmador.jar"))
            {
                mensajeRespuesta = $"No se encuentra jar para realizar el proceso : {strPath}";
                return -1;
            }

            process.StartInfo = startinfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("ls -ltr /opt/*.tmp");

            while (!process.StandardOutput.EndOfStream)
            {
                json = process.StandardOutput.ReadLine();
            }

            //JObject result = JObject.Parse(json);
            var result = JsonConvert.DeserializeObject<ProcessResponse>(json);

            if (result == null)
            {
                mensajeRespuesta = $"Respuesta del servicio no valida {json}";
                return -1;
            }

            int.TryParse(result.Estado, out codigoRespuesta); // process.ExitCode;
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            process.Close();
            mensajeRespuesta = json;
            return codigoRespuesta;

        }

        public async Task<TerceroCertificadoDto> ConsultarCertificadoTercero(string idTercero, string idTerceroPadre)
        {

            var tercero = await _tercerosManager.ObtenerTerceroPorId(idTercero, idTerceroPadre);
            var estado = "PENDIENTE";
            var observacion = string.Empty;

            if (tercero == null)
                return null;

            var vigente = await ValidarCertificado(tercero);

            if (!vigente && !string.IsNullOrEmpty(tercero.NumSolicitudCert))
            {
                var msj = ValidarSolicitud(tercero, idTerceroPadre);
                observacion = msj.Item1;
                estado = "EN PROCESO";
            }
            else if (vigente)
            {
                estado = "VIGENTE";
            }

            TerceroCertificadoDto result = new()
            {
                Id = tercero.Id,
                IdTercero = tercero.IdTercero,
                NombreTercero = $"{tercero.NomTercero} {tercero.ApeTercero}",
                Vigente = vigente,
                NumSolicitud = tercero.NumSolicitudCert,
                TipoCertificado = tercero?.TiposOrdenantes?.TipoCertificado ??  0,
                Estado = estado,
                Observacion = observacion,
                FechaCert = tercero.FechaCert

            };

            if (result.Vigente)
                result.FechaVigencia = tercero.FecVigenciaCert;
            return result;

        }


        private int CountPage(string dirDocumento)
        {
            PdfSharp.Pdf.PdfDocument PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(dirDocumento, PdfDocumentOpenMode.Import);
            return PDFDoc.PageCount;
        }

        [SupportedOSPlatform("windows")]
        private void AddLog(string message)
        {

            try
            {
                string sSource;
                string sLog;


                sSource = "UnoSofNet";
                sLog = "Application";


                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                //  EventLog.WriteEntry(sSource, message);
                EventLog.WriteEntry(sSource, message,
                    EventLogEntryType.Warning, 234);

            }
            catch (Exception)
            {


            }
        }


    }
}
