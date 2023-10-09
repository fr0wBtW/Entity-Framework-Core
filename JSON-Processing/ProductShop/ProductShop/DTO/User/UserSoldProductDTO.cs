﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTO.User
{
    public class UserSoldProductDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("buyerFirstName")]
        public string BuyerFirstName { get; set; }
        [JsonProperty("buyerLastName")]
        public string BuyerLastName { get; set; }
    }
}
