using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class GioHangChiTiet_ToppingService : IGioHangChiTiet_ToppingService
    {
        private readonly IGioHangChiTiet_ToppingRepository _repository;

        public GioHangChiTiet_ToppingService(IGioHangChiTiet_ToppingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<GioHangChiTiet_Topping>> GetAllAsync() => _repository.GetAllAsync();

        public Task<GioHangChiTiet_Topping?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(GioHangChiTiet_Topping entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(GioHangChiTiet_Topping entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}