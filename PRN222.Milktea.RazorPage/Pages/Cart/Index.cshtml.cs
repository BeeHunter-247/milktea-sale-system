using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.RazorPage.Extensions;
using PRN222.Milktea.Service.BusinessModels;

namespace PRN222.Milktea.RazorPage.Pages.Cart
{
    public class CartIndexModel : PageModel
    {
        public CartViewModel Cart { get; set; }

        public void OnGet()
        {
            Cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart") ?? new CartViewModel();
        }

        public IActionResult OnPostRemove(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<CartViewModel>("Cart");
            cart.Items.RemoveAll(i => i.ProductId == productId);
            HttpContext.Session.SetObjectToJson("Cart", cart);
            return RedirectToPage();
        }
    }
}
