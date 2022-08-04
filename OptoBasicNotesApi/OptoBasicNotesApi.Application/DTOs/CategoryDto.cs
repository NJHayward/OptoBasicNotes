using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotesApi.Application.DTOs
{
    // Including The NoteCategories in this DTO would produce a circular dependency when using automapper and cause errors
    //   If we ever want to know what Notes are within a category i would either make a call such as GetNotesForCategory
    //     or return a different set of dtos that would have the NoteCategories in the category dto but the category would
    //     not be in the note dto.    This would depend on requirements and wether or not we would need to avoid many calls
    //     getting notes from different categories.  Even then, GetNotesForCategories is a viable option also... and could 
    //     produce a simpler approach.    It would all depend on the requirements.

    public class CategoryDto
    {
        /// <summary>
        /// THe identity id of the Category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// THe Category Name
        /// </summary>
        [MaxLength(64)]
        public string CategoryName { get; set; }
    }
}
