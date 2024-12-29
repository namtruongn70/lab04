using lab04.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab04
{
    public partial class frmQLK : Form
    {
        public frmQLK()
        {
            InitializeComponent();
        }

        private void frmQLK_Load(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<Faculty> listFalcultys = context.Faculties.ToList(); // Lấy các khoa
                List<Student> listStudent = context.Students.ToList(); // Lấy sinh viên
                FillFalcultyCombobox(listFalcultys);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            cmbFaculty.DataSource = listFalcultys;
            cmbFaculty.DisplayMember = "FacultyName";
            cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                var existingStudent = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (existingStudent != null)
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!double.TryParse(txtAverageScore.Text, out double averageScore))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(cmbFaculty.SelectedValue.ToString(), out int facultyID))
                {
                    MessageBox.Show("Khoa không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newStudent = new Student
                {
                    StudentID = txtStudentId.Text,
                    FullName = txtFullname.Text,
                    AverageScore = averageScore,
                    FacultyID = facultyID
                };
                context.Students.Add(newStudent);
                context.SaveChanges();

                BindGrid(context.Students.ToList());
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (student != null)
                {
                    var duplicateStudent = context.Students
                        .FirstOrDefault(s => s.StudentID == txtStudentId.Text && s.StudentID != student.StudentID);
                    if (duplicateStudent != null)
                    {
                        MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    student.FullName = txtFullname.Text;
                    student.AverageScore = double.Parse(txtAverageScore.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    context.SaveChanges();
                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();

                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Sinh viên đã được xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xoá dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvStudent.Rows[e.RowIndex];
                txtStudentId.Text = selectedRow.Cells[0].Value.ToString();
                txtFullname.Text = selectedRow.Cells[1].Value.ToString();
                cmbFaculty.SelectedValue = selectedRow.Cells[2].Value.ToString();
                txtAverageScore.Text = selectedRow.Cells[3].Value.ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {

            try
            {
                StudentContextDB context = new StudentContextDB();
                var existingStudent = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (existingStudent != null)
                {
                    MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!double.TryParse(txtAverageScore.Text, out double averageScore))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(cmbFaculty.SelectedValue.ToString(), out int facultyID))
                {
                    MessageBox.Show("Khoa không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newStudent = new Student
                {
                    StudentID = txtStudentId.Text,
                    FullName = txtFullname.Text,
                    AverageScore = averageScore,
                    FacultyID = facultyID
                };
                context.Students.Add(newStudent);
                context.SaveChanges();

                BindGrid(context.Students.ToList());
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {

            try
            {
                StudentContextDB context = new StudentContextDB();
                var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (student != null)
                {
                    var duplicateStudent = context.Students
                        .FirstOrDefault(s => s.StudentID == txtStudentId.Text && s.StudentID != student.StudentID);
                    if (duplicateStudent != null)
                    {
                        MessageBox.Show("Mã SV đã tồn tại. Vui lòng nhập một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    student.FullName = txtFullname.Text;
                    student.AverageScore = double.Parse(txtAverageScore.Text);
                    student.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());

                    context.SaveChanges();
                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Chỉnh sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                var student = context.Students.FirstOrDefault(s => s.StudentID == txtStudentId.Text);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();

                    BindGrid(context.Students.ToList());
                    MessageBox.Show("Sinh viên đã được xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sinh viên không tìm thấy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xoá dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
