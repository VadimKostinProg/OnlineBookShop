using BookShop.DataAccess.RepositoryContracts;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductRepository productRepository, 
        ICategoryRepository categoryRepository,
        IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll()
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }

                    product.ImageUrl = @"\images\product\" + fileName;

                    product.Id = Guid.NewGuid();
                    _productRepository.Insert(product);
                    await _productRepository.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll()
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

        public IActionResult Edit(Guid productId)
        {
            Product? product = _productRepository.GetValueById(productId);

            if(product == null)
            {
                return BadRequest(productId);
            }

            IEnumerable<SelectListItem> categoryList = _categoryRepository.GetAll()
                .Select(category => new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });

            ViewBag.CategoryList = categoryList;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? image)
        {
            if(_productRepository.GetValueById(product.Id) == null)
            {
                return BadRequest(product.Id);
            }

            if(image != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                product.ImageUrl = @"\images\product\" + fileName;
            }

            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid productId)
        {
            Product? product = _productRepository.GetValueById(productId);

            if(product == null)
            {
                return BadRequest(productId);
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            if(_productRepository.GetValueById(product.Id) == null)
            {
                return BadRequest(product.Id);
            }

            _productRepository.Remove(product);

            await _productRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _productRepository.GetAll().ToList();

            return Json(new { data = products });
        }
        #endregion
    }
}
