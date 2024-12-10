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
    public partial class frmPersonInfo : Form
    {
        clsPerson _person;
        int _PersonID;

        public frmPersonInfo(int ID)
        {
            InitializeComponent();
            _PersonID = ID;
        }

        public frmPersonInfo()
        {
            InitializeComponent();
        }

        private void RetrievePersonDetails()
        {
            _person = clsPerson.Find(_PersonID);

            if (_person != null)
            {
                ucPerson.LoadData(_person);
            }
            else
            {
                MessageBox.Show($"This Person With ID {_PersonID} was not found :-(", "Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmPersonInfo_Load(object sender, EventArgs e)
        {
            RetrievePersonDetails();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
