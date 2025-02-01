using DVLD.DTOs;

namespace DVLD_Business
{
    public class LicenseMapper
    {
        private readonly LicenseTypeService _licenseClassService;
        private readonly DetainedService _detainedService;
        private readonly DriverService _driverService;

        public LicenseMapper()
        {
            _licenseClassService = new LicenseTypeService();
            _detainedService = new DetainedService();
            _driverService = new DriverService();
        }

        public LicenseDTO ToDTO(License license)
        {
            return new LicenseDTO()
            {
                AppId = license.AppId,
                LicenseId = license.LicenseId,
                DriverId = license.DriverId,
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
                Driver = _driverService.FindByDriverId(dto.DriverId),
                LicenseId = dto.LicenseId,
                AppId = dto.AppId,
                LicenseClass = _licenseClassService.Find(dto.LicenseClassId),
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                Notes = dto.Notes,
                PaidFees = dto.PaidFees,
                IsActive = dto.IsActive,
                IssueReasonText = LicenseHelpers.GetIssueReasonText(dto.IssueReasonId),
                DetainedId = _detainedService.FindByLicenseID(dto.LicenseClassId).DetainId,
                UserID = dto.UserID,
                IsDetained = _detainedService.IsLicenseDetained(dto.LicenseId)
            };

        }

    }

    
}
