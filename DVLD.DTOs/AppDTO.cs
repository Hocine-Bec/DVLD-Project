using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class AppDTO
    {
        public int AppId { set; get; }
        public int ApplicantPersonId { set; get; }
        public DateTime AppDate { set; get; }
        public int AppTypeId { set; get; }
        public short StatusId { set; get; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { set; get; }
        public int UserID { set; get; }

    }
}
