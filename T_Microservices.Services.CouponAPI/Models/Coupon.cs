﻿using System.ComponentModel.DataAnnotations;

namespace T_Microservices.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
