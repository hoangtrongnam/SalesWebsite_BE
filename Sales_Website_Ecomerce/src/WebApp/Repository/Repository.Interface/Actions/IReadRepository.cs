namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T, Y> where T : class
    {
        List<T> GetAll(Y pageIndex);
        List<T> GetAll();
        T Get(Y id);
        T Get(Y id, Y pageIndex); //CartProduct
    }
}
