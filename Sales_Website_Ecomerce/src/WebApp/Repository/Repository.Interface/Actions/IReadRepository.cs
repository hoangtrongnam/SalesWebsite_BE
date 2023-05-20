namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T, Y> where T : class
    {
        List<T> GetAll(Y item);
        T Get(Y id);
        T Get(Y id, Y pageIndex); //CartProduct
    }
}
