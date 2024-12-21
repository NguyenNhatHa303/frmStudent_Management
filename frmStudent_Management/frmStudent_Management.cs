using frmStudent_Management.Model1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frmStudent_Management
{
    public partial class frmStudent_Management : Form
    {
        private StudentContextDB context;
        private Student selectedStudent;
        public frmStudent_Management()
        {
            InitializeComponent();
           
        }
        private void FillFalcutyComboBox(List<Faculty> listFalcutys)
        {
            this.cmbKhoa.DataSource = listFalcutys;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudents)
        {
            dgvDanhSach.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = item.StudentID;
                dgvDanhSach.Rows[index].Cells[1].Value = item.FullName;
                dgvDanhSach.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvDanhSach.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void ClearForm()
        {
            txtMssv.Clear();
            txtHoten.Clear();
            cmbKhoa.SelectedIndex = -1;
            txtDiem.Clear();
        }

        private void LoadData()
        {
            try
            {
                context = new StudentContextDB();
                List<Faculty> listFalcutys = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
                FillFalcutyComboBox(listFalcutys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmStudent_Management_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                Student newStudent = new Student()
                {
                    StudentID = txtMssv.Text,
                    FullName = txtHoten.Text,
                    FacultyID = Convert.ToInt32(cmbKhoa.SelectedValue),
                    AverageScore = float.Parse(txtDiem.Text)
                };

                context.Students.Add(newStudent);
                context.SaveChanges();

                MessageBox.Show("Thêm sinh viên thành công.");
                ClearForm();
                LoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    selectedStudent.StudentID = txtMssv.Text;
                    selectedStudent.FullName = txtHoten.Text;
                    selectedStudent.FacultyID = Convert.ToInt32(cmbKhoa.SelectedValue);
                    selectedStudent.AverageScore = float.Parse(txtDiem.Text);

                    context.SaveChanges();

                    MessageBox.Show("Cập nhật thông tin sinh viên thành công.");
                    ClearForm();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.");
            }
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];
                string studentID = row.Cells[0].Value.ToString();

                selectedStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                if (selectedStudent != null)
                {

                    txtMssv.Text = selectedStudent.StudentID.ToString();
                    txtHoten.Text = selectedStudent.FullName;
                    cmbKhoa.SelectedValue = selectedStudent.FacultyID;
                    txtDiem.Text = selectedStudent.AverageScore.ToString();
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên có mã số " + selectedStudent.StudentID + " ?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(selectedStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công.");
                        ClearForm();
                        LoadData();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
