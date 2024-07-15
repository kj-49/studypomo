using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PomodoroLibrary.Services.Interfaces;

namespace PomodoroUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IUserService _userService;

        public IndexModel(ILogger<IndexModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            //if (_userService.GetCurrentUserAsync().Result != null)
            //{
            //    return RedirectToPage("/Timer/Index", new { area = "Registered" });
            //}
            //return RedirectToPage("/Timer/Index", new { area = "Public" });

            return RedirectToPage("/Timer/Index", new { area = "Registered" });
        }
    }
}
