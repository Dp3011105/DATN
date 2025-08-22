using BE.Data;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly MyDbContext _context;

        public SanPhamRepository(MyDbContext context)
        {
            _context = context;
        }
        //public async Task<List<SanPham>> GetAllWithDetailsAsync()
        //{
        //    return await _context.San_Pham
        //        .Include(sp => sp.SanPhamSizes)
        //            .ThenInclude(sps => sps.Size)
        //        .Include(sp => sp.SanPhamLuongDas)
        //            .ThenInclude(spld => spld.LuongDa)
        //        .Include(sp => sp.SanPhamDoNgots)
        //            .ThenInclude(spdn => spdn.DoNgot)
        //        .Include(sp => sp.SanPhamToppings)
        //            .ThenInclude(spt => spt.Topping)
        //        .Select(sp => new SanPham
        //        {
        //            ID_San_Pham = sp.ID_San_Pham,
        //            Ten_San_Pham = sp.Ten_San_Pham,
        //            Gia = sp.Gia,
        //            So_Luong = sp.So_Luong,
        //            Hinh_Anh = sp.Hinh_Anh,
        //            Mo_Ta = sp.Mo_Ta,
        //            Trang_Thai = sp.Trang_Thai,
        //            SanPhamSizes = sp.SanPhamSizes.Select(sps => new SanPhamSize
        //            {
        //                ID_Size = sps.ID_Size,
        //                ID_San_Pham = sps.ID_San_Pham,
        //                Size = new Size
        //                {
        //                    ID_Size = sps.Size.ID_Size,
        //                    SizeName = sps.Size.SizeName,
        //                    Trang_Thai = sps.Size.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamLuongDas = sp.SanPhamLuongDas.Select(spld => new SanPhamLuongDa
        //            {
        //                ID_LuongDa = spld.ID_LuongDa,
        //                ID_San_Pham = spld.ID_San_Pham,
        //                LuongDa = new LuongDa
        //                {
        //                    ID_LuongDa = spld.LuongDa.ID_LuongDa,
        //                    Ten_LuongDa = spld.LuongDa.Ten_LuongDa,
        //                    Trang_Thai = spld.LuongDa.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamDoNgots = sp.SanPhamDoNgots.Select(spdn => new SanPhamDoNgot
        //            {
        //                ID_DoNgot = spdn.ID_DoNgot,
        //                ID_San_Pham = spdn.ID_San_Pham,
        //                DoNgot = new DoNgot
        //                {
        //                    ID_DoNgot = spdn.DoNgot.ID_DoNgot,
        //                    Muc_Do = spdn.DoNgot.Muc_Do,
        //                    Trang_Thai = spdn.DoNgot.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamToppings = sp.SanPhamToppings.Select(spt => new SanPhamTopping
        //            {
        //                ID_Topping = spt.ID_Topping,
        //                ID_San_Pham = spt.ID_San_Pham,
        //                Topping = new Topping
        //                {
        //                    ID_Topping = spt.Topping.ID_Topping,
        //                    Ten = spt.Topping.Ten,
        //                    So_Luong = spt.Topping.So_Luong,
        //                    Gia = spt.Topping.Gia,
        //                    Trang_Thai = spt.Topping.Trang_Thai
        //                }
        //            }).ToList()
        //        })
        //        .ToListAsync();
        //}

        //public async Task<SanPham> GetByIdWithDetailsAsync(int id)
        //{
        //    return await _context.San_Pham
        //        .Where(sp => sp.ID_San_Pham == id)
        //        .Include(sp => sp.SanPhamSizes)
        //            .ThenInclude(sps => sps.Size)
        //        .Include(sp => sp.SanPhamLuongDas)
        //            .ThenInclude(spld => spld.LuongDa)
        //        .Include(sp => sp.SanPhamDoNgots)
        //            .ThenInclude(spdn => spdn.DoNgot)
        //        .Include(sp => sp.SanPhamToppings)
        //            .ThenInclude(spt => spt.Topping)
        //        .Select(sp => new SanPham
        //        {
        //            ID_San_Pham = sp.ID_San_Pham,
        //            Ten_San_Pham = sp.Ten_San_Pham,
        //            Gia = sp.Gia,
        //            So_Luong = sp.So_Luong,
        //            Hinh_Anh = sp.Hinh_Anh,
        //            Mo_Ta = sp.Mo_Ta,
        //            Trang_Thai = sp.Trang_Thai,
        //            SanPhamSizes = sp.SanPhamSizes.Select(sps => new SanPhamSize
        //            {
        //                ID_Size = sps.ID_Size,
        //                ID_San_Pham = sps.ID_San_Pham,
        //                Size = new Size
        //                {
        //                    ID_Size = sps.Size.ID_Size,
        //                    SizeName = sps.Size.SizeName,
        //                    Trang_Thai = sps.Size.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamLuongDas = sp.SanPhamLuongDas.Select(spld => new SanPhamLuongDa
        //            {
        //                ID_LuongDa = spld.ID_LuongDa,
        //                ID_San_Pham = spld.ID_San_Pham,
        //                LuongDa = new LuongDa
        //                {
        //                    ID_LuongDa = spld.LuongDa.ID_LuongDa,
        //                    Ten_LuongDa = spld.LuongDa.Ten_LuongDa,
        //                    Trang_Thai = spld.LuongDa.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamDoNgots = sp.SanPhamDoNgots.Select(spdn => new SanPhamDoNgot
        //            {
        //                ID_DoNgot = spdn.ID_DoNgot,
        //                ID_San_Pham = spdn.ID_San_Pham,
        //                DoNgot = new DoNgot
        //                {
        //                    ID_DoNgot = spdn.DoNgot.ID_DoNgot,
        //                    Muc_Do = spdn.DoNgot.Muc_Do,
        //                    Trang_Thai = spdn.DoNgot.Trang_Thai
        //                }
        //            }).ToList(),
        //            SanPhamToppings = sp.SanPhamToppings.Select(spt => new SanPhamTopping
        //            {
        //                ID_Topping = spt.ID_Topping,
        //                ID_San_Pham = spt.ID_San_Pham,
        //                Topping = new Topping
        //                {
        //                    ID_Topping = spt.Topping.ID_Topping,
        //                    Ten = spt.Topping.Ten,
        //                    So_Luong = spt.Topping.So_Luong,
        //                    Gia = spt.Topping.Gia,
        //                    Trang_Thai = spt.Topping.Trang_Thai
        //                }
        //            }).ToList()
        //        })
        //        .FirstOrDefaultAsync();
        //}


        public async Task<List<SanPham>> GetAllWithDetailsAsync()
        {
            return await _context.San_Pham
                .Include(sp => sp.SanPhamSizes)
                    .ThenInclude(sps => sps.Size)
                .Include(sp => sp.SanPhamLuongDas)
                    .ThenInclude(spld => spld.LuongDa)
                .Include(sp => sp.SanPhamDoNgots)
                    .ThenInclude(spdn => spdn.DoNgot)
                .Include(sp => sp.SanPhamToppings)
                    .ThenInclude(spt => spt.Topping)
                .Include(sp => sp.SanPhamKhuyenMais)
                    .ThenInclude(spkm => spkm.BangKhuyenMai)
                .Select(sp => new SanPham
                {
                    ID_San_Pham = sp.ID_San_Pham,
                    Ten_San_Pham = sp.Ten_San_Pham,
                    Gia = sp.Gia,
                    So_Luong = sp.So_Luong,
                    Hinh_Anh = sp.Hinh_Anh,
                    Mo_Ta = sp.Mo_Ta,
                    Trang_Thai = sp.Trang_Thai,
                    SanPhamSizes = sp.SanPhamSizes.Select(sps => new SanPhamSize
                    {
                        ID_Size = sps.ID_Size,
                        ID_San_Pham = sps.ID_San_Pham,
                        Size = new Size
                        {
                            ID_Size = sps.Size.ID_Size,
                            SizeName = sps.Size.SizeName,
                            Trang_Thai = sps.Size.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamLuongDas = sp.SanPhamLuongDas.Select(spld => new SanPhamLuongDa
                    {
                        ID_LuongDa = spld.ID_LuongDa,
                        ID_San_Pham = spld.ID_San_Pham,
                        LuongDa = new LuongDa
                        {
                            ID_LuongDa = spld.LuongDa.ID_LuongDa,
                            Ten_LuongDa = spld.LuongDa.Ten_LuongDa,
                            Trang_Thai = spld.LuongDa.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamDoNgots = sp.SanPhamDoNgots.Select(spdn => new SanPhamDoNgot
                    {
                        ID_DoNgot = spdn.ID_DoNgot,
                        ID_San_Pham = spdn.ID_San_Pham,
                        DoNgot = new DoNgot
                        {
                            ID_DoNgot = spdn.DoNgot.ID_DoNgot,
                            Muc_Do = spdn.DoNgot.Muc_Do,
                            Trang_Thai = spdn.DoNgot.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamToppings = sp.SanPhamToppings.Select(spt => new SanPhamTopping
                    {
                        ID_Topping = spt.ID_Topping,
                        ID_San_Pham = spt.ID_San_Pham,
                        Topping = new Topping
                        {
                            ID_Topping = spt.Topping.ID_Topping,
                            Ten = spt.Topping.Ten,
                            So_Luong = spt.Topping.So_Luong,
                            Gia = spt.Topping.Gia,
                            Trang_Thai = spt.Topping.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamKhuyenMais = sp.SanPhamKhuyenMais.Select(spkm => new SanPhamKhuyenMai
                    {
                        ID_San_Pham_Khuyen_Mai = spkm.ID_San_Pham_Khuyen_Mai,
                        ID_San_Pham = spkm.ID_San_Pham,
                        ID_Khuyen_Mai = spkm.ID_Khuyen_Mai,
                        Phan_Tram_Giam = spkm.Phan_Tram_Giam,
                        Gia_Giam = spkm.Gia_Giam,
                        BangKhuyenMai = new KhuyenMai
                        {
                            ID_Khuyen_Mai = spkm.BangKhuyenMai.ID_Khuyen_Mai,
                            Ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                            Ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                            Ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc,
                            Mo_Ta = spkm.BangKhuyenMai.Mo_Ta,
                            Trang_Thai = spkm.BangKhuyenMai.Trang_Thai
                        }
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<SanPham> GetByIdWithDetailsAsync(int id)
        {
            return await _context.San_Pham
                .Where(sp => sp.ID_San_Pham == id)
                .Include(sp => sp.SanPhamSizes)
                    .ThenInclude(sps => sps.Size)
                .Include(sp => sp.SanPhamLuongDas)
                    .ThenInclude(spld => spld.LuongDa)
                .Include(sp => sp.SanPhamDoNgots)
                    .ThenInclude(spdn => spdn.DoNgot)
                .Include(sp => sp.SanPhamToppings)
                    .ThenInclude(spt => spt.Topping)
                .Include(sp => sp.SanPhamKhuyenMais)
                    .ThenInclude(spkm => spkm.BangKhuyenMai)
                .Select(sp => new SanPham
                {
                    ID_San_Pham = sp.ID_San_Pham,
                    Ten_San_Pham = sp.Ten_San_Pham,
                    Gia = sp.Gia,
                    So_Luong = sp.So_Luong,
                    Hinh_Anh = sp.Hinh_Anh,
                    Mo_Ta = sp.Mo_Ta,
                    Trang_Thai = sp.Trang_Thai,
                    SanPhamSizes = sp.SanPhamSizes.Select(sps => new SanPhamSize
                    {
                        ID_Size = sps.ID_Size,
                        ID_San_Pham = sps.ID_San_Pham,
                        Size = new Size
                        {
                            ID_Size = sps.Size.ID_Size,
                            SizeName = sps.Size.SizeName,
                            Trang_Thai = sps.Size.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamLuongDas = sp.SanPhamLuongDas.Select(spld => new SanPhamLuongDa
                    {
                        ID_LuongDa = spld.ID_LuongDa,
                        ID_San_Pham = spld.ID_San_Pham,
                        LuongDa = new LuongDa
                        {
                            ID_LuongDa = spld.LuongDa.ID_LuongDa,
                            Ten_LuongDa = spld.LuongDa.Ten_LuongDa,
                            Trang_Thai = spld.LuongDa.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamDoNgots = sp.SanPhamDoNgots.Select(spdn => new SanPhamDoNgot
                    {
                        ID_DoNgot = spdn.ID_DoNgot,
                        ID_San_Pham = spdn.ID_San_Pham,
                        DoNgot = new DoNgot
                        {
                            ID_DoNgot = spdn.DoNgot.ID_DoNgot,
                            Muc_Do = spdn.DoNgot.Muc_Do,
                            Trang_Thai = spdn.DoNgot.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamToppings = sp.SanPhamToppings.Select(spt => new SanPhamTopping
                    {
                        ID_Topping = spt.ID_Topping,
                        ID_San_Pham = spt.ID_San_Pham,
                        Topping = new Topping
                        {
                            ID_Topping = spt.Topping.ID_Topping,
                            Ten = spt.Topping.Ten,
                            So_Luong = spt.Topping.So_Luong,
                            Gia = spt.Topping.Gia,
                            Trang_Thai = spt.Topping.Trang_Thai
                        }
                    }).ToList(),
                    SanPhamKhuyenMais = sp.SanPhamKhuyenMais.Select(spkm => new SanPhamKhuyenMai
                    {
                        ID_San_Pham_Khuyen_Mai = spkm.ID_San_Pham_Khuyen_Mai,
                        ID_San_Pham = spkm.ID_San_Pham,
                        ID_Khuyen_Mai = spkm.ID_Khuyen_Mai,
                        Phan_Tram_Giam = spkm.Phan_Tram_Giam,
                        Gia_Giam = spkm.Gia_Giam,
                        BangKhuyenMai = new KhuyenMai
                        {
                            ID_Khuyen_Mai = spkm.BangKhuyenMai.ID_Khuyen_Mai,
                            Ten_Khuyen_Mai = spkm.BangKhuyenMai.Ten_Khuyen_Mai,
                            Ngay_Bat_Dau = spkm.BangKhuyenMai.Ngay_Bat_Dau,
                            Ngay_Ket_Thuc = spkm.BangKhuyenMai.Ngay_Ket_Thuc,
                            Mo_Ta = spkm.BangKhuyenMai.Mo_Ta,
                            Trang_Thai = spkm.BangKhuyenMai.Trang_Thai
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }


        public async Task<SanPhamDTO> CreateSanPhamAsync(SanPhamDTO sanPhamDTO, string imagePath)
        {
            var sanPham = new SanPham
            {
                Ten_San_Pham = sanPhamDTO.Ten_San_Pham,
                Gia = sanPhamDTO.Gia,
                So_Luong = sanPhamDTO.So_Luong ?? 0,
                Hinh_Anh = imagePath,
                Mo_Ta = sanPhamDTO.Mo_Ta,
                Trang_Thai = sanPhamDTO.Trang_Thai
            };

            _context.San_Pham.Add(sanPham);
            await _context.SaveChangesAsync();

            // Thêm Sizes
            if (sanPhamDTO.Sizes != null && sanPhamDTO.Sizes.Any())
            {
                foreach (var sizeId in sanPhamDTO.Sizes)
                {
                    _context.SanPham_Size.Add(new SanPhamSize
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_Size = sizeId
                    });
                }
            }

            // Thêm Lượng Đá
            if (sanPhamDTO.LuongDas != null && sanPhamDTO.LuongDas.Any())
            {
                foreach (var luongDaId in sanPhamDTO.LuongDas)
                {
                    _context.SanPhamLuongDa.Add(new SanPhamLuongDa
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_LuongDa = luongDaId
                    });
                }
            }

            // Thêm Độ Ngọt
            if (sanPhamDTO.DoNgots != null && sanPhamDTO.DoNgots.Any())
            {
                foreach (var doNgotId in sanPhamDTO.DoNgots)
                {
                    _context.SanPham_DoNgot.Add(new SanPhamDoNgot
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_DoNgot = doNgotId
                    });
                }
            }

            // Thêm Toppings
            if (sanPhamDTO.Toppings != null && sanPhamDTO.Toppings.Any())
            {
                foreach (var toppingId in sanPhamDTO.Toppings)
                {
                    _context.SanPham_Topping.Add(new SanPhamTopping
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_Topping = toppingId
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Trả về DTO để tránh vòng lặp
            return new SanPhamDTO
            {
                ID_San_Pham = sanPham.ID_San_Pham,
                Ten_San_Pham = sanPham.Ten_San_Pham,
                Gia = sanPham.Gia,
                So_Luong = sanPham.So_Luong,
                Hinh_Anh = sanPham.Hinh_Anh,
                Mo_Ta = sanPham.Mo_Ta,
                Trang_Thai = sanPham.Trang_Thai,
                Sizes = sanPhamDTO.Sizes,
                LuongDas = sanPhamDTO.LuongDas,
                DoNgots = sanPhamDTO.DoNgots,
                Toppings = sanPhamDTO.Toppings
            };
        }

        public async Task<SanPhamDTO> UpdateSanPhamAsync(int id, SanPhamDTO sanPhamDTO, string imagePath)
        {
            var sanPham = await _context.San_Pham
                .Include(sp => sp.SanPhamSizes)
                .Include(sp => sp.SanPhamLuongDas)
                .Include(sp => sp.SanPhamDoNgots)
                .Include(sp => sp.SanPhamToppings)
                .FirstOrDefaultAsync(sp => sp.ID_San_Pham == id);

            if (sanPham == null)
            {
                return null;
            }

            // Cập nhật thông tin sản phẩm
            sanPham.Ten_San_Pham = sanPhamDTO.Ten_San_Pham;
            sanPham.Gia = sanPhamDTO.Gia;
            sanPham.So_Luong = sanPhamDTO.So_Luong ?? 0;
            sanPham.Hinh_Anh = imagePath ?? sanPham.Hinh_Anh;
            sanPham.Mo_Ta = sanPhamDTO.Mo_Ta;
            sanPham.Trang_Thai = sanPhamDTO.Trang_Thai;

            // Xóa các quan hệ cũ
            _context.SanPham_Size.RemoveRange(sanPham.SanPhamSizes);
            _context.SanPhamLuongDa.RemoveRange(sanPham.SanPhamLuongDas);
            _context.SanPham_DoNgot.RemoveRange(sanPham.SanPhamDoNgots);
            _context.SanPham_Topping.RemoveRange(sanPham.SanPhamToppings);

            // Thêm Sizes mới
            if (sanPhamDTO.Sizes != null && sanPhamDTO.Sizes.Any())
            {
                foreach (var sizeId in sanPhamDTO.Sizes)
                {
                    _context.SanPham_Size.Add(new SanPhamSize
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_Size = sizeId
                    });
                }
            }

            // Thêm Lượng Đá mới
            if (sanPhamDTO.LuongDas != null && sanPhamDTO.LuongDas.Any())
            {
                foreach (var luongDaId in sanPhamDTO.LuongDas)
                {
                    _context.SanPhamLuongDa.Add(new SanPhamLuongDa
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_LuongDa = luongDaId
                    });
                }
            }

            // Thêm Độ Ngọt mới
            if (sanPhamDTO.DoNgots != null && sanPhamDTO.DoNgots.Any())
            {
                foreach (var doNgotId in sanPhamDTO.DoNgots)
                {
                    _context.SanPham_DoNgot.Add(new SanPhamDoNgot
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_DoNgot = doNgotId
                    });
                }
            }

            // Thêm Toppings mới
            if (sanPhamDTO.Toppings != null && sanPhamDTO.Toppings.Any())
            {
                foreach (var toppingId in sanPhamDTO.Toppings)
                {
                    _context.SanPham_Topping.Add(new SanPhamTopping
                    {
                        ID_San_Pham = sanPham.ID_San_Pham,
                        ID_Topping = toppingId
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Trả về DTO để tránh vòng lặp
            return new SanPhamDTO
            {
                ID_San_Pham = sanPham.ID_San_Pham,
                Ten_San_Pham = sanPham.Ten_San_Pham,
                Gia = sanPham.Gia,
                So_Luong = sanPham.So_Luong,
                Hinh_Anh = sanPham.Hinh_Anh,
                Mo_Ta = sanPham.Mo_Ta,
                Trang_Thai = sanPham.Trang_Thai,
                Sizes = sanPhamDTO.Sizes,
                LuongDas = sanPhamDTO.LuongDas,
                DoNgots = sanPhamDTO.DoNgots,
                Toppings = sanPhamDTO.Toppings
            };
        }
    }

}