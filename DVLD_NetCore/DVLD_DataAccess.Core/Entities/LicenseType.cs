using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities
{
    public class LicenseType : BaseEntity
    {
        public required string ClassName { set; get; }
        public string? ClassDescription { set; get; }
        public required byte MinimumAllowedAge { set; get; }
        public required byte DefaultValidityLength { set; get; }
        public required decimal ClassFees { set; get; }
    }

}
