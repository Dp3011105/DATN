using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MyDbContext _context;

        public MessageRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Message.ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Message.FindAsync(id);
        }

        public async Task AddAsync(Message entity)
        {
            await _context.Message.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Message entity)
        {
            _context.Message.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Message.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}