using BE.Data;
using BE.DTOs.Requests;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class GanVoucherRepository : IGanVoucherRepository
    {
        private readonly MyDbContext _context;
        public GanVoucherRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<KhachHang>> GetAllKhachHangAsync()
        {
            return await _context.Khach_Hang.ToListAsync();
        }

        // Lấy top 10 khách hàng VIP
        public async Task<List<KhachHang>> GetTop10KhachHangVipAsync()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            var topKhachHang = await (from hd in _context.Hoa_Don
                                      join kh in _context.Khach_Hang on hd.ID_Khach_Hang equals kh.ID_Khach_Hang
                                      where hd.Trang_Thai == "Hoan_Thanh" && hd.Ngay_Tao >= oneWeekAgo
                                      group kh by new { kh.ID_Khach_Hang, kh.Ho_Ten, kh.Email } into g
                                      orderby g.Count() descending
                                      select g.First())
                                     .Take(10)
                                     .ToListAsync();
            return topKhachHang;
        }

        public async Task<List<Voucher>> GetAllVouchersAsync()
        {
            return await _context.Voucher
            .Where(v => v.So_Luong > 0 && v.Trang_Thai == true)
            .OrderBy(v => v.Ten)
            .ToListAsync();
        }

        public async Task<string> GanVoucherAsync(GanVoucherRequest req)
        {
            int successCount = 0;
            int errorCount = 0;
            var messages = new List<string>();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Dictionary để lưu thông tin khách hàng và voucher (tránh query nhiều lần)
                var khachHangDict = new Dictionary<int, string>();
                var voucherDict = new Dictionary<int, (string Ten, int SoLuong)>();

                // Load tất cả khách hàng cần thiết
                var khachHangs = await _context.Khach_Hang
                    .Where(kh => req.ID_Khach_Hang.Contains(kh.ID_Khach_Hang))
                    .ToListAsync();

                foreach (var kh in khachHangs)
                {
                    khachHangDict[kh.ID_Khach_Hang] = kh.Ho_Ten ?? "Chưa cập nhật";
                }

                // Load tất cả voucher cần thiết
                var vouchers = await _context.Voucher
                    .Where(v => req.ID_Voucher.Contains(v.ID_Voucher))
                    .ToListAsync();

                foreach (var v in vouchers)
                {
                    voucherDict[v.ID_Voucher] = (v.Ten ?? "Voucher không tên", v.So_Luong ?? 0); // Handle nullable int
                }

                // Kiểm tra dữ liệu đầu vào
                var missingCustomers = req.ID_Khach_Hang.Where(id => !khachHangDict.ContainsKey(id)).ToList();
                var missingVouchers = req.ID_Voucher.Where(id => !voucherDict.ContainsKey(id)).ToList();

                foreach (var missingId in missingCustomers)
                {
                    messages.Add($"❌ Khách hàng ID {missingId}: Không tồn tại trong hệ thống.");
                    errorCount++;
                }

                foreach (var missingId in missingVouchers)
                {
                    messages.Add($"❌ Voucher ID {missingId}: Không tồn tại trong hệ thống.");
                    errorCount++;
                }

                // Tính toán và kiểm tra số lượng voucher
                var voucherUsage = new Dictionary<int, int>();
                foreach (var voucherId in req.ID_Voucher.Where(id => voucherDict.ContainsKey(id)))
                {
                    // Mỗi voucher sẽ cần số lượng = số khách hàng * số lượng voucher mỗi người
                    var totalNeeded = req.ID_Khach_Hang.Count(id => khachHangDict.ContainsKey(id)) * req.SoLuong;
                    voucherUsage[voucherId] = totalNeeded;

                    var available = voucherDict[voucherId].SoLuong; // Đã được handle nullable ở trên
                    if (available < totalNeeded)
                    {
                        messages.Add($"❌ Voucher '{voucherDict[voucherId].Ten}': Không đủ số lượng (có {available}, cần {totalNeeded}).");
                        errorCount++;
                    }
                }

                // Nếu có lỗi nghiêm trọng thì dừng
                if (errorCount > 0 && successCount == 0)
                {
                    await transaction.RollbackAsync();
                    messages.Insert(0, $"❌ THẤT BẠI: Có {errorCount} lỗi, không thể thực hiện gán voucher.");
                    return string.Join("\n", messages);
                }

                // Thực hiện gán voucher
                var recordsToAdd = new List<KhachHangVoucher>();

                foreach (var khachHangId in req.ID_Khach_Hang.Where(id => khachHangDict.ContainsKey(id)))
                {
                    var khachHangTen = khachHangDict[khachHangId];

                    foreach (var voucherId in req.ID_Voucher.Where(id => voucherDict.ContainsKey(id)))
                    {
                        var voucherInfo = voucherDict[voucherId];

                        // Kiểm tra đã gán chưa (chỉ cần check tồn tại, không quan tâm ID vì nó auto-increment)
                        var existingRecord = await _context.KhachHang_Voucher
                            .FirstOrDefaultAsync(kv => kv.ID_Khach_Hang == khachHangId &&
                                                     kv.ID_Voucher == voucherId &&
                                                     kv.Trang_Thai == true);

                        if (existingRecord != null)
                        {
                            messages.Add($"⚠️ '{khachHangTen}' - '{voucherInfo.Ten}': Đã được gán trước đó, bỏ qua.");
                            continue;
                        }

                        // Kiểm tra số lượng voucher còn đủ không
                        var currentVoucher = await _context.Voucher.FirstOrDefaultAsync(v => v.ID_Voucher == voucherId);
                        if (currentVoucher == null || (currentVoucher.So_Luong ?? 0) < req.SoLuong)
                        {
                            messages.Add($"❌ '{khachHangTen}' - '{voucherInfo.Ten}': Voucher không đủ số lượng.");
                            errorCount++;
                            continue;
                        }

                        // Tạo records mới - mỗi voucher tạo req.SoLuong records
                        for (int i = 0; i < req.SoLuong; i++)
                        {
                            var newKhachHangVoucher = new KhachHangVoucher
                            {
                                // Không cần set ID vì nó auto-increment
                                ID_Khach_Hang = khachHangId,
                                ID_Voucher = voucherId,
                                Ghi_Chu = req.GhiChu ?? string.Empty,
                                Trang_Thai = true
                            };

                            recordsToAdd.Add(newKhachHangVoucher);
                        }

                        // Trừ số lượng voucher (handle nullable)
                        currentVoucher.So_Luong = (currentVoucher.So_Luong ?? 0) - req.SoLuong;

                        successCount++;
                        messages.Add($"✅ '{khachHangTen}' - '{voucherInfo.Ten}': Gán thành công {req.SoLuong} voucher.");
                    }
                }

                // Lưu tất cả bản ghi
                if (recordsToAdd.Any() || _context.ChangeTracker.HasChanges())
                {
                    await _context.KhachHang_Voucher.AddRangeAsync(recordsToAdd);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var totalAssignments = successCount;
                    var totalVouchersCreated = recordsToAdd.Count; // Số records được tạo
                    var totalVouchersDeducted = successCount * req.SoLuong; // Số voucher bị trừ từ kho

                    messages.Insert(0, $"🎉 HOÀN THÀNH: {totalAssignments} lượt gán thành công, tạo {totalVouchersCreated} voucher records, trừ {totalVouchersDeducted} voucher từ kho" +
                                     (errorCount > 0 ? $", {errorCount} lỗi bỏ qua." : "."));
                }
                else
                {
                    await transaction.RollbackAsync();
                    messages.Insert(0, errorCount > 0
                        ? $"❌ THẤT BẠI: {errorCount} lỗi, không có voucher nào được gán."
                        : "⚠️ KHÔNG CÓ THAY ĐỔI: Tất cả voucher đã được gán trước đó.");
                }

                return string.Join("\n", messages);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Log chi tiết lỗi
                var errorMessage = $"❌ Lỗi hệ thống khi gán voucher: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
                }

                // Thêm thông tin debug
                if (ex is DbUpdateException)
                {
                    errorMessage += "\n💡 Gợi ý: Kiểm tra ràng buộc dữ liệu, khóa ngoại, hoặc trùng lặp bản ghi.";
                }

                throw new Exception(errorMessage, ex);
            }
        }

        public async Task<List<KhachHangVoucher>> GetVouchersByKhachHangAsync(int khachHangId)
        {
            return await _context.KhachHang_Voucher
                .Include(kv => kv.Voucher)
                .Include(kv => kv.KhachHang)
                .Where(kv => kv.ID_Khach_Hang == khachHangId && kv.Trang_Thai == true)
                .OrderByDescending(kv => kv.ID_Khach_Hang)
                .ToListAsync();
        }

        public async Task<bool> IsVoucherAssignedToCustomerAsync(int khachHangId, int voucherId)
        {
            return await _context.KhachHang_Voucher
                .AnyAsync(kv => kv.ID_Khach_Hang == khachHangId &&
                               kv.ID_Voucher == voucherId &&
                               kv.Trang_Thai == true);
        }
        public async Task<Dictionary<int, List<int>>> GetAllCustomerVoucherAssignmentsAsync()
        {
            try
            {
                var assignments = await _context.KhachHang_Voucher
                    .Where(kv => kv.Trang_Thai == true)
                    .GroupBy(kv => kv.ID_Khach_Hang)
                    .Select(g => new
                    {
                        CustomerId = g.Key,
                        VoucherIds = g.Select(kv => kv.ID_Voucher).ToList()
                    })
                    .ToListAsync();

                return assignments.ToDictionary(a => a.CustomerId, a => a.VoucherIds);
            }
            catch (Exception ex)
            {
                // Log error nếu cần
                return new Dictionary<int, List<int>>();
            }
        }
    }
}