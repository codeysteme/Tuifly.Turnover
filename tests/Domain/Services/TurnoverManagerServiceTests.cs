using FluentAssertions;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;
using TuiFly.Turnover.Domain.Services;

namespace TuiFly.Turnover.Domain.Tests.Unit.Services
{
    public class TurnoverManagerServiceTests
    {
        private readonly TurnoverManagerService _turnoverService;

        public TurnoverManagerServiceTests()
        {
            _turnoverService = new TurnoverManagerService();
        }

        [Fact]
        public void DistributePassengersAndFamiliesOnPlane_should_distribute_passengers_on_empty()
        {
            //Actual
            var resultList = _turnoverService.DistributePassengersAndFamiliesOnPlane(new List<Family>());

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<PassengerTicket>>(resultList);
            Assert.Empty(resultList);
        }

        [Theory]
        [MemberData(nameof(ToDistributePassengersMock))]
        public void DistributePassengersAndFamiliesOnPlane_should_distribute_passengers(List<Family> families, List<PassengerTicket> expectedResult)
        {
            //Actual
            var resultList = _turnoverService.DistributePassengersAndFamiliesOnPlane(families);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<PassengerTicket>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> ToDistributePassengersMock => new List<object[]>
        {
            new object[] {
                new List<Family>
                {
                    new Family
                    {
                        Name = "A",
                        Members = new List<Passenger>
                        {
                            new Passenger
                            {
                                Age = 18,
                                Family = "A",
                                Type = PassengerTypeEnum.Adulte,
                                Price = Constants.ADULTE_PRICE,
                                OverSize = true,
                            },
                            new Passenger
                            {
                                Age = 7,
                                Family = "A",
                                Type = PassengerTypeEnum.Enfant,
                                Price = Constants.ENFANT_PRICE
                            },
                            new Passenger
                            {
                                Age = 9,
                                Family = "A",
                                Type = PassengerTypeEnum.Enfant,
                                Price = Constants.ENFANT_PRICE
                            }
                        },
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
                },
                new List<PassengerTicket>
                {
                    //new PassengerTicket
                    //{
                    //    Age = 18,
                    //    Family = "A",
                    //    Type = PassengerTypeEnum.Adulte,
                    //    Price = Constants.ADULTE_PRICE,
                    //    OverSize = true,
                    //    Seats = new string[] {"P_1","P_2"}
                    //},
                    //new PassengerTicket
                    //{
                    //    Age = 7,
                    //    Family = "A",
                    //    Type = PassengerTypeEnum.Enfant,
                    //    Price = Constants.ENFANT_PRICE,
                    //    Seats = new string[] {"P_3"}
                    //},
                    //new PassengerTicket
                    //{
                    //    Age = 9,
                    //    Family = "A",
                    //    Type = PassengerTypeEnum.Enfant,
                    //    Price = Constants.ENFANT_PRICE,
                    //    Seats = new string[] {"P_4"}
                    //},
                    new PassengerTicket
                    {
                        Age = 12,
                        Family = "E",
                        Type = PassengerTypeEnum.Adulte,
                        Price = Constants.ADULTE_PRICE,
                        Seats = new string[] {"P_1"}
                    },
                    new PassengerTicket
                    {
                        Age = 7,
                        Family = "E",
                        Type = PassengerTypeEnum.Enfant,
                        Price = Constants.ENFANT_PRICE,
                        Seats = new string[] {"P_2"}
                    }
                }
            }
        };
    }
}
