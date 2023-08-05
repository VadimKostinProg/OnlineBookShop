using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using BookShop.Core.ServiceContracts;
using BookShop.Core.DTO;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryAddRequest category)
        {
            if (category == null)
            {
                return BadRequest(category);
            }

            if (ModelState.IsValid)
            {
                await _categoryService.CreateAsync(category);

                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(categoryId);
                return View(category.ToCategoryUpdateRequest());
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryUpdateRequest category)
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

            await _categoryService.UpdateAsync(category);

            TempData["Success"] = "Successfuly updated";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(categoryId);
                return View(category);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryResponse category)
        {
            try
            {
                await _categoryService.DeleteAsync(category.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
