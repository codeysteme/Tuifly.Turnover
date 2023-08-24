using FluentAssertions;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Domain.Tests.Unit.Services
{
    public class PassengerManagerServiceTests
    {
        [Fact]
        public void GeneratePassengersList_should_generate_empty_list()
        {
            //Actual
            var resultList = PassengerManagerService.GeneratePassengersList(new List<RawPassenger>());

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<Passenger[]>(resultList);
            Assert.Empty(resultList);
        }

        [Theory]
        [MemberData(nameof(ToGetRawPassengersListMock))]
        public void GeneratePassengersList_should_generate_valid_list_of_passengers(List<RawPassenger> rawPassengers, List<Passenger> expectedResult)
        {
            //Actual
            var resultList = PassengerManagerService.GeneratePassengersList(rawPassengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<Passenger>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> ToGetRawPassengersListMock => new List<object[]>
        {
            new object[] {
                new List<RawPassenger>
                {
                    new RawPassenger
                    {
                        Age = 12,
                        Famille = "E",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte,
                    },
                    new RawPassenger
                    {
                        Age = 45,
                        Famille = "-",
                        Places = Constants.TWO_PLACES,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 7,
                        Famille = "E",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    }
                },
                new List<Passenger>
                {
                    new Passenger
                    {
                        Age = 12,
                        Family = "E",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE
                    },
                    new Passenger
                    {
                        Age = 45,
                        Family = "-",
                        OverSize = true,
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.OVERSIZE_PRICE
                    },
                    new Passenger
                    {
                        Age = 7,
                        Family = "E",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    }
                },
            }
        };


    }
}
