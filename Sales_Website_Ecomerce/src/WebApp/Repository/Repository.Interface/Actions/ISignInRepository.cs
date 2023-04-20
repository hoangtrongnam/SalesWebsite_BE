namespace Repository.Interface.Actions
{
    public interface ISignInRepository<T, Y> where T : class
    {
        Y SignInAsync(T t);
    }
}
