namespace OptoBasicNotesApi.Application.DTOs
{
    public class AllNotesItemDto
    {
        /// <summary>
        /// The identity id of the note
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date/TIme the note was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The main text of the note.  Supports markdown.
        /// </summary>
        public string NoteBody { get; set; }

        /// <summary>
        /// The main text of the note.  With markdown as html.
        /// </summary>
        public string NoteBodyHtml { get; set; }

        /// <summary>
        /// The Not categories linking Notes and Categories
        /// </summary>
        public IEnumerable<NoteCategoryDto> NoteCategories { get; set; }
    }
}
