using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HoaDonChiTietThueService : IHoaDonChiTietThueService
    {
        private readonly IHoaDonChiTietThueRepository _repository;

        public HoaDonChiTietThueService(IHoaDonChiTietThueRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HoaDonChiTietThue>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HoaDonChiTietThue?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HoaDonChiTietThue entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HoaDonChiTietThue entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}