using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using Dimain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repository;

        public CategoryService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryResponse> CreateAsync(CategoryAddRequest request)
        {
            //Validation
            if(request is null)
                throw new ArgumentNullException("Category object is null or empty.");

            if (await _repository.ExistsAsync<Category>(category => category.Name == request.Name))
                throw new ArgumentException("Category with such name alreay exists.");

            if (await _repository.ExistsAsync<Category>(category => category.DisplayOrder == request.DisplayOrder))
                throw new ArgumentException("Category with such display order alreay exists.");

            var category = request.ToCategory();
            await _repository.AddAsync(category);

            return category.ToCategoryResponse();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool result = await _repository.DeleteAsync<Category>(id);

            if (!result)
                throw new KeyNotFoundException("Category with such Id is not found.");
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync<Category>();

            return categories.Select(category => category.ToCategoryResponse()).ToList();
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync<Category>(id);

            if (category is null)
                throw new KeyNotFoundException("Category with such Id is not found.");

            return category.ToCategoryResponse();
        }

        public async Task<CategoryResponse> GetByNameAsync(string name)
        {
            //Validation
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name of category is null or empty.");

            var category = await _repository.FirstOrDefaultAsync<Category>(category => category.Name == name);

            if (category is null)
                throw new KeyNotFoundException("Category with such name is not found.");

            return category.ToCategoryResponse();
        }

        public async Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest request)
        {
            //Validation
            if (request is null)
                throw new ArgumentNullException("Category object is null or empty.");

            if (!(await _repository.ExistsAsync<Category>(category => category.Id == request.Id)))
                throw new ArgumentException("Category with such Id is not found.");

            if (!(await _repository.ExistsAsync<Category>(category => category.Id != request.Id &&
                                                          category.Name == request.Name)))
                throw new ArgumentException("Category with such name is already exists.");

            if (!(await _repository.ExistsAsync<Category>(category => category.Id != request.Id &&
                                                          category.DisplayOrder == request.DisplayOrder)))
                throw new ArgumentException("Category with such discplay order is already exists.");

            var category = request.ToCategory();
            await _repository.UpdateAsync(category);

            return category.ToCategoryResponse();
        }
    }
}
