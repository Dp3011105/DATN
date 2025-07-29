using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class DoNgotService : IDoNgotService
    {
        private readonly IDoNgotRepository _repository;

        public DoNgotService(IDoNgotRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<DoNgot>> GetAllAsync() => _repository.GetAllAsync();

        public Task<DoNgot?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(DoNgot entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(DoNgot entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}