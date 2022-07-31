namespace OptoBasicNotes.Core.Models.DTOs
{
    public class NoteCategoryDto
    {
        /// <summary>
        /// The identity id of the NoteCategory relationship table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The note id
        /// </summary>
        public int NoteId { get; set; }

        /// <summary>
        /// The category id
        /// </summary>
        public int CategoryId { get; set; }
    }
}
