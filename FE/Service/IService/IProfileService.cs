using FE.Models;

namespace FE.Service.IService
{
    public interface IProfileService
    {
        Task<ProfileModel> GetProfileAsync(int khachHangId);
        Task<bool> UpdateProfileAsync(int khachHangId, ProfileUpdateModel model);
        Task<bool> AddAddressAsync(int khachHangId, AddressCreateModel model);
        Task<bool> UpdateAddressAsync(int khachHangId, int diaChiId, AddressModel model);
        Task<bool> DeleteAddressAsync(int khachHangId, int diaChiId);
    }
}