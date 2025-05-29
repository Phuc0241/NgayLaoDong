namespace QuanLyNgayLaoDong.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SoNgayLaoDong")]
    public partial class SoNgayLaoDong
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? MSSV { get; set; }

        public int? TongSoNgay { get; set; }

        public int? Ma_phieu_xac_nhan { get; set; }

        public virtual PhieuXacNhanHoanThanh PhieuXacNhanHoanThanh { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
