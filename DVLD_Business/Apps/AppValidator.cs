using DVLD.DTOs;


namespace DVLD_Business
{
    public class AppValidator
    {
        public static bool IsAppDTOEmpty(AppDTO appDTO) => (appDTO == null) ? true : false;

        public static bool IsAppObjectEmpty(App app) => (app == null) ? true : false;
    }
}
