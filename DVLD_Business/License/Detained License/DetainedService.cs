using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class DetainedService
    {
        private readonly DetainedRepoService _repoService;
        private readonly DetainedMapper _mapper;

        public DetainedService()
        {
            _repoService = new DetainedRepoService();
            _mapper = new DetainedMapper();
        }

        public int Detain(float fineFees, int userId, int licenseId)
        {
            var detained = new Detained();

            detained.LicenseId = licenseId;
            detained.DetainDate = DateTime.Now;
            detained.FineFees = Convert.ToSingle(fineFees);
            detained.CreatedByUserId = userId;

            if (!this.AddNewDetainedLicense(detained))
            {
                return -1;
            }

            return detained.DetainId;
        }

        public bool AddNewDetainedLicense(Detained detained)
        {
            if (DetainedValidator.IsDetainedObjectEmpty(detained))
                return false;

            var dto = _mapper.ToDTO(detained);

            dto.DetainId = _repoService.AddNew(dto);

            return (dto.DetainId != -1);
        }

        public bool UpdateDetainedLicense(Detained detained)
        {
            if (DetainedValidator.IsDetainedObjectEmpty(detained))
                return false;

            var dto = _mapper.ToDTO(detained);

            return _repoService.Update(dto);
        }

        public Detained Find(int detainId)
        {
            var dto = _repoService.FindById(detainId);

            if (DetainedValidator.IsDetainedDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public Detained FindByLicenseID(int licenseId)
        {
            var dto = _repoService.FindByLicenseId(licenseId);

            if (DetainedValidator.IsDetainedDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public DataTable GetAllDetainedLicenses() => _repoService.GetAllDetained();

        public bool IsLicenseDetained(int licenseId) => _repoService.IsDetained(licenseId);

        public bool ReleaseDetainedLicense(int detainId, int releasedByUserId, int releaseAppId)
            => _repoService.Release(detainId, releasedByUserId, releaseAppId);

    }

}
