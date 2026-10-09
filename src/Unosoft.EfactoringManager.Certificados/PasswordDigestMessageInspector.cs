using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace Unosoft.EfactoringManager.Certificados
{
    public class PasswordDigestMessageInspector : IClientMessageInspector
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public PasswordDigestMessageInspector(string username, string password)
        {
            Username = username;
            Password = password;
        }

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            return;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var doc = new XmlDocument();

            var securityElement = doc.CreateElement(
                "wsse",
                "Security",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");

            var usernameTokenElement = CreateUsernameTokenElement(doc);
            securityElement.AppendChild(usernameTokenElement);

            var securityHeader = MessageHeader.CreateHeader(
                "Security",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd",
                securityElement,
                mustUnderstand: false);

            request.Headers.Add(securityHeader);

            return null;
        }

        private XmlElement CreateUsernameTokenElement(XmlDocument doc)
        {
            var wsseNs =
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            var wsuNs =
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            var usernameToken = doc.CreateElement("wsse", "UsernameToken", wsseNs);
            var idAttribute = doc.CreateAttribute("wsu", "Id", wsuNs);
            idAttribute.Value = $"UsernameToken-{Guid.NewGuid():N}";
            usernameToken.Attributes.Append(idAttribute);

            var usernameElement = doc.CreateElement("wsse", "Username", wsseNs);
            usernameElement.InnerText = Username;
            usernameToken.AppendChild(usernameElement);

            // Nonce aleatorio
            var nonceBytes = RandomNumberGenerator.GetBytes(16);
            var nonceBase64 = Convert.ToBase64String(nonceBytes);
            var nonceElement = doc.CreateElement("wsse", "Nonce", wsseNs);
            nonceElement.SetAttribute("EncodingType",
                "http://docs.oasis-open.org/wss/2004/01/" +
                "oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            nonceElement.InnerText = nonceBase64;
            usernameToken.AppendChild(nonceElement);

            // Created
            var createdElement = doc.CreateElement("wsu", "Created", wsuNs);
            var created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            createdElement.InnerText = created;
            usernameToken.AppendChild(createdElement);

            // PasswordDigest = Base64( SHA1( NonceBytes + UTF8(Created) + UTF8(Password) ) )
            var createdBytes = Encoding.UTF8.GetBytes(created);
            var passwordBytes = Encoding.UTF8.GetBytes(Password);
            var toHash = new byte[nonceBytes.Length + createdBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(nonceBytes, 0, toHash, 0, nonceBytes.Length);
            Buffer.BlockCopy(createdBytes, 0, toHash, nonceBytes.Length, createdBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, toHash,
                nonceBytes.Length + createdBytes.Length,
                passwordBytes.Length);

            byte[] hashBytes;
            using (var sha1 = SHA1.Create())
            {
                hashBytes = sha1.ComputeHash(toHash);
            }

            var passwordDigest = Convert.ToBase64String(hashBytes);

            var passwordElement = doc.CreateElement("wsse", "Password", wsseNs);
            passwordElement.SetAttribute("Type",
                "http://docs.oasis-open.org/wss/2004/01/" +
                "oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            passwordElement.InnerText = passwordDigest;
            usernameToken.AppendChild(passwordElement);

            return usernameToken;
        }

        #endregion
    }
}
