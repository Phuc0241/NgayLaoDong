namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TaoDotNgayLaoDong")]
    public partial class TaoDotNgayLaoDong
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string TenDotLaoDong { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayBatDau { get; set; } // Cho phép NULL

        [Column(TypeName = "date")]
        public DateTime? NgayKetThuc { get; set; } // Cho phép NULL

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public int? NguoiTao { get; set; }

        [StringLength(255)]
        public string DotLaoDong { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayLaoDong { get; set; }

        [StringLength(100)]
        public string Buoi { get; set; }

        [StringLength(255)]
        public string LoaiLaoDong { get; set; }

        public int? GiaTri { get; set; }

        [StringLength(100)]
        public string ThoiGian { get; set; }

        [StringLength(100)]
        public string KhuVuc { get; set; }

        public DateTime? NgayXoa { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}