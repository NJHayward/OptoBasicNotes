using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptoBasicNotesApi.Core.Models
{
    [Table("NoteCategories")]
    public class NoteCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Note")]
        public int NoteId { get; set; }

        public Note Note { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
