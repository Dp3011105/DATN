using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SizeService : ISizeService
    {
        private readonly ISizeService _repository;

        public SizeService(ISizeService repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Size>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Size?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Size entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Size entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}