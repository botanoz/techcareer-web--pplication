using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using TechCareer.Models.Dtos.Instructor;
using Xunit;
using Core.Persistence.Extensions;
using Core.CrossCuttingConcerns.Serilog;

namespace TechCareer.Service.Tests.UnitTests
{
    public class InstructorServiceTests
    {
        private readonly Mock<IInstructorRepository> _mockInstructorRepository;
        private readonly Mock<LoggerServiceBase> _mockLogger;
        private readonly InstructorService _instructorService;

        public InstructorServiceTests()
        {
           
            _mockInstructorRepository = new Mock<IInstructorRepository>();

          
            _mockLogger = new Mock<LoggerServiceBase>();

   
            _instructorService = new InstructorService(_mockInstructorRepository.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task AddAsync_ShouldReturnAddedInstructor_WhenGivenValidDto()
        {
        
            var instructorDto = new InstructorAddRequestDto
            {
                Name = "Test Instructor",
                About = "This is a test instructor."
            };

            var instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                Name = instructorDto.Name,
                About = instructorDto.About
            };

            _mockInstructorRepository.Setup(service => service.AddAsync(It.IsAny<Instructor>()))
                .ReturnsAsync(instructor);

           
            var result = await _instructorService.AddAsync(instructorDto);

         
            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            Assert.Equal(instructor.About, result.About);

         
            _mockInstructorRepository.Verify(service => service.AddAsync(It.IsAny<Instructor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkInstructorAsDeleted_WhenPermanentIsFalse()
        {
           
            var instructor = new Instructor
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Hamit Mızrak",
                IsDeleted = false
            };

            var instructorRequestDto = new InstructorRequestDto
            {
                Id = instructor.Id
            };

        
            _mockInstructorRepository.Setup(service => service.GetAsync(
                It.Is<Expression<Func<Instructor, bool>>>(predicate => predicate.Compile().Invoke(instructor)), 
                false, 
                false, 
                true, 
                default)) 
            .ReturnsAsync(instructor); 

            _mockInstructorRepository.Setup(service => service.UpdateAsync(It.Is<Instructor>(i => i.IsDeleted == true)))
                .ReturnsAsync(new Instructor { Id = instructor.Id, IsDeleted = true });

       
            var result = await _instructorService.DeleteAsync(instructorRequestDto, permanent: false);

       
            Assert.NotNull(result);  
            _mockInstructorRepository.Verify(service => service.UpdateAsync(It.Is<Instructor>(i => i.IsDeleted == true)), Times.Once);  
            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);  
        }

        [Fact]
        public async Task GetAsync_ShouldReturnInstructorResponseDto_WhenInstructorExists()
        {
           
            var instructor = new Instructor
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),  
                Name = "Hamit Mızrak",
                About = "Test instructor"
            };

           
            var expectedResponse = new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                About = instructor.About
            };

     
            _mockInstructorRepository.Setup(service => service.GetAsync(
                    It.Is<Expression<Func<Instructor, bool>>>(predicate => predicate.Compile().Invoke(instructor)),  
                    false,
                    false,
                    true,
                    default))
                .ReturnsAsync(instructor); 

          
            var result = await _instructorService.GetAsync(x => x.Id == instructor.Id); 

            Assert.NotNull(result); 
            Assert.Equal(expectedResponse.Id, result.Id);  
            Assert.Equal(expectedResponse.Name, result.Name); 
            Assert.Equal(expectedResponse.About, result.About); 

            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);
        }



        [Fact]
        public async Task GetListAsync_ShouldReturnListOfInstructors()
        {
         
            var instructors = new List<Instructor>
            {
                new Instructor { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Hamit Mızrak" },
                new Instructor { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Ahmet Kaya" }
            };

       
            _mockInstructorRepository.Setup(service => service.GetListAsync(
                It.IsAny<Expression<Func<Instructor, bool>>>(),
                null,
                false,
                false,
                true,
                default))
            .ReturnsAsync(instructors);

           
            var result = await _instructorService.GetListAsync();

     
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Name == "Hamit Mızrak");
            Assert.Contains(result, x => x.Name == "Ahmet Kaya");

       
            _mockInstructorRepository.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResult()
        {
           
            var instructors = new List<Instructor>
            {
                new Instructor { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Hamit Mızrak" },
                new Instructor { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Ahmet Kaya" }
            };

            _mockInstructorRepository.Setup(service => service.GetPaginateAsync(
                It.IsAny<Expression<Func<Instructor, bool>>>(),
                null,
                false,
                0,
                10,
                false,
                true,
                default))
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
            Assert.Equal(1, result.TotalPages);

      
            _mockInstructorRepository.Verify(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default), Times.Once);
        }
    }
}
