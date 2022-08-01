using OptoBasicNotes.Core.Models;
using OptoBasicNotes.Core.Models.DTOs;

namespace OptoBasicNotes.Core.Interfaces
{
    public interface IOptoBasicNotesApi
    {
        #region Categories 

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>A list of the category models</returns>
        Task<IList<CategoryModel>> GetAllCategoriesAsync();

        /// <summary>
        /// Creates a category witht he given dto
        /// </summary>
        /// <param name="categoryName">The name of the category to create</param>
        /// <returns>The category model</returns>
        Task<CategoryModel> CreateCategoryAsync(string categoryName);

        #endregion

        #region Notes 

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <returns>The list of note models</returns>
        Task<IList<NoteModel>> GetAllNotesAsync();

        /// <summary>
        /// Get note by the id parameter
        /// </summary>
        /// <param name="id">The id of the note to find</param>
        /// <returns>The note dto</returns>
        Task<NoteModel> GetNoteAsync(int id);

        /// <summary>
        /// Get the note by the id parameter.  will convert any markdown in the note to html.
        /// </summary>
        /// <param name="id">The id of the note to find</param>
        /// <returns>The note dto</returns>
        Task<NoteModel> GetNoteConvertedMarkdownAsync(int id);

        /// <summary>
        /// Creates the note acording the the dto passed in the body
        /// </summary>
        /// <param name="createNoteDto">The dto with the not details to create</param>
        /// <returns>The note dto with updated id values</returns>
        Task<NoteModel> CreateNoteAsync(string noteBody, IList<int> noteCategoryIds);

        /// <summary>
        /// Update the note
        /// </summary>
        /// <param name="id">The id of the note to update</param>
        /// <param name="noteDto">The note dto passed via the body containing the information to update</param>
        /// <returns>OK result</returns>
        Task UpdateNoteAsync(int id, string noteBody, IList<int> noteCategoryIds);

        /// <summary>
        /// Deleted the note for the given id parameter
        /// </summary>
        /// <param name="id">The id of the note to delete</param>
        /// <returns>Ok result</returns>
        Task DeleteNoteAsync(int id);

        #endregion
    }
}
