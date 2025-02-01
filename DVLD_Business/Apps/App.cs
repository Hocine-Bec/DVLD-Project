using System;


namespace DVLD_Business
{
    public class App
    {
        public int AppId { set; get; }
        public int PersonId { set; get; }
        public int ApplicantPersonId { set; get; }
        public string ApplicantFullName { set; get; }
        public DateTime AppDate { set; get; }
        public int AppTypeId { set; get; }
        public AppType AppType { set; get; }
        public string StatusText { set; get; }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int UserId { set; get; }
        public User CreatedByUser { set; get; }
    }
}
