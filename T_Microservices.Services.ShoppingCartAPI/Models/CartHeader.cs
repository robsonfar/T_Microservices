﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace T_Microservices.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        [NotMapped]
        public double Discount { get; set; }

        [NotMapped]
        public double CartTotal { get; set; }
    }
}
