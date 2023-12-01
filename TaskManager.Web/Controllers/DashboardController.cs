using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Core.DTO_s.Post;
using TaskManager.Core.DTO_s.User;
using TaskManager.Core.Entities.User;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;
using TaskManager.Core.Validation.User;
using X.PagedList;

namespace TaskManager.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly UserServices _userService;
        private readonly IPostService _postService;
        


        public DashboardController(UserServices userService, IPostService postService, UserManager<AppUser> userManager)
        {
            _userService = userService;
            _postService = postService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                string userId = user.Id;
                List<PostDto> posts = await _postService.GetAll(userId);

                int pageNumber = page ?? 1; 
                IPagedList<PostDto> pagedList = posts.ToPagedList(pageNumber, 10); 

                return View(pagedList);
            }
            return View(new List<PostDto>());
        }

        [AllowAnonymous]
        public async Task <IActionResult> SignIn()
        {
            var user = HttpContext.User.Identity.IsAuthenticated;
            if (user)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
            //return View("login");
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginUserDto model)
        {
            var validator = new LoginUserValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                ServiceResponse result = await _userService.LoginUserAsync(model);
                if (result.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.AuthError = result.Message;
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View(model);
        }

        public IActionResult SignUp()
        {
            return View("Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var validator = new RegisterValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.Success)
                {
                    return View("SignIn");
                }
                ViewBag.AuthError = result.Payload;
                return View(model);
            }
            ViewBag.AuthError = validationResult.Errors[0];
            return View("Index");
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction(nameof(SignIn));
        }
        public async Task<IActionResult> AddTask()
        {
            return View("AddTask");
        }
        [HttpPost]
        public async Task<IActionResult> AddTask(PostDto model)
        {
            
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                model.UserId = user.Id;
                await _postService.Create(model);

                return RedirectToAction("Index");
            }
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result.Success)
            {                
                return View(result.Payload);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string FirstName, string LastName)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var result = await _userService.UpdateUserInfoAsync(userId, FirstName, LastName);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Помилка оновлення імені користувача.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(ChangePasswordDto model)
        {
            var validator = new ChangePasswordValidation();
            var validationResult = await validator.ValidateAsync(model);
            if (validationResult.IsValid)
            {
                var result = await _userService.ChangePasswordAsync(model);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(validationResult.Errors);
            }
        }

        public async Task<IActionResult> DeleteById(int id)
        {
            await _postService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var posts = await _postService.Get(id);

            if (posts == null) return NotFound();
            return View(posts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostDto model)
        {
                await _postService.Update(model);
                return RedirectToAction("Index");
        }

    }
}
