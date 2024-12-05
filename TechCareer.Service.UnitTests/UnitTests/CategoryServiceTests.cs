using System.Linq.Expressions;
using Xunit;
using Moq;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using TechCareer.Service.Rules;
using TechCareer.DataAccess.Repositories.Abstracts;
using Core.Security.Entities;
using System.Collections.Generic;
using FluentAssertions;
using System.Linq;
using TechCareer.Models.Dtos.Category;

namespace TechCareer.Service.Tests.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<CategoryBusinessRules> _mockCategoryBusinessRules;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockCategoryBusinessRules = new Mock<CategoryBusinessRules>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockCategoryBusinessRules.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedCategory()
        {
            // Arrange
            var categoryAddRequestDto = new CategoryAddRequestDto { Name = "TestCategory" };
            var category = new Category { Id = 1, Name = "TestCategory" };

            // Mock: AddAsync
            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                                   .ReturnsAsync(category);

            // Act
            var result = await _categoryService.AddAsync(categoryAddRequestDto);

            // Assert
            result.Should().BeEquivalentTo(category);  // Beklenen ve dönen sonuçları karşılaştırma
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);  // AddAsync'ın bir kez çağrıldığını doğrulama
        }


        [Fact]
        public async Task GetListAsync_ShouldReturnCategoryList()
        {
            // Arrange
            var categories = new List<Category>
    {
        new Category { Id = 1, Name = "Category1" },
        new Category { Id = 2, Name = "Category2" }
    };

            // Mock: Setting up GetListAsync to return categories
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetListAsync();

            // Assert
            result.Should().BeEquivalentTo(categories.Select(c => new CategoryResponseDto { Id = c.Id, Name = c.Name }));
            _mockCategoryRepository.Verify(repo => repo.GetListAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
        [Fact]
        public async Task GetAsync_ShouldReturnCategoryById()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1" };

            _mockCategoryRepository
                .Setup(repo => repo.GetAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),   // Include parameter
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetAsync(
                x => x.Id == 1,
                withDeleted: false,
                cancellationToken: CancellationToken.None
            );

            // Assert
            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = category.Id, Name = category.Name });

            _mockCategoryRepository.Verify(repo => repo.GetAsync(
                It.Is<Expression<Func<Category, bool>>>(expr => expr.Compile()(category)),
                It.IsAny<bool>(), // Include parameter
                false,            // WithDeleted
                It.IsAny<bool>(), // EnableTracking
                CancellationToken.None
            ), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnCategoryWithDeletedFlag()
        {
            // Arrange
            var categoryRequestDto = new CategoryRequestDto { Id = 1 };
            var category = new Category { Id = 1, Name = "Category1", IsDeleted = false };

            // Mock: GetListAsync to retrieve the category
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    null,
                    false,
                    false,
                    true,
                    default))
                .ReturnsAsync(new List<Category> { category });

            // Mock: UpdateAsync method to simulate soft deletion
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.DeleteAsync(categoryRequestDto, permanent: false);

            // Assert
            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = category.Id, Name = category.Name });
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "OldName" };
            var updateRequestDto = new CategoryUpdateRequestDto { Id = 1, Name = "NewName" };
            var updatedCategory = new Category { Id = 1, Name = "NewName" };

            // Mock: GetListAsync to retrieve the category to update
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    null,
                    false,
                    false,
                    true,
                    default))
                .ReturnsAsync(new List<Category> { existingCategory });

            // Mock: UpdateAsync to return the updated category
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(updatedCategory);

            // Act
            var result = await _categoryService.UpdateAsync(updateRequestDto);

            // Assert
            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = updatedCategory.Id, Name = updatedCategory.Name });
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }


    }

}