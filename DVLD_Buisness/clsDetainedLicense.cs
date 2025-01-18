using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsDetainedLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float  FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }
       
        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        public clsDetainedLicense(DetainedLicensesDTO detainedLicensesDTO)
        {
            this.DetainID = detainedLicensesDTO.DetainID;
            this.LicenseID = detainedLicensesDTO.LicenseID;
            this.DetainDate = detainedLicensesDTO.DetainDate;
            this.FineFees = detainedLicensesDTO.FineFees;
            this.CreatedByUserID = detainedLicensesDTO.CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);
            this.IsReleased = detainedLicensesDTO.IsReleased;
            this.ReleaseDate = detainedLicensesDTO.ReleaseDate;
            this.ReleasedByUserID = detainedLicensesDTO.ReleasedByUserID;
            this.ReleaseApplicationID = detainedLicensesDTO.ReleaseApplicationID;
            this.ReleasedByUserInfo = clsUser.FindByPersonID(this.ReleasedByUserID);
            Mode = enMode.Update;
        }


        private bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense( 
                this.LicenseID,this.DetainDate,this.FineFees,this.CreatedByUserID);
            
            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicense(
                this.DetainID,this.LicenseID,this.DetainDate,this.FineFees,this.CreatedByUserID);
        }

        public static clsDetainedLicense Find(int detainID)
        {
            var detainedLicenseDTO = clsDetainedLicenseData.GetDetainedLicenseInfoById(detainID);

            if (detainedLicenseDTO != null)
                return new clsDetainedLicense(detainedLicenseDTO);
            else
                return null;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            var detainedLicenseDTO = clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseId(LicenseID);

            if (detainedLicenseDTO != null)
                return new clsDetainedLicense(detainedLicenseDTO);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID,
                   ReleasedByUserID, ReleaseApplicationID);
        }
    }
}
