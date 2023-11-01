using MeetingsManagementWeb.Models;
using MeetingsManagementWeb.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsManagementWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserSignUpDto userDto)
        {
            if (!ModelState.IsValid)
                return View();
            var userCheck = _userManager.FindByEmailAsync(userDto.Email!).Result;
            if (userCheck is not null) {
                ModelState.AddModelError("Email", "Email already exists.");
                return View();
            }
            var userIdentity = new ApplicationUser {
                Nickname = userDto.Nickname,
                UserName = userDto.Email,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var result = _userManager.CreateAsync(userIdentity, userDto.Password!).Result;
            if (result.Succeeded)
                return RedirectToAction("LogIn", "Account");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View();
        }
        
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(UserLogInDto userDto)
        {
            if (!ModelState.IsValid)
                return View();
            var user = _userManager.FindByEmailAsync(userDto.Email!).Result;
            if (user is null)
            {
                ModelState.AddModelError("Email", "The email is not registered.");
                return View();
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("Email", "The email is not confirmed.");
                return View();
            }
            var result = _signInManager.PasswordSignInAsync(user, userDto.Password!, false, true).Result;
            if (result.Succeeded)
            {
                _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UserRole", "Admin"));
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "The account is locked.");
            else
                ModelState.AddModelError(string.Empty, "Invalid Credentials.");
            return View();
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            if (User.Identity is null)
                return RedirectToAction();
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        #region API CALLS
        [Authorize, HttpPost]
        public IActionResult ChangePassword([FromBody] UserPasswordDto userDto)
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
                return BadRequest(new { status = "Failed", message = "The user has to be logged in." });
            if (userDto is null)
                return BadRequest(new { status = "Failed", message = "The request cannot be empty." });
            if (string.IsNullOrEmpty(userDto.CurrentPassword))
                return BadRequest(new { status = "Faild", message = "`Current Password` cannot be empty.", element = "CurrentPassword" });
            if (string.IsNullOrEmpty(userDto.NewPassword))
                return BadRequest(new { status = "Failed", message = "`New Password` cannot be empty.", element = "NewPassword" });
            if (IsStrongPassword(userDto.NewPassword))
                return BadRequest(new { status = "Failed", message = "The password is weak.", element = "NewPassword" });
            if (userDto.NewPassword != userDto.NewPasswordConfirmation)
                return BadRequest(new { status = "Failed", message = "`Confirm Password doesn't match `New Password`.", element = "NewPasswordConfirmation" });
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var result = _userManager.ChangePasswordAsync(user!, userDto.CurrentPassword, userDto.NewPassword).Result;
            if (result.Succeeded)
                return Ok(new { status = "Success", message = "The password is updated successfully" });
            return BadRequest(new { status = "Failed",
                message = string.Join('\n', result.Errors.Select(error => error.Description)) });
        }
        #endregion

        private static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;
            bool hasLowercase = false, hasUppercase = false, hasDigit = false, hasSymbol = false;
            foreach (char c in password)
            {
                if (char.IsLower(c)) hasLowercase = true;
                else if (char.IsUpper(c)) hasUppercase = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (char.IsSymbol(c)) hasSymbol = true;
                else return false;
            }
            return hasLowercase && hasUppercase && hasDigit && hasSymbol;
        }
    }
}
