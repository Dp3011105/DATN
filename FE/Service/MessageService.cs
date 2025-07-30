using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;

        public MessageService(IMessageRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Message>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Message?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Message entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(Message entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}