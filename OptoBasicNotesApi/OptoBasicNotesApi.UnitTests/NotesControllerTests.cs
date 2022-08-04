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
    public class NotesControllerTests
    {
        /// <summary>
        /// Ensure an empty list is returned when there are no notes to retreive
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAllNotes_Returns_EmptyList_When_No_Notes()
        {
            //Arrange
            var notesController = BuildTestNotesController(false);

            //Act
            var result = await notesController.GetAllNotes();

            //Assert
            if (result.Result is not OkObjectResult)
            {
                Assert.Fail("Result is not a OK result");
                return;
            }

            var resultAs = result.Result as OkObjectResult;
            if (resultAs == null || resultAs.Value is not IList<AllNotesItemDto>)
            {
                Assert.Fail("Result value is not a IList<AllNotesItemDto> result");
                return;
            }

            var valueAs = resultAs.Value as IList<AllNotesItemDto>;
            Assert.IsTrue(valueAs != null && valueAs.Count == 0);
        }

        /// <summary>
        /// Ensure results are actually returned when there are notes to retreive
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetAllNotes_Returns_Result_When_Have_Notes()
        {
            //Arrange
            var notesController = BuildTestNotesController(true);

            //Act
            var result = await notesController.GetAllNotes();

            //Assert
            if (result.Result is not OkObjectResult)
            {
                Assert.Fail("Result is not a OK result");
                return;
            }

            var resultAs = result.Result as OkObjectResult;
            if (resultAs == null || resultAs.Value is not IList<AllNotesItemDto>)
            {
                Assert.Fail("Result value is not a IList<AllNotesItemDto> result");
                return;
            }

            var valueAs = resultAs.Value as IList<AllNotesItemDto>;
            Assert.IsTrue(valueAs != null && valueAs.Count == 1);
        }

        /// <summary>
        /// Ensure BadRequest is returned if noteid passed in does not exist.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetNote_Returns_BadRequest_When_No_Note()
        {
            //Arrange
            var notesController = BuildTestNotesController(false);

            //Act
            var result = await notesController.GetNote(1);

            //Assert
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        /// <summary>
        /// Ensure a note is returned when there are notes to retreive.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetNote_Returns_Note_When_Have_Notes()
        {
            //Arrange
            var notesController = BuildTestNotesController(true);

            //Act
            var result = await notesController.GetNote(1);

            //Assert
            if (result.Result is not OkObjectResult)
            {
                Assert.Fail("Result is not a OK result");
                return;
            }

            var resultAs = result.Result as OkObjectResult;
            if (resultAs == null || resultAs.Value is not NoteDto)
            {
                Assert.Fail("Result value is not a NoteDto result");
                return;
            }

            var valueAs = resultAs.Value as NoteDto;
            Assert.IsTrue(valueAs != null);
        }

        /// <summary>
        /// Ensure the result returned contains converted Markdown
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetNoteConvertedMarkdown_Converts_Markdown_To_Html()
        {
            //Arrange
            var notesController = BuildTestNotesController(true);

            //Act
            var result = await notesController.GetNoteConvertedMarkdown(1);

            //Assert
            if (result.Result is not OkObjectResult)
            {
                Assert.Fail("Result is not a OK result");
                return;
            }

            var resultAs = result.Result as OkObjectResult;
            if (resultAs == null || resultAs.Value is not NoteDto)
            {
                Assert.Fail("Result value is not a NoteDto result");
                return;
            }

            var valueAs = resultAs.Value as NoteDto;
            Assert.IsTrue(valueAs != null && valueAs.NoteBody.Contains("<h2>"));
            Assert.IsTrue(valueAs != null && !valueAs.NoteBody.Contains("## Header 1"));
        }

        /// <summary>
        /// Ensure CreateNote returns BadRequest when there are no categories in the DTO passed in.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateNote_Reutrns_BadRequest_When_No_Categories_Passed_In()
        {
            //Arrange
            var notesController = BuildTestNotesController(true);
            var createDto = new CreateUpdateNoteDto
            {
                NoteBody = "test note body",
                NoteCategoryIds = Array.Empty<int>(),
            };

            //Act
            var result = await notesController.CreateNote(createDto);

            //Assert
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        /// <summary>
        /// Ensure BadRequest is returned when there are no categories in the DTO passed in.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task UpdateNote_Reutrns_BadRequest_When_No_Categories()
        {
            //Arrange
            var notesController = BuildTestNotesController(true);
            var createDto = new CreateUpdateNoteDto
            {
                NoteBody = "test note body",
                NoteCategoryIds = Array.Empty<int>(),
            };

            //Act
            var result = await notesController.UpdateNote(1, createDto);

            //Assert
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        /// <summary>
        /// Ensure BadRequest is returned when the Note to delete does not exist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DeleteNote_Reutrns_BadRequest_When_No_Note()
        {
            //Arrange
            var notesController = BuildTestNotesController(false);

            //Act
            var result = await notesController.DeleteNote(1);

            //Assert
            Assert.IsTrue(result.Result is BadRequestObjectResult);
        }

        private NotesController BuildTestNotesController(bool hasNotes)
        {
            IList<Note> notes = new List<Note>()
            {
                new Note
                {
                    Id = 1,
                    NoteBody = "## Header 1 \n mock note model 1 <script>alert('inject')</script><script>alert('inject 2')</script>",
                    DateCreated = DateTime.Now,
                    NoteCategories = new List<NoteCategory>
                    {
                        new NoteCategory
                        {
                            Id = 1,
                            NoteId = 1,
                            CategoryId = 2
                        }
                    }
                },
            };

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

            var mockNotesService = new Mock<INoteService>();
            if (hasNotes)
            {
                mockNotesService.Setup(m => m.GetAllAsync()).ReturnsAsync(notes);
                mockNotesService.Setup(m => m.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(notes[0]);
            }
            else
            {
                mockNotesService.Setup(m => m.GetAllAsync()).ReturnsAsync(new List<Note>());
                //mockNotesService.Setup(m => m.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null, TimeSpan.Zero);
            }

            var mockCategoryService = new Mock<ICategoryService>();
            mockCategoryService.Setup(m => m.FindByIdsAsync(It.IsAny<IList<int>>())).ReturnsAsync(categories);

            return new NotesController(logger.Object, mapper, mockNotesService.Object, mockCategoryService.Object);
        }
    }
}