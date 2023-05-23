using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dashboard.Domain.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Infrastructure.Repositories;
using Moq;
using Dashboard.Domain.Models;
using System.Threading;
using System;
using Dashboard.Domain.Seedwork;
using System.Data.Entity.Infrastructure;

namespace Dashboard.Domain.Tests.Unit
{
    [TestClass()]
    public class VacancyServiceTests
    {
        private Mock<IVacancyRepository> _vacancyRepository = new Mock<IVacancyRepository>();
        private Vacancy _vacancy = new Vacancy("Fullstack Developer", "Java", "Remote",
            "13/08/2022", 1, "Open", "GS", "GS");

        [TestMethod()]
        public async Task GetVacancyAsync_WithValidID_ReturnsVacancyDTO()
        {
            _vacancyRepository.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(_vacancy);

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetVacancyAsync(It.IsAny<int>(), CancellationToken.None);

            Assert.AreEqual("Fullstack Developer", endResult.Name);
        }

        [TestMethod()]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task GetVacancyAsync_WithInvalidID_ReturnsKeyNotFoundException()
        {
            _vacancy = null;
            _vacancyRepository.Setup(c => c.GetAsync(It.IsAny<int>(), CancellationToken.None)).Throws(new KeyNotFoundException());

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetVacancyAsync(It.IsAny<int>(), CancellationToken.None);
        }

        [TestMethod()]
        public async Task GetVacancies_VacanciesListExists_ReturnsVacancyList()
        {
            List<Vacancy> vacancies = new List<Vacancy>();
            vacancies.Add(_vacancy);
            vacancies.Add(_vacancy);
            _vacancyRepository.Setup(c => c.GetAllVacancies()).ReturnsAsync(vacancies);

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetAllVacancies();

            Assert.AreEqual(2, endResult.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetVacancies_VacanciesListExists_ReturnsArgumentNullException()
        {
            _vacancyRepository.Setup(c => c.GetAllVacancies()).Throws(new ArgumentNullException());

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetAllVacancies();
        }

        [TestMethod()]
        public async Task GetVacancies_VacanciesFromValidProjectID_ReturnsVacancyList()
        {
            List<Vacancy> vacancies = new List<Vacancy>();
            vacancies.Add(_vacancy);
            vacancies.Add(_vacancy);

            _vacancyRepository.Setup(c => c.GetAllVacanciesPerProject(It.IsAny<int>())).ReturnsAsync(vacancies);

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetAllVacanciesByProject(It.IsAny<int>());

            Assert.AreEqual(2, endResult.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetVacancies_VacanciesFromInvalidProjectID_ReturnsKeyNotFoundException()
        {
            _vacancyRepository.Setup(c => c.GetAllVacanciesPerProject(It.IsAny<int>())).Throws(new ArgumentNullException());

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.GetAllVacanciesByProject(It.IsAny<int>());
        }

        [TestMethod()]
        public async Task AddVacancy_NoExistingVacancyExists_ReturnsOk()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            _vacancyRepository.Setup(v => v.UnitOfWork).Returns(unitOfWorkMock.Object);
            _vacancyRepository.Setup(v => v.Add(_vacancy)).Returns(_vacancy);

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.AddVacancy(_vacancy);

            Assert.IsTrue(endResult.Name == "Fullstack Developer");
        }

        [TestMethod()]
        [ExpectedException(typeof(DbUpdateException))]
        public async Task AddVacancy_FailsToAddVacancy_ReturnsDbUpdateException()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            _vacancyRepository.Setup(v => v.UnitOfWork).Throws(new DbUpdateException());
            _vacancyRepository.Setup(v => v.Add(_vacancy)).Returns(_vacancy);

            IVacancyService vacancyService = new VacancyService(_vacancyRepository.Object);

            var endResult = await vacancyService.AddVacancy(_vacancy);
        }
    }
}