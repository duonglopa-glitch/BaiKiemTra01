using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMartProductManager;

namespace Bai03
{
    public partial class Form1 : Form
    {
        private BindingList<Product> _productList;
        private BindingSource _bindingSource;
        private List<Category> _categoryList;
        private string _selectedImagePath = "";
        public Form1()
        {
            InitializeComponent();
            InitCustomComponents();
            LoadCategoryData();
            InitDataBinding();
            SetupDataGridViewColumns();
            UpdateStatusCount();
        }
        private void InitCustomComponents()
        {
            // Cấu hình phím tắt Menu Strip (Ctrl+E, Ctrl+X)
            exportCSVCtrlEToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            exitCtrlXToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;

            // Cấu hình PictureBox
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;

            // Cấu hình ErrorProvider
            errorProvider1.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
        }
        private void LoadCategoryData()
        {
            _categoryList = new List<Category>
            {
                new Category(1, "Điện thoại"),
                new Category(2, "Laptop"),
                new Category(3, "Phụ kiện")
            };

            cboCategory.DataSource = _categoryList;
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Id";
        }
        private void InitDataBinding()
        {
            _productList = new BindingList<Product>
            {
                new Product("SP001", "Laptop Dell XPS 13", 2, "Laptop", 25000000m, 10, ""),
                new Product("SP002", "iPhone 15 Pro", 1, "Điện thoại", 28000000m, 15, ""),
                new Product("SP003", "Tai nghe AirPods Pro", 3, "Phụ kiện", 5500000m, 30, "")
            };

            _bindingSource = new BindingSource();
            _bindingSource.DataSource = _productList;

            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.DataSource = _bindingSource;
        }
        private void SetupDataGridViewColumns()
        {
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã SP",
                Width = 80
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên Sản Phẩm",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CategoryName",
                HeaderText = "Danh Mục",
                Width = 110
            });

            DataGridViewTextBoxColumn colPrice = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice",
                HeaderText = "Đơn Giá (VNĐ)",
                Width = 120
            };
            colPrice.DefaultCellStyle.Format = "N0";
            colPrice.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProducts.Columns.Add(colPrice);

            DataGridViewTextBoxColumn colQty = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Số Lượng",
                Width = 90
            };
            colQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns.Add(colQty);
        }


        #region 2. Kiểm tra Dữ liệu (Validation)

        private bool ValidateInputs()
        {
            bool isValid = true;
            errorProvider1.Clear();


            if (string.IsNullOrWhiteSpace(txtProductId.Text))
            {
                errorProvider1.SetError(txtProductId, "Mã sản phẩm không được để trống!");
                isValid = false;
            }


            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                isValid = false;
            }


            decimal price;
            if (!decimal.TryParse(txtUnitPrice.Text, out price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0!");
                isValid = false;
            }


            int qty;
            if (!int.TryParse(txtQuantity.Text, out qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên lớn hơn hoặc bằng 0!");
                isValid = false;
            }

            return isValid;
        }

        #endregion
        #region 3. Các Chức năng Hành động (Add, Update, Delete, Select)


        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem != null)
            {
                Product selectedProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;

                txtProductId.Text = selectedProduct.ProductId;
                txtProductName.Text = selectedProduct.ProductName;
                cboCategory.SelectedValue = selectedProduct.CategoryId;
                txtUnitPrice.Text = selectedProduct.UnitPrice.ToString("G0");
                txtQuantity.Text = selectedProduct.Quantity.ToString();

                _selectedImagePath = selectedProduct.ImagePath;
                if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    picAvatar.Image = Image.FromFile(_selectedImagePath);
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }
        private void txtProductId_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                _bindingSource.DataSource = _productList;
            }
            else
            {
                var filtered = _productList
                    .Where(p => p.ProductName.ToLower().Contains(keyword) || p.ProductId.ToLower().Contains(keyword))
                    .ToList();
                _bindingSource.DataSource = new BindingList<Product>(filtered);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategoryData();
            InitDataBinding();
            SetupDataGridViewColumns();
            UpdateStatusCount();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện sản phẩm";
                ofd.Filter = "File Ảnh (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picAvatar.Image = Image.FromFile(_selectedImagePath);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string id = txtProductId.Text.Trim();
            if (_productList.Any(p => p.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Category cat = (Category)cboCategory.SelectedItem;

            Product newProd = new Product(
                id,
                txtProductName.Text.Trim(),
                cat.Id,
                cat.Name,
                decimal.Parse(txtUnitPrice.Text),
                int.Parse(txtQuantity.Text),
                _selectedImagePath
            );

            _productList.Add(newProd);
            UpdateStatusCount();
            ClearInputs();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            if (!ValidateInputs()) return;

            Product currentProd = (Product)dgvProducts.CurrentRow.DataBoundItem;
            Category cat = (Category)cboCategory.SelectedItem;

            currentProd.ProductName = txtProductName.Text.Trim();
            currentProd.CategoryId = cat.Id;
            currentProd.CategoryName = cat.Name;
            currentProd.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            currentProd.Quantity = int.Parse(txtQuantity.Text);
            currentProd.ImagePath = _selectedImagePath;

            _bindingSource.ResetBindings(false);
            MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            Product currentProd = (Product)dgvProducts.CurrentRow.DataBoundItem;

            DialogResult dr = MessageBox.Show(
                string.Format("Bạn có chắc chắn muốn xóa sản phẩm \"{0}\" không?", currentProd.ProductName),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr == DialogResult.Yes)
            {
                _productList.Remove(currentProd);
                UpdateStatusCount();
                ClearInputs();
                MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void UpdateStatusCount()
        {
            lblStatus.Text = string.Format("Tổng số sản phẩm: {0}", _productList.Count);
        }

        private void ClearInputs()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            if (cboCategory.Items.Count > 0) cboCategory.SelectedIndex = 0;
            picAvatar.Image = null;
            _selectedImagePath = "";
            errorProvider1.Clear();
        }

        #endregion

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void exportCSVCtrlEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToCSV();
        }

        private void exitCtrlXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ExportToCSV()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất danh mục sản phẩm ra file CSV";
                sfd.Filter = "CSV File (*.csv)|*.csv";
                sfd.FileName = "DanhSachSanPham_TechMart.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        // Ghi Header
                        sb.AppendLine("MaSP,TenSanPham,DanhMuc,DonGia,SoLuong");

                        // Ghi Rows
                        foreach (Product p in _productList)
                        {
                            sb.AppendLine(string.Format("\"{0}\",\"{1}\",\"{2}\",{3},{4}",
                                p.ProductId, p.ProductName, p.CategoryName, p.UnitPrice, p.Quantity));
                        }

                        // Xuất File hỗ trợ hiển thị Tiếng Việt UTF-8 (BOM)
                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Xuất dữ liệu ra file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            string id = txtProductId.Text.Trim();
            if (_productList.Any(p => p.ProductId.Equals(id, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Category cat = (Category)cboCategory.SelectedItem;

            Product newProd = new Product(
                id,
                txtProductName.Text.Trim(),
                cat.Id,
                cat.Name,
                decimal.Parse(txtUnitPrice.Text),
                int.Parse(txtQuantity.Text),
                _selectedImagePath
            );

            _productList.Add(newProd);
            UpdateStatusCount();
            ClearInputs();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            if (!ValidateInputs()) return;

            Product currentProd = (Product)dgvProducts.CurrentRow.DataBoundItem;
            Category cat = (Category)cboCategory.SelectedItem;

            currentProd.ProductName = txtProductName.Text.Trim();
            currentProd.CategoryId = cat.Id;
            currentProd.CategoryName = cat.Name;
            currentProd.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            currentProd.Quantity = int.Parse(txtQuantity.Text);
            currentProd.ImagePath = _selectedImagePath;

            _bindingSource.ResetBindings(false);
            MessageBox.Show("Cập nhật thông tin sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            Product currentProd = (Product)dgvProducts.CurrentRow.DataBoundItem;

            DialogResult dr = MessageBox.Show(
                string.Format("Bạn có chắc chắn muốn xóa sản phẩm \"{0}\" không?", currentProd.ProductName),
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr == DialogResult.Yes)
            {
                _productList.Remove(currentProd);
                UpdateStatusCount();
                ClearInputs();
                MessageBox.Show("Đã xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void picAvatar_Click(object sender, EventArgs e)
        {

        }

        private void btnChooseImage_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh đại diện sản phẩm";
                ofd.Filter = "File Ảnh (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picAvatar.Image = Image.FromFile(_selectedImagePath);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem != null)
            {
                Product selectedProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;

                txtProductId.Text = selectedProduct.ProductId;
                txtProductName.Text = selectedProduct.ProductName;
                cboCategory.SelectedValue = selectedProduct.CategoryId;
                txtUnitPrice.Text = selectedProduct.UnitPrice.ToString("G0");
                txtQuantity.Text = selectedProduct.Quantity.ToString();

                _selectedImagePath = selectedProduct.ImagePath;
                if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    picAvatar.Image = Image.FromFile(_selectedImagePath);
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }
    }
}