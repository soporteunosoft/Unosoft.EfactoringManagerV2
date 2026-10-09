using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EfactoringManager.Certificados;
using Unosoft.EfactoringManager.Certificados.Dto;
using Unosoft.EFactoringManager.CertificadosDigitales.Configuration;

namespace Unosoft.EfactoringManager.Certificados
{
    public class CertificadosProxy
    {

        //TO DO : Metodo Consulta de Certificados
        // Eliminar Datos quemados

        AndesCSD.Certificado.ConsultarCertPortTypeClient consultarCertPortTypeClient;
        AndesCSD.Certificado.ConsultarCertEmpresaPortTypeClient consultarCertEmpresaPortTypeClient;
        AndesCSD.Certificado.CertificadosPortTypeClient certificadosPortTypeClient;
        AndesCSD.Certificado.CertificadosPersonaNaturalRUTPortTypeClient certificadosPersonaNaturalRUTPortTypeClient;
        AndesCSD.Certificado.CertificateVinculacionEmpresaPortTypeClient certificateVinculacionEmpresaPortTypeClient;
        AndesCSD.Certificado.ConsultarCertPortTypeClient consultarCertPortType;
        AndesCSD.Certificado.ConsultarCertEmpresaPortTypeClient consultarCertEmpresaPortType;
        AndesCSD.Certificado.ConsultarSolicitudPortTypeClient consultarSolicitudPortTypeClient;

        private readonly PasswordDigestBehavior behavior;
        public CertificadosProxy(AndesConfiguration settings)
        {


            EndpointAddress address = new EndpointAddress(settings.UrlBase); //  "https://ra.andesscd.com.co/test/WebService/soap-server_new.php");
            BasicHttpBinding bindings = new BasicHttpBinding();

            bindings.Security.Mode = BasicHttpSecurityMode.Transport;
            consultarCertPortTypeClient = new AndesCSD.Certificado.ConsultarCertPortTypeClient(bindings, address);
            consultarCertEmpresaPortTypeClient = new AndesCSD.Certificado.ConsultarCertEmpresaPortTypeClient( bindings, address);
            
            certificadosPortTypeClient = new AndesCSD.Certificado.CertificadosPortTypeClient(bindings, address);
            certificadosPersonaNaturalRUTPortTypeClient = new AndesCSD.Certificado.CertificadosPersonaNaturalRUTPortTypeClient(bindings, address);
            certificateVinculacionEmpresaPortTypeClient = new AndesCSD.Certificado.CertificateVinculacionEmpresaPortTypeClient(bindings, address);
            consultarCertPortType = new  AndesCSD.Certificado.ConsultarCertPortTypeClient(bindings, address);
            consultarCertEmpresaPortType = new AndesCSD.Certificado.ConsultarCertEmpresaPortTypeClient(bindings, address);
            consultarSolicitudPortTypeClient = new AndesCSD.Certificado.ConsultarSolicitudPortTypeClient(bindings, address);

            behavior = new PasswordDigestBehavior(settings.UserName, settings.Password);
        }


        //public RespuestasDto CertificateVinculacion(CertificateVinculacionEmpresaInput certificadosVinEmpInput)
        //{
        //    return null;
        //}


        public RespuestasDto CertificadosService(CertificadosInput certificadosInput)
        {
            try
            {

                string mensajeConsulta = "";
                //var behavior = new PasswordDigestBehavior(settings.UserName, settings.Password); // "WILO", "a69rACtfhK");
                certificadosPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                certificadosPortTypeClient.Open();
                int resut =certificadosPortTypeClient.Certificados(certificadosInput.tipoCert, certificadosInput.tipoDoc, certificadosInput.documento,
                                                        certificadosInput.nombres, certificadosInput.apellidos, certificadosInput.municipio, certificadosInput.direccion,
                                                        certificadosInput.email, certificadosInput.telefono, certificadosInput.celular, certificadosInput.ocupacion, certificadosInput.fechaCert,
                                                        certificadosInput.vigenciaCert, certificadosInput.formato, certificadosInput.testigo, certificadosInput.foto, certificadosInput.soporte,
                                                        certificadosInput.pin, certificadosInput.pkcs10, certificadosInput.verific_doc, certificadosInput.plantillaHuella, out mensajeConsulta);

                certificadosPortTypeClient.Close();
                
                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta = resut;
                respuestaConsulta.NumeroSolicitud = mensajeConsulta;
                respuestaConsulta.MensajeRespuesta = resut!=0 ? mensajeConsulta : "Solicitud de certificado realizada con éxito";
                return respuestaConsulta;

               

            }
            catch (Exception)
            {
                throw;
            }
        }


        public RespuestasDto CertificateVinculacionEmpresaService(CertificateVinculacionEmpresaInput certificadosVinEmpInput)
        {
            try
            {
                
                //var behavior = new PasswordDigestBehavior("WILO", "a69rACtfhK");
                certificateVinculacionEmpresaPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                certificateVinculacionEmpresaPortTypeClient.Open();

                int resut = certificateVinculacionEmpresaPortTypeClient.CertificateVinculacionEmpresa(certificadosVinEmpInput.tipoCert, 
                                                                                                      certificadosVinEmpInput.tipoDoc, 
                                                                                                      certificadosVinEmpInput.documento,
                                                                                                      certificadosVinEmpInput.nombres, 
                                                                                                      certificadosVinEmpInput.apellidos, 
                                                                                                      certificadosVinEmpInput.municipio, 
                                                                                                      certificadosVinEmpInput.direccion,
                                                                                                        certificadosVinEmpInput.email, 
                                                                                                        certificadosVinEmpInput.emailEnt, 
                                                                                                        certificadosVinEmpInput.telefono, 
                                                                                                        certificadosVinEmpInput.celular, 
                                                                                                        certificadosVinEmpInput.ocupacion,
                                                                                                        certificadosVinEmpInput.tipoDocEnt, 
                                                                                                        certificadosVinEmpInput.documentoEnt, 
                                                                                                        certificadosVinEmpInput.razonsocial, 
                                                                                                        certificadosVinEmpInput.municipioEnt, 
                                                                                                        certificadosVinEmpInput.direccionEnt,
                                                                                                        certificadosVinEmpInput.cargo, 
                                                                                                        certificadosVinEmpInput.unidadOrganizacional, 
                                                                                                        certificadosVinEmpInput.fechaCert, 
                                                                                                        certificadosVinEmpInput.formato,
                                                                                                        certificadosVinEmpInput.vigenciaCert, 
                                                                                                        certificadosVinEmpInput.testigo, 
                                                                                                        certificadosVinEmpInput.foto, 
                                                                                                        certificadosVinEmpInput.pin, 
                                                                                                        certificadosVinEmpInput.pkcs10, 
                                                                                                        certificadosVinEmpInput.soporte,
                                                                                                        certificadosVinEmpInput.verific_doc,
                                                                                                        "certificadosVinEmpInput.token_andesid",
                                                                                                        certificadosVinEmpInput.plantillaHuella, 
                                                                                                        out string mensajeConsulta);
                //certificadosPortType.Certificados()
                certificateVinculacionEmpresaPortTypeClient.Close();
                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta = resut;
                respuestaConsulta.MensajeRespuesta = mensajeConsulta;
                return respuestaConsulta;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RespuestasDto CertificadosPersonaNaturalRUTService(CertificadoPersonaNRInput certificadosPerNRInput)
        {
            try
            {
                string mensajeConsulta;
                //var behavior = new PasswordDigestBehavior("WILO", "a69rACtfhK");
                certificadosPersonaNaturalRUTPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                certificadosPersonaNaturalRUTPortTypeClient.Open();
                int resut = certificadosPersonaNaturalRUTPortTypeClient.CertificadosPersonaNaturalRUT(certificadosPerNRInput.tipoCert, certificadosPerNRInput.tipoDoc, certificadosPerNRInput.documento,
                                                        certificadosPerNRInput.nombres, certificadosPerNRInput.apellidos, certificadosPerNRInput.municipio, certificadosPerNRInput.direccion,
                                                        certificadosPerNRInput.email, certificadosPerNRInput.telefono, certificadosPerNRInput.celular, certificadosPerNRInput.ocupacion, certificadosPerNRInput.fechaCert,
                                                        certificadosPerNRInput.vigenciaCert, certificadosPerNRInput.formato, certificadosPerNRInput.testigo, certificadosPerNRInput.foto, certificadosPerNRInput.soporte,
                                                        certificadosPerNRInput.pin, certificadosPerNRInput.pkcs10, certificadosPerNRInput.verific_doc, certificadosPerNRInput.plantillaHuella, out mensajeConsulta);

                certificadosPersonaNaturalRUTPortTypeClient.Close();
                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta = resut;
                respuestaConsulta.MensajeRespuesta = mensajeConsulta;
                return respuestaConsulta;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public RespuestasDto ConsultarCertificadoService( ConsultarCertInput consultarCert)
        {
            try
            {
                string mensajeConsulta;
                //var behavior = new PasswordDigestBehavior("WILO", "a69rACtfhK");
                consultarCertPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                consultarCertPortTypeClient.Open();
                int result = consultarCertPortTypeClient.ConsultarCert(consultarCert.TipoDoc, consultarCert.Documento, out mensajeConsulta);
               
                consultarCertPortTypeClient.Close();

                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta =result;
                respuestaConsulta.MensajeRespuesta = mensajeConsulta;
                return respuestaConsulta;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public RespuestasDto ConsultarCertificadoXEmpresaService(ConsultarCertInput consultarCert)
        {
            try
            {
                string mensajeConsulta;
                //var behavior = new PasswordDigestBehavior("WILO", "a69rACtfhK");
                consultarCertEmpresaPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                consultarCertEmpresaPortTypeClient.Open();
                int resut = consultarCertEmpresaPortTypeClient.ConsultarCertEmpresa(consultarCert.TipoDoc, consultarCert.Documento, 
                                                                                    consultarCert.TipoDocEmp,
                                                                                    consultarCert.DocumentoEmp,  
                                                                                    out mensajeConsulta
                                                                                    );

                consultarCertEmpresaPortTypeClient.Close();

                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta = resut;
                respuestaConsulta.MensajeRespuesta = mensajeConsulta;
                return respuestaConsulta;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public RespuestasDto ConsultarSolicitudCertificado(ConsultarCertInput consultarCert)
        {
            try
            {
                string mensajeConsulta;
                //var behavior = new PasswordDigestBehavior("WILO", "a69rACtfhK");
                consultarSolicitudPortTypeClient.Endpoint.EndpointBehaviors.Add(behavior);
                consultarSolicitudPortTypeClient.Open();
                 int result = consultarSolicitudPortTypeClient.ConsultarSolicitud(consultarCert.NumeroSolicitud, consultarCert.TipoDoc, consultarCert.Documento, out mensajeConsulta);
                //int result = consultarCertEmpresaPortType.ConsultarCertEmpresa(consultarCert.TipoDoc, consultarCert.Documento, consultarCert.TipoDocEmp, consultarCert.DocumentoEmp,  out mensajeConsulta);
                consultarCertPortTypeClient.Close();

                RespuestasDto respuestaConsulta = new RespuestasDto();
                respuestaConsulta.CodigoRespuesta = mensajeConsulta.Split('|').Length > 1 ? Convert.ToInt32(mensajeConsulta.Split('|')[1].Trim()) : result;
                respuestaConsulta.MensajeRespuesta = mensajeConsulta.Split('|')[0];
                return respuestaConsulta;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
