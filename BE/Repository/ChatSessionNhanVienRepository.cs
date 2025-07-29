using BE.models;
using BE.Data;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository
{
    public class ChatSessionNhanVienRepository : IChatSessionNhanVienRepository
    {
        private readonly MyDbContext _context;

        public ChatSessionNhanVienRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatSessionNhanVien>> GetAllAsync()
        {
            return await _context.Chat_Session_Nhan_Vien.ToListAsync();
        }

        public async Task<ChatSessionNhanVien?> GetByIdAsync(int id)
        {
            return await _context.Chat_Session_Nhan_Vien.FindAsync(id);
        }

        public async Task AddAsync(ChatSessionNhanVien entity)
        {
            await _context.Chat_Session_Nhan_Vien.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatSessionNhanVien entity)
        {
            _context.Chat_Session_Nhan_Vien.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Chat_Session_Nhan_Vien.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}