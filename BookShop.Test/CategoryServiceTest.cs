using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using BookShop.Core.Services;
using Dimain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Test
{
    public class CategoryServiceTest
    {
        private readonly Mock<IRepository> _repositoryMock;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTest()
        {
            _repositoryMock = new Mock<IRepository>();
            _categoryService = new CategoryService(_repositoryMock.Object);
        }

        #region CreateAsync
        [Fact]
        public async Task CreateAsync_ValidCategory_ReturnsCategoryResponse()
        {
            // Arrange
            var request = new CategoryAddRequest
            {
                Name = "TestCategory",
                DisplayOrder = 1
            };

            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Category>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.DisplayOrder, result.DisplayOrder);
        }

        [Fact]
        public async Task CreateAsync_CategoryWithNameExists_ThrowsArgumentException()
        {
            // Arrange
            var request = new CategoryAddRequest
            {
                Name = "ExistingCategory",
                DisplayOrder = 1
            };

            _repositoryMock.Setup(x => x.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.CreateAsync(request));
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_ExistingCategoryId_DeletesCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.DeleteAsync<Category>(categoryId))
                           .ReturnsAsync(true);

            // Act
            await _categoryService.DeleteAsync(categoryId);

            // Assert: The repository's DeleteAsync method should be called with the correct categoryId
            _repositoryMock.Verify(x => x.DeleteAsync<Category>(categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingCategoryId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.DeleteAsync<Category>(nonExistingCategoryId))
                           .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _categoryService.DeleteAsync(nonExistingCategoryId));
        }
        #endregion

        #region GetAllAsync
        [Fact]
        public async Task GetAllAsync_ReturnsListOfCategoryResponses()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category1", DisplayOrder = 1 },
                new Category { Id = Guid.NewGuid(), Name = "Category2", DisplayOrder = 2 },
                new Category { Id = Guid.NewGuid(), Name = "Category3", DisplayOrder = 3 }
            };

            _repositoryMock.Setup(x => x.GetAllAsync<Category>(null))
                           .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories.Count, result.Count());
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ExistingCategoryId_ReturnsCategoryResponse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "TestCategory", DisplayOrder = 1 };

            _repositoryMock.Setup(x => x.GetByIdAsync<Category>(categoryId))
                           .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.DisplayOrder, result.DisplayOrder);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingCategoryId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.GetByIdAsync<Category>(nonExistingCategoryId))
                           .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _categoryService.GetByIdAsync(nonExistingCategoryId));
        }
        #endregion

        #region GetByNameAsync
        [Fact]
        public async Task GetByNameAsync_ExistingCategoryName_ReturnsCategoryResponse()
        {
            // Arrange
            var categoryName = "TestCategory";
            var category = new Category { Id = Guid.NewGuid(), Name = categoryName, DisplayOrder = 1 };

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetByNameAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Name, result.Name);
            Assert.Equal(category.DisplayOrder, result.DisplayOrder);
        }

        [Fact]
        public async Task GetByNameAsync_NonExistingCategoryName_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingCategoryName = "NonExistingCategory";

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync((Category)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _categoryService.GetByNameAsync(nonExistingCategoryName));
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_ValidCategory_ReturnsUpdatedCategoryResponse()
        {
            // Arrange
            var request = new CategoryUpdateRequest
            {
                // Set up the properties for the category update request
                Id = Guid.NewGuid(),
                Name = "UpdatedCategory",
                DisplayOrder = 2
            };

            var existingCategory = new Category
            {
                Id = request.Id,
                Name = "ExistingCategory",
                DisplayOrder = 1
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id == request.Id))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id != request.Id && category.Name == request.Name))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id != request.Id && category.DisplayOrder == request.DisplayOrder))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.GetByIdAsync<Category>(request.Id))
                           .ReturnsAsync(existingCategory);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Category>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.DisplayOrder, result.DisplayOrder);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingCategoryId_ThrowsArgumentException()
        {
            // Arrange
            var request = new CategoryUpdateRequest
            {
                // Set up the properties for the category update request
                Id = Guid.NewGuid(),
                Name = "UpdatedCategory",
                DisplayOrder = 2
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.UpdateAsync(request));
        }

        [Fact]
        public async Task UpdateAsync_CategoryWithNameExists_ThrowsArgumentException()
        {
            // Arrange
            var request = new CategoryUpdateRequest
            {
                // Set up the properties for the category update request
                Id = Guid.NewGuid(),
                Name = "UpdatedCategory",
                DisplayOrder = 2
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id == request.Id))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id != request.Id && category.Name == request.Name))
                           .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.UpdateAsync(request));
        }

        [Fact]
        public async Task UpdateAsync_CategoryWithDisplayOrderExists_ThrowsArgumentException()
        {
            // Arrange
            var request = new CategoryUpdateRequest
            {
                // Set up the properties for the category update request
                Id = Guid.NewGuid(),
                Name = "UpdatedCategory",
                DisplayOrder = 2
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(It.IsAny<Expression<Func<Category, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id == request.Id))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id != request.Id && category.Name == request.Name))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id != request.Id && category.DisplayOrder == request.DisplayOrder))
                           .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.UpdateAsync(request));
        }
        #endregion
    }
}