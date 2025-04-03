using PRN222.Milktea.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel> GetProductsByIdAsync(int id);
        Task AddProductAsync(ProductModel product);
        Task UpdateProductAsync(ProductModel product);
        Task DeleteProductAsync(int id);
        Task<List<ProductModel>> IsActiveProduct();
        Task<IEnumerable<ProductViewModel>> GetProductsAsync(int? productId);
        Task<ProductViewModel> GetProductDetailsAsync(int productId);
    }
}
