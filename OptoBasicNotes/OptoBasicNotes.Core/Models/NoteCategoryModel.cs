namespace OptoBasicNotes.Core.Models
{
    public class NoteCategoryModel
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
