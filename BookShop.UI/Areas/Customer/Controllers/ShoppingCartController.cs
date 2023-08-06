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

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
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
                return NotFound(ex.Message);
            }
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> SetShoppingCartItem([FromBody] ShoppingCartItemSetRequest request)
        {
            try
            {
                await _shoppingCartService.SetShoppingCartItemAsync(request);
                return Ok($"Shopping cart item has been set successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShoppingCartItem([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            try
            {
                await _shoppingCartService.DeleteShoppingCartItemAsync(userId, productId);
                return Ok("Shopping cart item has been deleted successfully.");
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Clear([FromRoute] Guid userId)
        {
            try
            {
                await _shoppingCartService.ClearShoppingCartAsync(userId);
                return Ok($"Shopping cart for user {userId} has been cleared successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
