using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OptoBasicNotesApi.Application.DTOs;
using OptoBasicNotesApi.Application.Interfaces;
using OptoBasicNotesApi.Application.Mappings;
using OptoBasicNotesApi.Controllers;
using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.UnitTests
{
    [TestClass]
    public class CategoriesControllerTests
    {
        /// <summary>
        /// Ensure categories are returned from this method
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAllCategories_Returns_Categories()
        {
            //Arrange
            var categoriesController = BuildTestCategoriesController();

            //Act
            var result = await categoriesController.GetAllCategories();

            //Assert
            if (result.Result is not OkObjectResult)
            {
                Assert.Fail("Result is not a OK result");
                return;
            }

            var resultAs = (result.Result as OkObjectResult);
            if (resultAs == null || resultAs.Value is not IList<CategoryDto>)
            {
                Assert.Fail("Result value is not a IList<CategoryDto> result");
                return;
            }

            var valueAs = resultAs.Value as IList<CategoryDto>;
            Assert.IsTrue(valueAs != null && valueAs.Count == 3);
        }


        [TestMethod]
        public async Task CreateCategory_Returns_BadRequest_When_Category_Already_Exists()
        {
            //Arrange
            var categoriesController = BuildTestCategoriesController();

            //Act
            var result = await categoriesController.CreateCategory(new CreateCategoryDto
            {
                CategoryName = "mock category 1"
            });

            //Assert
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        private CategoriesController BuildTestCategoriesController()
        {
            IList<Category> categories = new List<Category>()
            {
                new Category
                {
                    Id = 1,
                    CategoryName = "mock category 1"
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "mock category 2"
                },
                new Category
                {
                    Id = 3,
                    CategoryName = "mock category 3"
                }
            };

            var logger = new Mock<ILogger<NotesController>>();

            //no need to mock automapper, can just load the profile and give a real one.
            var myProfile = new AppMappings();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(m => m.GetAllAsync()).ReturnsAsync(categories);
            mockCategoryService.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(categories[0]);

            return new CategoriesController(logger.Object, mapper, mockCategoryService.Object);
        }
    }
}