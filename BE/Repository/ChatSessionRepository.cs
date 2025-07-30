using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly MyDbContext _context;

        public ChatSessionRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatSession>> GetAllAsync()
        {
            return await _context.Chat_Session.ToListAsync();
        }

        public async Task<ChatSession?> GetByIdAsync(int id)
        {
            return await _context.Chat_Session.FindAsync(id);
        }

        public async Task AddAsync(ChatSession entity)
        {
            await _context.Chat_Session.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatSession entity)
        {
            _context.Chat_Session.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Chat_Session.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}