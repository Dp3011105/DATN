namespace BE.Repository.IRepository
{
    public interface IKhuyenMaiSanPhamRepository
    {
        Task AssignKhuyenMaiToProductsAsync(int idKhuyenMai, List<int> idSanPhams, decimal phanTramGiam);
        Task RemoveKhuyenMaiFromProductsAsync(int idKhuyenMai, List<int> idSanPhams);

        Task<IEnumerable<object>> GetSanPhamByKhuyenMai(int idKhuyenMai);
        Task<bool> HuyKhuyenMai(int idSanPham, int idKhuyenMai);

        Task<IEnumerable<object>> GetAllSanPhamWithKhuyenMai();

    }
}
