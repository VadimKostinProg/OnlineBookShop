using BookShop.Core.Domain.Entities;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using System.Linq.Expressions;

namespace BookShop.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository _repository;

        public ProductService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductResponse> CreateAsync(ProductAddRequest request)
        {
            //Validation
            if(request is null)
                throw new ArgumentNullException("Product object is null or empty.");

            if (await _repository.ExistsAsync<Product>(product => product.Title == request.Title))
                throw new ArgumentException("Product with such title already exists.");

            if (await _repository.ExistsAsync<Product>(product => product.ISBN == request.ISBN))
                throw new ArgumentException("Product with such ISBN already exists.");

            if (!(await _repository.ExistsAsync<Category>(category => category.Id == request.CategoryId)))
                throw new ArgumentException("Category of product does not exist.");

            var product = request.ToProduct();
            await _repository.AddAsync(product);

            var insertedProduct = await _repository.GetByIdAsync<Product>(product.Id, includeStrings: "Category");

            return insertedProduct.ToProductResponse();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool result = await _repository.DeleteAsync<Product>(id);

            if (!result)
                throw new KeyNotFoundException("Product with such Id is not found.");
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync<Product>(predicate: null, includeStrings: "Category");

            return products.Select(product => product.ToProductResponse()).ToList();
        }

        public async Task<ProductResponse> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync<Product>(id, includeStrings: "Category");

            if (product is null)
                throw new KeyNotFoundException("Product with such Id is not found.");

            return product.ToProductResponse();
        }

        public async Task<ProductResponse> GetByNameAsync(string name)
        {
            var product = await _repository.FirstOrDefaultAsync<Product>(product => product.Title == name, 
                                                                         includeStrings: "Category");

            if (product is null)
                throw new KeyNotFoundException("Product with such title is not found.");

            return product.ToProductResponse();
        }

        public async Task<IEnumerable<ProductResponse>> GetFilteredAsync(Expression<Func<ProductResponse, bool>> predicate)
        {
            var products = await _repository.GetAllAsync<Product>(predicate: null, includeStrings: "Category");

            return products.Select(product => product.ToProductResponse())
                           .AsQueryable()
                           .Where(predicate)
                           .ToList();
        }

        public async Task<ProductResponse> UpdateAsync(ProductUpdateRequest request)
        {
            if (request is null)
                throw new ArgumentNullException("Product object is null or empty.");

            if (!(await _repository.ExistsAsync<Product>(product => product.Id == request.Id)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            if (await _repository.ExistsAsync<Product>(product => product.Id != request.Id && 
                                                                  product.Title == request.Title))
                throw new ArgumentException("Product with such title already exists.");

            if (await _repository.ExistsAsync<Product>(product => product.Id != request.Id && 
                                                                  product.ISBN == request.ISBN))
                throw new ArgumentException("Product with such ISBN already exists.");

            if (!(await _repository.ExistsAsync<Category>(category => category.Id == request.CategoryId)))
                throw new KeyNotFoundException("Category with such Id is not found.");

            var product = request.ToProduct();
            await _repository.UpdateAsync(product);

            var updatedProduct = await _repository.GetByIdAsync<Product>(product.Id, includeStrings: "Category");

            return updatedProduct.ToProductResponse();
        }
    }
}
