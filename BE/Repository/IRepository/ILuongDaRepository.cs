using BE.DTOs;

namespace BE.Repository.IRepository
{
    public interface ILuongDaRepository
    {
        Task<List<LuongDaDTO>> GetAllAsync();
        Task<LuongDaDTO> GetByIdAsync(int id);
        Task<LuongDaDTO> CreateAsync(LuongDaDTO luongDaDTO);
        Task<LuongDaDTO> UpdateAsync(int id, LuongDaDTO luongDaDTO);
    }
}

