using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotesApi.Application.DTOs
{
    public class CategoryDto
    {
        /// <summary>
        /// THe identity id of the Category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// THe Category Name
        /// </summary>
        public string CategoryName { get; set; }
    }
}
