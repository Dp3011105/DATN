using FE.Models;
namespace FE.Service.IService
{
    public interface ICheckoutService
    {
        Task<List<HinhThucThanhToanCheckOutTK>> GetPaymentMethodsAsync();
        Task<List<DiaChiCheckOutTK>> GetCustomerAddressesAsync(string customerId);
        Task<List<VoucherCheckOutTK>> GetVouchersAsync(string customerId);
        Task<List<ChiTietGioHangCheckOutTK>> GetCartItemsByIdsAsync(string customerId, List<int> selectedIds);
        Task<bool> AddAddressAsync(string customerId, DiaChiCheckOutTK newAddress);
        Task<bool> UpdateAddressAsync(string customerId, DiaChiCheckOutTK updatedAddress);
        Task<object> ProcessCheckoutAsync(CheckoutModel model); // CheckoutModel là class map với JSON
    }
}
