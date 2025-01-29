using DVLD.DTOs;

namespace DVLD_Business
{
    public class InternationalValidator
    {
        public static bool IsInternationalDTOEmpty(InternationalDTO dto) => (dto == null) ? true : false;

        public static bool IsInternationalObjectEmpty(International international) => (international == null)
            ? true : false;
    }

}
