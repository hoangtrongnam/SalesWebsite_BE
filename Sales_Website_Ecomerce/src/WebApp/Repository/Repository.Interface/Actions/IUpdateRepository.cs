namespace Repository.Interfaces.Actions
{
    public interface IUpdateRepository<T, y> where T : class
    {
        int Update(T item, y id);
    }
}
