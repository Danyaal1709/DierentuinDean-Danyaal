using Dierentuin.Models;
using Xunit;

namespace Dierentuin.Tests
{
    public class ZooTests
    {
        [Fact]
        public void Zoo_CanBeCreated()
        {
            // Arrange & Act
            var zoo = new Zoo { Name = "Zoo A" };

            // Assert
            Assert.Equal("Zoo A", zoo.Name);
            Assert.NotNull(zoo.Enclosures);
        }

        [Fact]
        public void Zoo_EnclosuresList_IsInitialized()
        {
            // Arrange & Act
            var zoo = new Zoo { Name = "Zoo B" };

            // Assert
            Assert.NotNull(zoo.Enclosures);
            Assert.Empty(zoo.Enclosures);
        }

        [Fact]
        public void Zoo_ActieSunrise_ReturnsEmptyList_WhenNoEnclosures()
        {
            // Arrange
            var zoo = new Zoo { Name = "Empty Zoo" };

            // Act
            var result = zoo.ActieSunrise();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Zoo_ActieSunset_ReturnsEmptyList_WhenNoEnclosures()
        {
            // Arrange
            var zoo = new Zoo { Name = "Empty Zoo" };

            // Act
            var result = zoo.ActieSunset();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Zoo_ActieFeedingTime_ReturnsEmptyList_WhenNoEnclosures()
        {
            // Arrange
            var zoo = new Zoo { Name = "Empty Zoo" };

            // Act
            var result = zoo.ActieFeedingTime();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Zoo_CheckConstraints_ReturnsConstraints()
        {
            // Arrange
            var zoo = new Zoo { Name = "Test Zoo" };

            // Act
            var result = zoo.CheckConstraints();

            // Assert
            Assert.NotNull(result);
            // When zoo has no enclosures, constraints list is empty
            // This is expected behavior
        }
    }
}
