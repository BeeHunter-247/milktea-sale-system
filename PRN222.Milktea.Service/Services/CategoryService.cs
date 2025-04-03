using AutoMapper;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAsync();

                var categoriesModel = _mapper.Map<List<CategoryModel>>(categories);

                return categoriesModel;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }

                return _mapper.Map<CategoryModel>(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category by ID {id}: {ex.Message}");
            }
        }
    }
}
