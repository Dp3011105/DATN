using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class DiemDanhService : IDiemDanhService
    {
        private readonly IDiemDanhRepository _repository;

        public DiemDanhService(IDiemDanhRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<DiemDanh>> GetAllAsync() => _repository.GetAllAsync();

        public Task<DiemDanh?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(DiemDanh entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(DiemDanh entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}