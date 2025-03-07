using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;
using DVLD_DataAccess.Core.Enums;

namespace DVLD_DataAccess.Core.Entities
{
    public class DrivingLicense : BaseEntity
    {
        public required int BaseAppId { set; get; }
        public required int DriverId { set; get; }
        public required int LicenseTypeId { set; get; }
        public required DateTime IssueDate { set; get; }
        public required DateTime ExpirationDate { set; get; }
        public string? Notes { set; get; }
        public required decimal PaidFees { set; get; }
        public required bool IsActive { set; get; }
        public required IssueReason IssueReason { set; get; }
        public required int UserId { set; get; }


        public BaseApp? BaseApp { set; get; }
        public Driver? Driver { set; get; }
        public User? User { set; get; }
        public LicenseType? LicenseType { set; get; }
    }

}
