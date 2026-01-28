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
    public class EnclosuresApiControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly EnclosuresApiController _controller;

        public EnclosuresApiControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"EnclosuresApiTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new EnclosuresApiController(_context);

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
        public async Task GetEnclosures_ReturnsListOfEnclosures()
        {
            // Act
            var result = await _controller.GetEnclosures();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Enclosure>>>(result);
            var enclosures = Assert.IsType<List<Enclosure>>(actionResult.Value);
            Assert.Equal(2, enclosures.Count);
        }

        [Fact]
        public async Task GetEnclosure_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.GetEnclosure(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetEnclosure_ReturnsEnclosure_WhenIdExists()
        {
            // Act
            var result = await _controller.GetEnclosure(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Enclosure>>(result);
            var enclosure = Assert.IsType<Enclosure>(actionResult.Value);
            Assert.Equal("Savannah", enclosure.Name);
        }

        [Fact]
        public async Task PostEnclosure_CreatesNewEnclosure()
        {
            // Arrange
            var enclosure = new Enclosure { Name = "Desert", Climate = ClimateEnum.Temperate, HabitatType = HabitatTypeEnum.Desert, SecurityLevel = SecurityLevelEnum.Low, Size = 800, ZooId = 1 };

            // Act
            var result = await _controller.PostEnclosure(enclosure);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdEnclosure = Assert.IsType<Enclosure>(createdAtActionResult.Value);
            Assert.Equal("Desert", createdEnclosure.Name);
        }

        [Fact]
        public async Task DeleteEnclosure_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.DeleteEnclosure(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
