using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;

        private readonly ICategoryService _categoryService;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger, ICategoryService categoryService)
        {
            _discountService = discountService;
            _logger = logger;
            _categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var discounts = await _discountService.GetAllDiscountsAsync();
                return View(discounts);
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> SetDiscount()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.CategoriesList = 
                    categories.Select(category => 
                    new SelectListItem() 
                    { 
                        Value = category.Id.ToString(), 
                        Text = category.Name 
                    });

                return View();
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetDiscount(DiscountSetRequest request)
        {
            try
            {
                await _discountService.SetDiscountAsync(request);
                return RedirectToAction(nameof(Index));
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

        #region API CALLS
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<double>> GetDiscountForProduct([FromQuery] Guid productId, [FromQuery] int count)
        {
            try
            {
                var discount = await _discountService.GetDiscountAmountByProductAsync(productId, count);
                return Ok(discount);
            }
            catch(KeyNotFoundException ex)
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> DeleteDiscount([FromQuery] Guid productId, [FromQuery] int count)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(productId, count);
                return Ok("Discount is deleted successfully.");
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> DeleteAllDiscounts([FromQuery] Guid productId)
        {
            try
            {
                await _discountService.DeleteAllDiscountsAsync(productId);
                return Ok($"Discount for product {productId} is deleted successfully.");
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
