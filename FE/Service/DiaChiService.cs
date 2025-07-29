using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class DiaChiService : IDiaChiService
    {
        private readonly IDiaChiRepository _repository;

        public DiaChiService(IDiaChiRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<DiaChi>> GetAllAsync() => _repository.GetAllAsync();

        public Task<DiaChi?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(DiaChi entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(DiaChi entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}