using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestAppointment
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { set; get; }
        public clsTestType.enTestType TestTypeID { set; get; }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public clsApplication RetakeTestAppInfo { set; get; }
        public int  TestID   
        {
            get { return _GetTestID(); }   
          
        }

        public clsTestAppointment()

        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;  
            Mode = enMode.AddNew;

        }

        public clsTestAppointment(TestsAppointmentsDTO testsAppointmentsDTO)

        {
            this.TestAppointmentID = testsAppointmentsDTO.TestAppointmentID;
            this.TestTypeID = (clsTestType.enTestType)testsAppointmentsDTO.TestTypeID;
            this.LocalDrivingLicenseApplicationID = testsAppointmentsDTO.LocalDrivingLicenseApplicationID;
            this.AppointmentDate = testsAppointmentsDTO.AppointmentDate;
            this.PaidFees = testsAppointmentsDTO.PaidFees;
            this.CreatedByUserID = testsAppointmentsDTO.CreatedByUserID;
            this.IsLocked = testsAppointmentsDTO.IsLocked;
            this.RetakeTestApplicationID = testsAppointmentsDTO.RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            var testsAppointmentsDTO = new TestsAppointmentsDTO
            {
                TestTypeID = (int) this.TestTypeID,
                LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID,
                AppointmentDate = this.AppointmentDate, 
                PaidFees = this.PaidFees,
                CreatedByUserID = this.CreatedByUserID,
                RetakeTestApplicationID = this.RetakeTestApplicationID
            };

            this.TestAppointmentID = TestAppointmentRepository.AddNewTestAppointment(testsAppointmentsDTO);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            var testsAppointmentsDTO = new TestsAppointmentsDTO
            {
                TestTypeID = (int)this.TestTypeID,
                LocalDrivingLicenseApplicationID = this.LocalDrivingLicenseApplicationID,
                AppointmentDate = this.AppointmentDate,
                PaidFees = this.PaidFees,
                CreatedByUserID = this.CreatedByUserID,
                RetakeTestApplicationID = this.RetakeTestApplicationID
            };

            return TestAppointmentRepository.UpdateTestAppointment(testsAppointmentsDTO);
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            var testsAppointmentsDTO = TestAppointmentRepository.GetTestAppointmentInfoById(TestAppointmentID);

            if (testsAppointmentsDTO != null)
                return new clsTestAppointment(testsAppointmentsDTO);
            else
                return null;

        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID )
        {
            var testsAppointmentsDTO = (TestAppointmentRepository.GetLastTestAppointment(
                LocalDrivingLicenseApplicationID, (int)TestTypeID));

            if (testsAppointmentsDTO != null)
                return new clsTestAppointment(testsAppointmentsDTO);
            else
                return null;
        }

        public static DataTable GetAllTestAppointments()
        {
            return TestAppointmentRepository.GetAllTestAppointments();

        }

        public  DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return TestAppointmentRepository.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID,(int) TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return TestAppointmentRepository.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int) TestTypeID);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }

        private int  _GetTestID()
        {
            return TestAppointmentRepository.GetTestId(TestAppointmentID);
        }

    }
}
