using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Entities
{
    public class Test : BaseEntity
    {
        public required int TestAppointmentId { set; get; }
        public required bool TestResult { set; get; }
        public string? Notes { set; get; }
        public required int UserId { set; get; }


        public TestAppointment? TestAppointment { set; get; }
        public User? User { set; get; }
    }

}
