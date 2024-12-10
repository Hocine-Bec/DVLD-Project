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
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadData(int ID)
        {
            clsUsers User = clsUsers.FindUserWithID(ID);

            ctrlPersonCard1.LoadData(User.Person);

            lbUserID.Text = User.UserID.ToString();
            lbUsername.Text = User.Username;
            lbIsActive.Text = (User.IsActive) ? "Yes" : "No";
        }

        private void lkEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int ID = Convert.ToInt32(lbUserID.Text);

            var person = new frmAddUpdatePerson(ID);

            person.ShowDialog();
        }
    }
}
