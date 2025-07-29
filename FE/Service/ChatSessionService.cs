using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class ChatSessionService : IChatSessionService
    {
        private readonly IChatSessionRepository _repository;

        public ChatSessionService(IChatSessionRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ChatSession>> GetAllAsync() => _repository.GetAllAsync();

        public Task<ChatSession?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(ChatSession entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(ChatSession entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}