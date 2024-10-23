using AdminPortal.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdminPortal.Models;

namespace AdminPortal.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _httpClient;
        private readonly Tools _tools;
        private string userID => HttpContext.Session.GetString("UserId");

        public ManageUserController(

            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<LoginController> logger,
            IHttpClientFactory httpClientFactory,
            Tools tools)
        {
            _tools = tools;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("api");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageUser(string userName)
        {
            // get user by username
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) 
            {
                ViewBag.UserNameSearchFailed = "Username could not be found";
                return RedirectToAction("Index", "ManageUser");
            }
            
            return View(user);
        }
    }
}
