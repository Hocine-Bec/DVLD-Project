using DVLD.DTOs;

namespace DVLD_Business
{
    public class DriverValidator
    {
        public static bool IsDriverDTOEmpty(DriverDTO dto) => (dto == null) ? true : false;

        public static bool IsDriverObjectEmpty(Driver driver) => (driver == null) ? true : false;
    }
}
