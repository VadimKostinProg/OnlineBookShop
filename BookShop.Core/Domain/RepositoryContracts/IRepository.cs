using BookShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Data access logic layer.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Method for reading all rows from the table related to passed entity.
        /// </summary>
        /// <typeparam name="T">Entity type to read table rows.</typeparam>
        /// <param name="predicate">Expression to filter objects.</param>
        /// <param name="includeStrings">Include strings for objects of other entities.</param>
        /// <returns>Collection IEnumerable of entity objects.</returns>
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>>? predicate = null,
                                            params string[] includeStrings) where T : EntityBase;

        /// <summary>
        /// Method for reading single entity from the table by it`s primary key.
        /// </summary>
        /// <typeparam name="T">Entity type to read.</typeparam>
        /// <param name="id">Guid of entity to read.</param>
        /// <param name="includeStrings">Include strings for objects of other entities.</param>
        /// <returns>Found entity with passed guid, null - if entity is not found.</returns>
        Task<T?> GetByIdAsync<T>(Guid id, params string[] includeStrings) where T : EntityBase;

        /// <summary>
        /// Method for reading first object which satisfy passed condition.
        /// </summary>
        /// <typeparam name="T">Entity type to read.</typeparam>
        /// <param name="predicate">Condition to search object.</param>
        /// <param name="includeStrings">Include strings for objects of other entities.</param>
        /// <returns>Found entity object, null - if object with passed conditions is not found.</returns>
        Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate,
                                        params string[] includeStrings) where T : EntityBase;

        /// <summary>
        /// Method for checking the object for existance in the table.
        /// </summary>
        /// <typeparam name="T">Entity type to check object for existance.</typeparam>
        /// <param name="predicate">Condition for checking the object for existance.</param>
        /// <returns>True - if object with passed condition exists, otherwise - false.</returns>
        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : EntityBase;

        /// <summary>
        /// Method for inserting new row into the table.
        /// </summary>
        /// <typeparam name="T">Entity type which indicades the table to insert new row.</typeparam>
        /// <param name="entity">Entity to insert.</param>
        Task AddAsync<T>(T entity) where T : EntityBase;

        /// <summary>
        /// Method for updating existing row in the table.
        /// </summary>
        /// <typeparam name="T">Entity type which indicades the table to update existing row.</typeparam>
        /// <param name="entity">Entity to update.</param>
        Task UpdateAsync<T>(T entity) where T : EntityBase;

        /// <summary>
        /// Method for deleting row from the table by it`s primary key.
        /// </summary>
        /// <typeparam name="T">Entity type which indicades the table to delete row.</typeparam>
        /// <param name="id">Guid of entity to delete.</param>
        /// <returns>True - if row was successfully deleted, otherwise - false.</returns>
        Task<bool> DeleteAsync<T>(Guid id) where T : EntityBase;
    }
}
