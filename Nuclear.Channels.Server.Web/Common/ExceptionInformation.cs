namespace Nuclear.Channels.Server.Web.Common
{
    public class ExceptionInformation
    {
        public string Message { get; }
        public string InvokationMethod { get; }

        public ExceptionInformation(string message,string invokationMethod)
        {
            Message = message;
            InvokationMethod = invokationMethod;
        }

    }
}
