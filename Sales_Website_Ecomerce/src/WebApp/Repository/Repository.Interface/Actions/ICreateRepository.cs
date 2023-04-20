namespace Repository.Interfaces.Actions
{
    public interface ICreateRepository<T> where T : class
    {
        int Create(T item);
    }
}
