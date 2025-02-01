namespace DVLD_Business
{
    public class AppHelpers
    {
        public static string SetStatusText(int statusId)
        {
            switch ((enAppStatus)statusId)
            {
                case enAppStatus.New:
                    return "New";
                case enAppStatus.Cancelled:
                    return "Cancelled";
                case enAppStatus.Completed:
                    return "Completed";
                default:
                    return "Unknown";
            }
        }

        public static short SetStatusId(string status)
        {
            switch (status)
            {
                case "New":
                    return 1;
                case "Cancelled":
                    return 2;
                case "Completed":
                    return 3;
                default:
                    return -1;
            }
        }

    }
}
