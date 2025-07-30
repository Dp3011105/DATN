using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HoaDonVoucherService : IHoaDonVoucherService
    {
        private readonly IHoaDonVoucherRepository _repository;

        public HoaDonVoucherService(IHoaDonVoucherRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HoaDonVoucher>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HoaDonVoucher?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HoaDonVoucher entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HoaDonVoucher entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}