using BE.DTOs;
using BE.models;

namespace Repository.IRepository
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<VoucherDTO>> GetAllAsync();//Ph??c làm ph?n này 
        Task<VoucherDTO> GetByIdAsync(int id);
        Task<VoucherDTO> AddAsync(VoucherDTO voucherDTO);
        Task<VoucherDTO> UpdateAsync(int id, VoucherDTO voucherDTO);
    }
}