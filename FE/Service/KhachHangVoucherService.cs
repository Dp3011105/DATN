using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class KhachHangVoucherService : IKhachHangVoucherService
    {
        private readonly IKhachHangVoucherRepository _repository;

        public KhachHangVoucherService(IKhachHangVoucherRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<KhachHangVoucher>> GetAllAsync() => _repository.GetAllAsync();

        public Task<KhachHangVoucher?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(KhachHangVoucher entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(KhachHangVoucher entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}