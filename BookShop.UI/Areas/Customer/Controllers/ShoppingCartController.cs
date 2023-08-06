using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
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
    }
}
