using T_Microservices.Web.Models;
using T_Microservices.Web.Models.Dto;

namespace T_Microservices.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetByIdAsync(int id);
        Task<ResponseDto?> CreateAsync(ProductDto dto);
        Task<ResponseDto?> UpdateAsync(ProductDto dto);
        Task<ResponseDto?> DeleteAsync(int id);
    }
}
