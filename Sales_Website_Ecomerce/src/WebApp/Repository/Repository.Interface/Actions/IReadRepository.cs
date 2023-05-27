using System.Linq.Expressions;

namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T, Y> where T : class
    {
        List<T> GetAll(Y item);
        T GetByCondition(params int[] values);
        T Get(Y id);
    }
}
