using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities.Identity
{
    public class Driver : BaseEntity
    {
        public required int PersonId { set; get; }
        public required int UserId { set; get; }
        public required DateTime CreatedDate { set; get; }

        public User? User { get; set; }
        public Person? Person { get; set; }
    }

}
