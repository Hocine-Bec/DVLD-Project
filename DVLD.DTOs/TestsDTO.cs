using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class TestsDTO
    {
        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }
    }
}
