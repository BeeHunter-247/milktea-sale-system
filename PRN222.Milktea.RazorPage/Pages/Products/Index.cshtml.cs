using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.RazorPage.Extensions;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.RazorPage.Pages.Products
{
    public class ProductsIndexModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductsIndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<ProductViewModel> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _productService.GetProductsAsync(null);
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel();
            var product = await _productService.GetProductDetailsAsync(productId);
            cart.Items.Add(new CartItemViewModel { ProductId = productId, ProductName = product.Name, UnitPrice = product.Price, Quantity = 1 });
            HttpContext.Session.SetObjectToJson("Cart", cart);
            return RedirectToPage();
        }
    }
}
