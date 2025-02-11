using DVLD_DataAccess.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Entities.Applications
{
    public class AppType : BaseEntity
    {
        public required string Title { set; get; }
        public required decimal Fees { set; get; }
    }
}
