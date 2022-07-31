using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotesApi.Application.DTOs
{
    public class CreateUpdateNoteDto
    {
        /// <summary>
        /// The main message of the note.  Supports markdown.
        /// </summary>
        [Required]
        [MaxLength(2048)]
        public string NoteBody { get; set; }

        /// <summary>
        /// List of cateogry ids to solely link to the note.
        /// </summary>
        [MinLength(1)]
        public IList<int> NoteCategoryIds { get; set; }
    }
}
