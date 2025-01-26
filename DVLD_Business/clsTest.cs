using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public clsTestAppointment TestAppointmentInfo { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; } 
       
        public clsTest()

        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes ="";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        public clsTest(TestsDTO testsDTO)

        {
            this.TestID = testsDTO.TestID;
            this.TestAppointmentID = testsDTO.TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = testsDTO.TestResult;
            this.Notes = testsDTO.Notes;
            this.CreatedByUserID = testsDTO.CreatedByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            var testsDTO = new TestsDTO
            {
                TestAppointmentID = this.TestAppointmentID,
                TestResult = this.TestResult,
                Notes = this.Notes,
                CreatedByUserID = this.CreatedByUserID
            };

            this.TestID = TestRepository.AddNewTest(testsDTO);
              

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            var testsDTO = new TestsDTO
            {
                TestID = this.TestID,
                TestAppointmentID = this.TestAppointmentID,
                TestResult = this.TestResult,
                Notes = this.Notes,
                CreatedByUserID = this.CreatedByUserID
            };

            return TestRepository.UpdateTest(testsDTO);
        }

        public static clsTest Find(int TestID)
        {
            var testsDTO = TestRepository.GetTestInfoById(TestID);

            if (testsDTO != null)
                return new clsTest(testsDTO);
            else
                return null;

        }

        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, 
            clsTestType.enTestType TestTypeID)
        {
            var testsDTO = TestRepository.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID, LicenseClassID, (int)TestTypeID);

            if (testsDTO != null)
                return new clsTest(testsDTO);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return TestRepository.GetAllTests();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return TestRepository.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool  PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }

    }
}
