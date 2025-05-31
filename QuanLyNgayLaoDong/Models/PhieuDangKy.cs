namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuDangKy")]
    public partial class PhieuDangKy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuDangKy()
        {
            PhieuDuyets = new HashSet<PhieuDuyet>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int? DotId { get; set; }

        public int? MSSV { get; set; }

        public bool? LaoDongTheoLop { get; set; }

        public bool? LaoDongCaNhan { get; set; }

        public DateTime? ThoiGian { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuDuyet> PhieuDuyets { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
