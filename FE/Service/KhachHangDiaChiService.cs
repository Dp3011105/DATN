using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class KhachHangDiaChiService : IKhachHangDiaChiService
    {
        private readonly IKhachHangDiaChiRepository _repository;

        public KhachHangDiaChiService(IKhachHangDiaChiRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<KhachHangDiaChi>> GetAllAsync() => _repository.GetAllAsync();

        public Task<KhachHangDiaChi?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(KhachHangDiaChi entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(KhachHangDiaChi entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}