using System;

namespace DVLD_Business
{
    public class License
    {
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int LicenseId { set; get; }
        public int AppId { set; get; }
        public LicenseClass LicenseClass { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public string IssueReasonText { set; get; }
        public int UserID { set; get; }
        public int DetainedId { set; get; }
        public bool IsDetained { set; get; }
    }

    
}


