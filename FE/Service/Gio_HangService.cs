using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class Gio_HangService : IGio_HangService
    {
        private readonly IGio_HangRepository _repository;

        public Gio_HangService(IGio_HangRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Gio_Hang>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Gio_Hang?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Gio_Hang entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Gio_Hang entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}