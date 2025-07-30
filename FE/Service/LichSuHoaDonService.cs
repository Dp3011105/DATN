using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class LichSuHoaDonService : ILichSuHoaDonService
    {
        private readonly ILichSuHoaDonRepository _repository;

        public LichSuHoaDonService(ILichSuHoaDonRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<LichSuHoaDon>> GetAllAsync() => _repository.GetAllAsync();

        public Task<LichSuHoaDon?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(LichSuHoaDon entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(LichSuHoaDon entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}