using Microsoft.AspNetCore.Mvc;
using Moq;
using SciencePortalMVC.Controllers;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Tests
{
    [TestClass]
    public class PublicationsControllerTests
    {
        private Mock<IPublicationRepository> _mockRepo;
        private PublicationsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPublicationRepository>();
            _controller = new PublicationsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfPublications()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Publication> { new Publication(), new Publication() });

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Publication>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }
    }
}