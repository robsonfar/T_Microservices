using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using T_Microservices.Services.ShoppingCartAPI.Models.Dto;

namespace T_Microservices.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int CartHeaderId { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}
