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
    public class CategoriesApiControllerTests
    {
        private readonly DierentuinContext _context;
        private readonly CategoriesApiController _controller;

        public CategoriesApiControllerTests()
        {
            var options = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: $"CategoriesApiTestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new DierentuinContext(options);
            _controller = new CategoriesApiController(_context);

            // Seed the database
            _context.Category.AddRange(
                new Category { Id = 1, Name = "Mammals" },
                new Category { Id = 2, Name = "Birds" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCategories_ReturnsListOfCategories()
        {
            // Act
            var result = await _controller.GetCategories();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
            var categories = Assert.IsType<List<Category>>(actionResult.Value);
            Assert.Equal(2, categories.Count);
        }

        [Fact]
        public async Task GetCategory_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.GetCategory(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCategory_ReturnsCategory_WhenIdExists()
        {
            // Act
            var result = await _controller.GetCategory(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var category = Assert.IsType<Category>(actionResult.Value);
            Assert.Equal("Mammals", category.Name);
        }

        [Fact]
        public async Task PostCategory_CreatesNewCategory()
        {
            // Arrange
            var category = new Category { Name = "Reptiles" };

            // Act
            var result = await _controller.PostCategory(category);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdCategory = Assert.IsType<Category>(createdAtActionResult.Value);
            Assert.Equal("Reptiles", createdCategory.Name);
        }

        [Fact]
        public async Task PutCategory_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Updated" };

            // Act
            var result = await _controller.PutCategory(999, category);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNotFound_WhenIdNotFound()
        {
            // Act
            var result = await _controller.DeleteCategory(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent_WhenDeleted()
        {
            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetCategoryAnimals_ReturnsNotFound_WhenCategoryNotFound()
        {
            // Act
            var result = await _controller.GetCategoryAnimals(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
