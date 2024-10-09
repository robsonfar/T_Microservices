using T_Microservices.Web.Models;

namespace T_Microservices.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
