using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoSpeedLogistics
{
    public class QuanLyPhuongTien
    {
        private List<PhuongTien> _danhSachPT;

        public QuanLyPhuongTien()
        {
            _danhSachPT = new List<PhuongTien>();
        }

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null)
            {
                throw new ArgumentNullException("pt", "Phương tiện không được null!");
            }
            _danhSachPT.Add(pt);
        }

        public void DisplayAll()
        {
            if (_danhSachPT.Count == 0)
            {
                Console.WriteLine("Danh sách phương tiện rỗng.");
                return;
            }

            Console.WriteLine("\n=================== DANH SÁCH PHƯƠNG TIỆN AUTOSPEED ===================");
            foreach (PhuongTien pt in _danhSachPT)
            {
                Console.WriteLine(pt.GetInfo());
            }
        }

        public PhuongTien FindMaxGiaLanBanh()
        {
            if (_danhSachPT.Count == 0) return null;

            PhuongTien maxPt = _danhSachPT[0];
            foreach (PhuongTien pt in _danhSachPT)
            {
                if (pt.TinhGiaLanBanh() > maxPt.TinhGiaLanBanh())
                {
                    maxPt = pt;
                }
            }
            return maxPt;
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return new List<PhuongTien>();
            }

            return _danhSachPT
                .Where(pt => pt.TenHang.IndexOf(keyword.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
}