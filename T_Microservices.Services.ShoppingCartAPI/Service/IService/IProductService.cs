using T_Microservices.Services.ShoppingCartAPI.Models.Dto;

namespace T_Microservices.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
