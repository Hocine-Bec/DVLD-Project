using System;

namespace DVLD_Business
{
    public class TestAppointment
    {
        public int TestAppointmentId { set; get; }
        public enTestType TestTypeId { set; get; }
        public int LocalLicenseAppId { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int UserId { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestAppId { set; get; }
        public App RetakeTestApp { set; get; }
        public int TestId { get; set; } 
    }
}
