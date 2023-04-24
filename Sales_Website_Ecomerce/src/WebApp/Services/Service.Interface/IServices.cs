using Common;
namespace Service.Interface
{
    public interface IServices<T, y>
    {
        ResultModel GetAll(int pageIndex); //use Product
        ResultModel GetAll();
        ResultModel Get(int ID); //Category k pageIndex
        ResultModel Get(y ID, y pageIndex);
        ResultModel Create(T model);
        ResultModel Update(T model, y ID);
        ResultModel Delete(T model, y ID);
        ResultModel Delete(int ID);
    }
}