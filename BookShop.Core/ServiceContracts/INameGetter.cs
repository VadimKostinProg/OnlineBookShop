using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service interface for entities that has unique name.
    /// </summary>
    /// <typeparam name="TResponse">Response DTO model.</typeparam>
    public interface INameGetter<TResponse>
    {
        /// <summary>
        /// Method for reading entity with passed name.
        /// </summary>
        /// <param name="name">Name to read entity.</param>
        /// <returns>Response DTO model with passed name.</returns>
        Task<TResponse> GetByNameAsync(string name);
    }
}
