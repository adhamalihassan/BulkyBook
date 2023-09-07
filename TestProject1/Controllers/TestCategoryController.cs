using System;
using System.Collections.Generic;
using System.Linq;
using BulkyBook.Controllers;
using BulkyBook.Dependencies;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BulkyBook.Tests
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _controller;
        private readonly ApplicationDbContext _dbContextMock;
        private readonly Mock<ILogger<CategoryController>> _loggerMock;
        private readonly Mock<IMyDependency> _myDepMock;
        private readonly Mock<IMyDependency> _myDep2Mock;

        public CategoryControllerTests()
        {
            // Create mock instances of dependencies
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContextMock = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<CategoryController>>();
            _myDepMock = new Mock<IMyDependency>();
            _myDep2Mock = new Mock<IMyDependency>();

            // Configure the mock dependencies
            _myDepMock.Setup(d => d.Name).Returns("Dependency1");
            _myDep2Mock.Setup(d => d.Name).Returns("Dependency2");

            // Create the controller instance with the mock dependencies
            _controller = new CategoryController(
                _dbContextMock,
                _loggerMock.Object,
                new List<IMyDependency> { _myDepMock.Object, _myDep2Mock.Object }
            );
        }

        [Fact]
        public void GetCategories_ReturnsOkResultWithCategories()
        {
            // Arrange
            // Add some test data to the in-memory database
            _dbContextMock.Categories.AddRange(new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            });
            _dbContextMock.SaveChanges();

            // Act
            var result = _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Dictionary<string, object>>(okResult.Value);
            Assert.Equal("Dependency1", model["name"]);
            Assert.Equal("Dependency2", model["name2"]);
            var resultCategories = Assert.IsType<List<Category>>(model["categories"]);
            Assert.Equal(_dbContextMock.Categories.ToList().Count, resultCategories.Count);
            Assert.Equal(_dbContextMock.Categories.ToList()[0].Name, resultCategories[0].Name);
            Assert.Equal(_dbContextMock.Categories.ToList()[1].Name, resultCategories[1].Name);
        }

    }
}
