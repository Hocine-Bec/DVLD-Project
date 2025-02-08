using DVLD_DataAccess.Core.Entities.Base;

namespace DVLD_DataAccess.Core.Entities
{
    public class Country : BaseEntity
    {
        public required string CountryName { set; get; }
    }
}
