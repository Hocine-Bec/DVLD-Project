using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Entities.Applications
{
    public class DetainedLicense : BaseEntity
    {
        public required int LicenseId { get; set; }
        public required DateTime DetainDate { get; set; }
        public required decimal FineFees { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserId { get; set; }
        public int ReleaseAppId { get; set; }


        public DrivingLicense? License { get; set; }
        public User? CreatedByUser { get; set; }
        public User? ReleasedByUser { get; set; }
    }

}
