using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using Newtonsoft.Json; // Đảm bảo đã cài package Newtonsoft.Json
using System.Linq;
using System.Threading.Tasks;

namespace PRN222.Milktea.RazorPage.Pages.Products
{
    public class ProductsIndexModel : PageModel
    {
        private readonly IProductService _productService;
        private const string CartCookieName = "Cart"; // Tên Cookie để lưu trữ giỏ hàng

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
            // Lấy giỏ hàng từ cookie
            var cart = GetCartFromCookie();

            var product = await _productService.GetProductDetailsAsync(productId);

            if (product != null)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity++; // Tăng số lượng nếu đã có trong giỏ hàng
                }
                else
                {
                    cart.Items.Add(new CartItemViewModel
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        UnitPrice = product.Price,
                        Quantity = 1
                    });
                }

                // Lưu giỏ hàng vào cookie
                SetCartToCookie(cart);
            }

            return RedirectToPage();
        }

        private CartViewModel GetCartFromCookie()
        {
            var cartCookie = Request.Cookies[CartCookieName];
            if (cartCookie != null)
            {
                return JsonConvert.DeserializeObject<CartViewModel>(cartCookie);
            }
            return new CartViewModel(); // Trả về giỏ hàng rỗng nếu không có cookie
        }

        private void SetCartToCookie(CartViewModel cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart); // Chuyển giỏ hàng thành JSON

            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTime.Now.AddDays(1), // Thiết lập cookie hết hạn sau 1 ngày
                HttpOnly = true, // Cookie chỉ có thể được truy cập qua HTTP, không phải JavaScript
                Secure = false // Đặt thành true nếu bạn sử dụng HTTPS
            };

            Response.Cookies.Append(CartCookieName, cartJson, cookieOptions); // Lưu giỏ hàng vào cookie
        }
    }
}
