using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;

namespace Unosoft.EFactoringManager.Utils
{
    [SupportedOSPlatform("windows")]
    public class DrawImage
    {

        /// <summary>
        /// Converting text to image (png).
        /// </summary>
        /// <param name="text">text to convert</param>
        /// <param name="font">Font to use</param>
        /// <param name="textColor">text color</param>
        /// <param name="maxWidth">max width of the image</param>
        /// <param name="path">path to save the image</param>
        public static void DrawFirma(String nombre, string identificacion,  int maxWidth, String path)
        {


            Font font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
            Color textColor = Color.Black;

            //DrawText("DIEGO ANDRES BORRERO TIGREROS", font, Color.Black, 400, "c:\\temp\\imagenprueba.png");


            String pnglogo = @"c:\temp\LogoFirma.png";


            Image img1 = Image.FromFile(pnglogo);

            var byteImagen = ImageToByteArray(img1);
            string temp_inBase64 = Convert.ToBase64String(byteImagen);

            byte[] temp_backToBytes = Convert.FromBase64String(temp_inBase64);

            img1 = byteArrayToImage(temp_backToBytes);

            //first, create a dummy bitmap just to get a graphics object
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            //measure the string to see how big the image needs to be
            SizeF textSize = drawing.MeasureString(nombre, font, maxWidth);

            //set the stringformat flags to rtl
            StringFormat sf = new StringFormat();
            //uncomment the next line for right to left languages
            //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            sf.Trimming = StringTrimming.Word;
            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap(img1.Width + 400, (int)textSize.Height * 4);

            drawing = Graphics.FromImage(img);
            //Adjust for high quality
            drawing.CompositingQuality = CompositingQuality.HighQuality;
            drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
            drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            //paint the background
            drawing.Clear(Color.Transparent);

            //create a brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawImage(img1, new Point(0, 0));

            //drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);
            font = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Pixel);
            drawing.DrawString("Firmado electronicamento por:", font, textBrush, new RectangleF(160, 0, textSize.Width, textSize.Height), sf);
            font = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit.Pixel);
            drawing.DrawString(nombre, font, textBrush, new RectangleF(160, 17, textSize.Width, textSize.Height), sf);
            drawing.DrawString($"CC {identificacion}", font, textBrush, new RectangleF(160, 34, textSize.Width, textSize.Height), sf);
            font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            drawing.DrawString($"Fecha: {DateTime.Now: dd/MM/yyyy HH:mm:ss}", font, textBrush, new RectangleF(160, 51, textSize.Width, textSize.Height), sf);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            img.Save(path, ImageFormat.Png);


            img.Dispose();

        }

        private static string FormatIndentificacion (String identificacion)
        {
            if (int.TryParse(identificacion, out int valor))
            {
                NumberFormatInfo nfi = new NumberFormatInfo { NumberGroupSeparator = ".", NumberDecimalDigits = 0 };
                return valor.ToString(nfi); // 12 345 678
            }
            return identificacion;
        
        }

        private static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
