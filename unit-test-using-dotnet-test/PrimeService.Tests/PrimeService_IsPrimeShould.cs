using System.Collections.Generic;
using Moq;
using Xunit;
namespace PrimeService.Tests
{
    public class PrimeService_IsPrimeShould
    {
        private readonly PrimeService _primeService;


        // WOW! No record/replay weirdness?! :)
        protected Mock<IDependentService> mock = new Mock<IDependentService>();
        public PrimeService_IsPrimeShould()
        {
            mock.Setup(x => x.IsTrue()).Returns(false);

            _primeService = new PrimeService(mock.Object);
            //_primeService = new PrimeService();

        }

        [Fact]
        public void ReturnFalseGivenValueOf1()
        {
            var result = _primeService.IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void ReturnFalseGivenValuesLessThan2(int value)
        {
            var result = _primeService.IsPrime(value);

            Assert.False(result, $"{value} should not be prime");
        }
    }
}
