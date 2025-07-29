using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class GioHang_ChiTietService : IGioHang_ChiTietService
    {
        private readonly IGioHang_ChiTietRepository _repository;

        public GioHang_ChiTietService(IGioHang_ChiTietRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<GioHang_ChiTiet>> GetAllAsync() => _repository.GetAllAsync();

        public Task<GioHang_ChiTiet?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(GioHang_ChiTiet entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(GioHang_ChiTiet entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}