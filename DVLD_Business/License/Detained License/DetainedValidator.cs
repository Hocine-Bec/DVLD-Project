using DVLD.DTOs;

namespace DVLD_Business
{
    public class DetainedValidator
    {
        public static bool IsDetainedDTOEmpty(DetainedDTO dto) => (dto == null) ? true : false;

        public static bool IsDetainedObjectEmpty(Detained detained) => (detained == null) ? true : false;
    }

}
