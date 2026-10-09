using Docnet.Core;
using Docnet.Core.Models;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Authorization.Users;
using Unosoft.EFactoringManager.DetalleDocumentos;
using Unosoft.EFactoringManager.SolicitudesOperaciones;
using Unosoft.EFactoringManager.Titulos;
using Unosoft.EFactoringManager.Web;
using ZXing;

namespace Unosoft.EFactoringManager.Controllers
{
    [Route("api/[controller]/[action]")]
    public class IncorporarDocumentoController : EFactoringManagerControllerBase
    {
        #region Atributos
        private readonly TitulosManager _titulosManager;
        private readonly UserManager _userManager;
        private readonly DetalleDocumentos.DetalleDocumentosManager _detalleDocumentosManager;
        private readonly SolicitudesOperacionesManager _solicitudesOperacionesManager;
      
        #endregion


        public IncorporarDocumentoController(TitulosManager titulosManager,
                                             UserManager userManager,
                                             DetalleDocumentosManager detalleDocumentosManager,
                                             SolicitudesOperacionesManager solicitudesOperacionesManager

                                             )
        {
            _titulosManager = titulosManager;
            _userManager = userManager;
            _detalleDocumentosManager = detalleDocumentosManager;
            _solicitudesOperacionesManager = solicitudesOperacionesManager;
        }        



        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            try
            {
                long idUsuario = AbpSession.UserId.Value;
                var httpRequest = HttpContext.Request;

                var parser = await MultipartParser.CreateAsync(httpRequest.Body);

                HttpResponseMessage response = new HttpResponseMessage();

                #region Codigo Comentado
                //                SautinSoft.PdfFocus f = new SautinSoft.PdfFocus();

                //                string inputPdfPath = "C:\\Users\\DESARROLLO02\\Desktop\\facturas sespem\\BAQ291.pdf";

                //                f.OpenPdf(inputPdfPath);
                //                f.ImageOptions.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                //                f.ImageOptions.Dpi = 200;
                //                var Imagen = f.ToImage(1);

                //                MemoryStream ms = new MemoryStream(Imagen, 0, Imagen.Length);
                //                ms.Position = 0; // this is important 
                //                var Img = Image.FromStream(ms, true);






                //                Image image1 = Image.FromFile("c:\\Users\\DESARROLLO02\\Desktop\\Facturas\\ad.jpg");
                //                Bitmap BitmapImage = new Bitmap(Img);

                //                float nPercent = ((float)300 / 100);

                //                int destinoWidth = (int)(BitmapImage.Width * nPercent);
                //                int destinoHeight = (int)(BitmapImage.Height * nPercent);
                //                Bitmap Imagen2 = new Bitmap(destinoWidth, destinoHeight);
                //                using (Graphics g = Graphics.FromImage((Image)Imagen2))
                //                {
                //                    g.DrawImage(BitmapImage, 0, 0, destinoWidth, destinoHeight);
                //                }
                //                BitmapImage.Dispose();
                //                // Imagen2.Save("C:\\Users\\DESARROLLO02\\Desktop\\Facturas\\CA3prueba2.jpg");


                //#if DEBUG
                //                // current directory
                //                string CurDir = Environment.CurrentDirectory;
                //                string WorkDir = CurDir.Replace("bin\\Debug", "Work");
                //                if (WorkDir != CurDir && Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;

                //                // open trace file
                //                QRCodeTrace.Open("QRCodeDecoderTrace.txt");
                //                QRCodeTrace.Write("QRCodeDecoderDemo");
                //#endif


                //                QRCodeDecoder = new QRDecoder();
                //                // dispose previous image
                //                if (QRCodeInputImage != null) QRCodeInputImage.Dispose();

                //                // load image to bitmap
                //                QRCodeInputImage = new Bitmap(@"C:\Users\DESARROLLO02\Desktop\Facturas\CA3prueba2.jpg");


                //                // trace
                //#if DEBUG
                //                QRCodeTrace.Format("****");
                //                // QRCodeTrace.Format("Decode image: {0} ", @"c:\Users\DESARROLLO02\Desktop\Facturas\CA3prueba2.jpg");
                //                QRCodeTrace.Format("Image width: {0}, Height: {1}", Imagen2.Width, Imagen2.Height);
                //#endif

                //                // decode image
                //                byte[][] DataByteArray = QRCodeDecoder.ImageDecoder(Imagen2);


                //                string info = QRCodeResult(DataByteArray);

                //                string[] words = info.Split('\n');

                #endregion

                if (parser.Success)
                {
                    Unosoft.EFactoringManager.Authorization.Users.User user = await _userManager.GetUserByIdAsync(idUsuario);

                    string numeroTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[0].Data);
                    numeroTercero = numeroTercero.Replace("\r", "");

                    //var sol = parser.MyContents.Where(x => x.PropertyName == "idSolicitud").Select(x => x.Data).FirstOrDefault();

                    //WIGELO: COMO SE CARGA LOS VALORES DE ESTAS VARIABLES
                    string idSolicitud = System.Text.Encoding.UTF8.GetString(parser.MyContents[3].Data)?.Replace("\r", "");
                    string tipoDocumento = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data)?.Replace("\r", "");
                    string idOficina = System.Text.Encoding.UTF8.GetString(parser.MyContents[2].Data)?.Replace("\r", "");

                    //string porcNegociacion = System.Text.Encoding.UTF8.GetString(parser.MyContents[4].Data)?.Replace("\r", "");

                    //string idSolicitud = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data)?.Replace("\r", "");
                    //string tipoDocumento = System.Text.Encoding.UTF8.GetString(parser.MyContents[2].Data)?.Replace("\r", "");
                    //string idOficina = System.Text.Encoding.UTF8.GetString(parser.MyContents[3].Data)?.Replace("\r", "");

                    var filesTitulo = new List<TitulosFileInfo>();

                    if (Path.GetExtension(parser.Filename).ToLower() == ".zip")
                    {
                        //GetDocumentFromZip(byte[] zipFile)
                        filesTitulo = _titulosManager.GetDocumentFromZip(parser.FileContents);
                    }
                    else
                        filesTitulo.Add(new TitulosFileInfo(parser.Filename, parser.FileContents));

                    var solicitud = await _solicitudesOperacionesManager.ObtenerSolicitudOpePorId(tipoDocumento, int.Parse(idOficina), int.Parse(idSolicitud));


                    if (solicitud == null)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    }

                    if (solicitud.PorcNegociacion == null || solicitud.FecSolicitud == null)
                    {
                        // decidir qué hacer: devolver error adecuado, valor por defecto, etc.
                    }



                    var porcNegociacion = (decimal)solicitud.PorcNegociacion.GetValueOrDefault(0f);
                    var fecSolicitud = solicitud.FecSolicitud ?? DateTime.Now;

                    var resultFactura = await _titulosManager.RegistrarFactura(
                            int.Parse(numeroTercero),
                            tipoDocumento,
                            int.Parse(idOficina),
                            int.Parse(idSolicitud),
                            porcNegociacion,
                            fecSolicitud,
                            solicitud.TipoEndoso,
                            filesTitulo);

                    if (resultFactura.Item1)
                    {
                        response.StatusCode = HttpStatusCode.Created;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Content = new StringContent(resultFactura.Item2);
                        return response;
                    }

                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }

            }
            catch (IOException ex)
            {
                throw new Exception("A generic error occured. Please try again later. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostPdf()
        {
            try
            {
                long idUsuario = AbpSession.UserId.Value;
                var httpRequest = HttpContext.Request;
                var parser = await MultipartParser.CreateAsync(httpRequest.Body);

                if (!parser.Success)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var user = await _userManager.GetUserByIdAsync(idUsuario);

                string numeroTercero =
                    Encoding.UTF8.GetString(parser.MyContents[0].Data);

                // ============================
                // 1️⃣ PDF → Imagen base
                // ============================

                using var docReader = DocLib.Instance.GetDocReader(
                    parser.FileContents,
                    new PageDimensions(1080, 1920));

                using var pageReader = docReader.GetPageReader(0);

                var rawBytes = pageReader.GetImage();
                var width = pageReader.GetPageWidth();
                var height = pageReader.GetPageHeight();

                using var baseImage = Image.LoadPixelData<Rgb24>(
                    rawBytes,
                    width,
                    height);

                // ============================
                // 2️⃣ Intentar leer QR
                // ============================

                string qrText = TryDecodeQR(baseImage);

                if (string.IsNullOrEmpty(qrText))
                    qrText = TryDecodeQR(baseImage, 2);

                if (string.IsNullOrEmpty(qrText))
                    qrText = TryDecodeQR(baseImage, 3);

                // Aquí ya tienes el texto QR si existe
                // Puedes validarlo si lo necesitas

                var resultPdf = await _titulosManager.RegistrarFactura(
                        int.Parse(numeroTercero),
                        parser.FileContents);

                if (resultPdf.Item1)
                {
                    return new HttpResponseMessage(HttpStatusCode.Created);
                }

                return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(resultPdf.Item2) };
            }
            catch (Exception ex)
            {
                throw new Exception("Error procesando PDF: " + ex.Message);
            }
        }

        [HttpPost()]
        public async Task<HttpResponseMessage> UploadFiles()
        {
            long idUsuario = AbpSession.UserId.Value;
            Unosoft.EFactoringManager.Authorization.Users.User user = await _userManager.GetUserByIdAsync(idUsuario);

            var httpRequest = HttpContext.Request;
            var parser = await MultipartParser.CreateAsync(httpRequest.Body);
            //System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            DateTime? fecVencimiento;

            if (parser.MyContents[parser.MyContents.Count - 1].PropertyName.Remove(7, 1) == "fechVen")
            {
                string _fecVencimiento = parser.MyContents[parser.MyContents.Count - 1].StringData;
                fecVencimiento = Convert.ToDateTime(Convert.ToDateTime(_fecVencimiento).ToString("yyyy-MM-dd"));
            }
            else { fecVencimiento = null; }

            string numeroTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[0].Data);

            if (parser.MyContents.Count > 0 && parser.Success)
            {
                foreach (MyContent hpf in parser.MyContents)
                {
                    if (hpf.PropertyName.Remove(7, 1) != "fechVen")
                    {
                        var resultUpload = await _titulosManager.RegistrarFactura(int.Parse(numeroTercero), hpf.Data);

                        return new HttpResponseMessage()
                        {
                            StatusCode = resultUpload.Item1 ? HttpStatusCode.Created : HttpStatusCode.InternalServerError,
                            Content = new StringContent(resultUpload.Item2)
                        };
                    }


                }
            }
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.Created };
        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubirDocumentos()
        {
            try
            {

                var httpRequest = HttpContext.Request;

                var parser = await MultipartParser.CreateAsync(httpRequest.Body);


                string idTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data);
                string idDocLinea = System.Text.Encoding.UTF8.GetString(parser.MyContents[2].Data);
                idTercero = idTercero.Replace("\r", "");
                idDocLinea = idDocLinea.Replace("\r", "");
                HttpResponseMessage response = new HttpResponseMessage();

                if (parser.Success)
                {


                    if (await _detalleDocumentosManager.GuardarDocumentosVinculacion(idTercero, parser.FileContents, parser.Filename, Convert.ToInt32(idDocLinea)))
                    {
                        response.StatusCode = HttpStatusCode.Created;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    }

                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }




            }
            catch (IOException)
            {
                throw new Exception("A generic error occured. Please try again later.");
            }


        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubirRepresentacionTitulo()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                long idUsuario = AbpSession.UserId.Value;
                var parser = await MultipartParser.CreateAsync(httpRequest.Body);
                Unosoft.EFactoringManager.Authorization.Users.User user = await _userManager.GetUserByIdAsync(idUsuario);
                string idUsuarioMod = user.UserName;
                //string idTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data);
                string idTitulo = System.Text.Encoding.UTF8.GetString(parser.MyContents[2].Data);
                string numeroTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data);

                numeroTercero = numeroTercero.Replace("\r", "");
                idTitulo = idTitulo.Replace("\r", "");
                HttpResponseMessage response = new HttpResponseMessage();

                if (parser.Success)
                {


                    if (await _titulosManager.SubirRepresentacionTitulos(int.Parse(numeroTercero), parser.FileContents, parser.Filename, idTitulo, idUsuarioMod))
                    {
                        response.StatusCode = HttpStatusCode.Created;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    }

                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }

            }
            catch (IOException)
            {
                throw new Exception("A generic error occured. Please try again later.");
            }


        }

        [HttpPost]
        public async Task<HttpResponseMessage> SubirActDocumentos()
        {
            try
            {

                var httpRequest = HttpContext.Request;
                long idUsuario = AbpSession.UserId.Value;
                var parser = await MultipartParser.CreateAsync(httpRequest.Body);

                Unosoft.EFactoringManager.Authorization.Users.User user = await _userManager.GetUserByIdAsync(idUsuario);
                string idUsuarioMod = user.UserName;
                string idDetDoc = System.Text.Encoding.UTF8.GetString(parser.MyContents[1].Data);
                idDetDoc = idDetDoc.Replace("\r", "");
                string NomDocumento = System.Text.Encoding.UTF8.GetString(parser.MyContents[2].Data);
                NomDocumento = NomDocumento.Replace("\r", "");
                string numeroTercero = System.Text.Encoding.UTF8.GetString(parser.MyContents[3].Data);
                numeroTercero = numeroTercero.Replace("\r", "");

                HttpResponseMessage response = new HttpResponseMessage();

                if (parser.Success)
                {
                    if (await _detalleDocumentosManager.ActualizarDocumentos(Int32.Parse(numeroTercero), parser.FileContents, NomDocumento, Convert.ToInt32(idDetDoc), idUsuarioMod))
                    {
                        response.StatusCode = HttpStatusCode.Created;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        return response;
                    }

                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    return response;
                }

            }
            catch (IOException)
            {
                throw new Exception("A generic error occured. Please try again later.");
            }


        }



        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private string TryDecodeQR(Image<Rgb24> originalImage, int scale = 1)
        {
            using var workingImage = scale == 1
                ? originalImage.Clone()
                : originalImage.Clone(ctx =>
                    ctx.Resize(
                        originalImage.Width * scale,
                        originalImage.Height * scale));

            var reader = new BarcodeReaderGeneric
            {
                AutoRotate = true,
                Options = new ZXing.Common.DecodingOptions
                {
                    TryHarder = true,
                    TryInverted = true,
                    PossibleFormats = new List<BarcodeFormat>
            {
                BarcodeFormat.QR_CODE
            }
                }
            };

            var luminanceSource =
                new ZXing.ImageSharp.ImageSharpLuminanceSource<Rgb24>(workingImage);

            var result = reader.Decode(luminanceSource);

            return result?.Text ?? string.Empty;
        }


    }
}