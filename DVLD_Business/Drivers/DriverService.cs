using DVLD.DTOs;
using DVLD_DataAccess;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace DVLD_Business
{
    public class DriverService
    {
        private readonly DriverRepoService _repoService;
        private readonly DriverMapper _mapper;
        private readonly InternationalService _internationalService;
        private readonly LicenseService _licenseService;

        public DriverService()
        {
            _repoService = new DriverRepoService();
            _mapper = new DriverMapper();
            _internationalService = new InternationalService();
            _licenseService = new LicenseService();
        }

        public int AddNewDriver(int personId, int userId)
        {
            if (personId == -1 && userId == -1)
                return -1;
            

            return _repoService.AddNew(personId, userId);
        }

        public bool UpdateDriver(Driver driver)
        {
            if (DriverValidator.IsDriverObjectEmpty(driver))
                return false;

            var dto = _mapper.ToDTO(driver);
            return _repoService.Update(dto);
        }

        public Driver FindByDriverId(int driverId)
        {
            var dto = _repoService.FindById(driverId);

            if (DriverValidator.IsDriverDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public Driver FindByPersonId(int personId)
        {
            var dto = _repoService.FindByPersonId(personId);

            if (DriverValidator.IsDriverDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public DataTable GetAllDrivers() => _repoService.GetAllDrivers();

        public DataTable GetLicenses(int driverId) => _licenseService.GetDriverLicenses(driverId);

        public DataTable GetInternationalLicenses(int driverId) => _internationalService.GetDriverInternationalLicenses(driverId);
    

    }


}

