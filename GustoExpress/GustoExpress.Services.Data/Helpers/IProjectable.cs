namespace GustoExpress.Services.Data.Helpers
{
    public interface IProjectable<P>
    {
        public T ProjectTo<T>(P obj);
    }
}
