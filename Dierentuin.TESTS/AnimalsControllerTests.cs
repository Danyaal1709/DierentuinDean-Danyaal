using Dierentuin.Controllers;
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
    public class AnimalsControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly AnimalsController _controller;

        public AnimalsControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"AnimalsTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new AnimalsController(_context);

            // Seed the database
            var category = new Category { Name = "Mammal" };
            _context.Category.Add(category);
            _context.SaveChanges();

            _context.Animal.AddRange(
                new Animal { Id = 1, Name = "Lion", Species = "Panthera leo", CategoryId = category.Id, Size = SizeEnum.Large, DietaryClass = DietaryClassEnum.Carnivore, ActivityPattern = ActivityPatternEnum.Diurnal, SpaceRequirement = 100, SecurityRequirement = SecurityLevelEnum.High },
                new Animal { Id = 2, Name = "Elephant", Species = "Loxodonta", CategoryId = category.Id, Size = SizeEnum.VeryLarge, DietaryClass = DietaryClassEnum.Herbivore, ActivityPattern = ActivityPatternEnum.Diurnal, SpaceRequirement = 200, SecurityRequirement = SecurityLevelEnum.Medium }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfAnimals()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Animal>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Filter_WithSearchString_ReturnsFilteredAnimals()
        {
            // Act
            var result = await _controller.Filter("Lion");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Animal>>(viewResult.ViewData.Model);
            var animal = Assert.Single(model);
            Assert.Equal("Lion", animal.Name);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenAnimalNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithAnimal()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Animal>(viewResult.ViewData.Model);
            Assert.Equal("Lion", model.Name);
        }
    }
}

