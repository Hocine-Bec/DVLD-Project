using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public class InternationalService
    {
        private readonly InternationalRepoService _repoService;
        private readonly InternationalMapper _mapper;

        public InternationalService()
        {
            _repoService = new InternationalRepoService();
            _mapper = new InternationalMapper();
        }

        public bool AddNewInternationalLicense(International international)
        {
            if (InternationalValidator.IsInternationalObjectEmpty(international))
                return false;

            var dto = _mapper.ToDTO(international);

            dto.InternationalId = _repoService.AddNew(dto);

            return (dto.InternationalId != -1);
        }

        public bool UpdateInternationalLicense(International international)
        {
            if (InternationalValidator.IsInternationalObjectEmpty(international))
                return false;

            var dto = _mapper.ToDTO(international);

            return _repoService.Update(dto);
        }

        public International Find(int InternationalLicenseID)
        {
            var dto = _repoService.Find(InternationalLicenseID);

            if (InternationalValidator.IsInternationalDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }
        
        public DataTable GetAllInternationalLicenses()
            => _repoService.GetAll();

        public int GetActiveInternationalLicenseIDByDriverID(int driverId)
            => _repoService.GetLicenseIdByDriverId(driverId);

        public DataTable GetDriverInternationalLicenses(int driverId)
            => _repoService.GetDriverInternationalLicenses(driverId);

    }
   
}
