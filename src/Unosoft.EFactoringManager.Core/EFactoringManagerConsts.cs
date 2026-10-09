using Unosoft.EFactoringManager.Debugging;

namespace Unosoft.EFactoringManager;

public class EFactoringManagerConsts
{
    public const string LocalizationSourceName = "EFactoringManager";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;
    public const string DefaultPassPhrase = "EFactoringManager@2024";

    #region constantes EFactoring
    public const string gSolicitud = "SFE";
        public const string gSolicitudCufe = "SCU";
        public const string gSolicitudSol = "SOL";
        public const string gFinanciera = "FIN";



        public enum TipoOperacion
        {
            gFactoring = 1,
            gCredito = 2,
            gDescuentoCheque = 3,
            gFactoring_Electronico = 4
        }

        public enum ModalidadesOperacion
        {
            gFactoring_Electronico = 1
        }

        public enum TiposTasaInteres
        {
            gMensual = 1,
            gEfectiva = 2,
            gFija = 3,
            gCompuesta = 4
        }

        public enum BaseDias
        {
            gDias360 = 360,
            gDias365 = 365
        }

        public enum ModalidadesInteres
        {
            gVencida = 1,
            gAnticipada = 2
        }

        public enum TipoContribuyente
        {
            gNatural = 1,
            gJuridico = 2,
            gGranContribuyente = 3
        }

        public enum TipoLiquidacion
        {
            gVrFuturo = 0,
            gVrPresente = 1,
            gVrNetoRecibir = 2
        }

        public enum TipoTercero
        {
            gInversionista = 1,
            gOrdenante = 1,
            gCliente = 2,
            gPagador = 3,
            gComisionista = 4,
        }

        public enum EstadosSolicitudes
        {
            gInicial = 0,
            gEnviada = 1,
            gPTEAceptacion = 4,
            gAceptada = 3,
            gEndosos = 3,
            gNoAceptada = 4,
            gErrcufes = 2,
            gErrend = 4,
            gInicalEmisor = 5,
            gPendienteEnviar = 5,
        }

        public enum EstadosTitulos
        {
            gNoAplica = 0,
            gCargado = 1,

            gEndosable = 2,
            gNoEndosable = 3,

            gEndosada = 4,
            gNoEndosada = 5,


            gPTEAceptacion = 10,

            gReendosable = 6,

            gError = 7,
            gAceptadaPagador = 12,


            gRechazadaCli = 12,
            gRechazada = 8,
            gValidando = 9,
            gEnProceso = 99

//NO APLICA
//CARGADO
//ENDOSABLE
//NO ENDOSABLE
//ENDOSADA
//NO ENDOSADA
//FACTURA RECHAZADA FACTOR

            //gOfrecidas = 2,
            //gPTEAceptacion = 3,
            //gNegociada = 4,
            //gRechazadaCli = 5,
            //gRechazada = 6
        }
        public enum TiposTitulos
        {
            gFacturaElectronica = 1
        }

        public enum TiposLinea
        {
            gDescuento = 1
        }

        public enum DocumentoSoporte
        {
            gPagare = 4,
            gCedulaRepresentante = 9,
            gRut = 10,
            gContratoFactoring = 15,
            gCartaInstrucciones = 17,
            gConvenioRecaudo = 23,
            gCartaRepresentante = 24,
            gAnexoConvenioRecaudo = 25,
            gEndoso = 26,
            gMandato = 27,
            gCesion = 29,
            gCamaraComercioRepresentante = 1,
            //WIGEL0:08/10/2021
            gCartaRevocatoria = 18,
            gFormatoVinculacion = 30
        }

        public enum DocumentoOrigen
        {
            gVinculacion = 0,
            gClientes = 1,
            gJuridicos = 2,
            gDocSolicitudes = 3


            //gVinculacion = 0,
            //gClientes = 1,
            //gDocLogros = 2,
            //gDocSolicitudes = 3

        }

        public enum TiposCertificados
        {
            gPersonaNatual = 16,//5 en producción
            gProfesionalTitulado = 7,
            gRepresentanteLegal = 8,
            gPerteneceEmpresa = 9,
            gPerteneceJuridica = 13
        }


        /*
        0 ACERCAMIENTO COMERCIAL
        1 VINCULACION
        2 DOCUMENTOS JURIDICOS
        3 CONFIRMACION PAGADOR
        4 VALIDACION DE LA OPERACION
        5 DESEMBOLSO
        */
        #endregion

        public class CodigosEventos
        {
            public const string Acuse = "030";
            public const string Recibo = "032";
            public const string AceptacionExpresa = "033";
            public const string AceptacionTacita = "034";
            public const string Inscripcion = "036";
            public const string EndosoResponsabilidad = "037";
            public const string EndosoSinResponsabilidad = "038";
            public const string EndosoOrdinario = "047";
        }

        public class TipoEndoso
        {
            public const int ConResposabilidad = 1;
            public const int SinResponsabilidad = 2;
            public const int CesionOrdinaria = 3;
        }

        public class CodigosEndoso
        {
            public const string ConResposabilidad = "371";
            public const string SinResponsabilidad = "372";
            public const string CesionOrdinaria = "047";
        }

        public static class CamposEventosFelEvento
        {
            public const string ReferenciaTipoOperacion = "ReferenciaTipoOperacion";
            public const string Moneda = "Moneda";
            public const string CodigoCantidadDocumentos = "CodigoCantidadDocumentos";
            public const string Facultades = "Facultades";
            public const string DescripcionFacultades = "DescripcionFacultades";
            public const string Notas = "Notas";
            public const string FechaActuarMandatario = "FechaActuarMandatario";
        }

        public static class CamposEventosDocumentoReferenciado
        {
            public const string Consecutivo = "Consecutivo";
            public const string CufeCude = "CufeCude";
            public const string TipoDocumento = "TipoDocumento";
            public const string CodigoTiempoMandato = "CodigoTiempoMandato";
            public const string DescripcionEvento = "DescripcionEvento";
            //public List<Fact_PersonaNaturalJuridica> Deudores { get; set; }
        }


        public static class CamposEventosPersonaNaturalJuridica
        {

            public const string TipoDocumento = "TipoDocumento";
            public const string NumeroDocumento = "NumeroDocumento";
            public const string NombreCompleto = "NombreCompleto";
            public const string TipoPersona = "TipoPersona";
            public const string TipoFactor = "TipoFactor";
            public const string DescripcionFactor = "DescripcionFactor";
            public const string DigitoVerificacion = "DigitoVerificacion";
            public const string Monto = "Monto";
        }

        public static class CamposEventosInformacionCampoValor
        {

            public const string ValorFEVaval = "ValorFEVaval";
            public const string ValorPagado = "ValorPagado";
            public const string NuevoValorTV = "NuevoValorTV";
        }

        public static class TipoObjetosEventosDian
        {
            public const string Fel_Evento = "FEL_EVENTO";
            public const string Fact_ArchivoAdjunto = "FACT_ARCHIVOADJUNTO";
            public const string Fact_DocumentoReferenciado = "FACT_DOCUMENTOREFERENCIADO";
            public const string Fel_GeneradorEvento = "FEL_GENERADOREVENTO";
            public const string InformacionCampoValor = "INFORMACIONCAMPOVALOR";

        }

        public enum TipoFirma
        {
            Digital = 1,
            Electronica = 2
        }

        public enum OrigenDocumento
        {
            Vinculacion = 0,
            Evaluacion = 1,
            Juridico = 2
        }


        public static class EstadosSolicitudesTipoDocumentoSCU
        {
            public const string SCU_Inicial = "INICIAL";
            public const string SCU_Cufes = "CUFES";
            //public const string SCU_AcpCufes = "ACPCUFES";
            public const string SCU_ErrCufes = "ERRCUFES";

            public const string SCU_Endosos = "ENDOSOS";
            //public const string SCU_AcpEndosos = "ACPEND";
            public const string SCU_ErrEndosos = "ERREND";

        }


    
}
