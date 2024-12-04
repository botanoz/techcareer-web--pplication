using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using Core.Security.Entities;
using Xunit;
using TechCareer.Service.Concretes;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.Service.Tests.UnitTests
{
    public class JobServiceTests
    {
        private readonly Mock<IJobRepository> _mockJobRepository;
        private readonly JobService _jobService;

        public JobServiceTests()
        {
            _mockJobRepository = new Mock<IJobRepository>();
            _jobService = new JobService(_mockJobRepository.Object);  // Doğru repository'yi mock'lıyoruz
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedJob()
        {
            // Arrange
            var newJob = new Job
            {
                Title = "Software Developer",
                TypeOfWork = 1,
                YearsOfExperience = 3,
                WorkPlace = 2,
                StartDate = DateTime.UtcNow,
                Content = "Develop software solutions",
                Description = "Work on various projects",
                Skills = "C#, SQL, .NET",
                CompanyId = 1
            };

            _mockJobRepository
                .Setup(service => service.AddAsync(It.IsAny<Job>()))
                .ReturnsAsync(newJob);

            // Act
            var result = await _jobService.AddAsync(newJob);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Software Developer", result.Title);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnJob()
        {
            // Arrange
            var job = new Job
            {
                Id = 1,
                Title = "Software Developer",
                TypeOfWork = 1,
                YearsOfExperience = 3,
                WorkPlace = 2,
                StartDate = DateTime.UtcNow,
                Content = "Develop software solutions",
                Description = "Work on various projects",
                Skills = "C#, SQL, .NET",
                CompanyId = 1
            };
                            _mockJobRepository
                .Setup(service => service.GetAsync(
                It.IsAny<Expression<Func<Job, bool>>>(), // predicate
                false,                                   // include
                false,                                   // withDeleted
                true,                                    // enableTracking
                It.IsAny<CancellationToken>()           // cancellationToken
                ))
                .ReturnsAsync(job);





            // Act
            var result = await _jobService.GetAsync(x => x.Id == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Software Developer", result.Title);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnJobList()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job { Id = 1, Title = "Software Developer" },
                new Job { Id = 2, Title = "Product Manager" }
            };

            _mockJobRepository
         .Setup(service => service.GetListAsync(
             It.IsAny<Expression<Func<Job, bool>>>(),  // predicate
             null,                                    // orderBy
             false,                                   // include
             false,                                   // withDeleted
             true,                                    // enableTracking
             It.IsAny<CancellationToken>()           // cancellationToken
         ))
         .ReturnsAsync(jobs);


            // Act
            var result = await _jobService.GetListAsync(x => x.TypeOfWork == 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var jobs = new List<Job>
            {
                new Job { Id = 1, Title = "Software Developer" },
                new Job { Id = 2, Title = "Product Manager" },
                new Job { Id = 3, Title = "Data Analyst" }
            };

                        _mockJobRepository
             .Setup(service => service.GetListAsync(
                 null,                                     // predicate
                 null,                                     // orderBy
                 false,                                    // include
                 false,                                    // withDeleted
                 true,                                     // enableTracking
                 It.IsAny<CancellationToken>()            // cancellationToken
             ))
             .ReturnsAsync(jobs);


            // Act
            var result = await _jobService.GetPaginateAsync(index: 0, size: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkAsDeleted()
        {
            // Arrange
            var job = new Job
            {
                Id = 2,
                Title = "Software Developer",
                IsDeleted = false,
                TypeOfWork = 1,
                YearsOfExperience = 3,
                WorkPlace = 2,
                StartDate = DateTime.UtcNow,
                Content = "Develop software solutions",
                Description = "Work on various projects",
                Skills = "C#, SQL, .NET",
                CompanyId = 1
            };

                    _mockJobRepository
            .Setup(service => service.GetListAsync(
                It.IsAny<Expression<Func<Job, bool>>>(), // predicate
                null,                                   // orderBy
                false,                                  // include
                false,                                  // withDeleted
                true,                                   // enableTracking
                It.IsAny<CancellationToken>()          // cancellationToken
            ))
            .ReturnsAsync(new List<Job> { job });

            // Act
            var result = await _jobService.DeleteAsync(job);

            // Assert
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotImplementedException()
        {
            // Arrange
            var job = new Job
            {
                Id = 1,
                Title = "Software Developer"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotImplementedException>(() => _jobService.UpdateAsync(job));
            Assert.Equal("The method or operation is not implemented.", exception.Message);
        }
    }
}
