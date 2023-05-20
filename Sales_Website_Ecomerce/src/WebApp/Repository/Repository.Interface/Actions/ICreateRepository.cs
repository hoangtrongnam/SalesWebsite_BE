namespace Repository.Interfaces.Actions
{
    public interface ICreateRepository<T, Y> where T : class
    {
        Y Create(T item);
    }
}
