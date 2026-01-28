using Dierentuin.Models;
using Xunit;

namespace Dierentuin.Tests
{
    public class AnimalsTests
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
            var animal = new Animal 
            { 
                Name = "Test Animal", 
                Species = "Test Species", 
                DietaryClass = DietaryClassEnum.Herbivore,
                Prey = "Grass, Leaves",
                Size = SizeEnum.Medium,
                ActivityPattern = ActivityPatternEnum.Diurnal,
                SpaceRequirement = 50,
                SecurityRequirement = SecurityLevelEnum.Low
            };

            // Act
            var result = animal.ActieFeedingTime();

            // Assert
            Assert.Equal("Eats: Grass, Leaves", result);
        }

        [Fact]
        public void ActieFeedingTime_ShouldReturnCarnivorePrey()
        {
            // Arrange
            var animal = new Animal 
            { 
                Name = "Lion", 
                Species = "Panthera leo", 
                DietaryClass = DietaryClassEnum.Carnivore,
                Prey = "Zebra, Buffalo",
                Size = SizeEnum.Large,
                ActivityPattern = ActivityPatternEnum.Diurnal,
                SpaceRequirement = 100,
                SecurityRequirement = SecurityLevelEnum.High
            };

            // Act
            var result = animal.ActieFeedingTime();

            // Assert
            Assert.Equal("Eats other animals: Zebra, Buffalo", result);
        }
    }
}

