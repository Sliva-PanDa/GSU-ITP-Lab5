using Microsoft.AspNetCore.Mvc;
using Moq; 
using SciencePortalMVC.Controllers;
using SciencePortalMVC.Interfaces; 
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Tests
{
    [TestClass]
    public class DepartmentsControllerTests
    {
        private Mock<IDepartmentRepository> _mockRepo;
        private DepartmentsController _controller;

        [TestInitialize]
        public void Setup()
        {
            // 1. Создаем мок (имитацию) репозитория
            _mockRepo = new Mock<IDepartmentRepository>();

            // 2. Создаем контроллер, передавая ему наш мок-объект
            _controller = new DepartmentsController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfDepartments()
        {
            // Подготовка (Arrange)
            // Готовим список данных, который мы "ожидаем" от репозитория
            var testDepartments = new List<Department>
            {
                new Department { DepartmentId = 1, Name = "Кафедра ИТ" },
                new Department { DepartmentId = 2, Name = "Кафедра АЭП" }
            };

            // Настраиваем мок: "Когда кто-нибудь вызовет метод GetAllAsync, верни наш тестовый список"
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testDepartments);

            // Действие (Act)
            var result = await _controller.Index();

            // Утверждение (Assert)
            Assert.IsInstanceOfType(result, typeof(ViewResult)); 

            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Department>;

            Assert.IsNotNull(model); 
            Assert.AreEqual(2, model.Count); 
        }
    }
}