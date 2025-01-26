using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsDriver
    {
        //This is for testing purpose, it will be updated later
        private PersonService _personService = new PersonService();

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public Person PersonInfo;

        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate {  get; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate=DateTime.Now;
            Mode = enMode.AddNew;
        }

        public clsDriver(DriverDTO driverDTO)
        {
            this.DriverID = driverDTO.DriverID;
            this.PersonID = driverDTO.PersonID;
            this.CreatedByUserID = driverDTO.CreatedByUserID;
            this.CreatedDate = driverDTO.CreatedDate;
            this.PersonInfo = _personService.Find(PersonID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = DriverRepository.AddNewDriver( PersonID,  CreatedByUserID);
              
            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            var dto = new DriverDTO()
            {
                PersonID = this.PersonID,
                DriverID = this.DriverID,
                CreatedByUserID = this.CreatedByUserID
            };

            return DriverRepository.UpdateDriver(dto);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            var dto = DriverRepository.GetDriverInfoByDriverId(DriverID);

            return (dto != null) ? new clsDriver(dto) : null;
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            var dto = DriverRepository.GetDriverInfoByPersonId(PersonID);

            return (dto != null) ? new clsDriver(dto) : null;
        }

        public static DataTable GetAllDrivers()
        {
            return DriverRepository.GetAllDrivers();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }

    }
}
