using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;
using DVLD_DataAccess.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Entities.Applications
{
    public class BaseApp : BaseEntity
    {
        public required int PersonId { set; get; }
        public required DateTime AppDate { set; get; }
        public required int AppTypeId { set; get; }
        public required AppStatus Status { set; get; }
        public DateTime? LastStatusDate { get; set; }
        public required decimal PaidFees { set; get; }
        public required int UserId { set; get; }


        public Person? Person { get; set; }
        public AppType? AppType { get; set; }
        public User? User { get; set; }
    }
}
