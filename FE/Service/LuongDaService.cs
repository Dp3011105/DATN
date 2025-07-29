using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class LuongDaService : ILuongDaService
    {
        private readonly ILuongDaRepository _repository;

        public LuongDaService(ILuongDaRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<LuongDa>> GetAllAsync() => _repository.GetAllAsync();

        public Task<LuongDa?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(LuongDa entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(LuongDa entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}