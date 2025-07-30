using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class ChatSessionNhanVienService : IChatSessionNhanVienService
    {
        private readonly IChatSessionNhanVienRepository _repository;

        public ChatSessionNhanVienService(IChatSessionNhanVienRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ChatSessionNhanVien>> GetAllAsync() => _repository.GetAllAsync();

        public Task<ChatSessionNhanVien?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(ChatSessionNhanVien entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(ChatSessionNhanVien entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}