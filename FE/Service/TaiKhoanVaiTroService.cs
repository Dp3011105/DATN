using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class TaiKhoanVaiTroService : ITaiKhoanVaiTroService
    {
        private readonly ITaiKhoanVaiTroRepository _repository;

        public TaiKhoanVaiTroService(ITaiKhoanVaiTroRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TaiKhoanVaiTro>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TaiKhoanVaiTro?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(TaiKhoanVaiTro entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(TaiKhoanVaiTro entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}