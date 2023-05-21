using Models.RequestModel;
using UnitOfWork.Interface;
using Common;
using Quartz;
using Quartz.Impl;
using Models.ResponseModels;
using System;

namespace Services
{
    public enum JobName
    {
        RejectOrder
    }

    public class ConstParamClass
    {
        public const int StatusNoContact = 12; //1. Get order with satus = 12 (Sale không liên hệ được với KH)
        public const int StatusSuccess = 1;
        public const int StatusFailed = 0;
        public const int StatusCanceled = 15;
        public const int StatusNewNotificationCustomer = 26;
        public const int TimeHours = 12;
        // Thiết lập ITrigger để kích hoạt công việc vào 7 giờ sáng theo múi giờ Việt Nam
        public static DateTimeOffset StartTime = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0, TimeSpan.FromHours(7));
        public const int TimeMinutes = 12;
    }
    public interface IJobScheduleServices
    {
        Task RunJob(string nameJob);
        Task<ResultModel> StopJob(string nameJob);
    }

    public class JobScheduleServices : IJobScheduleServices
    {
        public IUnitOfWork _unitOfWork;
        public JobScheduleServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task RunJob(string nameJob)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            var jobDataMap = new JobDataMap();
            jobDataMap.Add("unitOfWork", _unitOfWork);

            IJobDetail job = JobBuilder.Create<RejectOrder>()
                             .UsingJobData(jobDataMap)
                             .Build();

            //if(nameJob == "RejectOrder")
            //{
            //job = JobBuilder.Create<RejectOrder>().Build();
            //}


            ITrigger trigger = TriggerBuilder.Create()
               .WithIdentity("TriggerRejectOrder", "OrderGroup")
               .StartAt(ConstParamClass.StartTime)
               .WithSimpleSchedule(x => x
                   .WithIntervalInHours(ConstParamClass.TimeHours) //cách 12H chạy 1 lần
                   .RepeatForever())
               .Build();

            await scheduler.ScheduleJob(job, trigger);


            await scheduler.Start();

            //await scheduler.Shutdown();
        }

        public async Task<ResultModel> StopJob(string nameJob)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            ResultModel resultModel = new ResultModel();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            //Console.WriteLine("StopJob");
            if (nameJob == JobName.RejectOrder.ToString())
            {
                await scheduler.UnscheduleJob(new TriggerKey("TriggerRejectOrder", "OrderGroup"));

                using (var contextUOF = _unitOfWork.Create())
                {
                    JobRequestModel jobRequestModel = new JobRequestModel();
                    jobRequestModel.Status = ConstParamClass.StatusSuccess;
                    jobRequestModel.JobName = JobName.RejectOrder.ToString();
                    jobRequestModel.Content = "Stopped";
                    contextUOF.Repositories.JobScheduleRepository.Create(jobRequestModel);

                    contextUOF.SaveChanges();
                }
            }
            else
            {
                resultModel.Message = "Khong co job nay";
                return resultModel;
            }
            return resultModel;
        }
    }
    public class RejectOrder : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var _unitOfWork = (IUnitOfWork)context.JobDetail.JobDataMap["unitOfWork"];
            JobRequestModel jobRequestModel = new JobRequestModel();

            try
            {
                using (var contextUOF = _unitOfWork.Create())
                {
                    //1. Get order with ConstParamClass.StatusCancle = 12 (Sale không liên hệ được với KH)
                    OrderResponseModel Order = contextUOF.Repositories.OrderRepository.GetLstOrder(ConstParamClass.StatusNoContact);

                    if (!Order.OrderID.Contains(',')) //không có đơn hàng hết hạn
                    {
                        //Ghi table history Job (Status = thanh cong)
                        jobRequestModel.Status = ConstParamClass.StatusSuccess;
                        jobRequestModel.JobName = JobName.RejectOrder.ToString();
                        jobRequestModel.Content = "Không có đơn hàng hết hạn";
                        contextUOF.Repositories.JobScheduleRepository.Create(jobRequestModel);

                        contextUOF.SaveChanges();

                        return;
                    }

                    string[] stringArray = Order.OrderID.Split(',');

                    foreach (string str in stringArray)
                    {
                        if (string.IsNullOrEmpty(str)) continue;

                        int orderID = Convert.ToInt32(str);

                        //2. Update satus = 15 (Hủy Đơn hàng)
                        OrderRequestModel orderRequestModel = new OrderRequestModel();
                        orderRequestModel.Status = ConstParamClass.StatusCanceled;

                        contextUOF.Repositories.OrderRepository.Update(orderRequestModel, orderID);

                        //3 Insert table notifications (KH)
                        NotificationRequestModel notificationRequestModel = new NotificationRequestModel();
                        notificationRequestModel.Status = ConstParamClass.StatusNewNotificationCustomer;
                        notificationRequestModel.Content = "Đơn hàng: " + orderID + " đã bị hủy";
                        var step3 = contextUOF.Repositories.NotificationRepository.Create(notificationRequestModel);
                    }

                    //4. Update Product Quantity
                    foreach (var item in Order.lstProduct)
                    {
                        //4.1. Get Product
                        var product = contextUOF.Repositories.ProductRepository.Get(item.ProductID);
                        ProductRequestModel productRequestModel = new ProductRequestModel();
                        productRequestModel.Quantity = product.Quantity + item.Quantity;

                        //4.2. Update Product Quantity
                        contextUOF.Repositories.ProductRepository.Update(productRequestModel, item.ProductID);
                    }

                    //5. Ghi table history Job (Status = thanh cong)
                    jobRequestModel.Status = ConstParamClass.StatusSuccess;
                    jobRequestModel.JobName = JobName.RejectOrder.ToString();
                    jobRequestModel.Content = "Started";
                    contextUOF.Repositories.JobScheduleRepository.Create(jobRequestModel);

                    contextUOF.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                using (var contextUOF = _unitOfWork.Create())
                {
                    //Ghi table history Job (Status = that bai)
                    jobRequestModel.Status = ConstParamClass.StatusFailed;
                    jobRequestModel.JobName = JobName.RejectOrder.ToString();
                    jobRequestModel.Content = ex.Message;
                    contextUOF.Repositories.JobScheduleRepository.Create(jobRequestModel);
                    contextUOF.SaveChanges();
                }
                throw new Exception(ex.Message);
            }
        }
    }
}
