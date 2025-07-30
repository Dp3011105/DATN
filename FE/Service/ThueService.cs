using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class ThueService : IThueService
    {
        private readonly IThueRepository _repository;

        public ThueService(IThueRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Thue>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Thue?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Thue entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Thue entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}