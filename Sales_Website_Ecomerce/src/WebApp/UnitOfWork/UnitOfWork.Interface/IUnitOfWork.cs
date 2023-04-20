namespace UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create();
    }
}
