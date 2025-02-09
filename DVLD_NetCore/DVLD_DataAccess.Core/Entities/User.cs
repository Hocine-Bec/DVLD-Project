using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities
{
    public class User : BaseEntity
    {
        public required int PersonId { set; get; }
        public Person? Person { set; get; }
        public required string Username { set; get; }
        public required string Password { set; get; }
        public required bool IsActive { set; get; }
    }
}