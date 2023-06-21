using UnitOfWork.Interface;

namespace Services
{
    public interface ICommonService
    {
        string GetConfigValueService(int key);
    }
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get Config Value Service
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueService(int key)
        {
            using (var context = _unitOfWork.Create())
            {
                return context.Repositories.CommonRepository.GetConfigValue(key);
            }
        }
    }
}
