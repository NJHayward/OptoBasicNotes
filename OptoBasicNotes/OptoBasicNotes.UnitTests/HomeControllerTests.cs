using Microsoft.AspNetCore.Mvc;
using Moq;
using OptoBasicNotes.Controllers;
using OptoBasicNotes.Core.Interfaces;
using OptoBasicNotes.Core.Models;
using OptoBasicNotes.Core.Services;
using OptoBasicNotes.Models;
using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotes.UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        /// <summary>
        /// Index must build the category select list when initially loading the form
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Index_Builds_SelectList()
        {
            //Arrange
            var mockNotesApi = BuildMockNotesApi();
            var homecontroller = new HomeController(null, mockNotesApi.Object);

            //Act
            var indexResult = await homecontroller.Index();

            //Assert
            Assert.IsTrue(((indexResult as ViewResult)?.Model as IndexViewModel)?.Categories.Count == 3);
        }

        /// <summary>
        /// Must ensure CreateNot will not create a note when there is no NoteNext
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateNote_Doesnt_Create_On_Invalid_Model()
        {
            //Arrange
            var mockNotesApi = BuildMockNotesApi();
            var homecontroller = new HomeController(null, mockNotesApi.Object);
            var indexViewModel = new IndexViewModel()
            {
                NoteText = "",
                Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>()
                {
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 1", Value = "1", Selected = true },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 2", Value = "2", Selected = true },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 3", Value = "3", Selected = true }
                }
            };

            //Act
            SimulateValidation(indexViewModel, homecontroller);
            var result = await homecontroller.CreateNote(indexViewModel);

            //Assert
            Assert.IsFalse(result);
        }
        
        /// <summary>
        /// Must ensure CreateNot will not create a note when no categories are selected
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateNote_Doesnt_Create_Without_Category()
        {
            //Arrange
            var mockNotesApi = BuildMockNotesApi();
            var homecontroller = new HomeController(null, mockNotesApi.Object);
            var indexViewModel = new IndexViewModel()
            {
                NoteText = "My note text",
                Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>()
                {
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 1", Value = "1", Selected = false },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 2", Value = "2", Selected = false },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 3", Value = "3", Selected = false }
                }
            };

            //Act
            var result = await homecontroller.CreateNote(indexViewModel);

            //Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Must ensure will all criteria met the note will be created
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateNote_Does_Create_With_All_Valid()
        {
            //Arrange
            var mockNotesApi = BuildMockNotesApi();
            var homecontroller = new HomeController(null, mockNotesApi.Object);
            var indexViewModel = new IndexViewModel()
            {
                NoteText = "My note text",
                Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>()
                {
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 1", Value = "1", Selected = true },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 2", Value = "2", Selected = true },
                    new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem() { Text = "mock category 3", Value = "3", Selected = true }
                }
            };

            //Act
            var result = await homecontroller.CreateNote(indexViewModel);

            //Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Must ensure that script tags are replaced to stop XSS.  Must also ensure that there are no script tags left int he case of multiple script tags.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetNotesPartial_Doesnt_Return_Html_With_Script()
        {
            //Arrange
            var mockNotesApi = BuildMockNotesApi();
            var homecontroller = new HomeController(null, mockNotesApi.Object);

            //Act
            var result = await homecontroller.GetNotesPartial();

            //Assert
            Assert.IsTrue((result.Model as List<NotePartialViewModel>)?.Any(x => x.NoteBody.Contains("&lt;script&gt;"))); //check opening tag is converted
            Assert.IsTrue((result.Model as List<NotePartialViewModel>)?.Any(x => x.NoteBody.Contains("&lt;/script&gt;"))); //check closing tag is converted
            Assert.IsFalse((result.Model as List<NotePartialViewModel>)?.Any(x => x.NoteBody.Contains("<script>"))); //check no unconverted opening tag
            Assert.IsFalse((result.Model as List<NotePartialViewModel>)?.Any(x => x.NoteBody.Contains("</script>"))); //check no unconverted closing tag
        }

        private Mock<IOptoBasicNotesApi> BuildMockNotesApi()
        {
            IList<CategoryModel> categoryModels = new List<CategoryModel>()
            {
                new CategoryModel
                {
                    Id = 1,
                    CategoryName = "mock category 1"
                },
                new CategoryModel
                {
                    Id = 2,
                    CategoryName = "mock category 2"
                },
                new CategoryModel
                {
                    Id = 3,
                    CategoryName = "mock category 3"
                }
            };

            IList<AllNotesItemModel> noteModels = new List<AllNotesItemModel>()
            {
                new AllNotesItemModel
                {
                    Id = 1,
                    NoteBody = "mock note model 1 <script>alert('inject')</script><script>alert('inject 2')</script>",
                    NoteBodyHtml = "mock note model 1 <script>alert('inject')</script><script>alert('inject 2')</script>",
                    DateCreated = DateTime.Now,
                    NoteCategories = new List<NoteCategoryModel>
                    {
                        new NoteCategoryModel
                        {
                            Id = 1,
                            NoteId = 1,
                            CategoryId = 2
                        }
                    }
                },
            };

            var mockNotesApi = new Mock<IOptoBasicNotesApi>();
            mockNotesApi.Setup(m => m.GetAllCategoriesAsync()).Returns(Task.FromResult(categoryModels));
            mockNotesApi.Setup(m => m.CreateNoteAsync(It.IsAny<string>(), It.IsAny<IList<int>>())).Returns(Task.FromResult(new NoteModel
            {
                Id = 1
            }));

            mockNotesApi.Setup(m => m.GetAllNotesAsync()).Returns(Task.FromResult(noteModels));

            return mockNotesApi;
        }

        private void SimulateValidation(object model, Controller controller)
        {
            // mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage ?? "some error");
            }
        }
    }
}