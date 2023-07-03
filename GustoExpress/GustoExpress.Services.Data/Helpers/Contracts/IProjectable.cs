namespace GustoExpress.Services.Data.Helpers.Contracts
{
    public interface IProjectable<P>
    {
        public T ProjectTo<T>(P obj);
    }
}
