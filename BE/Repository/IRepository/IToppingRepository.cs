using BE.DTOs;

namespace BE.Repository.IRepository
{
    public interface IToppingRepository
    {
        Task<List<ToppingDTO>> GetAllAsync();
        Task<ToppingDTO> GetByIdAsync(int id);
        Task<ToppingDTO> CreateAsync(ToppingDTO toppingDTO);
        Task<ToppingDTO> UpdateAsync(int id, ToppingDTO toppingDTO);
    }
}
