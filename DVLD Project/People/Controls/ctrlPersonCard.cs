using Business_Layer;
using clsPerson_Layer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD_Project
{
    public partial class ctrlPersonCard : UserControl
    {
        int _PersonID = -1;

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private void SetImageForGendor(int Gendor)
        {
            if (Gendor == 0)
            {
                lblGendor.Text = "Male";
                pbGendor.Image = Resources.Man_32;
                pbPersonImage.Image = Resources.Male_512;
                
            }
            else
            {
                lblGendor.Text = "Female";
                pbGendor.Image = Resources.Woman_32;
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        public void LoadData(clsPerson PersonInfo)
        {
            if (PersonInfo == null) { return; }

            _PersonID = PersonInfo.PersonID;
            lblPersonID.Text  = PersonInfo.PersonID.ToString();
            lblFullName.Text  = PersonInfo.FirstName + " " + PersonInfo.SecondName + " " + PersonInfo.ThirdName + " " + PersonInfo.LastName;
            lblNationalNo.Text  = PersonInfo.NationalNo;
            lblEmail.Text  = PersonInfo.Email;
            lblAddress.Text  = PersonInfo.Address;
            lblPhone.Text  = PersonInfo.Phone;
            lblDateOfBirth.Text  = PersonInfo.DateOfBirth.ToShortDateString();
            lblCountry.Text  = PersonInfo.NationalityCountryID.ToString();
            pbPersonImage.ImageLocation = PersonInfo.ImagePath;

            SetImageForGendor(PersonInfo.Gendor);
            llEditPersonInfo.Enabled = true;

            if (pbPersonImage.ImageLocation != "")
            {
                pbPersonImage.Load();
                pbPersonImage.BackColor = Color.Transparent;
            }
           
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frmEditPerson = new frmAddUpdatePerson(_PersonID);
            frmEditPerson.ShowDialog();

            var person = clsPerson.Find(_PersonID);
            LoadData(person);
        }
    }
}
