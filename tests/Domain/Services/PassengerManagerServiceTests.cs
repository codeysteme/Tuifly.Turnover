using FluentAssertions;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Domain.Tests.Unit.Services
{
    public class PassengerManagerServiceTests
    {
        private readonly PassengerManagerService _passengerService;

        public PassengerManagerServiceTests()
        {
            _passengerService = new PassengerManagerService();
        }


        [Fact]
        public void GeneratePassengersList_should_generate_empty_list()
        {
            //Actual
            var resultList = _passengerService.GeneratePassengersList(new List<RawPassenger>());

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
            var resultList = _passengerService.GeneratePassengersList(rawPassengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<Passenger>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void BuildFamiliesByPassengers_should_construct_a_valid_empty_families_list()
        {
            //Actual
            var resultList = _passengerService.BuildFamiliesByPassengers(new List<Passenger>());

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<Family>>(resultList);
            Assert.Empty(resultList);
        }

        [Theory]
        [MemberData(nameof(ToBuildFamiliesByPassengersMock))]
        public void BuildFamiliesByPassengers_should_construct_a_valid_families_list(List<Passenger> passengers, List<Family> expectedResult)
        {
            //Actual
            var resultList = _passengerService.BuildFamiliesByPassengers(passengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<Family>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> ToBuildFamiliesByPassengersMock => new List<object[]>
        {
            new object[] {
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
                        Family = Constants.SINGLE_PASSENGER,
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
                    },
                    new Passenger
                    {
                        Age = 25,
                        Family = "F",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE
                    },
                    new Passenger
                    {
                        Age = 7,
                        Family = "F",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    },
                    new Passenger
                    {
                        Age = 5,
                        Family = "F",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    },
                    new Passenger
                    {
                        Age = 9,
                        Family = "F",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    },
                    new Passenger
                    {
                        Age = 11,
                        Family = "F",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    },
                    new Passenger
                    {
                        Age = 24,
                        Family = "Z",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE
                    },
                    new Passenger
                    {
                        Age = 21,
                        Family = "Z",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE
                    },
                    new Passenger
                    {
                        Age = 27,
                        Family = "Z",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE
                    },
                    new Passenger
                    {
                        Age = 10,
                        Family = "Z",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE
                    }
                },
                new List<Family>
                {
                    new Family
                    {
                        Name = Constants.SINGLE_PASSENGER,
                        Members = new List<Passenger>
                        {
                            new Passenger
                            {
                                Age = 45,
                                Family = Constants.SINGLE_PASSENGER,
                                OverSize = true,
                                Type = PassengerTypeEnum.Adulte,
                                Price = Constants.OVERSIZE_PRICE
                            }
                        }
                    },
                    new Family
                    {
                        Name = "E",
                        Members = new List<Passenger>
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
                                Age = 7,
                                Family = "E",
                                Type = PassengerTypeEnum.Enfant,
                                Price = Constants.ENFANT_PRICE
                            }
                        }
                    }
                }
            }
        };

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
                        Famille = Constants.SINGLE_PASSENGER,
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
                        Family = Constants.SINGLE_PASSENGER,
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
