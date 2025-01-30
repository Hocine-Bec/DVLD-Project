using DVLD.DTOs;

namespace DVLD_Business
{
    public class TestValidator
    {
        public static bool IsTestDTOEmpty(TestsDTO dto) => (dto == null) ? true : false;

        public static bool IsTestObjectEmpty(Test test) => (test == null) ? true : false;
    }

}