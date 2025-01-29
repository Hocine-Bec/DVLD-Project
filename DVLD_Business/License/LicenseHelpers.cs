namespace DVLD_Business
{
    public class LicenseHelpers
    {
        public static string GetIssueReasonText(short issueReasonId)
        {
            switch ((enIssueReason)issueReasonId)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public static short GetIssueReasonId(string issueReasonText)
        {
            switch (issueReasonText)
            {
                case "First Time":
                    return 1;
                case "Renew":
                    return 2;
                case "Replacement for Damaged":
                    return 3;
                case "Replacement for Lost":
                    return 4;
                default:
                    return -1;
            }
        }
    }

    
}
