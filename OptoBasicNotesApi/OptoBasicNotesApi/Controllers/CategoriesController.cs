using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OptoBasicNotesApi.Application.DTOs;
using OptoBasicNotesApi.Application.Interfaces;
using OptoBasicNotesApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OptoBasicNotesApi.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ILogger<NotesController> logger, IMapper mapper, ICategoryService categoryService)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>a list of the category dtos</returns>
        [HttpGet]
        public async Task<ActionResult<IList<NoteDto>>> GetAllCategories()
        {
            var results = await _categoryService.GetAllAsync();
            if (results == null || results.Count == 0)
            {
                return Ok(new List<NoteDto>());
            }

            return Ok(_mapper.Map<IList<CategoryDto>>(results));
        }

        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateCategory(CreateCategoryDto createNoteDto)
        {
            ///Check for existign category name first before creating the new category. 
            ///   This can preemptivly save errors being produced within SQL.
            var existingCategory = await _categoryService.FindByNameAsync(createNoteDto.CategoryName);
            if (existingCategory != null)
            {
                var name = createNoteDto.CategoryName;
                _logger.LogWarning("CreateCategory - create attempted on a category name that already exists.  Name = {name}", name);
                return BadRequest("Category Name already exists");
            }

            var category = new Category()
            {
                CategoryName = createNoteDto.CategoryName
            };

            await _categoryService.CreateCategoryAsync(category);

            return Ok(_mapper.Map<CategoryDto>(category));
        }
    }
}