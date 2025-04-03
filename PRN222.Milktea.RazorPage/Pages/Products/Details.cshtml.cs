using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System.Threading.Tasks;

namespace PRN222.Milktea.RazorPage.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public ProductViewModel Product { get; set; }
        public string ProductTypeName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            Product = product;

            var fullProduct = await _productService.GetProductsByIdAsync(id);
            ProductTypeName = GetProductTypeName(fullProduct.ProductType);

            return Page();
        }

        private string GetProductTypeName(int productType)
        {
            return productType switch
            {
                1 => "Regular",
                2 => "Extra",
                _ => "Unknown"
            };
        }
    }
}
