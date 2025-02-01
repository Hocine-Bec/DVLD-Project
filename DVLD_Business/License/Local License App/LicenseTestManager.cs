namespace DVLD_Business
{
    public class LicenseTestManager
    {
        private readonly int _localLicenseAppId;
        private readonly TestRepoService _repoService;

        public LicenseTestManager(int localLicenseAppId)
        {
            _localLicenseAppId = localLicenseAppId;
            _repoService = new TestRepoService();
        }

        public byte GetPassedTestCount()
            => _repoService.GetPassedTestCount(this._localLicenseAppId);

        public bool DoesPassTestType(enTestType testTypeId)
            => _repoService.DoesPassTestType(this._localLicenseAppId, (int)testTypeId);

        public bool PassedAllTests() => GetPassedTestCount() == 3;

        public bool DoesAttendTestType(enTestType testTypeId)
            => _repoService.DoesAttendTestType(this._localLicenseAppId, (int)testTypeId);

        public byte TotalTrialsPerTest(enTestType testTypeId)
            => _repoService.TotalTrialsPerTest(this._localLicenseAppId, (int)testTypeId);

        public bool IsThereAnActiveScheduledTest(enTestType TestTypeID)
            => _repoService.IsThereAnActiveScheduledTest(this._localLicenseAppId, (int)TestTypeID);

        public bool DoesPassPreviousTest(enTestType currentTestType)
        {
            switch (currentTestType)
            {
                case enTestType.VisionTest:
                    //in this case no required previous test to pass.
                    return true;

                case enTestType.WrittenTest:
                    //Written Test, you cannot Schedule it before person passes the vision test.
                    //we check if pass vision test 1.
                    return this.DoesPassTestType(enTestType.VisionTest);

                case enTestType.StreetTest:
                    //Street Test, you cannot schedule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.DoesPassTestType(enTestType.WrittenTest);

                default:
                    return false;
            }
        }
        public bool AttendedTest(enTestType testTypeId)
            => _repoService.TotalTrialsPerTest(this._localLicenseAppId, (int)testTypeId) > 0;

    }

}



