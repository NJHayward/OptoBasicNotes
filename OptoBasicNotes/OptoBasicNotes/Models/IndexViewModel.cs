using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotes.Models
{
    public class IndexViewModel
    {
        /// <summary>
        /// The Categories ussed modelbinded from the form with Selected properties indicating if the 
        ///   user has selected the category or if we want categories preselected.
        /// </summary>
        [Display(Name = "Select one or more categories")]
        public List<SelectListItem> Categories { get; set; }

        /// <summary>
        /// The note text to preload or modelbound from the form
        /// </summary>
        [Required]
        [Display(Name = "Note Text")]
        public string NoteText { get; set; }
    }
}
