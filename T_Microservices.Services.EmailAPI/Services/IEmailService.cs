using T_Microservices.Services.EmailAPI.Models.Dto;

namespace T_Microservices.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
