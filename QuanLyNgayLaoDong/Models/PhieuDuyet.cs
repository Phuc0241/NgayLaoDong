namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuDuyet")]
    public partial class PhieuDuyet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuDuyet()
        {
            PhieuXacNhanHoanThanhs = new HashSet<PhieuXacNhanHoanThanh>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? Nguoiduyet { get; set; }

        public DateTime? ThoiGian { get; set; }

        public int? PhieuDangKy { get; set; }

        public virtual PhieuDangKy PhieuDangKy1 { get; set; }

        public virtual QuanLy QuanLy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXacNhanHoanThanh> PhieuXacNhanHoanThanhs { get; set; }
    }
}
