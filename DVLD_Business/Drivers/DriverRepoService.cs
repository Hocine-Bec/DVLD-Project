using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class DriverRepoService
    {
        private DriverRepository _repository;

        public DriverRepoService()
        {
            _repository = new DriverRepository();
        }

        public int AddNew(int personId, int userId) => _repository.AddNewDriver(personId, userId);

        public bool Update(DriverDTO dto) => _repository.UpdateDriver(dto);

        public DriverDTO FindById(int driverId) => _repository.GetDriverInfoByDriverId(driverId);

        public DriverDTO FindByPersonId(int personId) => _repository.GetDriverInfoByPersonId(personId);

        public DataTable GetAllDrivers() => _repository.GetAllDrivers();
    }
}
