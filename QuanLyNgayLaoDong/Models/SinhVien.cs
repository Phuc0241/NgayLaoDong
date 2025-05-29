namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVien")]
    public partial class SinhVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SinhVien()
        {
            PhieuDangKies = new HashSet<PhieuDangKy>();
            PhieuXacNhanHoanThanhs = new HashSet<PhieuXacNhanHoanThanh>();
            SoNgayLaoDongs = new HashSet<SoNgayLaoDong>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MSSV { get; set; }

        [Required]
        [StringLength(255)]
        public string hoten { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaysinh { get; set; }

        [StringLength(255)]
        public string quequan { get; set; }

        public bool? gioitinh { get; set; }

        public int? taikhoan { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        public int? lop_id { get; set; }

        public int? anh_id { get; set; }

        public virtual Anh Anh { get; set; }

        public virtual Lop Lop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuDangKy> PhieuDangKies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXacNhanHoanThanh> PhieuXacNhanHoanThanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoNgayLaoDong> SoNgayLaoDongs { get; set; }

        public virtual TaiKhoan TaiKhoan1 { get; set; }
    }
}
