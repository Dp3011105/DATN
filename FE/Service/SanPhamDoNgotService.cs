using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SanPhamDoNgotService : ISanPhamDoNgotService
    {
        private readonly ISanPhamDoNgotRepository _repository;

        public SanPhamDoNgotService(ISanPhamDoNgotRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SanPhamDoNgot>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SanPhamDoNgot?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SanPhamDoNgot entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(SanPhamDoNgot entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}