using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OptoBasicNotes.Models
{
    public class IndexViewModel
    {
        [Display(Name = "Select one or more categories")]
        public List<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Note Text")]
        public string NoteText { get; set; }
    }
}
