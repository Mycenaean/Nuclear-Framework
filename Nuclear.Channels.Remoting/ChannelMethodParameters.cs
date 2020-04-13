using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuclear.Channels.Remoting
{
    public class ChannelMethodParameters
    {
        private List<ChannelMethodParameter> _parameters { get; set; }
        public RequestContentType ContentType { get; set; }

        public ChannelMethodParameters(RequestContentType contentType)
        {
            if (_parameters == null)
                _parameters = new List<ChannelMethodParameter>();

            ContentType = contentType;
        }

        public void AddParameter(ChannelMethodParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void RemoveParameter(ChannelMethodParameter parameter)
        {
            _parameters.Remove(parameter);
        }

        public void AddParameter(string name, object value)
        {
            _parameters.Add(new ChannelMethodParameter { Name = name, Value = value });
        }

        public void RemoveParameter(string name)
        {
            _parameters.Remove(_parameters.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
        }

        public ChannelMethodParameter GetFirstParam()
        {
            return _parameters.First();
        }

        public int Count()
        {
            return _parameters.Count();
        }

        public IReadOnlyCollection<ChannelMethodParameter> AllParameters()
        {
            return _parameters;
        }

    }
}
