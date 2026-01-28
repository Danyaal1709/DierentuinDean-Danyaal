using Dierentuin.Models;
using Xunit;

namespace TestDierentuin13
{
    public class AnimalTests
    {
        [Fact]
        public void ActieSunrise_ShouldReturnCorrectAction()
        {
            // Arrange
            var animal = new Animal { Name = "Test Animal", Species = "Test Species", ActivityPattern = ActivityPatternEnum.Diurnal };

            // Act
            var result = animal.ActieSunrise();

            // Assert
            Assert.Equal("Wakes up", result);
        }

        [Fact]
        public void ActieSunset_ShouldReturnCorrectAction()
        {
            // Arrange
            var animal = new Animal { Name = "Test Animal", Species = "Test Species", ActivityPattern = ActivityPatternEnum.Nocturnal };

            // Act
            var result = animal.ActieSunset();

            // Assert
            Assert.Equal("Wakes up", result);
        }

        [Fact]
        public void ActieFeedingTime_ShouldReturnCorrectPrey()
        {
            // Arrange
            var animal = new Animal { Name = "Test Animal", Species = "Test Species", Prey = "Zebra, Buffalo" };

            // Act
            var result = animal.ActieFeedingTime();

            // Assert
            Assert.Equal("Eats: Zebra, Buffalo", result);
        }
    }
}

