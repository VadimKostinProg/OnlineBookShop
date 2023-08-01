using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Domain.Entities
{
    public class Discount : EntityBase
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(2, 1000)]
        public int Count { get; set; }

        [Required]
        [Range(1, 100)]
        public double DiscountAmount { get; set; }
    }
}
