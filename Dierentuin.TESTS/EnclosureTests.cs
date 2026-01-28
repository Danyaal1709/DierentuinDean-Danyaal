using Dierentuin.Models;
using Xunit;

namespace Dierentuin.Tests
{
    public class EnclosureTests
    {
        [Fact]
        public void Enclosure_CanBeCreated()
        {
            // Arrange & Act
            var enclosure = new Enclosure
            {
                Name = "Savannah",
                Climate = ClimateEnum.Tropical,
                HabitatType = HabitatTypeEnum.Grassland,
                SecurityLevel = SecurityLevelEnum.Medium,
                Size = 1000
            };

            // Assert
            Assert.Equal("Savannah", enclosure.Name);
            Assert.Equal(ClimateEnum.Tropical, enclosure.Climate);
            Assert.Equal(HabitatTypeEnum.Grassland, enclosure.HabitatType);
            Assert.Equal(1000, enclosure.Size);
        }

        [Fact]
        public void Enclosure_ActieSunrise_ReturnsEmptyList_WhenNoAnimals()
        {
            // Arrange
            var enclosure = new Enclosure
            {
                Name = "Empty Enclosure",
                Climate = ClimateEnum.Temperate,
                HabitatType = HabitatTypeEnum.Forest,
                SecurityLevel = SecurityLevelEnum.Low,
                Size = 500
            };

            // Act
            var result = enclosure.ActieSunrise();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Enclosure_ActieSunrise_ReturnsActions_WhenAnimalsPresent()
        {
            // Arrange
            var enclosure = new Enclosure
            {
                Name = "Test Enclosure",
                Climate = ClimateEnum.Tropical,
                HabitatType = HabitatTypeEnum.Grassland,
                SecurityLevel = SecurityLevelEnum.Medium,
                Size = 1000
            };

            var animal = new Animal
            {
                Name = "Lion",
                Species = "Panthera leo",
                ActivityPattern = ActivityPatternEnum.Diurnal,
                Size = SizeEnum.Large,
                DietaryClass = DietaryClassEnum.Carnivore,
                SpaceRequirement = 100,
                SecurityRequirement = SecurityLevelEnum.High
            };

            enclosure.Animals.Add(animal);

            // Act
            var result = enclosure.ActieSunrise();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains("Lion", result[0]);
        }

        [Fact]
        public void Enclosure_CheckConstraints_ReturnsConstraints()
        {
            // Arrange
            var enclosure = new Enclosure
            {
                Name = "Test Enclosure",
                Climate = ClimateEnum.Tropical,
                HabitatType = HabitatTypeEnum.Grassland,
                SecurityLevel = SecurityLevelEnum.Medium,
                Size = 1000
            };

            // Act
            var result = enclosure.CheckConstraints();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
