using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotesApi.Application.DTOs
{
    public class CreateCategoryDto
    {
        /// <summary>
        /// The Category name to create
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string CategoryName { get; set; }
    }
}
