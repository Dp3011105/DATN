using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class ToppingService : IToppingService
    {
        private readonly IToppingRepository _repository;

        public ToppingService(IToppingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Topping>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Topping?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Topping entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Topping entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}