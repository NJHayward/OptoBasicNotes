using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptoBasicNotesApi.Core.Models
{
    [Table("Notes")]
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(2048)]
        public string NoteBody { get; set; }

        public IList<NoteCategory> NoteCategories { get; set; }
    }
}
