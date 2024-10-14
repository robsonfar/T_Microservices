using Newtonsoft.Json;

using T_Microservices.Services.ShoppingCartAPI.Models.Dto;
using T_Microservices.Services.ShoppingCartAPI.Service.IService;

namespace T_Microservices.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }


        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");

            var response = await client.GetAsync($"/api/product");

            var apiContet = await response.Content.ReadAsStringAsync();

            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContet);

            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }

            return new List<ProductDto>();
        }
    }
}
