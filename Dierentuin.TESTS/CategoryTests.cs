using Dierentuin.Models;
using Xunit;

namespace Dierentuin.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void Category_CanBeCreated()
        {
            // Arrange & Act
            var category = new Category { Name = "Mammals" };

            // Assert
            Assert.Equal("Mammals", category.Name);
            Assert.NotNull(category.Animals);
        }

        [Fact]
        public void Category_AnimalsList_IsInitialized()
        {
            // Arrange & Act
            var category = new Category { Name = "Birds" };

            // Assert
            Assert.NotNull(category.Animals);
            Assert.Empty(category.Animals);
        }
    }
}
