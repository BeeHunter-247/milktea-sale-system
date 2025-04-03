using PRN222.Milktea.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Milktea.Service.Services.Interfaces
{
    public interface IComboService
    {
        Task<Pagination<ComboModel>> GetComboAsync(PaginationModel pagination);
        Task<ComboModel> GetComboByIdAsync(int id);
        Task AddComboAsync(ComboModel model);
        Task UpdateComboAsync(ComboModel model);
        Task DeleteComboAsync(int id);
    }
}
