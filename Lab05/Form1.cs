using Lab05.BLL;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab05
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtStudentID.Text == "" || txtStudentName.Text == "" || cmbFaculty.SelectedIndex == -1 || txtAverageScore.Text == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            var student = new Student()
            {
                StudentID = txtStudentID.Text.Trim(),
                FullName = txtStudentName.Text.Trim(),
                AverageScore = float.Parse(txtAverageScore.Text),
                FacultyID = (int)cmbFaculty.SelectedValue,
            };

            // 2️⃣ Kiểm tra sinh viên có tồn tại không
            StudentService studentService = new StudentService();
            var existingStudent = studentService.GetAll()
                                            .FirstOrDefault(s => s.StudentID == student.StudentID);

            // 3️⃣ Nếu chưa có → thêm mới, nếu có rồi → cập nhật
            if (existingStudent == null)
            {
                studentService.AddStudent(student);
                MessageBox.Show("Đã thêm sinh viên mới!");
            }
            else
            {
                studentService.UpdateStudent(student);
                MessageBox.Show("Đã cập nhật thông tin sinh viên!");
            }

            // 4️⃣ Refresh lại lưới
            RefreshGrid();
        }
        private void RefreshGrid()
        {
            var list = studentService.GetAll();

            dgvStudent.Rows.Clear();
            var listFacultys = facultyService.GetAll();
            var listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            string id = txtStudentID.Text.Trim();

            if (dgvStudent.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Bạn có chắc muốn xóa sinh viên này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            StudentService studentService = new StudentService();
            if (dr == DialogResult.Yes)
            {
                studentService.DeleteStudent(id);
                MessageBox.Show("Đã xóa sinh viên thành công.");

                // Cập nhật lại DataGridView
                RefreshGrid();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name + "";
                ShowAvatar(item.Avatar);
            }
        }
        private void ShowAvatar(string ImageName)
        {
            if (string.IsNullOrEmpty(ImageName))
            {
                picAvatar.Image = null;
            }
            else
            {
                string parentDirectory =
                    Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", ImageName + ".jpg");
                if (File.Exists(imagePath))
                {
                    using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        picAvatar.Image = Image.FromStream(fs);
                    }
                    picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    picAvatar.Image = null;
                }
                picAvatar.Refresh();
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ckbox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.ckbox1.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        }
        string selectedImagePath = "";
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = ofd.FileName;
                picAvatar.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvStudent.Rows[e.RowIndex];
            txtStudentID.Text = row.Cells[0].Value.ToString();
            txtStudentName.Text = row.Cells[1].Value.ToString();
            cmbFaculty.Text = row.Cells[2].Value.ToString();
            txtAverageScore.Text = row.Cells[3].Value.ToString();
            string studentID = txtStudentID.Text.Trim();
            ShowAvatar(studentID);
        }
    }
}

