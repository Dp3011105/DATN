using BE.DTOs;

namespace BE.Repository.IRepository
{
    public interface ISizeRepository
    {
        Task<List<SizeDTO>> GetAllAsync();
        Task<SizeDTO> GetByIdAsync(int id);
        Task<SizeDTO> CreateAsync(SizeDTO sizeDTO);
        Task<SizeDTO> UpdateAsync(int id, SizeDTO sizeDTO);
    }
}
