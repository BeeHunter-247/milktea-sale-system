using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.Models;
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
    public class ComboService : IComboService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComboService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddComboAsync(ComboModel model)
        {
            try
            {
                var entity = _mapper.Map<Combo>(model);
                await _unitOfWork.ComboRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteComboAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ComboRepository.GetByIdAsync(id);

                if (product != null)
                {
                    product.IsActive = false;
                    await _unitOfWork.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ComboModel>> GetComboAsync()
        {
            try
            {
                var comboes = await _unitOfWork.ComboRepository.GetByConditionAsync(
                    null,
                    query => query.Include(p => p.ProductId1Navigation).Include(p => p.ProductId2Navigation).Include(p => p.ProductId3Navigation));

                var comboModel = comboes.Select(p => new ComboModel
                {
                    ComboId = p.ComboId,
                    ComboName = p.ComboName,
                    ComboPrice = p.ComboPrice,
                    Description = p.Description,
                    Image = p.Image,
                    ProductId1 = p.ProductId1,
                    ProductId2 = p.ProductId2,
                    ProductId3 = p.ProductId3,
                    IsActive = p.IsActive,
                    ProductId1Name = p.ProductId1Navigation.Name,
                    ProductId2Name = p.ProductId2Navigation.Name,
                    ProductId3Name = p.ProductId3Navigation.Name,
                });

                return comboModel;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ComboModel> GetComboByIdAsync(int id)
        {
            try 
            {
                var combo = await _unitOfWork.ComboRepository.FindAsync(
                    p => p.ComboId.Equals(id), query => query.Include(p => p.ProductId1Navigation).Include(p => p.ProductId2Navigation).Include(p => p.ProductId3Navigation)
                    );

                var comboModel = new ComboModel
                {
                    ComboId = combo.ComboId,
                    ComboName = combo.ComboName,
                    ComboPrice = combo.ComboPrice,
                    Description = combo.Description,
                    Image = combo.Image,
                    ProductId1 = combo.ProductId1,
                    ProductId2 = combo.ProductId2,
                    ProductId3 = combo.ProductId3,
                    IsActive = combo.IsActive,
                    ProductId1Name= combo.ProductId1Navigation.Name,
                    ProductId2Name= combo.ProductId2Navigation.Name,
                    ProductId3Name= combo.ProductId3Navigation.Name,
                };
                return comboModel;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateComboAsync(ComboModel model)
        {
			try
			{
				var entity = _mapper.Map<Combo>(model);
				_unitOfWork.ComboRepository.Update(entity);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
    }
}
