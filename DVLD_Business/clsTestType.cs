using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestType
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public  enum enTestType { VisionTest = 1, WrittenTest = 2,StreetTest=3 };

        public clsTestType.enTestType ID { set; get; }
        public string Title { set; get; }
        public string Description { set; get; } 
        public float Fees { set; get; }
        public clsTestType()

        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public clsTestType(TestTypesDTO testTypesDTO)
        {
            this.ID = (enTestType)testTypesDTO.TestTypeID;
            this.Title = testTypesDTO.Title;
            this.Description = testTypesDTO.Description;
            this.Fees = testTypesDTO.Fees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            var testTypeDTO = new TestTypesDTO()
            {
                Title = this.Title,
                Description = this.Description, 
                Fees = this.Fees
            };

            this.ID = (clsTestType.enTestType) TestTypeRepository.AddNewTestType(testTypeDTO);
              
            return (this.Title !="");
        }

        private bool _UpdateTestType()
        {
            var testTypeDTO = new TestTypesDTO()
            {
                TestTypeID = (int)this.ID,
                Title = this.Title,
                Description = this.Description,
                Fees = this.Fees
            };

            return TestTypeRepository.UpdateTestType(testTypeDTO);
        }

        public static clsTestType Find(clsTestType.enTestType testTypeID)
        {
            var testTypeDTO = TestTypeRepository.GetTestTypeInfoById((int)testTypeID);

            if (testTypeDTO != null)

                return new clsTestType(testTypeDTO);
            else
                return null;

        }

        public static DataTable GetAllTestTypes()
        {
            return TestTypeRepository.GetAllTestTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestType();

            }

            return false;
        }

    }
}
