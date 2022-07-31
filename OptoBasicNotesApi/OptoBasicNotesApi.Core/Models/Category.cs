using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OptoBasicNotesApi.Core.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string CategoryName { get; set; }

        public IList<NoteCategory> NoteCategories { get; set; }
    }
}
