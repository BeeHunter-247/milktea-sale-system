using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PRN222.Milktea.RazorPage.Hubs;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.RazorPage.Pages.Cart
{
    public class CartIndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IHubContext<CartHub> _hubContext;
        private const string CartCookieName = "Cart";

        public CartIndexModel(IProductService productService, IHubContext<CartHub> hubContext)
        {
            _productService = productService;
            _hubContext = hubContext;
        }

        public CartViewModel Cart { get; set; } = new CartViewModel();
        public decimal TotalAmount => Cart.Items.Sum(item => item.Quantity * item.UnitPrice);

        public void OnGet()
        {
            var cartCookie = Request.Cookies[CartCookieName];
            if (cartCookie != null)
            {
                Cart = JsonConvert.DeserializeObject<CartViewModel>(cartCookie);
            }
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            var cart = GetCartFromCookie();
            var product = await _productService.GetProductDetailsAsync(productId);

            if (product != null)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
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

                SetCartToCookie(cart);
                await _hubContext.Clients.All.SendAsync("ReceiveCart", cart);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int productId)
        {
            var cart = GetCartFromCookie();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                SetCartToCookie(cart);
                await _hubContext.Clients.All.SendAsync("ReceiveCart", cart);
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateQuantitiesAsync(Dictionary<string, int> quantities)
        {
            var cart = GetCartFromCookie();

            foreach (var quantity in quantities)
            {
                var productId = int.Parse(quantity.Key.Replace("quantity_", ""));
                var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    item.Quantity = quantity.Value;
                }
            }

            SetCartToCookie(cart);
            await _hubContext.Clients.All.SendAsync("ReceiveCart", cart); 

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            return RedirectToPage("/Cart/Checkout");
        }

        private CartViewModel GetCartFromCookie()
        {
            var cartCookie = Request.Cookies[CartCookieName];
            if (cartCookie != null)
            {
                return JsonConvert.DeserializeObject<CartViewModel>(cartCookie);
            }
            return new CartViewModel();
        }

        private void SetCartToCookie(CartViewModel cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);

            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = false
            };

            Response.Cookies.Append(CartCookieName, cartJson, cookieOptions);
        }
    }
}
