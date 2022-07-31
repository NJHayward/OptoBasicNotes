using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotes.Core.Models
{
    public class CreateCategoryModel
    {
        /// <summary>
        /// The Category name to create
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string CategoryName { get; set; }
    }
}
