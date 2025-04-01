using PRN222.Milktea.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryModel>> GetCategories();
    }
}
