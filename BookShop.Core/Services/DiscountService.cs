using BookShop.Core.Domain.Entities;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;

namespace BookShop.Core.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IRepository _repository;

        public DiscountService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateDiscountAsync(DiscountSetRequest request)
        {
            if(request == null) 
                throw new ArgumentNullException("Discount is null or empty.");

            if (!(await _repository.ExistsAsync<Product>(product => product.Id == request.ProductId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            var discount = await _repository.FirstOrDefaultAsync<Discount>(discount =>
            discount.ProductId == request.ProductId && discount.Count == request.Count);

            var discountToSet = request.ToDiscount();

            if(discount is null)
            {
                await _repository.AddAsync(discountToSet);
                return;
            }

            discountToSet.Id = discount.Id;
            await _repository.UpdateAsync(discountToSet);
        }

        public async Task DeleteAllDiscountsAsync(Guid productId)
        {
            if (!(await _repository.ExistsAsync<Product>(product => product.Id == productId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            var discounts = await _repository.GetAllAsync<Discount>(discount => discount.ProductId == productId);

            foreach(var discount in discounts)
            {
                await _repository.DeleteAsync<Discount>(discount.Id);
            }    
        }

        public async Task DeleteDiscountAsync(Guid productId, int count)
        {
            if (!(await _repository.ExistsAsync<Product>(product => product.Id == productId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            var discount = await _repository.FirstOrDefaultAsync<Discount>(discount => 
            discount.ProductId == productId && discount.Count == count);

            if (discount != null)
            {
                await _repository.DeleteAsync<Discount>(discount.Id);
            }
        }

        public async Task<double> GetDiscountByProductAsync(Guid productId, int count)
        {
            if (!(await _repository.ExistsAsync<Product>(product => product.Id == productId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            var discount = await _repository.FirstOrDefaultAsync<Discount>(discount =>
            discount.ProductId == productId && discount.Count == count);

            return discount is null ? 0 : discount.DiscountAmount;
        }
    }
}
