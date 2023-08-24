using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TuiFly.Turnover.Domain.Common;
using TuiFly.Turnover.Domain.Models;
using TuiFly.Turnover.Infrastructure.Services;

namespace TuiFly.Turnover.Infrastructure.Tests.Unit.Services
{
    public class RawPassengersServiceTests
    {
        private readonly Mock<ILogger<RawPassengersService>> _mockLogger;
        private readonly RawPassengersService _rawPassengersService;

        public RawPassengersServiceTests()
        {
            _mockLogger = new Mock<ILogger<RawPassengersService>>();
            _rawPassengersService = new RawPassengersService(_mockLogger.Object);
        }

        [Fact]
        public void ReadFile_should_read_and_return_valid_raw_passenger_list()
        {
            //Arrange
            var expectedLength = 60;

            //Actual
            var result = _rawPassengersService.ReadFile("Mock/passengers_test.csv");

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<RawPassenger>>(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count() == expectedLength);
            CheckLogger(Times.Exactly(0));
        }

        [Fact]
        public void ReadFile_should_throw_an_exception_when_value_not_exist()
        {
            //Actual
            var result = _rawPassengersService.ReadFile("Mock/passengers_test_a.csv");

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<RawPassenger[]>(result);
            CheckLogger(Times.Exactly(1));
        }

        [Theory]
        [MemberData(nameof(ToRawPassengersChildMock))]
        public void FilterAndGetRawPassengers_validate_child_for_all_constraints(List<RawPassenger> rawPassengers, List<RawPassenger> expectedResult)
        {
            //Actual
            var resultList = RawPassengersService.FilterAndGetRawPassengers(rawPassengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<RawPassenger>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(ToRawPassengersAdultMock))]
        public void FilterAndGetRawPassengers_should_validate_Adult_for_all_constraints(List<RawPassenger> rawPassengers, List<RawPassenger> expectedResult)
        {
            //Actual
            var resultList = RawPassengersService.FilterAndGetRawPassengers(rawPassengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<RawPassenger>>(resultList);
            resultList.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(ToRawPassengersAdultMock))]
        public void GetRawPassengersList_validate_all_constraints_and_return_list(List<RawPassenger> rawPassengers, List<RawPassenger> expectedResult)
        {
            //Actual
            var resultList = RawPassengersService.FilterAndGetRawPassengers(rawPassengers);

            //Assert
            Assert.NotNull(resultList);
            Assert.IsType<List<RawPassenger>>(resultList);
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
                        Type = PassengerTypeEnum.Adulte
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
                        Famille = "B",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 7,
                        Famille = "E",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                    new RawPassenger
                    {
                        Age = 7,
                        Famille = "-",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    }
                },
                new List<RawPassenger>
                {
                    new RawPassenger
                    {
                        Age = 12,
                        Famille = "E",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
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
            }
        };


        public static IEnumerable<object[]> ToRawPassengersAdultMock => new List<object[]>
        {
            new object[] {
                new List<RawPassenger>
                {
                    new RawPassenger
                    {
                        Age = 12,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 45,
                        Famille = "A",
                        Places = Constants.TWO_PLACES,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 11,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    }
                },
                new List<RawPassenger>
                {
                   new RawPassenger
                    {
                        Age = 12,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 45,
                        Famille = "A",
                        Places = Constants.TWO_PLACES,
                        Type = PassengerTypeEnum.Adulte
                    },
                },
            }
        };

        public static IEnumerable<object[]> ToRawPassengersChildMock => new List<object[]>
        {
            new object[] {
                new List<RawPassenger>
                {
                    new RawPassenger
                    {
                        Age = 15,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 5,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                    new RawPassenger
                    {
                        Age = 15,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                    new RawPassenger
                    {
                        Age = 6,
                        Famille = "-",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                    new RawPassenger
                    {
                        Age = 11,
                        Famille = "Z",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                    new RawPassenger
                    {
                        Age = 1,
                        Famille = "A",
                        Places = Constants.TWO_PLACES,
                        Type = PassengerTypeEnum.Enfant
                    },
                },
                new List<RawPassenger>
                {
                    new RawPassenger
                    {
                        Age = 15,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Adulte
                    },
                    new RawPassenger
                    {
                        Age = 5,
                        Famille = "A",
                        Places = Constants.ONE_PLACE,
                        Type = PassengerTypeEnum.Enfant
                    },
                },
            }
        };

        private void CheckLogger(Times times)
        {
            _mockLogger.Verify(
                x => x.Log(
                   LogLevel.Error,
                   It.IsAny<EventId>(),
                   It.IsAny<It.IsAnyType>(),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()), times);
        }
    }
}
