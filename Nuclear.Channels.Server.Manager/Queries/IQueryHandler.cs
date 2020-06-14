namespace Nuclear.Channels.Server.Manager.Queries
{
    public interface IQueryHandler<in IQuery, out TResult>
    {
        TResult Handle(IQuery query);
    }
}
