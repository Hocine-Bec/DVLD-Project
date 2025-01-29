using System;

namespace DVLD_Business
{
    public class International
    {
        public clsDriver Driver { get; set; }
        public App App { get; set; }
        public int InternationalId { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLicenseId { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpireDate { set; get; }
        public bool IsActive { set; get; }
        public int UserId { set; get; }
    }

}
