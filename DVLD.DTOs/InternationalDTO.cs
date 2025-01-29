using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class InternationalDTO
    {
        public int AppId { get; set; }
        public int InternationalId { set; get; }
        public int DriverId { set; get; }
        public int IssuedUsingLicenseId { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpireDate { set; get; }
        public bool IsActive { set; get; }
        public int UserId { set; get; }

    }
}
