using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        //public List<Customer> Customers { get; } = [];
    }
}