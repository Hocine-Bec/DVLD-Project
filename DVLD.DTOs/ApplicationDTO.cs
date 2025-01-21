using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class ApplicationDTO
    {
        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public int ApplicationStatus { set; get; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }

    }
}
