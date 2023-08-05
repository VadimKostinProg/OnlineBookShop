using Azure.Core;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
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
    public class ProductServiceTest
    {
        private readonly Mock<IRepository> _repositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _repositoryMock = new Mock<IRepository>();
            _productService = new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidProduct_ReturnsProductResponse()
        {
            // Arrange
            var request = new ProductAddRequest
            {
                Title = "ExistingProduct",
                Description = "TestDesrciption",
                Author = "TestAuthor",
                Price = 100,
                ISBN = "1234567890",
                CategoryId = Guid.NewGuid(),
                ImageUrl = "testUrl"
            };

            var createdProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Title = "ExistingProduct",
                Description = "TestDesrciption",
                Author = "TestAuthor",
                Price = 100,
                ISBN = "1234567890",
                CategoryId = request.CategoryId,
                Category = new Category() { Id = request.CategoryId, Name = "Category1" },
                ImageUrl = "testUrl"
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id == request.CategoryId))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Product>()))
                           .Returns(Task.CompletedTask);

            _repositoryMock.Setup(x => x.GetByIdAsync<Product>(It.IsAny<Guid>(), "Category"))
                           .ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.ISBN, result.ISBN);
            Assert.Equal(request.CategoryId, result.CategoryId);
        }

        [Fact]
        public async Task CreateAsync_ProductWithTitleExists_ThrowsArgumentException()
        {
            // Arrange
            var request = new ProductAddRequest
            {
                Title = "ExistingProduct",
                Description = "TestDesrciption",
                Author = "TestAuthor",
                Price = 100,
                ISBN = "1234567890",
                CategoryId = Guid.NewGuid(),
                ImageUrl = "testUrl"
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _productService.CreateAsync(request));
        }

        [Fact]
        public async Task DeleteAsync_ExistingProductId_DeletesProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.DeleteAsync<Product>(productId))
                           .ReturnsAsync(true);

            // Act
            await _productService.DeleteAsync(productId);

            // Assert
            _repositoryMock.Verify(x => x.DeleteAsync<Product>(productId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingProductId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingProductId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.DeleteAsync<Product>(nonExistingProductId))
                           .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.DeleteAsync(nonExistingProductId));
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfProductResponses()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "Product1", ISBN = "1111111111", Category = new Category { Id = Guid.NewGuid(), Name = "Category1" } },
                new Product { Id = Guid.NewGuid(), Title = "Product2", ISBN = "2222222222", Category = new Category { Id = Guid.NewGuid(), Name = "Category2" } },
                new Product { Id = Guid.NewGuid(), Title = "Product3", ISBN = "3333333333", Category = new Category { Id = Guid.NewGuid(), Name = "Category3" } }
            };

            _repositoryMock.Setup(x => x.GetAllAsync<Product>(null, "Category"))
                           .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingProductId_ReturnsProductResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Title = "TestProduct",
                ISBN = "1234567890",
                Category = new Category { Id = Guid.NewGuid(), Name = "TestCategory" }
            };

            _repositoryMock.Setup(x => x.GetByIdAsync<Product>(productId, "Category"))
                           .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Title, result.Title);
            Assert.Equal(product.ISBN, result.ISBN);
            Assert.Equal(product.Category.Name, result.CategoryName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingProductId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingProductId = Guid.NewGuid();

            _repositoryMock.Setup(x => x.GetByIdAsync<Product>(nonExistingProductId))
                           .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.GetByIdAsync(nonExistingProductId));
        }

        [Fact]
        public async Task GetByNameAsync_ExistingProductName_ReturnsProductResponse()
        {
            // Arrange
            var productName = "TestProduct";

            Guid categoryId = Guid.NewGuid();

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = productName,
                Description = "TestDescription",
                ISBN = "1234567890",
                Price = 100,
                CategoryId = categoryId,
                Category = new Category { Id = categoryId, Name = "TestCategory" },
                ImageUrl = "testUrl"
            };

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync<Product>(It.IsAny<Expression<Func<Product, bool>>>(), "Category"))
                           .ReturnsAsync(product);

            // Act
            var result = await _productService.GetByNameAsync(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Title, result.Title);
            Assert.Equal(product.ISBN, result.ISBN);
            Assert.Equal(product.Category.Name, result.CategoryName);
        }

        [Fact]
        public async Task GetByNameAsync_NonExistingProductName_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistingProductName = "NonExistingProduct";

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync<Product>(It.IsAny<Expression<Func<Product, bool>>>(), "Category"))
                           .ReturnsAsync((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _productService.GetByNameAsync(nonExistingProductName));
        }

        [Fact]
        public async Task GetFilteredAsync_FilterPredicate_ReturnsFilteredProductResponses()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Title = "TestProduct1", ISBN = "1111111111", Category = new Category { Id = Guid.NewGuid(), Name = "Category1" } },
                new Product { Id = Guid.NewGuid(), Title = "AnotherProduct", ISBN = "2222222222", Category = new Category { Id = Guid.NewGuid(), Name = "Category2" } },
                new Product { Id = Guid.NewGuid(), Title = "TestProduct2", ISBN = "3333333333", Category = new Category { Id = Guid.NewGuid(), Name = "Category3" } }
            };

            _repositoryMock.Setup(x => x.GetAllAsync<Product>(null, "Category"))
                           .ReturnsAsync(products);

            // Act
            var result = await _productService.GetFilteredAsync(response => response.Title.Contains("Test"));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Only products with "Test" in the title should be included
        }

        [Fact]
        public async Task UpdateAsync_ValidProduct_ReturnsUpdatedProductResponse()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();

            var request = new ProductUpdateRequest
            {
                // Set up the properties for the product update request
                Id = Guid.NewGuid(),
                Title = "UpdatedProduct",
                Description = "Test description",
                Author = "Test Author",
                Price = 100,
                ISBN = "9876543210",
                CategoryId = categoryId,
                ImageUrl = "testUrl"
            };

            var updatedProduct = new Product
            {
                Id = request.Id,
                Title = "UpdatedProduct",
                Description = "Test description",
                Author = "Test Author",
                ISBN = "9876543210",
                Price = 100,
                CategoryId = categoryId,
                Category = new Category { Id = categoryId, Name = "TestCategory" },
                ImageUrl = "testUrl"
            };

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(It.IsAny<Expression<Func<Product, bool>>>()))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Category>(category => category.Id == request.CategoryId))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(product => product.Id == request.Id))
                           .ReturnsAsync(true);

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(product => product.Id != request.Id && product.Title == request.Title))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.ExistsAsync<Product>(product => product.Id != request.Id && product.ISBN == request.ISBN))
                           .ReturnsAsync(false);

            _repositoryMock.Setup(x => x.GetByIdAsync<Product>(request.Id, "Category"))
                           .ReturnsAsync(updatedProduct);

            _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _productService.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.ISBN, result.ISBN);
            Assert.Equal(request.CategoryId, result.CategoryId);
        }
    }
}
