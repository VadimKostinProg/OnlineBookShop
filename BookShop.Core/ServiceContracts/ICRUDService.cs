using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Base CRUD interface for services.
    /// </summary>
    /// <typeparam name="TAddRequest">Add request DTO model.</typeparam>
    /// <typeparam name="TUpdateRequest">Update request DTO model.</typeparam>
    /// <typeparam name="TResponse">Response DTO model.</typeparam>
    public interface ICRUDService<TAddRequest, TUpdateRequest, TResponse>
    {
        /// <summary>
        /// Method for reading all values from the table.
        /// </summary>
        /// <returns>Collection IEnumerable of response DTO models.</returns>
        Task<IEnumerable<TResponse>> GetAllAsync();

        /// <summary>
        /// Method for reading value from the table by primary key.
        /// </summary>
        /// <param name="id">Guid of entity to read.</param>
        /// <returns>Response DTO model of found value.</returns>
        Task<TResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// Method for inserting new value into the table.
        /// </summary>
        /// <param name="request">Add request DTO model to insert.</param>
        /// <returns>Response DTO model of inserted model.</returns>
        Task<TResponse> CreateAsync(TAddRequest request);

        /// <summary>
        /// Method for updating entity in the table.
        /// </summary>
        /// <param name="request">Update request DTO model to update.</param>
        /// <returns>Response DTO model of updated entity.</returns>
        Task<TResponse> UpdateAsync(TUpdateRequest request);

        /// <summary>
        /// Method for the deleting entity from the table.
        /// </summary>
        /// <param name="id">Guid of entity to delete.</param>
        Task DeleteAsync(Guid id);
    }
}
