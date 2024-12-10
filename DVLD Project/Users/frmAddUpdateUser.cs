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
    public partial class frmAddUpdateUser : Form
    {
        clsUsers _user;
        int _userID = -1;
        int _PersonID = -1;

        enum enMode { AddNew = 1, Update = 2 };
        enMode mode;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int ID)
        {
            InitializeComponent();
            _userID = ID;
            mode = enMode.Update;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_PersonID == -1)
            {
                MessageBox.Show("Please Enter Person Info First", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mode == enMode.Update)
            {
                tcUserInfo.SelectedTab = tpLoginInfo;
                return;
            }
            
            if (!clsUsers.IsUser(_PersonID) && mode == enMode.AddNew)
            {
                tcUserInfo.SelectedTab = tpLoginInfo;
            }
            else
            {
                MessageBox.Show("This person is already a user", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _LoadData()
        {
            _user = clsUsers.FindUserWithID(_userID);
            ctrlPersonCardWithFilter1.LoadUserData(_user);
            _PersonID = _user.Person.PersonID;
            lblUserID.Text = _user.UserID.ToString();
            txtUserName.Text = _user.Username;
            txtPassword.Text = _user.Password;
            txtConfirmPassword.Text = _user.Password;
            chkIsActive.Checked = _user.IsActive;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_user == null)
            {
                _user = new clsUsers();
            }

            _user.Username = txtUserName.Text;
            _user.Password = txtPassword.Text;
            _user.PersonID = _PersonID;
            _user.IsActive = chkIsActive.Checked;
           

            if (_user.Save())
            {
                MessageBox.Show("User Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTitle.Text = "Update User";
                lblUserID.Text = _user.UserID.ToString();
            }
            else
            {
                MessageBox.Show("Error Occurred! Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrlPersonDatailsWithFilter1_OnPersonSelection(int obj)
        {
            _PersonID = obj;
        }


        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            if (mode == enMode.Update)
            {
                lblTitle.Text = "Update User";
                _LoadData();
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Password Doesn't match");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }

       
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                txtUserName.Focus();
                errorProvider1.SetError(txtUserName, "This Field Is Required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "This Field Is Required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

    }
}
