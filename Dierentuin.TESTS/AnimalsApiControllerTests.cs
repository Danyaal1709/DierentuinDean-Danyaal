using Dierentuin.Controllers.Api;
using Dierentuin.Data;
using Dierentuin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dierentuin.Tests
{
    public class AnimalsApiControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly AnimalsApiController _controller;

        public AnimalsApiControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"AnimalsApiTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new AnimalsApiController(_context);

            // Seed the database
            var category = new Category { Id = 1, Name = "Mammal" };
            _context.Category.Add(category);
            _context.SaveChanges();

            _context.Animal.AddRange(
                new Animal { Id = 1, Name = "Lion", Species = "Panthera leo", CategoryId = category.Id, Size = SizeEnum.Large, DietaryClass = DietaryClassEnum.Carnivore, ActivityPattern = ActivityPatternEnum.Diurnal, SpaceRequirement = 100, SecurityRequirement = SecurityLevelEnum.High },
                new Animal { Id = 2, Name = "Elephant", Species = "Loxodonta", CategoryId = category.Id, Size = SizeEnum.VeryLarge, DietaryClass = DietaryClassEnum.Herbivore, ActivityPattern = ActivityPatternEnum.Diurnal, SpaceRequirement = 200, SecurityRequirement = SecurityLevelEnum.Medium }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAnimals_ReturnsListOfAnimals()
        {
            // Act
            var result = await _controller.GetAnimals();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Animal>>>(result);
            var animals = Assert.IsType<List<Animal>>(actionResult.Value);
            Assert.Equal(2, animals.Count);
        }

        [Fact]
        public async Task GetAnimal_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.GetAnimal(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAnimal_ReturnsAnimal_WhenIdExists()
        {
            // Act
            var result = await _controller.GetAnimal(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Animal>>(result);
            var animal = Assert.IsType<Animal>(actionResult.Value);
            Assert.Equal("Lion", animal.Name);
        }

        [Fact]
        public async Task PostAnimal_CreatesNewAnimal()
        {
            // Arrange
            var animal = new Animal { Name = "Tiger", Species = "Panthera tigris", CategoryId = 1, Size = SizeEnum.Large, DietaryClass = DietaryClassEnum.Carnivore, ActivityPattern = ActivityPatternEnum.Diurnal, SpaceRequirement = 120, SecurityRequirement = SecurityLevelEnum.High };

            // Act
            var result = await _controller.PostAnimal(animal);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdAnimal = Assert.IsType<Animal>(createdAtActionResult.Value);
            Assert.Equal("Tiger", createdAnimal.Name);
        }

        [Fact]
        public async Task DeleteAnimal_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.DeleteAnimal(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
