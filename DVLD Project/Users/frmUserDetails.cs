using Business_Layer;
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
    public partial class frmUserDetails : Form
    {
        int _UserID = -1;

        public frmUserDetails()
        {
            InitializeComponent();
        }

        public frmUserDetails(int ID)
        {
            InitializeComponent();
            _UserID = ID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            ctrlUserDetails1.LoadData(_UserID);
        }
    }
}
