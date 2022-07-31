using OptoBasicNotesApi.Core.Models;

namespace OptoBasicNotesApi.Application.Interfaces
{
    public interface INoteService
    {
        /// <summary>
        /// Gets all notes
        /// </summary>
        /// <returns>The note Model</returns>
        Task<IList<Note>> GetAllAsync();

        /// <summary>
        /// Finds the note by its id
        /// </summary>
        /// <param name="id">The id of the note to find</param>
        /// <returns></returns>
        Task<Note> FindByIdAsync(int id);

        /// <summary>
        /// Created the note passed in and its noteCategory items.
        /// </summary>
        /// <param name="note">The note to create</param>
        /// <returns>The note model with the identity id assigned</returns>
        Task<Note> CreateNote(Note note);

        /// <summary>
        /// Updates the note passed in
        /// </summary>
        /// <param name="note">The note to update</param>
        Task Update(Note note);
        
        /// <summary>
        /// Deletes the note passed in
        /// </summary>
        /// <param name="note">The note to delete</param>
        Task Delete(Note note);
    }
}
