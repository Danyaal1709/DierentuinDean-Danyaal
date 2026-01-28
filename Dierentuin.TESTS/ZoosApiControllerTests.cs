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
    public class ZoosApiControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly ZoosApiController _controller;

        public ZoosApiControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"ZoosApiTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new ZoosApiController(_context);

            // Seed the database
            _context.Zoo.AddRange(
                new Zoo { Id = 1, Name = "Zoo A" },
                new Zoo { Id = 2, Name = "Zoo B" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetZoos_ReturnsListOfZoos()
        {
            // Act
            var result = await _controller.GetZoos();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Zoo>>>(result);
            var zoos = Assert.IsType<List<Zoo>>(actionResult.Value);
            Assert.Equal(2, zoos.Count);
        }

        [Fact]
        public async Task GetZoo_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.GetZoo(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetZoo_ReturnsZoo_WhenIdExists()
        {
            // Act
            var result = await _controller.GetZoo(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Zoo>>(result);
            var zoo = Assert.IsType<Zoo>(actionResult.Value);
            Assert.Equal("Zoo A", zoo.Name);
        }

        [Fact]
        public async Task PostZoo_CreatesNewZoo()
        {
            // Arrange
            var zoo = new Zoo { Name = "Zoo C" };

            // Act
            var result = await _controller.PostZoo(zoo);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdZoo = Assert.IsType<Zoo>(createdAtActionResult.Value);
            Assert.Equal("Zoo C", createdZoo.Name);
        }

        [Fact]
        public async Task DeleteZoo_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.DeleteZoo(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
