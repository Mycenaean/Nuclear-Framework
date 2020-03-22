using Nuclear.Channels.Remoting.Enums;
using Nuclear.Channels.Remoting.Services;
using Nuclear.ExportLocator.Decorators;
using Nuclear.ExportLocator.Enumerations;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    [Export(typeof(IRemoteInvoker), ExportLifetime.Transient)]
    internal class RemoteInvoker : IRemoteInvoker
    {
        public string InvokeChannel(string URL, HttpChannelMethod method, IChannelCredentials credentials)
        {
            bool authenticate = credentials != null;
            HttpWebRequest webRequest = CreateWebRequest(URL, method, authenticate, null);
            if (authenticate)
                webRequest = InserCredentials(webRequest, credentials);

            return GetResponse(webRequest);
        }

        public string InvokeChannel(string URL, string bodyInput, HttpChannelMethod method, IChannelCredentials credentials, string contentType = null)
        {
            bool authenticate = credentials != null;
            HttpWebRequest webRequest = CreateWebRequest(URL, method, authenticate, contentType);
            if (authenticate)
                webRequest = InserCredentials(webRequest, credentials);

            if (method == HttpChannelMethod.POST)
            {
                byte[] body = Encoding.UTF8.GetBytes(bodyInput);
                webRequest.ContentLength = body.Length;
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(body, 0, body.Length);
                requestStream.Close();
            }

            return GetResponse(webRequest);
        }


        private HttpWebRequest CreateWebRequest(string URL, HttpChannelMethod method, bool authenticate, string contentType = null)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(URL);
            webRequest.Method = method.ToString();
            webRequest.Accept = "application/json";
            if (contentType == null)
                webRequest.ContentType = "application/json";
            else
                webRequest.ContentType = contentType;
            if (authenticate)
            {
                webRequest.PreAuthenticate = true;
            }
            return webRequest;
        }

        private HttpWebRequest InserCredentials(HttpWebRequest webRequest, IChannelCredentials credentials)
        {
            string encodedAuthorization = string.Empty;
            if (credentials.AuthenticationOptions == ChannelAuthenticationOptions.Basic)
                encodedAuthorization = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{credentials.Username}:{credentials.Password}"));
            else
                encodedAuthorization = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{credentials.Token}"));

            webRequest.Headers.Add("Authorization", $"{credentials.AuthenticationOptions.ToString()} {encodedAuthorization}");
            return webRequest;
        }

        private string GetResponse(HttpWebRequest webRequest)
        {
            string read = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                read = reader.ReadToEnd();
                reader.Close();
            }
            return read;
        }
    }
}
