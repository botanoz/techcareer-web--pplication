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
        public async Task AddAsync_ShouldReturnAddedInstructor_WhenGivenValidDto()
        {
            // Arrange: Test için kullanılacak DTO'yu hazırlıyoruz
            var instructorDto = new InstructorAddRequestDto
            {
                Name = "Test Instructor",
                About = "This is a test instructor."
            };

            // AddAsync metodu çağrıldığında yeni bir Instructor döndürmesini sağlıyoruz
            var instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                Name = instructorDto.Name,
                About = instructorDto.About
            };

            _mockInstructorRepository.Setup(service => service.AddAsync(It.IsAny<Instructor>()))
                .ReturnsAsync(instructor);

            // Act: AddAsync metodunu çağırıyoruz
            var result = await _instructorService.AddAsync(instructorDto);

            // Assert: Sonuçları doğruluyoruz
            Assert.NotNull(result);  // Sonucun null olmadığını kontrol ediyoruz
            Assert.Equal(instructor.Id, result.Id);  // Dönen Instructor'Id'sinin doğru olduğunu kontrol ediyoruz
            Assert.Equal(instructor.Name, result.Name);  // Dönen Instructor'Name'inin doğru olduğunu kontrol ediyoruz
            Assert.Equal(instructor.About, result.About);  // Dönen Instructor'About'un doğru olduğunu kontrol ediyoruz

            // Verify: AddAsync metodunun bir kez çağrıldığını kontrol ediyoruz
            _mockInstructorRepository.Verify(service => service.AddAsync(It.IsAny<Instructor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkInstructorAsDeleted_WhenPermanentIsFalse()
        {
            // Arrange
            var instructor = new Instructor
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Geçerli bir GUID kullanıyoruz
                IsDeleted = false
            };

            var instructorRequestDto = new InstructorRequestDto
            {
                Id = instructor.Id
            };

            // GetAsync mock setup: Predicate'ı doğrudan ID ile eşleştiriyoruz.
            _mockInstructorRepository.Setup(service => service.GetAsync(
                It.Is<Expression<Func<Instructor, bool>>>(predicate => predicate.Compile().Invoke(instructor)),
                false,
                false,
                true,
                default
            ))
            .ReturnsAsync(instructor);

            // UpdateAsync mock setup: IsDeleted flag'ını doğru şekilde güncelliyoruz.
            _mockInstructorRepository.Setup(service => service.UpdateAsync(It.Is<Instructor>(i => i.Id == instructor.Id && i.IsDeleted == true)))
                .ReturnsAsync(new Instructor { Id = instructor.Id, IsDeleted = true });

            // Act: DeleteAsync metodunu çağırıyoruz
            var result = await _instructorService.DeleteAsync(instructorRequestDto, permanent: false);

            // Assert: Sonuçları doğruluyoruz
            Assert.NotNull(result);

            // Result'un IsDeleted flag'ını kontrol etmiyoruz, çünkü InstructorResponseDto'da böyle bir alan yok.
            // Bunun yerine, UpdateAsync'in doğru şekilde çağrıldığını kontrol ediyoruz.
            _mockInstructorRepository.Verify(service => service.UpdateAsync(It.Is<Instructor>(i => i.IsDeleted == true)), Times.Once);

            // GetAsync ve UpdateAsync metodlarının doğru çağrıldığını kontrol ediyoruz
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