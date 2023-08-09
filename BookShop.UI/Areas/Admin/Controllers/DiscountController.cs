using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.UI.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
        {
            _discountService = discountService;
            _logger = logger;
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

        #region API CALLS
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<bool>> IsDiscountActive([FromQuery] Guid productId, [FromQuery] int count)
        {
            try
            {
                var discount = await _discountService.GetDiscountByProductAsync(productId, count);
                bool isDiscountActive = discount == 0;
                return Ok(isDiscountActive);
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
        #endregion
    }
}
