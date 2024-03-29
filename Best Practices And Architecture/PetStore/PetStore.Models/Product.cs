﻿using PetStore.Common;
using PetStore.Models.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        [MinLength(GlobalConstants.ProductNameMinLength)]
        public string Name { get; set; }
        public ProductType ProductType { get; set; }
        [Range(GlobalConstants.SellableMinPrice, Double.MaxValue)]
        public decimal Price { get; set; }
    }
}
