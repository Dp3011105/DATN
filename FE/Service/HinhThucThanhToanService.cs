using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HinhThucThanhToanService : IHinhThucThanhToanService
    {
        private readonly IHinhThucThanhToanRepository _repository;

        public HinhThucThanhToanService(IHinhThucThanhToanRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HinhThucThanhToan>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HinhThucThanhToan?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HinhThucThanhToan entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HinhThucThanhToan entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}