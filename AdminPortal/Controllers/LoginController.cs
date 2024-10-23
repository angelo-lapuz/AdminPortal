using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using AdminPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebAPI.Models;
using Newtonsoft.Json;
using AdminPortal.Utilities;

namespace AdminPortal.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _httpClient;
        private readonly Tools _tools;

        public LoginController(

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

        public IActionResult Login() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid) return View(model);


            var response = await _httpClient.PostAsJsonAsync("api/users/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);
                
                // checks if user has admin role
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                _logger.LogInformation(isAdmin.ToString());
                
                // if user is not an admin return error message else redirect to home
                if (!isAdmin)
                {
                    ModelState.AddModelError("loginFailed", "You are not permitted to use this portal.");
                }
                else
                {
                    HttpContext.Session.SetString("UserId", user.Id);

                    return RedirectToAction("Index", "Home");
                }
            }

            // user has not confirmed email
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError("UserName", "Please confirm your email to sign in.");
            }

            // invalid username or password
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("loginFailed", "Invalid username or password.");
            }

            // unexpected error could be various things, no api, db error, etc.
            else
            {
                ModelState.AddModelError("UserName", "An unexpected error occurred, please try again.");
            }

            return View(model);
        }
    }
}
