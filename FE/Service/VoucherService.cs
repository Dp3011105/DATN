using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _repository;

        public VoucherService(IVoucherRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Voucher>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Voucher?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Voucher entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Voucher entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}