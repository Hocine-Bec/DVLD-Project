using Business_Layer;
using clsPerson_Layer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD_Project
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        clsPerson _person;
        string OriginalImagePath;
        int _PersonID;

        enum enMode { AddNew = 1, Update = 2 };
        enMode mode;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int ID)
        {
            InitializeComponent();
            _PersonID = ID;
            mode = enMode.Update;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string _HandlePersonImage(string SourcePath)
        {
         
            if (SourcePath == null)
            {
                return "";
            }

            string NewPath = clsUtil.CopyToFolder(SourcePath);

            return NewPath;
        }

        private void lkEditPicture_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(pbPersonImage.ImageLocation != null)
            {
                OriginalImagePath = pbPersonImage.ImageLocation;
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = openFileDialog1.FileName;
                pbPersonImage.Load();
                pbPersonImage.BackColor = Color.Transparent;
                llRemoveImage.Visible = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_person == null)
            {
                _person = new clsPerson();
            }

            _person.FirstName = txtFirstName.Text;
            _person.SecondName = txtSecondName.Text;
            _person.ThirdName = txtThirdName.Text;
            _person.LastName = txtLastName.Text;
            _person.NationalNo = txtNationalNo.Text;
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.Gendor = Convert.ToChar((rbMale.Checked) ? rbMale.Tag : rbFemale.Tag);
            _person.Address = txtAddress.Text;
            _person.Phone = txtPhone.Text;
            _person.Email = txtEmail.Text;
            _person.NationalityCountryID = cbCountry.SelectedIndex;

            if (mode == enMode.Update && OriginalImagePath != null)
            {
                try
                {
                    File.Delete(OriginalImagePath);
                }
                catch
                {

                }
            }

            _person.ImagePath = _HandlePersonImage(pbPersonImage.ImageLocation);

            if (_person.Save())
            {
                MessageBox.Show("Person Saved Successfully :-)", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblTitle.Text = "Update Person";
                int ID = _person.PersonID;
                lblPersonID.Text = ID.ToString();
                DataBack?.Invoke(sender, ID);
                mode = enMode.Update;           
            }
            else
            {
                MessageBox.Show("An Error Occurred. Please Try Again.", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void lkRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OriginalImagePath = pbPersonImage.ImageLocation; 
            pbPersonImage.Image.Dispose();
            pbPersonImage.ImageLocation = "";              
            llRemoveImage.Visible = false;
        }

        public void SetImageForGender(int Gendor)
        {
            if (Gendor == 0)
            {
                rbMale.Checked = true;
                pbPersonImage.Image = Resources.Male_512;

            }
            else
            {
                rbFemale.Checked = true;
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void _LoadData()
        {
            if (_person == null) { return; }

            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _person.FirstName;
            txtSecondName.Text = _person.SecondName;
            txtThirdName.Text = _person.ThirdName;
            txtLastName.Text = _person.LastName;
            txtNationalNo.Text = _person.NationalNo;
            dtpDateOfBirth.Value = _person.DateOfBirth;

            SetImageForGender(_person.Gendor);

            txtAddress.Text = _person.Address;
            txtPhone.Text = _person.Phone;
            txtEmail.Text = _person.Email;
            cbCountry.SelectedIndex = _person.NationalityCountryID;

            if (_person.ImagePath != "")
            {
                pbPersonImage.Load(_person.ImagePath);
                llRemoveImage.Visible = true;
            }
        }

        private void _FillCountriesComboBox()
        {
            DataTable d1 = clsPerson.GetCountriesList();

            foreach (DataRow d in d1.Rows)
            {
                cbCountry.Items.Add(d["CountryName"]);
            }

            cbCountry.SelectedIndex = cbCountry.FindString("Algeria");
        }

        private void frmAddPerson_Load(object sender, EventArgs e)
        {
            _FillCountriesComboBox();
            if (mode == enMode.Update)
            {
                lblTitle.Text = "Update Person";
                _person = clsPerson.Find(_PersonID);
                _LoadData();
            }
        }
        
        private void SetError(string msg, CancelEventArgs e, bool ShowError)
        {
            if (ShowError)
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, msg);
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, msg);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                SetError("This Field is Required", e, true);
                return;
            }
            else
            {
                SetError("", e, false);
            }


            if (clsPerson.IsPersonExist(txtNationalNo.Text))
            {
                SetError("This Person Exists", e, true);
                return;
            }
            else
            {
                SetError("", e, false);
            }

        }

        private void dtpDateOfBirth_Enter(object sender, EventArgs e)
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = dtpDateOfBirth.MaxDate.AddYears(-100);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!txtEmail.Text.Contains("@"))
            {
                errorProvider1.SetError(txtEmail, "This is not a valid email");
            }
            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "This is not a valid phone number");
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }
            
        }

        private void rbPicture_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
            {
                if (rbMale.Checked && pbPersonImage.Image != Resources.Male_512)
                {
                    pbPersonImage.Image = Resources.Male_512;
                    return;
                }

                if (rbFemale.Checked && pbPersonImage.Image != Resources.Female_512)
                {
                    pbPersonImage.Image = Resources.Female_512;
                    return;
                }
            }
            
        }

      
    }
}
