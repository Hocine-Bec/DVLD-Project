using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classes;
using DVLD.Properties;
using DVLD_Business;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Runtime.ConstrainedExecution;

namespace DVLD.People
{
    public partial class AddUpdatePersonForm : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int personId);

        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum Mode { AddNew = 0, Update = 1 };
        public enum Gender { Male = 0, Female = 1 };

        private Mode _mode;
        private int _personId = -1;
        private Person _person;
        private PersonService _personService;

        public AddUpdatePersonForm()
        {
            InitializeComponent();
            _personService = new PersonService();
            _person = new Person();
            _mode = Mode.AddNew;
        }

        public AddUpdatePersonForm(int personId)
        {
            InitializeComponent();
            _mode = Mode.Update;
            _personService = new PersonService();
            _person = _personService.Find(personId);
        }

        private void ResetDefaultValues()
        {

            if (_mode == Mode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _person = new Person();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            // Set default image for the person
            pbPersonImage.Image = (rbMale.Checked) ? Resources.Male_512 : Resources.Female_512;

            // Hide/show the remove link in case there is no image for the person
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            // Set the max date to 18 years from today, and set the default value the same
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            // Should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            // Set default country to Jordan
            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");

            EmptyTextBoxFields();
        }

        private void EmptyTextBoxFields()
        {
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }

        private void FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void LoadData()
        {
            if (_person == null)
            {
                MessageBox.Show($"No Person with ID [{_personId}] Exists", "Person Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            FillFormWithPersonData();

            // Hide/show the remove link in case there is no image for the person
            llRemoveImage.Visible = (_person.ImagePath != "");
        }

        private void FillFormWithPersonData()
        {
            // The following code will not be executed if the person was not found
            lblPersonID.Text = _personId.ToString();
            txtFirstName.Text = _person.FirstName;
            txtSecondName.Text = _person.SecondName;
            txtThirdName.Text = _person.ThirdName;
            txtLastName.Text = _person.LastName;
            txtNationalNo.Text = _person.NationalNo;
            dtpDateOfBirth.Value = _person.DateOfBirth;

            rbMale.Checked = (_person.Gender == 0) ? true : true;

            txtAddress.Text = _person.Address;
            txtPhone.Text = _person.Phone;
            txtEmail.Text = _person.Email;

            cbCountry.SelectedIndex = cbCountry.FindString(_person.Country.CountryName);

            //cbCountry.SelectedIndex = _person.NationalityCountryID;

            // Load person image in case it was set
            pbPersonImage.ImageLocation = (_person.ImagePath != string.Empty) ? _person.ImagePath : "";

        }

        private void AddUpdatePersonForm_Load(object sender, EventArgs e)
        {
            // This will initialize and reset the default values
            FillCountriesInComboBox();

            if (_mode == Mode.Update)
                LoadData();
            else
                ResetDefaultValues();
        }

        private bool HandlePersonImage()
        {
            /* 
             * This procedure will handle the person image,
             * it will take care of deleting the old image from the folder
             * in case the image changed. It will rename the new image with a GUID and
             * place it in the images folder.
            */

            // _person.ImagePath contains the old image, we check if it changed then we copy the new image
            if (_person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_person.ImagePath != "")
                {
                    // First, we delete the old image from the folder in case there is any
                    try
                    {
                        File.Delete(_person.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        // Log it later
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    // Then we copy the new image to the image folder after we rename it
                    string sourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
                    {
                        pbPersonImage.ImageLocation = sourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                // Here we don't continue because the form is not valid
                MessageBox.Show("Some fields are not valid! Put the mouse over the red icon(s) to see the error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!HandlePersonImage())
                return;

            FillPersonData();

            bool success = (_mode == Mode.AddNew) ? 
                _personService.AddNewPerson(_person) : _personService.UpdatePerson(_person);


            if (success)
            {
                lblPersonID.Text = _person.PersonID.ToString();
                _mode = Mode.Update;
                lblTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Trigger the event to send data back to the caller form
                DataBack?.Invoke(this, _person.PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillPersonData()
        {
            _person.FirstName = txtFirstName.Text.Trim();
            _person.SecondName = txtSecondName.Text.Trim();
            _person.ThirdName = txtThirdName.Text.Trim();
            _person.LastName = txtLastName.Text.Trim();
            _person.NationalNo = txtNationalNo.Text.Trim();
            _person.Email = txtEmail.Text.Trim();
            _person.Phone = txtPhone.Text.Trim();
            _person.Address = txtAddress.Text.Trim();
            _person.DateOfBirth = dtpDateOfBirth.Value;
            _person.NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _person.Gender = (rbMale.Checked) ? (short)Gender.Male : (short)Gender.Female;

            _person.ImagePath = !string.IsNullOrEmpty(pbPersonImage.ImageLocation)
                ? pbPersonImage.ImageLocation : "";

        }

        private void LlSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
            }
        }

        private void LlRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            pbPersonImage.Image = (rbMale.Checked) ? Resources.Male_512 : Resources.Female_512;
            llRemoveImage.Visible = false;
        }

        private void RbFemale_Click(object sender, EventArgs e)
        {
            // Change the default image to female in case there is no image set
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void RbMale_Click(object sender, EventArgs e)
        {
            // Change the default image to male in case there is no image set
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox temp = (TextBox)sender;
            if (string.IsNullOrEmpty(temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void TxtEmail_Validating(object sender, CancelEventArgs e)
        {
            // No need to validate the email in case it's empty
            if (txtEmail.Text.Trim() == "")
                return;

            // Validate email format
            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }
        }

        private void TxtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            // Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _person.NationalNo && _personService.IsPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }
    }
}


