﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dimain.Entities;

namespace BookShop.Core.DTO
{
    public class ProductResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Author { get; set; } = null!;

        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public ProductUpdateRequest ToProductUpdateRequest()
        {
            return new ProductUpdateRequest()
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                ISBN = this.ISBN,
                Author = this.Author,
                Price = this.Price,
                CategoryId = this.CategoryId,
                ImageUrl = this.ImageUrl,
            };
        }
    }

    public static class ProductExt
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ISBN = product.ISBN,
                Author = product.Author,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                ImageUrl = product.ImageUrl,
            };
        }
    }
}
