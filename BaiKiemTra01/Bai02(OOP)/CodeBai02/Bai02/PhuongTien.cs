using System;

namespace AutoSpeedLogistics
{
    public abstract class PhuongTien
    {
        private string _maPT;
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;


        public string MaPT
        {
            get { return _maPT; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _maPT = "PT000"; 
                }
                else
                {
                    _maPT = value.Trim();
                }
            }
        }
     
        public string TenHang
        {
            get { return _tenHang; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên hãng không được để trống!");
                }
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get { return _namSanXuat; }
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value < 1900 || value > currentYear)
                {
                    throw new ArgumentException(string.Format("Năm sản xuất phải từ 1900 đến năm hiện tại ({0})!", currentYear));
                }
                _namSanXuat = value;
            }
        }
        public decimal GiaGoc
        {
            get { return _giaGoc; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Giá gốc bắt buộc phải lớn hơn 0!");
                }
                _giaGoc = value;
            }
        }

        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }

        public abstract decimal TinhGiaLanBanh();

        public virtual string GetInfo()
        {
            return string.Format("Mã PT: {0} | Hãng: {1} | Năm SX: {2} | Giá gốc: {3:N0} VNĐ | Giá lăn bánh: {4:N0} VNĐ",
                MaPT, TenHang, NamSanXuat, GiaGoc, TinhGiaLanBanh());
        }
    }
}