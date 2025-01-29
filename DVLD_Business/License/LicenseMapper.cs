using DVLD.DTOs;

namespace DVLD_Business
{
    public class LicenseMapper
    {
        public LicenseDTO ToDTO(License license)
        {
            return new LicenseDTO()
            {
                AppId = license.AppId,
                LicenseId = license.LicenseId,
                DriverId = license.Driver.DriverID,
                LicenseClassId = license.LicenseClass.LicenseClassID,
                IssueDate = license.IssueDate,
                ExpirationDate = license.ExpirationDate,
                Notes = license.Notes,
                PaidFees = license.PaidFees,
                IsActive = license.IsActive,
                IssueReasonId = LicenseHelpers.GetIssueReasonId(license.IssueReasonText),
                UserID = license.UserID
            };
        }

        public License FromDTO(LicenseDTO dto)
        {
            return new License()
            {
                Driver = clsDriver.FindByDriverID(dto.DriverId),
                LicenseId = dto.LicenseId,
                AppId = dto.AppId,
                LicenseClass = clsLicenseClass.Find(dto.LicenseClassId),
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                Notes = dto.Notes,
                PaidFees = dto.PaidFees,
                IsActive = dto.IsActive,
                IssueReasonText = LicenseHelpers.GetIssueReasonText(dto.IssueReasonId),
                DetainedLicenseId = DetainedService.FindByLicenseID(dto.LicenseClassId).DetainID,
                UserID = dto.UserID,
                IsDetained = DetainedService.IsLicenseDetained(dto.LicenseId)
            };

        }

    }

    
}
