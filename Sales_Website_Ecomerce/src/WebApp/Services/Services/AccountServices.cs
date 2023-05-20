using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.RequestModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnitOfWork.Interface;
using Common;
using Models.ResponseModels;
using AutoMapper;

namespace Services
{
    public interface IAccountServices
    {
        string SignIn(SignInRequestModel model);
        int UserRegister(UserRegisterRequestModel model);
        int UpdateInfoUser(UpdateInfoUserRequestModel model, int Id);
        UserResponseModel GetUserByUserName(string username);
        int ChangePassword(ChangePasswordRequestModel model);
    }

    public class AccountServices : IAccountServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AccountServices(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public string SignIn(SignInRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var user = context.Repositories.AccountRepository.Get(FunctionHandleString.GetUsernameFromEmail(model.EmaiAddress));
                //Tên đăng nhập không đúng
                if (user == null)
                    return string.Empty;

                //không đúng mật khẩu
                if (!Hash_BCrypt.VerifyPassword(model.Password, user.PasswordHasd))
                    return string.Empty;    


                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.EmaiAddress),
                    new Claim("TenantID",user.TenanID),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken
                  (
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                  );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
        /// <summary>
        /// Register Service
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UserRegister(UserRegisterRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                model.Password = Hash_BCrypt.HashPassword(model.Password);
                model.UserName = FunctionHandleString.GetUsernameFromEmail(model.EmailAddress);
                var result = context.Repositories.AccountRepository.Create(model);
                context.SaveChanges();
                return result;
            }
        }
        /// <summary>
        /// Update Info User Service
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int UpdateInfoUser(UpdateInfoUserRequestModel model, int Id)
        {
            var modelMap = _mapper.Map<UpdateUserCommonRequestModel>(model);
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AccountRepository.Update(modelMap, Id);
                context.SaveChanges();
                return result;
            }
        }
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserResponseModel GetUserByUserName(string username)
        {
            using (var context = _unitOfWork.Create())
            {
                return context.Repositories.AccountRepository.Get(username);
            }
        }
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ChangePassword(ChangePasswordRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var user = context.Repositories.AccountRepository.Get(FunctionHandleString.GetUsernameFromEmail(model.EmailAddress));
                if (user == null)
                    return -2; //Không tồn tại user
                if (!Hash_BCrypt.VerifyPassword(model.PasswordOld, user.PasswordHasd))
                    return -3; //không đúng password

                model.Password = Hash_BCrypt.HashPassword(model.Password);
                var modelMap = _mapper.Map<UpdateUserCommonRequestModel>(model);
                var result = context.Repositories.AccountRepository.Update(modelMap, user.ID);

                context.SaveChanges();

                return result;
            }
        }
    }
}
