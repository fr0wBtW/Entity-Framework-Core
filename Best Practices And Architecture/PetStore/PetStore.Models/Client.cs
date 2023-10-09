﻿using PetStore.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class Client
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();

            this.PetsBuyed = new HashSet<Pet>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [MinLength(GlobalConstants.UsernameMinLength)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MinLength(GlobalConstants.EmailMinLength)]
        public string Email { get; set; }
        [Required]
        [MinLength(GlobalConstants.ClientNameMinLength)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(GlobalConstants.ClientNameMinLength)]
        public string LastName { get; set; }
        public virtual ICollection<Pet> PetsBuyed { get; set; }
    }
}
