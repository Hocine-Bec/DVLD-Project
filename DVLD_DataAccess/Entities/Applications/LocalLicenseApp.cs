using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities.Applications
{
    public class LocalLicenseApp : BaseEntity
    {
        public string? PersonFullName { set; get; }
        public required int LicenseTypeId { set; get; }
        public required int BaseAppId { set; get; }


        public LicenseType? LicenseType { set; get; }
        public BaseApp? BaseApp { set; get; }
    }
}
