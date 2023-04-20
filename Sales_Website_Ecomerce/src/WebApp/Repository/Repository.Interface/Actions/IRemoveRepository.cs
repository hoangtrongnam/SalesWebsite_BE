namespace Repository.Interfaces.Actions
{
    public interface IRemoveRepository<T, y>
    {
        int Remove(y id);
        int Remove(T item, y id);
    }
}
