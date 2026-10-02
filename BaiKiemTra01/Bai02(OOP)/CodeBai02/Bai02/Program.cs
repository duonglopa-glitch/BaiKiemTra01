using System;
using System.Collections.Generic;
using System.Text;

namespace AutoSpeedLogistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            int choice = -1;

            do
            {
                Console.Clear();
                Console.WriteLine("=================================================================");
                Console.WriteLine("        HỆ THỐNG KIỂM THỬ TỰ ĐỘNG (MENU) - AUTOSPEED LOGISTICS   ");
                Console.WriteLine("=================================================================");
                Console.WriteLine(" 1. [TC01] Kiểm tra Validation Năm sản xuất (Ném ngoại lệ)");
                Console.WriteLine(" 2. [TC02] Kiểm tra Tính Giá Lăn Bánh Ô tô (Thuế 12% & 30%)");
                Console.WriteLine(" 3. [TC03] Kiểm tra Tính Giá Lăn Bánh Xe máy (Thuế 2%)");
                Console.WriteLine(" 4. [TC04] Kiểm tra Đa hình trong danh sách List<PhuongTien>");
                Console.WriteLine(" 5. [TC05] Kiểm tra Tìm Phương Tiện có Giá Lăn Bánh Max");
                Console.WriteLine(" 6. [ALL]  Chạy TOÀN BỘ 5 Kịch bản Kiểm thử");
                Console.WriteLine(" 0. Thoát chương trình");
                Console.WriteLine("=================================================================");
                Console.Write("Mời bạn chọn kịch bản kiểm thử (0-6): ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out choice))
                {
                    Console.WriteLine("\n-----------------------------------------------------------------");
                    switch (choice)
                    {
                        case 1:
                            RunTC01();
                            break;
                        case 2:
                            RunTC02();
                            break;
                        case 3:
                            RunTC03();
                            break;
                        case 4:
                            RunTC04();
                            break;
                        case 5:
                            RunTC05();
                            break;
                        case 6:
                            Console.WriteLine(">>> THỰC THI TOÀN BỘ KỊCH BẢN KIỂM THỬ:\n");
                            RunTC01();
                            RunTC02();
                            RunTC03();
                            RunTC04();
                            RunTC05();
                            break;
                        case 0:
                            Console.WriteLine("Đã thoát chương trình kiểm thử AutoSpeed Logistics.");
                            break;
                        default:
                            Console.WriteLine("[LỖI] Lựa chọn không hợp lệ! Vui lòng nhập từ 0 đến 6.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n[LỖI] Dữ liệu nhập vào phải là số nguyên!");
                }

                if (choice != 0)
                {
                    Console.WriteLine("\nNhấn phím bất kỳ để quay lại Menu chính...");
                    Console.ReadKey();
                }

            } while (choice != 0);
        }

        #region Kịch bản kiểm thử chi tiết (TC01 -> TC05)

        static void RunTC01()
        {
            Console.WriteLine("--- [TC01] Validation Năm sản xuất (< 1900) ---");
            try
            {
                OTo otoInvalid = new OTo("OT001", "Toyota", 1850, 1000000000m, 5, 2.0);

                Console.WriteLine("   -> KẾT QUẢ: FAILED (Không ném ngoại lệ ArgumentException như kỳ vọng)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("   -> KẾT QUẢ: PASSED");
                Console.WriteLine("   -> Ngoại lệ bắt được: \"{0}\"", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("   -> KẾT QUẢ: FAILED (Sai loại ngoại lệ: {0})", ex.GetType().Name);
            }
            Console.WriteLine();
        }

        static void RunTC02()
        {
            Console.WriteLine("--- [TC02] Tính Giá Lăn Bánh Ô tô (5 chỗ, Giá gốc: 1 tỷ VNĐ) ---");

            OTo oto = new OTo("OT002", "Toyota Camry", 2023, 1000000000m, 5, 2.5);
            decimal expected = 1420000000m; 
            decimal actual = oto.TinhGiaLanBanh();

            if (actual == expected)
            {
                Console.WriteLine("   -> KẾT QUẢ: PASSED");
                Console.WriteLine("   -> Giá lăn bánh thực tế: {0:N0} VNĐ (Đúng kỳ vọng 1.42 tỷ)", actual);
            }
            else
            {
                Console.WriteLine("   -> KẾT QUẢ: FAILED (Kỳ vọng: {0:N0}, Thực tế: {1:N0})", expected, actual);
            }
            Console.WriteLine();
        }

        static void RunTC03()
        {
            Console.WriteLine("--- [TC03] Tính Giá Lăn Bánh Xe máy (150cc, Giá gốc: 50 triệu VNĐ) ---");

            XeMay xemay = new XeMay("XM001", "Honda Winner", 2023, 50000000m, 150);
            decimal expected = 51000000m; 
            decimal actual = xemay.TinhGiaLanBanh();

            if (actual == expected)
            {
                Console.WriteLine("   -> KẾT QUẢ: PASSED");
                Console.WriteLine("   -> Giá lăn bánh thực tế: {0:N0} VNĐ (Đúng kỳ vọng 51 triệu)", actual);
            }
            else
            {
                Console.WriteLine("   -> KẾT QUẢ: FAILED (Kỳ vọng: {0:N0}, Thực tế: {1:N0})", expected, actual);
            }
            Console.WriteLine();
        }

        static void RunTC04()
        {
            Console.WriteLine("--- [TC04] Kiểm tra Đa hình trong List<PhuongTien> ---");

            List<PhuongTien> ds = new List<PhuongTien>();
            ds.Add(new OTo("OT001", "Toyota Camry", 2023, 1000000000m, 5, 2.5)); 
            ds.Add(new XeMay("XM001", "Honda Winner", 2023, 50000000m, 150));    

            bool isOToCorrect = ds[0].TinhGiaLanBanh() == 1420000000m;
            bool isXeMayCorrect = ds[1].TinhGiaLanBanh() == 51000000m;

            if (isOToCorrect && isXeMayCorrect)
            {
                Console.WriteLine("   -> KẾT QUẢ: PASSED");
                Console.WriteLine("   -> Vòng lặp kích hoạt chính xác thuật toán thuế riêng của Ô tô ({0:N0} VNĐ) và Xe máy ({1:N0} VNĐ).",
                    ds[0].TinhGiaLanBanh(), ds[1].TinhGiaLanBanh());
            }
            else
            {
                Console.WriteLine("   -> KẾT QUẢ: FAILED (Đa hình không kích hoạt đúng phương thức overridden)");
            }
            Console.WriteLine();
        }

        
        static void RunTC05()
        {
            Console.WriteLine("--- [TC05] Tìm Phương Tiện Có Giá Lăn Bánh Max ---");

            QuanLyPhuongTien ql = new QuanLyPhuongTien();
            ql.AddPhuongTien(new OTo("OT001", "Toyota Camry", 2023, 1000000000m, 5, 2.5)); 
            ql.AddPhuongTien(new XeMay("XM001", "Honda SH", 2023, 150000000m, 150));      

            PhuongTien maxPt = ql.FindMaxGiaLanBanh();

            if (maxPt != null && maxPt.MaPT == "OT001" && maxPt.TinhGiaLanBanh() == 1420000000m)
            {
                Console.WriteLine("   -> KẾT QUẢ: PASSED");
                Console.WriteLine("   -> Phương tiện Max tìm được: {0} ({1}) - Giá lăn bánh: {2:N0} VNĐ",
                    maxPt.TenHang, maxPt.MaPT, maxPt.TinhGiaLanBanh());
            }
            else
            {
                Console.WriteLine("   -> KẾT QUẢ: FAILED (Trả về sai đối tượng Max)");
            }
            Console.WriteLine();
        }

        #endregion
    }
}