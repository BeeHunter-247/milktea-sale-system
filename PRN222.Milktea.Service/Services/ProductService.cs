using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.Service.Services
{
	public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddProductAsync(ProductModel product)
        {
            try
            {
                var entity = _mapper.Map<Product>(product);
                await _unitOfWork.ProductRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

                if(product != null)
                {
                    product.IsActive = false;
                    await _unitOfWork.SaveChangesAsync();
                }

            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetByConditionAsync(
                    null,
                    query => query.Include(c => c.Category),
                    null
                    );
                var productModel = _mapper.Map<List<ProductModel>>(products);

                return productModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductModel> GetProductsByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.FindAsync(c => c.ProductId.Equals(id), query => query.Include(c => c.Category));
                var productModel = _mapper.Map<ProductModel>(product);
                return productModel;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

		public async Task<List<ProductModel>> IsActiveProduct()
		{
            var list = await _unitOfWork.ProductRepository.GetByConditionAsync(
                p => p.IsActive == true
                );

            var listModel = _mapper.Map<List<ProductModel>>(list);
            return listModel;
        }

        public async Task UpdateProductAsync(ProductModel product)
        {
            try
            {
                var entity = _mapper.Map<Product>(product);
                _unitOfWork.ProductRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(int? categoryId)
        {
            var products = await _unitOfWork.ProductRepository.GetByConditionAsync(
                p => p.IsActive == true && (!categoryId.HasValue || p.CategoryId == categoryId),
                query => query.Include(c => c.Category),
                null
            );

            return products.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Image = p.Image,
                CategoryName = p.Category?.Name 
            });
        }

        public async Task<ProductViewModel> GetProductDetailsAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            return new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Image = product.Image
            };
        }


    }
}
