using AutoMapper;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Service.BusinessModels;

namespace PRN222.Milktea.Service.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountModel, Account>().ReverseMap();
        }
    }
}
