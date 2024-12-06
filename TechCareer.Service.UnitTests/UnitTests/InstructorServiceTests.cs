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
            // Arrange
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

            // Act
            var result = await _instructorService.AddAsync(instructorDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(instructor.Id, result.Id);
            Assert.Equal(instructor.Name, result.Name);
            Assert.Equal(instructor.About, result.About);

            // Verify
            _mockInstructorRepository.Verify(service => service.AddAsync(It.IsAny<Instructor>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkInstructorAsDeleted_WhenPermanentIsFalse()
        {
            // Arrange
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

            // GetAsync mock setup: Predicate'i doğru şekilde eşleştiriyoruz
            _mockInstructorRepository.Setup(service => service.GetAsync(
                It.Is<Expression<Func<Instructor, bool>>>(predicate => predicate.Compile().Invoke(instructor)), // Predicate'in doğru eşleştiğinden emin olun
                false, // include parametresi
                false, // withDeleted parametresi
                true,  // enableTracking parametresi
                default)) // cancellationToken
            .ReturnsAsync(instructor); // Mocklanan instructor objesini döndürüyoruz

            // UpdateAsync mock setup: IsDeleted flag'ını true yapıyoruz
            _mockInstructorRepository.Setup(service => service.UpdateAsync(It.Is<Instructor>(i => i.IsDeleted == true)))
                .ReturnsAsync(new Instructor { Id = instructor.Id, IsDeleted = true });

            // Act
            var result = await _instructorService.DeleteAsync(instructorRequestDto, permanent: false);

            // Assert
            Assert.NotNull(result);  // Result null olmamalı
            _mockInstructorRepository.Verify(service => service.UpdateAsync(It.Is<Instructor>(i => i.IsDeleted == true)), Times.Once);  // UpdateAsync doğru çağrıldığından emin olun
            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);  // GetAsync doğru çağrıldığından emin olun
        }

        [Fact]
        public async Task GetAsync_ShouldReturnInstructorResponseDto_WhenInstructorExists()
        {
            // Arrange: Test için kullanılacak Instructor verisini oluşturuyoruz
            var instructor = new Instructor
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),  // İstediğiniz ID ile
                Name = "Hamit Mızrak",
                About = "Test instructor"
            };

            // InstructorResponseDto'yu oluşturuyoruz
            var expectedResponse = new InstructorResponseDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                About = instructor.About
            };

            // GetAsync metodunun doğru şekilde mock'lanması
            _mockInstructorRepository.Setup(service => service.GetAsync(
                    It.Is<Expression<Func<Instructor, bool>>>(predicate => predicate.Compile().Invoke(instructor)),  // Predikatı doğru şekilde mock'lıyoruz
                    false,
                    false,
                    true,
                    default))
                .ReturnsAsync(instructor);  // Bu, metodun doğru Instructor döndürmesini sağlar

            // Act: GetAsync metodunu çağırıyoruz
            var result = await _instructorService.GetAsync(x => x.Id == instructor.Id);  // Burada GetAsync çağrısını yapıyoruz

            // Assert: Sonuçları doğruluyoruz
            Assert.NotNull(result);  // Sonuç null olmamalıdır
            Assert.Equal(expectedResponse.Id, result.Id);  // Dönen InstructorResponseDto'nun Id'si doğru olmalı
            Assert.Equal(expectedResponse.Name, result.Name);  // Dönen InstructorResponseDto'nun adı doğru olmalı
            Assert.Equal(expectedResponse.About, result.About);  // Dönen InstructorResponseDto'nun hakkında doğru olmalı

            // Verify: GetAsync metodunun doğru şekilde çağrıldığını kontrol ediyoruz
            _mockInstructorRepository.Verify(service => service.GetAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), false, false, true, default), Times.Once);
        }



        [Fact]
        public async Task GetListAsync_ShouldReturnListOfInstructors()
        {
            // Arrange
            var instructors = new List<Instructor>
            {
                new Instructor { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Hamit Mızrak" },
                new Instructor { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Ahmet Kaya" }
            };

            // GetListAsync mock setup
            _mockInstructorRepository.Setup(service => service.GetListAsync(
                It.IsAny<Expression<Func<Instructor, bool>>>(),
                null,
                false,
                false,
                true,
                default))
            .ReturnsAsync(instructors);

            // Act
            var result = await _instructorService.GetListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Name == "Hamit Mızrak");
            Assert.Contains(result, x => x.Name == "Ahmet Kaya");

            // Verify
            _mockInstructorRepository.Verify(service => service.GetListAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task GetPaginateAsync_ShouldReturnPaginatedResult()
        {
            // Arrange
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

            // Act
            var result = await _instructorService.GetPaginateAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalItems);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.TotalPages);

            // Verify
            _mockInstructorRepository.Verify(service => service.GetPaginateAsync(It.IsAny<Expression<Func<Instructor, bool>>>(), null, false, 0, 10, false, true, default), Times.Once);
        }
    }
}
