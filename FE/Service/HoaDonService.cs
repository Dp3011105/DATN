using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _repository;

        public HoaDonService(IHoaDonRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HoaDon>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HoaDon?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HoaDon entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HoaDon entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}