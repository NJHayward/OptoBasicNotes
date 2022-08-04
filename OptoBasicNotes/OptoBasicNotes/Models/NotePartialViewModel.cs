namespace OptoBasicNotes.Models
{
    public class NotePartialViewModel
    {
        /// <summary>
        /// The identity id of the note
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date/Time the note was created
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
        /// The Note categories linking Notes and Categories
        /// </summary>
        public IList<NoteCategoryPartialViewModel> Categories { get; set; }
    }

    public class NoteCategoryPartialViewModel
    {
        /// <summary>
        /// The category name to display
        /// </summary>
        public string CategoryName { get; set; }
    }
}
