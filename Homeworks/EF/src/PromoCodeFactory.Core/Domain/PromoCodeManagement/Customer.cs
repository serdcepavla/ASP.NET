using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Customer
        : BaseEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(50)]
        public string Email { get; set; }

        //TODO: Списки Preferences и Promocodes 
        public List<Preference> Preferences { get; set; } = [];
        public List<PromoCode> PromoCodes { get; set; }
    }
}