using AutoMapper;
using PRN222.Milktea.Repository.UnitOfWork;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        public async Task<AccountModel> Login(string email, string password)
        {
            try
            {
                var user = await _unitOfWork.AccountRepository.FindAsync(a => a.Email.Equals(email) && a.AccountPassword.Equals(password));

                if (user == null)
                {
                    return null;
                }

                var account = _mapper.Map<AccountModel>(user);
                return account;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
