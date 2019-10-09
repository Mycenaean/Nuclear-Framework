using Newtonsoft.Json;
using Nuclear.Channels.Remoting.Services;
using Nuclear.ExportLocator.Decorators;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    [Export(typeof(IChannelRemotingService))]
    public class ChannelRemotingService : IChannelRemotingService
    {
        public IChannelCredentials Credentials => new ChannelCredentials();
        public string InvokeChannel(string URL, string HttpMethod, string username = null, string password = null)
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(URL);
            _request.Method = HttpMethod.ToString();
            _request.Accept = "application/json";
            if (!String.IsNullOrEmpty(username))
            {
                CreateCredentials(_request, username, password);
            }

            return Invoke(_request);
        }

        public string InvokeChannel(string channel, string channelmethod, string baseURL)
        {
            string URL = $"{baseURL}/{channel}/{channelmethod}/";
            return InvokeChannel(URL, "GET");
        }

        public string InvokeChannel(string channel, string channelmethod, string baseURL, object inputBody)
        {
            string URL = $"{baseURL}/{channel}/{channelmethod}/";
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(URL);
            _request.Method = "POST";
            _request.Accept = "application/json";
            string body = JsonConvert.SerializeObject(inputBody);
            CreateInputBody(_request, body);
            return Invoke(_request);
        }


        public void CreateCredentials(HttpWebRequest request, string username, string password)
        {
            request.PreAuthenticate = true;
            string encodedAuthorization = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));
            request.Headers.Add("Authorization", $"Basic {encodedAuthorization}");
        }

        public void CreateInputBody(HttpWebRequest request, string inputBody)
        {
            byte[] body = Encoding.UTF8.GetBytes(inputBody);
            request.ContentLength = body.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(body, 0, body.Length);
            requestStream.Close();
        }

        public string Invoke(HttpWebRequest request)
        {
            string read = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    read = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

            }

            return read;
        }
    }

}

