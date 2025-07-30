using BE.DTOs;

namespace BE.Repository.IRepository
{
    public interface IDoNgotRepository
    {
        Task<List<DoNgotDTO>> GetAllAsync();
        Task<DoNgotDTO> GetByIdAsync(int id);
        Task<DoNgotDTO> CreateAsync(DoNgotDTO doNgotDTO);
        Task<DoNgotDTO> UpdateAsync(int id, DoNgotDTO doNgotDTO);
    }
}
