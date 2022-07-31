using Microsoft.AspNetCore.Mvc;
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

            //var test2 = await _notesApi.CreateCategoryAsync("yety another category testasd");

            //var test3 = _notesApi.GetAllCategoriesAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}