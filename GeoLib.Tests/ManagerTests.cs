using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Data;
using GeoLib.Contracts;
using GeoLib.Services;

namespace GeoLib.Tests
{
    [TestClass]
    public class ManagerTests
    {
        [TestMethod]
        public void test_zip_code_retrieval()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode()
            {
                City = "AUSTIN",
                State = new State() { Abbreviation = "TX" },
                Zip = "78759"
            };

            mockZipCodeRepository.Setup(obj => obj.GetByZip("78759")).Returns(zipCode);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            ZipCodeData data = geoService.GetZipInfo("78759");

            Assert.IsTrue(data.City.ToUpper() == "AUSTIN");
            Assert.IsTrue(data.State == "TX");
        }

        [TestMethod]
        public void test_state_retrieval()
        {
            Mock<IStateRepository> mockStateRepository = new Mock<IStateRepository>();

            List<State> stateList = new List<State>();

            State state = new State() {
                StateId = 1,
                Abbreviation = "TX",
                Name = "Texas",
                IsPrimaryState = true
            };

            stateList.Add(state);

            mockStateRepository.Setup(obj => obj.Get(true)).Returns(stateList);

            IGeoService geoService = new GeoManager(mockStateRepository.Object);

            IEnumerable<string> states = geoService.GetStates(true);

            Assert.IsTrue(states.ToList()[0].ToUpper() == "TX");
        }

        [TestMethod]
        public void test_zip_retrieval_by_state()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            List <ZipCode> zipList = new List<ZipCode>();

            ZipCode zipCode = new ZipCode()
            {
                City = "AUSTIN",
                State = new State() { Abbreviation = "TX" },
                Zip = "78759"
            };

            zipList.Add(zipCode);

            mockZipCodeRepository.Setup(obj => obj.GetByState("TX")).Returns(zipList);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            IEnumerable<ZipCodeData> zipCodes = geoService.GetZips("TX");

            Assert.IsTrue(zipCodes.ToList()[0].ZipCode == "78759");
            Assert.IsTrue(zipCodes.ToList()[0].State == "TX");
        }

        [TestMethod]
        public void test_zip_retrieval_by_zip_and_range()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            List<ZipCode> zipList = new List<ZipCode>();

            ZipCode zipCode = new ZipCode()
            {
                City = "AUSTIN",
                State = new State() { Abbreviation = "TX" },
                Zip = "78759"
            };
            ZipCode zipCode2 = new ZipCode()
            {
                City = "SAN FRANCISCO",
                State = new State() { Abbreviation = "CA" },
                Zip = "94105"
            };

            zipList.Add(zipCode);
            zipList.Add(zipCode2);

            int range = 1;

            mockZipCodeRepository.Setup(obj => obj.GetZipsForRange(zipCode, range)).Returns(zipList);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            IEnumerable<ZipCodeData> zipCodes = geoService.GetZips("TX");

            Assert.IsTrue(zipCodes.ToList()[0].ZipCode == "78759");
            Assert.IsTrue(zipCodes.ToList()[0].State == "TX");
        }
    }
}
