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
            var category = new Category { Id = 1, Name = "TestCategory" };
            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                                   .ReturnsAsync(category);

            // Act
            var result = await _categoryService.AddAsync(category);

            // Assert
            result.Should().BeEquivalentTo(category);
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
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
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetListAsync();

            // Assert
            result.Should().BeEquivalentTo(categories);
            _mockCategoryRepository.Verify(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnCategoryById()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1" };
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default))
                                   .ReturnsAsync(new List<Category> { category });

            // Act
            var result = await _categoryService.GetAsync(x => x.Id == 1, category);

            // Assert
            result.Should().BeEquivalentTo(category);
            _mockCategoryRepository.Verify(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldMarkCategoryAsDeleted()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category1", IsDeleted = false };
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default))
                                   .ReturnsAsync(new List<Category> { category });

            // Act
            var result = await _categoryService.DeleteAsync(category);

            // Assert
            result.IsDeleted.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
            // Arrange
            var existingCategory = new Category { Id = 1, Name = "OldName" };
            var updatedCategory = new Category { Id = 1, Name = "NewName" };
            _mockCategoryRepository.Setup(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default))
                                   .ReturnsAsync(new List<Category> { existingCategory });

            // Act
            var result = await _categoryService.UpdateAsync(updatedCategory);

            // Assert
            result.Name.Should().Be("NewName");
            _mockCategoryRepository.Verify(repo => repo.GetListAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, false, false, true, default), Times.Once);
        }
    }

}