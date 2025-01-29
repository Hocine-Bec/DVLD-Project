using System;

namespace DVLD_Business
{
    public class Detained
    {
        public int DetainId { set; get; }
        public int LicenseId { set; get; }
        public DateTime DetainDate { set; get; }
        public float FineFees { set; get; }
        public int CreatedByUserId { set; get; }
        public User CreatedByUser { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserId { set; get; }
        public User ReleasedByUser { set; get; }
        public int ReleaseAppId { set; get; }
    }

}
