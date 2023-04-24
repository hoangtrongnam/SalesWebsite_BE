using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.RequestModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnitOfWork.Interface;

namespace Services
{
    public interface IAccountServices
    {
        string SignInAsync(SignInRequestModel model);
    }

    public class AccountServices : IAccountServices
    {
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AccountServices(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public string SignInAsync(SignInRequestModel model)
        {
            using (var context = _unitOfWork.Create())
            {
                var result = context.Repositories.AccountRepository.SignInAsync(model);
                if (!result)
                {
                    return string.Empty;
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.EmaiAddress),
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
    }
}
