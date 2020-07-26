namespace Nuclear.Channels.Server.Web.Common
{
    public interface IEventHandler<in TRequest, out TResult>
    {
        TResult Handle(TRequest request);
    }

    public interface IEventHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}
