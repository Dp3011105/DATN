using BE.DTOs;
using BE.models;

namespace BE.Repository.IRepository
{
    public interface IKhuyenMaiRepository
    {
        
        Task<List<KhuyenMaiDTO>> GetAllAsync();
        Task<KhuyenMaiDTO> GetByIdAsync(int id);
        Task<KhuyenMai> CreateAsync(KhuyenMaiDTO khuyenMaiDTO);
        Task<KhuyenMai> UpdateAsync(int id, KhuyenMaiDTO khuyenMaiDTO);
    }
}

