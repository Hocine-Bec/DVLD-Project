using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class TestsAppointmentDTO
    {
        public int TestAppointmentId { set; get; }
        public int LocalLicenseAppId { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int UserId { set; get; }
        public bool IsLocked { set; get; }
        public int TestTypeId { set; get; }
        public int RetakeTestAppId { set; get; }
        public int TestId { get; set; }
    }
}
