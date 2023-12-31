﻿using BookShop.Core.Domain.Entities;
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

        public async Task SetDiscountAsync(DiscountSetRequest request)
        {
            if(request == null) 
                throw new ArgumentNullException("Discount is null or empty.");

            if (!(await _repository.ExistsAsync<Product>(product => product.Id == request.ProductId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            if (!(await ValidateDiscountAmount(request)))
                throw new ArgumentException("Discount amount should be more then previos by count discount and less then next one.");

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

        private async Task<bool> ValidateDiscountAmount(DiscountSetRequest request)
        {
            var discountsLower = await _repository.GetAllAsync<Discount>(discount => 
            discount.ProductId == request.ProductId && discount.Count < request.Count);

            var discountsUpper = await _repository.GetAllAsync<Discount>(discount =>
            discount.ProductId == request.ProductId && discount.Count > request.Count);

            double discountLower = discountsLower.Count() == 0 ? 0 : discountsLower.Max(discount => discount.DiscountAmount);

            double discountUpper = discountsUpper.Count() == 0 ? 100 : discountsUpper.Min(discount => discount.DiscountAmount);

            return discountLower < request.DiscountAmount && request.DiscountAmount < discountUpper;
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

        public async Task<IEnumerable<DiscountResponse>> GetAllDiscountsAsync()
        {
            var discounts = await _repository.GetAllAsync<Discount>(predicate: null, includeStrings: "Product");

            var result = discounts.GroupBy(discount => discount.ProductId)
                                  .Select(group => new DiscountResponse
                                  {
                                      ProductId = group.Key,
                                      ProductName = group.First().Product.Title,
                                      CountDiscountSet = group.ToDictionary(discount => discount.Count, discount => discount.DiscountAmount)
                                  }).ToList();

            return result;
        }

        public async Task<double> GetDiscountAmountByProductAsync(Guid productId, int count)
        {
            if (!(await _repository.ExistsAsync<Product>(product => product.Id == productId)))
                throw new KeyNotFoundException("Product with such Id is not found.");

            var discounts = await _repository.GetAllAsync<Discount>(discount => 
            discount.ProductId == productId && discount.Count <= count);

            if(discounts.Count() == 0)
            {
                return 0;
            }

            var discount = discounts.Max(discount => discount.DiscountAmount);

            return discount;
        }
    }
}
