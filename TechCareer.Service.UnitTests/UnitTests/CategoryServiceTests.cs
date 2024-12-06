using Xunit;
using Moq;
using System.Threading.Tasks;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Category;
using Core.Security.Entities;
using FluentAssertions;
using Core.CrossCuttingConcerns.Serilog;
using System.Linq.Expressions;
using TechCareer.Service.Rules;
using System.Collections.Generic;
using System.Threading;

namespace TechCareer.Service.Tests.UnitTests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<CategoryBusinessRules> _mockCategoryBusinessRules;
        private readonly Mock<LoggerServiceBase> _mockLoggerService;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockCategoryBusinessRules = new Mock<CategoryBusinessRules>(_mockCategoryRepository.Object); 
            _mockLoggerService = new Mock<LoggerServiceBase>();
            _categoryService = new CategoryService(
                _mockCategoryRepository.Object,
                _mockCategoryBusinessRules.Object,
                _mockLoggerService.Object
            );
        }

        [Fact]
        public async Task AddAsync_ShouldReturnAddedCategory()
        {
           
            var categoryAddRequestDto = new CategoryAddRequestDto { Name = "TestCategory" };
            var category = new Category { Id = 1, Name = "TestCategory" };

            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                                   .ReturnsAsync(category);

        
            var result = await _categoryService.AddAsync(categoryAddRequestDto);

        
            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = category.Id, Name = category.Name });
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.IsAny<Category>()), Times.Once);
            _mockLoggerService.Verify(logger => logger.Info(It.Is<string>(s => s.Contains("Category added"))), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ShouldReturnCategoryList()
        {
           
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };

            _mockCategoryRepository.Setup(repo => repo.GetListAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            
            var result = await _categoryService.GetListAsync();

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
        public async Task DeleteAsync_ShouldMarkCategoryAsDeleted()
        {
          
            var categoryRequestDto = new CategoryRequestDto { Id = 1 };
            var category = new Category { Id = 1, Name = "Category1", IsDeleted = false };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(
                x => x.Id == categoryRequestDto.Id, true, false, true, default))
                .ReturnsAsync(category);

            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);

       
            var result = await _categoryService.DeleteAsync(categoryRequestDto);

            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = category.Id, Name = category.Name });
            category.IsDeleted.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.IsDeleted == true)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCategory()
        {
      
            var existingCategory = new Category { Id = 1, Name = "OldName" };
            var updateRequestDto = new CategoryUpdateRequestDto { Id = 1, Name = "NewName" };
            var updatedCategory = new Category { Id = 1, Name = "NewName" };

            _mockCategoryRepository.Setup(repo => repo.GetAsync(
                x => x.Id == updateRequestDto.Id,
                true, 
                false, 
                true, 
                default)) 
                .ReturnsAsync(existingCategory);

            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync(updatedCategory);

       
            var result = await _categoryService.UpdateAsync(updateRequestDto);

         
            result.Should().BeEquivalentTo(new CategoryResponseDto { Id = updatedCategory.Id, Name = updatedCategory.Name });
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.Name == "NewName")), Times.Once);
        }
    }
}
