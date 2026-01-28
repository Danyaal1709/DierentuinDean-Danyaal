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
    public class ZoosControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly ZoosController _controller;

        public ZoosControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"ZoosTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new ZoosController(_context);

            // Seed the database
            _context.Zoo.AddRange(
                new Zoo { Id = 1, Name = "Zoo A" },
                new Zoo { Id = 2, Name = "Zoo B" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfZoos()
        {
            // Act
            string? searchString = null;
            var result = await _controller.Index(searchString);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Zoo>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Index_WithSearchString_ReturnsFilteredZoos()
        {
            // Act
            var result = await _controller.Index("Zoo A");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Zoo>>(viewResult.ViewData.Model);
            var zoo = Assert.Single(model);
            Assert.Equal("Zoo A", zoo.Name);
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
        public async Task Details_ReturnsNotFound_WhenZooNotFound()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithZoo()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Zoo>(viewResult.ViewData.Model);
            Assert.Equal("Zoo A", model.Name);
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
