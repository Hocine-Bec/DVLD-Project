using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public enum enMode { AddNew = 0, Update = 1 };

    public class AppType
    {
        public enMode Mode = enMode.AddNew;


        public int Id { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public AppType()
        {
            this.Id = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public AppType(AppTypeDTO appTypeDTO)
        {
            this.Id = appTypeDTO.ID;
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

            this.Id = AppTypeRepository.AddNewApplicationType(dto);
              
            return (this.Id != -1);
        }

        private bool _UpdateApplicationType()
        {
            var dto = new AppTypeDTO()
            {
                ID = this.Id,
                Title = this.Title,
                Fees = this.Fees
            };

            return AppTypeRepository.UpdateApplicationType(dto);
        }

        public static AppType Find(int id)
        {
            var dto = AppTypeRepository.GetApplicationTypeInfoById((int)id);

            return (dto != null) ? new AppType(dto) : null;
        }

        public static DataTable GetAllApplicationTypes() => AppTypeRepository.GetAllApplicationTypes();

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
