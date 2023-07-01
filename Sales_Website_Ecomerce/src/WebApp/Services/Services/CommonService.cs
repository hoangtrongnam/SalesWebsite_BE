using Models.RequestModel;
using UnitOfWork.Interface;

namespace Services
{
    public interface ICommonService
    {
        string GetConfigValueService(int key);
        void LogExceptionToDatabase(LogExceptionModel log);
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
        /// <summary>
        /// Write Log Exception to Database
        /// </summary>
        /// <param name="log"></param>
        public void LogExceptionToDatabase(LogExceptionModel log)
        {
            using (var context = _unitOfWork.Create())
            {
                context.Repositories.CommonRepository.LogExeption(log);
                context.SaveChanges();
            }
        }
    }
}
