namespace TuiFly.Turnover.Domain.Tests.Unit.Helpers
{
    public class RawPassengersHelpersTests
    {
        [Fact]
        public void ReadFile_should_read_and_return_valid_raw_passenger_list()
        {
            //Arrange

            //Actual
            //var result = RawPassengersHelpers.ReadFile("StaticFiles/passengers.csv");
            var result = new List<string>();

            //Assert

            Assert.NotNull(result);
            Assert.True(result.Count() == 10);
        }

    }
}
