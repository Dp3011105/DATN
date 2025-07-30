using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class HoaDonChiTietToppingService : IHoaDonChiTietToppingService
    {
        private readonly IHoaDonChiTietToppingRepository _repository;

        public HoaDonChiTietToppingService(IHoaDonChiTietToppingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<HoaDonChiTietTopping>> GetAllAsync() => _repository.GetAllAsync();

        public Task<HoaDonChiTietTopping?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(HoaDonChiTietTopping entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(HoaDonChiTietTopping entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}