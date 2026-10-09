using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Web
{
    public class MultipartParser
    {
        private byte[] requestData;

        private MultipartParser()
        {
        }

        public static async Task<MultipartParser> CreateAsync(Stream stream)
        {
            return await CreateAsync(stream, Encoding.UTF8);
        }

        public static async Task<MultipartParser> CreateAsync(Stream stream, Encoding encoding)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var parser = new MultipartParser();
            await parser.ParseAsync(stream, encoding).ConfigureAwait(false);
            await parser.ParseParameterAsync(stream, encoding).ConfigureAwait(false);
            return parser;
        }

        private async Task ParseAsync(Stream stream, Encoding encoding)
        {
            Success = false;

            var data = await ToByteArrayAsync(stream).ConfigureAwait(false);
            requestData = data;

            var content = encoding.GetString(data);
            var delimiterEndIndex = content.IndexOf("\r\n", StringComparison.Ordinal);

            if (delimiterEndIndex <= -1)
            {
                return;
            }

            var delimiter = content.Substring(0, delimiterEndIndex);

            var re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
            var contentTypeMatch = re.Match(content);

            re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
            var filenameMatch = re.Match(content);

            if (!contentTypeMatch.Success || !filenameMatch.Success)
            {
                return;
            }

            ContentType = contentTypeMatch.Value.Trim();
            Filename = filenameMatch.Value.Trim();

            var startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

            var delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
            var endIndex = IndexOf(data, delimiterBytes, startIndex);
            if (endIndex < 0)
            {
                return;
            }

            var contentLength = endIndex - startIndex;
            if (contentLength <= 0)
            {
                return;
            }

            var fileData = new byte[contentLength];
            Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);

            FileContents = fileData;
            Success = true;
        }

        private async Task ParseParameterAsync(Stream stream, Encoding encoding)
        {
            Success = false;

            byte[] data;
            if (requestData == null || requestData.Length == 0)
            {
                data = await ToByteArrayAsync(stream).ConfigureAwait(false);
            }
            else
            {
                data = requestData;
            }

            var content = encoding.GetString(data);
            var delimiterEndIndex = content.IndexOf("\r\n", StringComparison.Ordinal);

            if (delimiterEndIndex <= -1)
            {
                return;
            }

            var delimiter = content.Substring(0, delimiterEndIndex);
            var splitContents = content.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var t in splitContents)
            {
                var contentTypeRegex = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                var contentTypeMatch = contentTypeRegex.Match(t);

                var re = new Regex(@"(?<=name\=\"")(.*)");
                var name = re.Match(t);

                re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                var filenameMatch = re.Match(t);

                if (!name.Success && !filenameMatch.Success)
                {
                    continue;
                }

                var startIndex = contentTypeMatch.Success
                    ? contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length
                    : name.Index + name.Length + "\r\n\r\n".Length;

                if (filenameMatch.Success)
                {
                    Filename = filenameMatch.Value.Trim();
                }

                if (startIndex <= 0 || startIndex >= t.Length)
                {
                    continue;
                }

                // Ojo: en tu código original usabas startIndex - 1
                var propertyData = t.Substring(startIndex - 1, t.Length - startIndex);

                var myContent = new MyContent
                {
                    Data = encoding.GetBytes(propertyData),
                    StringData = propertyData,
                    PropertyName = name.Value.Trim()
                };

                MyContents ??= new List<MyContent>();
                MyContents.Add(myContent);
                Success = true;
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            var index = 0;
            var startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos == -1)
            {
                return -1;
            }

            while (startPos + index < searchWithin.Length)
            {
                if (searchWithin[startPos + index] == serachFor[index])
                {
                    index++;
                    if (index == serachFor.Length)
                    {
                        return startPos;
                    }
                }
                else
                {
                    startPos = Array.IndexOf(searchWithin, serachFor[0], startPos + index);
                    if (startPos == -1)
                    {
                        return -1;
                    }

                    index = 0;
                }
            }

            return -1;
        }

        private async Task<byte[]> ToByteArrayAsync(Stream stream)
        {
            var buffer = new byte[32768];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                {
                    await ms.WriteAsync(buffer, 0, read).ConfigureAwait(false);
                }

                return ms.ToArray();
            }
        }

        public List<MyContent> MyContents { get; set; }

        public bool Success { get; private set; }

        public string ContentType { get; private set; }

        public string Filename { get; private set; }

        public byte[] FileContents { get; private set; }
    }

    public class MyContent
    {
        public byte[] Data { get; set; }
        public string PropertyName { get; set; }
        public string StringData { get; set; }
    }
}
