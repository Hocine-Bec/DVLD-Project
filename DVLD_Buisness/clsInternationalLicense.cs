using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD.DTOs;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public class clsInternationalLicense : clsApplication
    {
        public new enum enMode { AddNew = 0, Update = 1 };
        public new enMode Mode = enMode.AddNew;

        public clsDriver DriverInfo;
        public int InternationalLicenseID { set; get; }  
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }   
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }    
        public bool IsActive { set; get; }
       

        public clsInternationalLicense()
        {
            //here we set the application type to New International License.
            this.ApplicationTypeID = (int) clsApplication.enApplicationType.NewInternationalLicense;
            
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
           
            this.IsActive = true;
            Mode = enMode.AddNew;
        }

        public clsInternationalLicense(InternationalLicenseDTO internationalLicenseDTO) 
            : base()
        {
            //this is for the base class
            clsApplication application = clsApplication.FindBaseApplication(ApplicationID);

            base.ApplicationID     = application.ApplicationID;
            base.ApplicantPersonID = application.ApplicantPersonID;
            base.ApplicationDate   = application.ApplicationDate;
            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = application.ApplicationStatus;
            base.LastStatusDate    = application.LastStatusDate;
            base.PaidFees          = application.PaidFees;
            base.CreatedByUserID   = application.CreatedByUserID;

            
            //Main Class
            this.InternationalLicenseID      = internationalLicenseDTO.InternationalLicenseID;
            this.ApplicationID               = internationalLicenseDTO.ApplicationID;
            this.DriverID                    = internationalLicenseDTO.DriverID;
            this.IssuedUsingLocalLicenseID   = internationalLicenseDTO.IssuedUsingLocalLicenseID;
            this.IssueDate                   = internationalLicenseDTO.IssueDate;
            this.ExpirationDate              = internationalLicenseDTO.ExpirationDate;
            this.IsActive                    = internationalLicenseDTO.IsActive;
            this.CreatedByUserID             = internationalLicenseDTO.CreatedByUserID;
             
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            var internationalLicenseDTO = new InternationalLicenseDTO()
            {
                ApplicationID = this.ApplicationID, 
                DriverID = this.DriverID, 
                IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID,
                IssueDate = this.IssueDate, 
                ExpirationDate = this.ExpirationDate,
                IsActive = this.IsActive, 
                CreatedByUserID = this.CreatedByUserID
            };

            this.InternationalLicenseID = 
                clsInternationalLicenseData.AddNewInternationalLicense(internationalLicenseDTO);


            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            var internationalLicenseDTO = new InternationalLicenseDTO()
            {
                InternationalLicenseID = this.InternationalLicenseID,
                ApplicationID = this.ApplicationID,
                DriverID = this.DriverID,
                IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID,
                IssueDate = this.IssueDate,
                ExpirationDate = this.ExpirationDate,
                IsActive = this.IsActive,
                CreatedByUserID = this.CreatedByUserID
            };

            return clsInternationalLicenseData.UpdateInternationalLicense(internationalLicenseDTO);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            var internationalLicenseDTO = clsInternationalLicenseData.GetInternationalLicenseInfoById(InternationalLicenseID);

            if (internationalLicenseDTO != null)
                return new clsInternationalLicense(internationalLicenseDTO);
            else
                return null;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public bool Save()
        {

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIdByDriverId(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
