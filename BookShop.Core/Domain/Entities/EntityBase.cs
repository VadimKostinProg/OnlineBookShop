using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Domain.Entities
{
    /// <summary>
    /// Base class for all entities for data base.
    /// </summary>
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
