using DVLD.Controls;
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

namespace DVLD.People
{
    public partial class ShowPersonInfoForm : Form
    {

        public ShowPersonInfoForm(int PersonID)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        public ShowPersonInfoForm(string NationalNo)
        {
            InitializeComponent();
            ctrlPersonCard1.LoadPersonInfo(NationalNo);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
