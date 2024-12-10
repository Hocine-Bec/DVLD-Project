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
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public delegate void SendDataBack(object sender, int PersonID);
        public event SendDataBack DataBack;

        int _PersonID = -1;

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelection;
        // Create a protected method to raise the event with a parameter
        protected virtual void OnPersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelection;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        public void LoadPersonData(clsPerson person)
        {
            if (person != null)
            { 
                ctrlPersonCard1.LoadData(person);

                if (OnPersonSelection != null)
                    // Raise the event with a parameter
                    OnPersonSelected(person.PersonID);
            }
            else
            {
                MessageBox.Show("This person Was not Found", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadUserData(clsUsers user)
        {
            if (user != null)
            {
                //These sets the filter to personID
                cbFilterBy.SelectedIndex = 1;

                //Sets the value of the personID
                txtFilterValue.Text = user.Person.PersonID.ToString();

                //And Disable Filter
                cbFilterBy.Enabled = false;
                txtFilterValue.Enabled = false;

                //And Disable Buttons
                btnAddNewPerson.Enabled = false;
                btnFind.Enabled = false;


                //This prints the user personal info
                ctrlPersonCard1.LoadData(user.Person);
            }
            else
            {
                MessageBox.Show("This user Was not Found", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var person = new clsPerson();

            switch (cbFilterBy.Text)
            {
                case "NationalNo":
                    person = clsPerson.Find(txtFilterValue.Text);
                    break;
                case "PersonID":
                    person = clsPerson.Find(Convert.ToInt32(txtFilterValue.Text));
                    break;
                default:
                    txtFilterValue.Text = string.Empty;
                    person = null;
                    break;
            }

            LoadPersonData(person);
        }

        private void _AddPerson(object sender, int PersonID)
        {
            var person = clsPerson.Find(PersonID);

            LoadPersonData(person);

            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();

            if (OnPersonSelection != null)
                // Raise the event with a parameter
                OnPersonSelected(PersonID);
        }

        private void _AddUser(int ID)
        {
            var user = clsUsers.FindUserWithPersonID(ID);
            LoadUserData(user);

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            var frmPerson = new frmAddUpdatePerson();
            frmPerson.DataBack += _AddPerson;
            frmPerson.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "PersonID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
    }
}
