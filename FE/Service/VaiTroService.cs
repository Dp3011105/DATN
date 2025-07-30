using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class VaiTroService : IVaiTroService
    {
        private readonly IVaiTroRepository _repository;

        public VaiTroService(IVaiTroRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<VaiTro>> GetAllAsync() => _repository.GetAllAsync();

        public Task<VaiTro?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(VaiTro entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(VaiTro entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}