using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HoaDonChiTietService : IHoaDonChiTietService
    {
        private readonly IHoaDonChiTietRepository _repository;

        public HoaDonChiTietService(IHoaDonChiTietRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HoaDonChiTiet>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HoaDonChiTiet?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HoaDonChiTiet entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HoaDonChiTiet entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}