using Trustesse_Assessment.Dto;

namespace Trustesse_Assessment.AuthServices
{
    public interface IAuth
    {
        Task<bool> ValidateUser(LoginDto loginDto);

        Task<string> CreateToken();
        //Task<string> ConfirmEmail(ConfirmEmailDto confirmEmail);

    }
}
