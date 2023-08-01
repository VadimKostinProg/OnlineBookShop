using BookShop.DataAccess.RepositoryContracts;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (category == null)
            {
                return BadRequest(category);
            }

            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                _categoryRepository.Insert(category);
                await _categoryRepository.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(Guid? categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest(categoryId);
            }

            Category? category = _categoryRepository.GetValueById(categoryId.Value);

            if (category == null)
            {
                return NotFound(categoryId);
            }

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (category == null)
            {
                TempData["Error"] = "Error! Bad request";

                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error! Validation failed";

                return RedirectToAction("Index");
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            TempData["Success"] = "Successfuly updated";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(Guid? categoryId)
        {
            if (categoryId == null)
            {
                return BadRequest(categoryId);
            }

            Category? category = _categoryRepository.GetValueById(categoryId.Value);

            if (category == null)
            {
                return NotFound(categoryId);
            }

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            if (category == null)
            {
                TempData["Error"] = "Error! Bad request";

                return RedirectToAction("Index");
            }

            Category? categoryToDelete = _categoryRepository.GetValueById(category.Id);

            if (categoryToDelete == null)
            {
                TempData["Error"] = "Error! Category not found";

                return RedirectToAction("Index");
            }

            _categoryRepository.Remove(categoryToDelete);
            await _categoryRepository.SaveChangesAsync();

            TempData["Success"] = "Successfuly deleted";

            return RedirectToAction("Index");
        }
    }
}
