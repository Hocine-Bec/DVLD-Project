using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Entities
{
    public class TestAppointment : BaseEntity
    {
        public required int LocalLicenseAppId { set; get; }
        public required DateTime AppointmentDate { set; get; }
        public required decimal PaidFees { set; get; }
        public required int UserId { set; get; }
        public required bool IsLocked { set; get; }
        public required int TestTypeId { set; get; }
        public int RetakeTestAppId { set; get; }
        public required int TestId { get; set; }


        public LocalLicenseApp? LocalLicense { set; get; }
        public User? User { set; get; }
        public Test? Test { set; get; }
        public BaseApp? RetakeTestApp { set; get; }
        public TestType? TestType { set; get; }
    }

}
