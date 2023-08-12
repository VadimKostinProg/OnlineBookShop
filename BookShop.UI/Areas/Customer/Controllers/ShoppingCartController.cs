using Azure.Core;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService, ILogger<ShoppingCartController> logger)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid userId)
        {
            try
            {
                var shoppingCart = await _shoppingCartService.GetShoppingCartByUserAsync(userId);
                return View(shoppingCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(Guid userId)
        {
            try
            {
                var shoppincCart = await _shoppingCartService.GetShoppingCartByUserAsync(userId);
                ViewBag.ShoppingCart = shoppincCart;
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

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderProceedRequest request)
        {
            try
            {
                await _orderService.ProceedOrderAsync(request);
                return Redirect("/");
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
        [HttpPost]
        public async Task<ActionResult<ShoppingCartResponse>> SetShoppingCartItem([FromBody] ShoppingCartItemSetRequest request)
        {
            try
            {
                await _shoppingCartService.SetShoppingCartItemAsync(request);
                var shoppincCart = await _shoppingCartService.GetShoppingCartByUserAsync(request.UserId);
                return Ok(shoppincCart);
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

        [HttpPost]
        public async Task<ActionResult<decimal>> DeleteShoppingCartItem([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            try
            {
                await _shoppingCartService.DeleteShoppingCartItemAsync(userId, productId);
                var shoppincCart = await _shoppingCartService.GetShoppingCartByUserAsync(userId);
                return Ok(shoppincCart.TotalPrice);
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

        [HttpPost]
        public async Task<IActionResult> Clear([FromQuery] Guid userId)
        {
            try
            {
                await _shoppingCartService.ClearShoppingCartAsync(userId);
                return Ok($"Shopping cart for user {userId} has been cleared successfully.");
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
