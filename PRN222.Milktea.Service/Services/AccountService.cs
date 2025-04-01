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
        public async Task<IEnumerable<AccountModelAdmin>> GetAllAccountsExceptAdminAsync(int currentAdminId)
        {
            var accounts = await _unitOfWork.AccountRepository.GetByConditionAsync(
                condition: a => a.AccountId != currentAdminId && a.AccountRole != 1
            );
            return _mapper.Map<IEnumerable<AccountModelAdmin>>(accounts);
        }

        public async Task BanAccountAsync(int accountId)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(accountId);
            if (account != null)
            {
                account.IsActive = false;
                _unitOfWork.AccountRepository.Update(account);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalRegisteredAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAsync();
            return accounts.Count();
        }

        public async Task<int> GetTotalBannedAccountsAsync()
        {
            var accounts = await _unitOfWork.AccountRepository.GetByConditionAsync(
                condition: a => !(a.IsActive ?? true) // Giá trị mặc định là true nếu null
            );
            return accounts.Count();
        }
    }
}
