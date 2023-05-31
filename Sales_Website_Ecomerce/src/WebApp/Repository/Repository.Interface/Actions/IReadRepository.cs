using System.Linq.Expressions;

namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T, Y> where T : class
    {
        T Get(Y id);
    }
}
