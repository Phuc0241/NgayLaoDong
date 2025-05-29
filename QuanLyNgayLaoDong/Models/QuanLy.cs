namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuanLy")]
    public partial class QuanLy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuanLy()
        {
            PhieuDuyets = new HashSet<PhieuDuyet>();
            PhieuXacNhanHoanThanhs = new HashSet<PhieuXacNhanHoanThanh>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int quanly_id { get; set; }

        [StringLength(255)]
        public string hoten { get; set; }

        public bool? gioitinh { get; set; }

        [StringLength(255)]
        public string diachi { get; set; }

        [StringLength(15)]
        public string SDT { get; set; }

        public int? taikhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuDuyet> PhieuDuyets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXacNhanHoanThanh> PhieuXacNhanHoanThanhs { get; set; }

        public virtual TaiKhoan TaiKhoan1 { get; set; }
    }
}
