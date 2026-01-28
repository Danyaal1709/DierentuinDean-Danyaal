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
    public class EnclosuresControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly EnclosuresController _controller;

        public EnclosuresControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"EnclosuresTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new EnclosuresController(_context);

            // Seed the database
            var zoo = new Zoo { Id = 1, Name = "Zoo A" };
            _context.Zoo.Add(zoo);
            _context.SaveChanges();

            _context.Enclosure.AddRange(
                new Enclosure { Id = 1, Name = "Savannah", Climate = ClimateEnum.Tropical, HabitatType = HabitatTypeEnum.Grassland, SecurityLevel = SecurityLevelEnum.Medium, Size = 1000, ZooId = zoo.Id },
                new Enclosure { Id = 2, Name = "Rainforest", Climate = ClimateEnum.Tropical, HabitatType = HabitatTypeEnum.Forest, SecurityLevel = SecurityLevelEnum.High, Size = 1500, ZooId = zoo.Id }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfEnclosures()
        {
            // Act
            var result = await _controller.Index(string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Enclosure>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Index_WithSearchString_ReturnsFilteredEnclosures()
        {
            // Act
            var result = await _controller.Index("Savannah");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Enclosure>>(viewResult.ViewData.Model);
            var enclosure = Assert.Single(model);
            Assert.Equal("Savannah", enclosure.Name);
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
        public async Task Details_ReturnsNotFound_WhenEnclosureNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithEnclosure()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Enclosure>(viewResult.ViewData.Model);
            Assert.Equal("Savannah", model.Name);
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Sunrise_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Sunrise(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Sunset_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Sunset(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task FeedingTime_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.FeedingTime(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CheckConstraints_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.CheckConstraints(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
