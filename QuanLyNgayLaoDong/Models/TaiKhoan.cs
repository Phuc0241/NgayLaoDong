namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            QuanLies = new HashSet<QuanLy>();
            SinhViens = new HashSet<SinhVien>();
            TaoDotNgayLaoDongs = new HashSet<TaoDotNgayLaoDong>();
        }

        [Key]
        public int taikhoan_id { get; set; }

        [StringLength(255)]
        public string username { get; set; }

        [StringLength(255)]
        public string password { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        public int? role_id { get; set; }

        // ?? Token ??t l?i m?t kh?u
        [StringLength(255)]
        public string reset_token { get; set; }

        // ?? Th?i gian h?t h?n token
        public DateTime? reset_token_expiry { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuanLy> QuanLies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhViens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaoDotNgayLaoDong> TaoDotNgayLaoDongs { get; set; }

        public virtual VaiTro VaiTro { get; set; }

        // Thêm c?t này vào database
        public DateTime? deleted_at { get; set; } // n?u null => ch?a xóa

    }
}
