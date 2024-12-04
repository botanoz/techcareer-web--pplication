using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using Xunit;
using Core.Security.Entities;
using Core.Persistence.Extensions;
using TechCareer.Service.Concretes;

namespace TechCareer.Service.Tests.UnitTests
{

    public class InstructorServiceTests
    {
        private readonly Mock<IInstructorService> _mockInstructorService;
        private readonly InstructorService _instructorService;

        public InstructorServiceTests()
        {

            _mockInstructorService = new Mock<IInstructorService>();

            _instructorService = new InstructorService(_mockInstructorService.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedInstructor()
        {

            var instructor = new Instructor { Id = Guid.NewGuid(), Name = "Test Instructor" };
            _mockInstructorService.Setup(service => service.AddAsync(It.IsAny<Instructor>()))
                .ReturnsAsync(instructor);


            var result = await _instructorService.AddAsync(instructor);


            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            _mockInstructorService.Verify(service => service.AddAsync(It.IsAny<Instructor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkInstructorAsDeleted()
        {

            var instructor = new Instructor { Id = Guid.NewGuid(), IsDeleted = false };
            _mockInstructorService.Setup(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default))
                .ReturnsAsync(new List<Instructor> { instructor });

            _mockInstructorService.Setup(service => service.DeleteAsync(It.IsAny<Instructor>(), false))
                .ReturnsAsync(new Instructor { Id = instructor.Id, IsDeleted = true });


            var result = await _instructorService.DeleteAsync(instructor);


            Assert.NotNull(result);
            Assert.True(result.IsDeleted);
            _mockInstructorService.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default), Times.Once);
            _mockInstructorService.Verify(service => service.DeleteAsync(It.IsAny<Instructor>(), false), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnInstructor()
        {

            var instructor = new Instructor { Id = Guid.NewGuid(), Name = "Test Instructor" };
            _mockInstructorService.Setup(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default))
                .ReturnsAsync(instructor);


            var result = await _instructorService.GetAsync(x => x.Id == instructor.Id);


            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            _mockInstructorService.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnListOfInstructors()
        {

            var instructors = new List<Instructor>
        {
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 1" },
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 2" }
        };
            _mockInstructorService.Setup(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default))
                .ReturnsAsync(instructors);


            var result = await _instructorService.GetListAsync();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockInstructorService.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResult()
        {

            var instructors = new List<Instructor>
        {
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 1" },
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 2" }
        };
            _mockInstructorService.Setup(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default))
                .ReturnsAsync(new Paginate<Instructor>
                {
                    Items = instructors,
                    Index = 0,
                    Size = 10,
                    TotalItems = 2,
                    TotalPages = 1
                });


            var result = await _instructorService.GetPaginateAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
            _mockInstructorService.Verify(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default), Times.Once);
        }
    }
}