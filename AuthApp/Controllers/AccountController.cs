//using Ecom.web.Data;
using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Models.AuthModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace MeetingRoom.Controllers
{

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<IdentityUser> _singInManager;
        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> singInManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _singInManager = singInManager;
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name
            }).ToList();

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {

            var existingUserByUsername = await _userManager.FindByNameAsync(user.UserName);
            var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);

            if (existingUserByUsername != null)
            {
                ModelState.AddModelError("UserName", "User with the same username already exists.");
            }

            if (existingUserByEmail != null)
            {
                ModelState.AddModelError("Email", "User with the same email already exists.");
            }

            if (!ModelState.IsValid)
            {

                var iUser = new IdentityUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.Phone,
                };
                IdentityResult res = await _userManager.CreateAsync(iUser, user.Password);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(iUser, user.Role);
                    await _dbContext.SaveChangesAsync();
                    return Redirect("/");
                }
            }
            var roles = _roleManager.Roles
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name

                }).ToList();
            ViewBag.Roles = roles;
            return View(user);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {

                return NotFound();
            }

            var roles = _roleManager.Roles.Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Name
            }).ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            var userModel = new UserModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Role = userRoles.FirstOrDefault()
            };

            ViewBag.Roles = roles;
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserModel user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.Phone;

            var result = await _userManager.UpdateAsync(existingUser);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                await _userManager.RemoveFromRolesAsync(existingUser, roles);
                await _userManager.AddToRoleAsync(existingUser, user.Role);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update user.");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete user.");
            }
            return View("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            IdentityUser u = await _userManager.FindByEmailAsync(model.Email);
            if (u != null)
            {
                bool res = await _userManager.CheckPasswordAsync(u, model.Password);
                if (res)
                {
                    await _singInManager.SignInAsync(u, true);
                    return Redirect("Home/Index");
                }
            }
            return View(model);
        }


        public IActionResult Logout()
        {
            _singInManager.SignOutAsync();
            return View("Login");
        }
    }

}
