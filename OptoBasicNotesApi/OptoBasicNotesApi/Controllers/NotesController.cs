using AutoMapper;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OptoBasicNotesApi.Application.DTOs;
using OptoBasicNotesApi.Application.Interfaces;
using OptoBasicNotesApi.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OptoBasicNotesApi.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly IMapper _mapper;
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;

        public NotesController(ILogger<NotesController> logger, IMapper mapper, INoteService noteService, ICategoryService categoryService)
        {
            _logger = logger;
            _mapper = mapper;
            _noteService = noteService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <returns>The list of note dtos</returns>
        [HttpGet]
        public async Task<ActionResult<IList<AllNotesItemDto>>> GetAllNotes()
        {
            var results = await _noteService.GetAllAsync();
            if (results == null || results.Count == 0)
            {
                return Ok(new List<AllNotesItemDto>());
            }

            //Mapper will also convert markdown ready for the callers to use without having to make extra calls for individual 
            // notes.   usefull when displaying a list of the notes that display the content.  This is also why we use a seperate
            // dto here with an extra field NoteBodyHtml
            var mappedResults = _mapper.Map<IList<AllNotesItemDto>>(results);

            return Ok(mappedResults);
        }

        /// <summary>
        /// Get note by the id parameter
        /// </summary>
        /// <param name="id">The id of the note to find</param>
        /// <returns>The note dto</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<NoteDto>> GetNote(int id)
        {
            var result = await _noteService.FindByIdAsync(id);
            if (result == null)
            {
                _logger.LogWarning("GetNote - Note retreival attempted on note that does not exist. id = {id}", id);
                return BadRequest("Note does not exist");
            }

            return Ok(_mapper.Map<NoteDto>(result));
        }

        /// <summary>
        /// Get the note by the id parameter.  will convert any markdown in the note to html.
        /// </summary>
        /// <param name="id">The id of the note to find</param>
        /// <returns>The note dto</returns>
        [HttpGet]
        [Route("{id}/html")]
        public async Task<ActionResult<NoteDto>> GetNoteConvertedMarkdown(int id)
        {
            var note = await _noteService.FindByIdAsync(id);
            if (note == null)
            {
                _logger.LogWarning("GetNoteConvertedMarkdown - Note retreival attempted on note that does not exist. id = {id}", id);
                return BadRequest("Note does not exist");
            }

            //Using Markdig we can convert the markdown to html automatically.
            note.NoteBody = Markdown.ToHtml(HttpUtility.HtmlEncode(note.NoteBody));

            return Ok(_mapper.Map<NoteDto>(note));
        }

        /// <summary>
        /// Creates the note acording the the dto passed in the body
        /// </summary>
        /// <param name="createNoteDto">The dto with the not details to create</param>
        /// <returns>The note dto with updated id values</returns>
        [HttpPost]
        public async Task<ActionResult<NoteDto>> CreateNote(CreateUpdateNoteDto createNoteDto)
        {
            var note = new Note()
            {
                DateCreated = System.DateTime.Now,
                NoteBody = createNoteDto.NoteBody
            };

            //Must ensure categories exist to pre-emptivly save sql from producing errors.
            var categories = await _categoryService.FindByIdsAsync(createNoteDto.NoteCategoryIds);
            if (categories == null || categories.Count == 0 || categories.Count != createNoteDto.NoteCategoryIds.Count)
            {
                StringBuilder idsStr = new StringBuilder();
                foreach(var id in createNoteDto.NoteCategoryIds)
                {
                    idsStr.Append(" " + id.ToString());
                }
                _logger.LogWarning("CreateNote - Create attempted with category ids that do not exist. id ={idsStr}", idsStr);

                return BadRequest("One or more categories does not exist");
            }

            note.NoteCategories = new List<NoteCategory>();
            foreach (var category in categories)
            {
                note.NoteCategories.Add(new NoteCategory
                {
                    CategoryId = category.Id,
                });
            }

            await _noteService.CreateNote(note);

            //Usually would log audit entries to the database.  However the spec says create audit logs via the .net logging framework.
            // so it is done in a very base way here.
            var noteId = note.Id;
            if (noteId > 0)
                _logger.LogInformation("CreateNote Audit - A note has been created.  Name = {noteId}", noteId);

            return Ok(_mapper.Map<NoteDto>(note));
        }

        /// <summary>
        /// Update the note
        /// </summary>
        /// <param name="id">The id of the note to update</param>
        /// <param name="noteDto">The note dto passed via the body containing the information to update</param>
        /// <returns>OK result</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<bool>> UpdateNote(int id, CreateUpdateNoteDto noteDto)
        {
            var note = await _noteService.FindByIdAsync(id);
            if (note == null)
            {
                _logger.LogWarning("UpdateNote - update attempted on note that does not exist. id = {id}", id);
                return BadRequest("Note does not exist");
            }

            //Must ensure categories exist to preemptivly save sql from producing errors.
            var categories = await _categoryService.FindByIdsAsync(noteDto.NoteCategoryIds);
            if (categories == null || categories.Count == 0 || categories.Count != noteDto.NoteCategoryIds.Count())
            {
                StringBuilder idsStr = new StringBuilder();
                foreach (var cid in noteDto.NoteCategoryIds)
                {
                    idsStr.Append(" " + cid.ToString());
                }
                _logger.LogWarning("UpdateNote - update attempted with category ids that do not exist. id ={idsStr}", idsStr);

                return BadRequest("One or more categories does not exist");
            }

            note.NoteBody = noteDto.NoteBody;

            note.NoteCategories = new List<NoteCategory>();
            foreach (var category in categories)
            {
                note.NoteCategories.Add(new NoteCategory
                {
                    CategoryId = category.Id,
                });
            }

            await _noteService.Update(note);

            //Usually would log audit entries to the database.  However the spec says create audit logs via the .net logging framework.
            // so it is done in a very base way here.
            var noteId = note.Id;
            _logger.LogInformation("CreateNote Audit - A note has been updated.  Name = {noteId}", noteId);

            return Ok(true);
        }

        /// <summary>
        /// Deleted the note for the given id parameter
        /// </summary>
        /// <param name="id">The id of the note to delete</param>
        /// <returns>Ok result</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<bool>> DeleteNote(int id)
        {
            var note = await _noteService.FindByIdAsync(id);
            if (note == null)
            {
                _logger.LogWarning("DeleteNote - delete attempted on note that does not exist. id = {id}", id);
                return BadRequest("Note does not exist");
            }

            await _noteService.Delete(note);

            //Usually would log audit entries to the database.  However the spec says create audit logs via the .net logging framework.
            // so it is done in a very base way here.
            var noteId = note.Id;
            _logger.LogInformation("CreateNote Audit - A note has been deleted.  Name = {noteId}", noteId);

            return Ok(true);
        }
    }
}