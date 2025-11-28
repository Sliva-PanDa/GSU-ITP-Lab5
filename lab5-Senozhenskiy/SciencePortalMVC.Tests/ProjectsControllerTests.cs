using Microsoft.AspNetCore.Mvc;
using Moq;
using SciencePortalMVC.Controllers;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Tests
{
    [TestClass]
    public class ProjectsControllerTests
    {
        private Mock<IProjectRepository> _mockRepo;
        private ProjectsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _controller = new ProjectsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfProjects()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Project> { new Project(), new Project() });

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Project>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }
    }
}