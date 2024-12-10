using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmManageUsers : Form
    {
        DataTable d1;
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void btnAddUsers_Click(object sender, EventArgs e)
        {
            var frmAddUser = new frmAddUpdateUser();
            frmAddUser.ShowDialog();
            _RefreshData();
        }

        private void _RefreshData()
        {
            d1 = clsUsers.GetUsersList();
            dgvUsers.DataSource = d1;

            dgvUsers.Columns[0].Width = 110;
            dgvUsers.Columns[1].Width = 110;
            dgvUsers.Columns[2].Width = 400;
            dgvUsers.Columns[3].Width = 110;
            dgvUsers.Columns[4].Width = 110;
            


            lblRecordsCount.Text = (dgvUsers.RowCount).ToString();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _RefreshData();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmUserDetail = new frmUserDetails((int)dgvUsers.CurrentRow.Cells[0].Value);
            frmUserDetail.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = clsUsers.FindUserWithID((int)dgvUsers.CurrentRow.Cells[0].Value);

            if (user.DeleteUser())
            {
                MessageBox.Show("User Deleted Successfully :-)", "Deleted",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("An Error Occurred. Please Try Again.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _RefreshData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmEditUser= new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            frmEditUser.ShowDialog();
            _RefreshData();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature will be implemented Soon InchaAllah :-)", "Coming Soon");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature will be implemented Soon InchaAllah :-)", "Coming Soon");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "IsActive")
            {
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
                return;
            }
            else
            {
                cbIsActive.Visible = false;
            }


            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Text = string.Empty;
                _RefreshData();
                txtFilterValue.Visible = false;
            }
            else
            {
                txtFilterValue.Visible = true;
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "PersonID":
                case "UserID":
                    {
                        if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = false;
                        }
                        break;
                    }
                default:
                    break;
            }
            
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text != "None" && txtFilterValue.Text != "")
            {
                DataView filter = d1.DefaultView;
                filter.RowFilter = $"{cbFilterBy.Text} = '{txtFilterValue.Text}'";
                dgvUsers.DataSource = filter;
            }
            else
            {
                _RefreshData();
            }
        
        }

        private void FilterActiveUsers(bool status)
        {
            DataView filter = d1.DefaultView;
            filter.RowFilter = $"{cbFilterBy.Text} = '{status}'";
            dgvUsers.DataSource = filter;
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIsActive.Text == "Yes")
            {
                FilterActiveUsers(true);
                return;
            }

            if (cbIsActive.Text == "No")
            {
                FilterActiveUsers(false);
                return;
            }

            _RefreshData();
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            var EditPassword = new frmChangePassword(ID);
            EditPassword.ShowDialog();
            _RefreshData();
        }
    }
}
