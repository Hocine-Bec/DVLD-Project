using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class LicenseDTO
    {
        public int LicenseId { set; get; }
        public int AppId { set; get; }
        public int DriverId { set; get; }
        public int LicenseClassId { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public short IssueReasonId { set; get; }
        public int UserID { set; get; }
    }
}
