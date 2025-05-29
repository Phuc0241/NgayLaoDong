namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuXacNhanHoanThanh")]
    public partial class PhieuXacNhanHoanThanh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuXacNhanHoanThanh()
        {
            SoNgayLaoDongs = new HashSet<SoNgayLaoDong>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? MSSV { get; set; }

        public int? phieuduyet { get; set; }

        public int? NguoiXacNhan { get; set; }

        public DateTime? ThoiGian { get; set; }

        public virtual PhieuDuyet PhieuDuyet1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SoNgayLaoDong> SoNgayLaoDongs { get; set; }

        public virtual QuanLy QuanLy { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
