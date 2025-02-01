using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.DTOs
{
    public class TestsDTO
    {
        public int TestId { set; get; }
        public int TestAppointmentId { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int UserId { set; get; }
    }
}
