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

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService,
        ICategoryService categoryService,
        IImageService imageService,
        IWebHostEnvironment webHostEnvironment,
        ILogger<ProductsController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
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
        public async Task<IActionResult> Create(ProductAddRequest product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageUrl = await _imageService.AddImage(folder: "products", image);

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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid productId)
        {
            var product = await _productService.GetByIdAsync(productId);

            IEnumerable<SelectListItem> categoryList = (await _categoryService.GetAllAsync())
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;

            return View(product.ToProductUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateRequest product, IFormFile image)
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
        public async Task<IActionResult> Delete(ProductResponse product)
        {
            try
            {
                await _productService.DeleteAsync(product.Id);
                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #region API CALLS
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var products = (await _productService.GetAllAsync()).ToList();

            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory([FromQuery] Guid categoryId)
        {
            try
            {
                var products = await _productService.GetFilteredAsync(product => product.CategoryId == categoryId);
                return Ok(products);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
