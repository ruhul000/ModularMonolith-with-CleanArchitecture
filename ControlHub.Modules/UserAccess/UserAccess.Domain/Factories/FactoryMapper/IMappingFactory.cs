namespace UserAccess.Domain.Factories.FactoryMapper
{
    public interface IMappingFactory<T>
    {
        T CreateFrom<S>(S sourceObject);

        IEnumerable<T> CreateMultipleFrom<S>(S sourceObject);
    }
}
