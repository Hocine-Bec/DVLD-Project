using DVLD.DTOs;

namespace DVLD_Business
{
    public class TestAppointmentValidator
    {
        public static bool IsTestAppointmentDTOEmpty(TestsAppointmentDTO dto) => (dto == null) ? true : false;

        public static bool IsTestAppointmentObjectEmpty(TestAppointment driver) => (driver == null) ? true : false;
    }
}
