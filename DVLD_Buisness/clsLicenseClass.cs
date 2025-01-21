using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsLicenseClass
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public clsLicenseClass()

        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
          
            Mode = enMode.AddNew;

        }

        public clsLicenseClass(LicenseClassDTO licenseClassDTO)

        {
            this.LicenseClassID = licenseClassDTO.LicenseClassID;
            this.ClassName = licenseClassDTO.ClassName;
            this.ClassDescription = licenseClassDTO.ClassDescription;
            this.MinimumAllowedAge = licenseClassDTO.MinimumAllowedAge;
            this.DefaultValidityLength = licenseClassDTO.DefaultValidityLength;
            this.ClassFees = licenseClassDTO.ClassFees;
            Mode = enMode.Update;
        }

        private bool _AddNewLicenseClass()
        {

            var licenseClassDTO = new LicenseClassDTO
            {
                ClassName = this.ClassName,
                ClassDescription = this.ClassDescription,
                MinimumAllowedAge = this.MinimumAllowedAge,
                DefaultValidityLength = this.DefaultValidityLength,
                ClassFees = this.ClassFees
            };

            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(licenseClassDTO);
              

            return (this.LicenseClassID != -1);
        }

        private bool _UpdateLicenseClass()
        {
            var licenseClassDTO = new LicenseClassDTO
            {
                LicenseClassID = this.LicenseClassID,
                ClassName = this.ClassName,
                ClassDescription = this.ClassDescription,
                MinimumAllowedAge = this.MinimumAllowedAge,
                DefaultValidityLength = this.DefaultValidityLength,
                ClassFees = this.ClassFees
            };

            return clsLicenseClassData.UpdateLicenseClass(licenseClassDTO);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {

            var licenseClassDTO = clsLicenseClassData.GetLicenseClassInfoById(LicenseClassID);

            if (licenseClassDTO != null)
                return new clsLicenseClass(licenseClassDTO);
            else
                return null;

        }

        public static clsLicenseClass Find(string className)
        {
            var licenseClassDTO = clsLicenseClassData.GetLicenseClassInfoByClassName(className);

            if (licenseClassDTO != null)
                return new clsLicenseClass(licenseClassDTO);
            else
                return null;
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseClass();

            }

            return false;
        }

    }
}
