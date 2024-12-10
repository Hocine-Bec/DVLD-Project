using Business_Layer;
using clsPerson_Layer;
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
    public partial class frmManagePeople : Form
    {
        DataTable d1;
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void btnAddPeople_Click(object sender, EventArgs e)
        {
            var AddPerson = new frmAddUpdatePerson();
            AddPerson.ShowDialog();
            _RefreshData();
        }

        private void _RefreshData()
        {
            d1 = clsPerson.GetPeopleList();
            dgvPeople.DataSource = d1;
            lblRecordsCount.Text = dgvPeople.RowCount.ToString();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = cbFilterBy.FindString("None");
            _RefreshData();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            var Person = new frmPersonInfo(ID);
            Person.ShowDialog();
            _RefreshData();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var AddPerson = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            AddPerson.ShowDialog();
            _RefreshData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var person = clsPerson.Find((int)dgvPeople.CurrentRow.Cells[0].Value);

            if (clsUsers.IsUser(person.PersonID))
            {
                MessageBox.Show("This Person Can't Be Deleted. He Has Data Linked To It", "Error",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (person.DeletePerson())
            {
                MessageBox.Show("Person Deleted Successfully :-)", "Deleted",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("An Error Occurred. Please Try Again.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _RefreshData();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature will be implemented Soon InchaAllah :-)", "Coming Soon", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Feature will be implemented Soon InchaAllah :-)", "Coming Soon", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            if (txtFilterValue.Text != "" && cbFilterBy.Text != "None")
            {
                DataView filter = d1.DefaultView;
                filter.RowFilter = $"{cbFilterBy.Text} = '{txtFilterValue.Text}'";
                dgvPeople.DataSource = filter;
            }
            else
            {
                _RefreshData();
            }
            
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "PersonID":
                case "Phone":
                case "Gendor":
                case "Nationality":
                    {
                        // Allow only digits, backspace, and delete
                        if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                        {
                            e.Handled = true;
                        }
                        break;
                    }
            }
           
        }

        private void dgvPeople_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int ID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            var Person = new frmPersonInfo(ID);
            Person.ShowDialog();
            _RefreshData();
        }
    }
}

