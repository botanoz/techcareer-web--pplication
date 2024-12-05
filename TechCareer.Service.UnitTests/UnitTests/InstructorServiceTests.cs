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
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Instructor;

namespace TechCareer.Service.Tests.UnitTests
{

    public class InstructorServiceTests
    {
        private readonly Mock<IInstructorRepository> _mockInstructorRepository;
        private readonly InstructorService _instructorService;

        public InstructorServiceTests()
        {
            // IInstructorRepository mock'ı oluşturuyoruz
            _mockInstructorRepository = new Mock<IInstructorRepository>();

            // InstructorService'i doğru bağımlılıkla oluşturuyoruz
            _instructorService = new InstructorService(_mockInstructorRepository.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedInstructor()
        {

            var instructor = new Instructor { Id = Guid.NewGuid(), Name = "Test Instructor" };
            _mockInstructorRepository.Setup(service => service.AddAsync(It.IsAny<Instructor>()))
                .ReturnsAsync(instructor);


            var result = await _instructorService.AddAsync(instructor);


            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            _mockInstructorRepository.Verify(service => service.AddAsync(It.IsAny<Instructor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkInstructorAsDeleted()
        {
            var instructor = new Instructor { Id = Guid.NewGuid(), IsDeleted = false };
            var deleteRequestDto = new InstructorDeleteRequestDto { Id = instructor.Id, Permanent = false };

            _mockInstructorRepository.Setup(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default))
                .ReturnsAsync(instructor);

            _mockInstructorRepository.Setup(service => service.UpdateAsync(It.IsAny<Instructor>()))
                .ReturnsAsync(new Instructor { Id = instructor.Id, IsDeleted = true });

            var result = await _instructorService.DeleteAsync(deleteRequestDto);

            Assert.NotNull(result);
            Assert.True(result.IsDeleted); 

            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);
            _mockInstructorRepository.Verify(service => service.UpdateAsync(It.IsAny<Instructor>()), Times.Once);
        }


        [Fact]
        public async Task GetAsync_ShouldReturnInstructor()
        {

            var instructor = new Instructor { Id = Guid.NewGuid(), Name = "Test Instructor" };
            _mockInstructorRepository.Setup(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default))
                .ReturnsAsync(instructor);


            var result = await _instructorService.GetAsync(x => x.Id == instructor.Id);


            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnListOfInstructors()
        {

            var instructors = new List<Instructor>
        {
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 1" },
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 2" }
        };
            _mockInstructorRepository.Setup(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default))
                .ReturnsAsync(instructors);


            var result = await _instructorService.GetListAsync();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockInstructorRepository.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResult()
        {

            var instructors = new List<Instructor>
        {
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 1" },
            new Instructor { Id = Guid.NewGuid(), Name = "Instructor 2" }
        };
            _mockInstructorRepository.Setup(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default))
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
            _mockInstructorRepository.Verify(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default), Times.Once);
        }
    }
}