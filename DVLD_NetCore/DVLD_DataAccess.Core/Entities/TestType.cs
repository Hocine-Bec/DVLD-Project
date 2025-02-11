using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities
{
    public class TestType : BaseEntity
    {
        public required string Title { set; get; }
        public string? Description { set; get; }
        public required decimal Fees { set; get; }
    }

}
