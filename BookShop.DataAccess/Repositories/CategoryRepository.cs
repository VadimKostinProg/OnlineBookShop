using BookShop.DataAccess.DataBaseContext;
using BookShop.DataAccess.RepositoryContracts;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext _db;

        public CategoryRepository(ApplicationContext db)
        {
            _db = db;
        }

        public IEnumerable<Category> GetAll()
        {
            return _db.Categories.Select(category => category);
        }

        public Category? GetValueById(Guid id)
        {
            return _db.Categories.FirstOrDefault(category => category.Id == id);
        }

        public void Insert(Category entity)
        {
            _db.Categories.Add(entity);
        }

        public void Remove(Category entity)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == entity.Id);

            if (category != null)
            {
                _db.Categories.Remove(category);
            }
        }

        public void RemoveRange(IEnumerable<Category> entities)
        {
            foreach(Category entity in entities)
            {
                Category? category = _db.Categories.FirstOrDefault(c => c.Id == entity.Id);

                if (category != null)
                {
                    _db.Categories.Remove(category);
                }
            }
        }

        public void Update(Category entity)
        {
            Category? category = _db.Categories.FirstOrDefault(c => c.Id == entity.Id);

            if (category != null)
            {
                category.Name = entity.Name;
                category.DisplayOrder = entity.DisplayOrder;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public IEnumerable<Category> GetFiltered(Expression<Func<Category, bool>> expression)
        {
            return _db.Categories.Where(expression);
        }
    }
}
