using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Entities.Applications
{
    public class InternationalLicense : BaseEntity
    { 
        public required int BaseAppId { get; set; }
        public required int DriverId { set; get; }
        public required int IssuedUsingLicenseId { set; get; }
        public required DateTime IssueDate { set; get; }
        public required DateTime ExpireDate { set; get; }
        public required bool IsActive { set; get; }
        public required int UserId { set; get; }


        public DrivingLicense? License { set; get; }
        public User? User { set; get; }
        public BaseApp? BaseApp { set; get; }
        public Driver? Driver { set; get; }
    }

}
