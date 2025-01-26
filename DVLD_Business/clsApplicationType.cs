using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsApplicationType
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public clsApplicationType()

        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public clsApplicationType(AppTypeDTO appTypeDTO)

        {
            this.ID = appTypeDTO.ID;
            this.Title = appTypeDTO.Title;
            this.Fees = appTypeDTO.Fees;
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            var dto = new AppTypeDTO()
            {
                Title = this.Title,
                Fees = this.Fees
            };

            this.ID = AppTypeRepository.AddNewApplicationType(dto);
              
            return (this.ID != -1);
        }

        private bool _UpdateApplicationType()
        {
            var dto = new AppTypeDTO()
            {
                ID = this.ID,
                Title = this.Title,
                Fees = this.Fees
            };

            return AppTypeRepository.UpdateApplicationType(dto);
        }

        public static clsApplicationType Find(int ID)
        {
            var dto = AppTypeRepository.GetApplicationTypeInfoById((int)ID);

            return (dto != null) ? new clsApplicationType(dto) : null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return AppTypeRepository.GetAllApplicationTypes();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();

            }

            return false;
        }

    }
}
