using BookShop.DataAccess.DataBaseContext;
using BookShop.DataAccess.RepositoryContracts;
using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext _db;

        public ProductRepository(ApplicationContext db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetAll()
        {
            return _db.Products.Include("Category").Select(product => product);
        }

        public Product? GetValueById(Guid id)
        {
            return _db.Products.Include("Category").FirstOrDefault(product => product.Id == id);
        }

        public void Insert(Product entity)
        {
            _db.Products.Add(entity);
        }

        public void Update(Product entity)
        {
            Product? product = _db.Products.FirstOrDefault(p => p.Id == entity.Id);

            if (product != null)
            {
                product.Title = entity.Title;
                product.Author = entity.Author;
                product.Description = entity.Description;
                product.CategoryId = entity.CategoryId;
                product.ISBN = entity.ISBN;
                product.ListPrice = entity.ListPrice;
                product.Price = entity.Price;
                product.Price50 = entity.Price50;
                product.Price100 = entity.Price100;
                product.ImageUrl = entity.ImageUrl;
            }
        }

        public void Remove(Product entity)
        {
            Product? product = _db.Products.FirstOrDefault(p => p.Id == entity.Id);

            if(product != null)
            {
                _db.Products.Remove(product);
            }
        }

        public void RemoveRange(IEnumerable<Product> entities)
        {
            foreach(Product entity in entities)
            {
                Product? product = _db.Products.FirstOrDefault(p => p.Id == entity.Id);

                if (product != null)
                {
                    _db.Products.Remove(product);
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public IEnumerable<Product> GetFiltered(Expression<Func<Product, bool>> expression)
        {
            return _db.Products.Include("Category").Where(expression);
        }
    }
}
