using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using BookShop.Core.ServiceContracts;
using BookShop.Core.DTO;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductService productService,
        ICategoryService categoryService,
        IImageService imageService,
        IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllAsync())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductAddRequest product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    product.ImageUrl = @"\images\product\" + fileName;

                    await _productService.CreateAsync(product);

                    return RedirectToAction("Index");
                }
            }

            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllAsync())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;


            ViewBag.Errors = ModelState.SelectMany(state => state.Value.Errors)
                .Select(error => error.ErrorMessage);

            return View(product);
        }

        public async Task<IActionResult> Edit(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product == null)
            {
                return BadRequest(productId);
            }

            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllAsync())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateRequest product, IFormFile? image)
        {
            try
            {
                if (image != null)
                {
                    string imageUrl = await _imageService.AddImage("products", image);
                    product.ImageUrl = imageUrl;
                }
                await _productService.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            if (product == null)
            {
                return BadRequest(productId);
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostDelete(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = (await _productService.GetAllAsync()).ToList();

            return Json(new { data = products });
        }
        #endregion
    }
}
