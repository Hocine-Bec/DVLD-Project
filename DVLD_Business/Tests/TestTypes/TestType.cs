using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

    public class TestType
    {
        public enMode Mode = enMode.AddNew;

        public enTestType Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; } 
        public float Fees { set; get; }

        public TestType()
        {
            this.Id = enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public TestType(TestTypesDTO testTypesDTO)
        {
            this.Id = (enTestType)testTypesDTO.TestTypeID;
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

            this.Id = (enTestType) TestTypeRepository.AddNewTestType(testTypeDTO);
              
            return (this.Title !="");
        }

        private bool _UpdateTestType()
        {
            var testTypeDTO = new TestTypesDTO()
            {
                TestTypeID = (int)this.Id,
                Title = this.Title,
                Description = this.Description,
                Fees = this.Fees
            };

            return TestTypeRepository.UpdateTestType(testTypeDTO);
        }

        public static TestType Find(enTestType testTypeID)
        {
            var testTypeDTO = TestTypeRepository.GetTestTypeInfoById((int)testTypeID);

            if (testTypeDTO != null)

                return new TestType(testTypeDTO);
            else
                return null;

        }

        public static DataTable GetAllTestTypes() => TestTypeRepository.GetAllTestTypes();

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
