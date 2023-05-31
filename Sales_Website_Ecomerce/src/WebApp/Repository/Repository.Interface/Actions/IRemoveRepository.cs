namespace Repository.Interfaces.Actions
{
    public interface IRemoveRepository<T>
    {
        int Remove(T id);
    }
}
