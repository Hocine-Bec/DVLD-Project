using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using DVLD.People;

namespace DVLD.Controls
{
    public partial class PersonCardConrol : UserControl
    {
        //This is for testing purpose, it will be updated later
        private PersonService _personService => new PersonService();

        private Person _person;

        public Person Person
        {
            get { return _person; } 
        }

        public int PersonID { get; set; }

        public PersonCardConrol()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int personId)
        {
            _person = _personService.Find(personId);

            if (_person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with Person ID No. = {personId.ToString()}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonInfo(string nationalNo)
        {
            _person = _personService.Find(nationalNo);
            if (_person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"No Person with National No. = {nationalNo.ToString()}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
             _FillPersonInfo();
        }

        private void _LoadPersonImage()
        {
            pbPersonImage.Image = (_person.Gender == 0) ? Resources.Male_512 : Resources.Female_512;

            if (!string.IsNullOrEmpty(_person.ImagePath))
            {
                if (File.Exists(_person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _person.ImagePath;
                }
                else
                {
                    MessageBox.Show($"Could not find this image: = {_person.ImagePath}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            lblPersonID.Text=_person.PersonID.ToString();
            lblNationalNo.Text = _person.NationalNo;
            lblFullName.Text = _person.FullName;
            lblGender.Text = _person.Gender == 0 ? "Male" : "Female";
            lblEmail.Text = _person.Email;
            lblPhone.Text = _person.Phone;
            lblDateOfBirth.Text = _person.DateOfBirth.ToShortDateString();
            lblCountry.Text= clsCountry.Find( _person.NationalityCountryID).CountryName ;
            lblAddress.Text= _person.Address;
            _LoadPersonImage();
        }

        public void ResetPersonInfo()
        {
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            pbGender.Image = Resources.Man_32;
            lblGender.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddUpdatePersonForm frm = new AddUpdatePersonForm(_person.PersonID);
            frm.ShowDialog();

            //refresh
            LoadPersonInfo(_person.PersonID);
        }

    }
}
