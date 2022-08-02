using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OptoBasicNotes.Core.Interfaces;
using OptoBasicNotes.Models;
using System.Diagnostics;

namespace OptoBasicNotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptoBasicNotesApi _notesApi;

        public HomeController(ILogger<HomeController> logger, IOptoBasicNotesApi notesApi)
        {
            _logger = logger;
            _notesApi = notesApi;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _notesApi.GetAllCategoriesAsync();

            IndexViewModel model = new();
            model.Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            }).ToList();




            //check task notes in case missed anything.

            //go through and add coments and cleanup


            return View(model);
        }

        [HttpPost]
        public async Task<bool> CreateNote(IndexViewModel model)
        {
            if (!ModelState.IsValid || model.Categories == null || !model.Categories.Any(x => x.Selected))
            {
                return false;
            }

            var categoryIds = model.Categories.Where(x => x.Selected).Select(x => int.Parse(x.Value)).ToList();

            var result = await _notesApi.CreateNoteAsync(model.NoteText, categoryIds);

            return result.Id > 0;
        }

        public async Task<PartialViewResult> GetNotesPartial()
        {
            var allNotes = (await _notesApi.GetAllNotesAsync());
            var allCategories = await _notesApi.GetAllCategoriesAsync();

            //Create partial view model 
            var model = new List<NotePartialViewModel>();
            foreach (var note in allNotes)
            {
                var categoryIds = note.NoteCategories.Select(y => y.CategoryId);

                model.Add(new NotePartialViewModel
                {
                    Id = note.Id,
                    DateCreated = note.DateCreated,
                    NoteBody = protectAgainstScript(note.NoteBody),
                    NoteBodyHtml = protectAgainstScript(note.NoteBodyHtml),
                    Categories = allCategories.Where(x => categoryIds.Contains(x.Id))
                                              .Select(x => new NoteCategoryPartialViewModel 
                                                           { 
                                                               CategoryName = x.CategoryName 
                                                           })
                                              .ToList()
                });
            }

            return PartialView("_NotesPartial", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string protectAgainstScript(string toProtect)
        {
            return toProtect.Replace("<script>", "&lt;script&gt;").Replace("</script>", "&lt;/script&gt;");
        }
    }
}