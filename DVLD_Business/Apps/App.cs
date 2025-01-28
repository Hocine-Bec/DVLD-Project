using System;


namespace DVLD_Business
{
    public class App
    {
        public int AppId { set; get; }
        public int ApplicantPersonId { set; get; }
        public string ApplicantFullName { set; get; }
        public DateTime AppDate { set; get; }
        public AppType AppType { set; get; }
        public string StatusText { set; get; }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public User CreatedByUser { set; get; }
    }
}
