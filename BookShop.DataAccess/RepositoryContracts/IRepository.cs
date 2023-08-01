using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.RepositoryContracts
{
    /// <summary>
    /// Repository for data base.
    /// </summary>
    /// <typeparam name="T">Entity of data base.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Method to insert new entity to data base.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        void Insert(T entity);

        /// <summary>
        /// Method for reading entity by guid.
        /// </summary>
        /// <param name="id">Guid of entity to read.</param>
        /// <returns>Entity with entered guid, null if not found.</returns>
        T? GetValueById(Guid id);

        /// <summary>
        /// Method that returns all entitties from data base.
        /// </summary>
        /// <returns>IEnumerable of entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Method that returns filtered collection of entities from data base.
        /// </summary>
        /// <param name="expression">Filter expression.</param>
        /// <returns>IEnumerable of filtered entities.</returns>
        IEnumerable<T> GetFiltered(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Method to update entity in data base.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Method to remove entity from data base.
        /// </summary>
        /// <param name="entity">Entity to remove.</param>
        void Remove(T entity);

        /// <summary>
        /// MEthod to remove range of entities from data base.
        /// </summary>
        /// <param name="entities">IEnumerable of entities to remove.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Method for saving changes.
        /// </summary>
        /// <returns>Task object.</returns>
        Task SaveChangesAsync();
    }
}
