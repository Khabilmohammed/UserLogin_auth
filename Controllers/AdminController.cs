using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Models.DTO;
using WebApplication1.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Domain;
using WebApplication1.Models;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly DatabaseContext _db;
        public AdminController(IUserAuthenticationService authService, DatabaseContext db)
        {
            this._authService = authService;
            _db = db;
        }
        public IActionResult Display()
        {
            return View();
        }

        public IActionResult AddUser()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await this._authService.ListAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ViewUser));
        }

        [HttpGet]
        public async Task<IActionResult> ViewUser()
        {
            var UserList = await _db.Users.ToListAsync();
            return View(UserList);
        }

        [HttpGet]
        public async Task<IActionResult> View(string id) 
        {
            
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(user != null)
            {
                var viewModel = new UpdateUserModel()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Name,
                   
                };
                return await Task.Run(() => View("View",viewModel));
            }
            
            return RedirectToAction("ViewUser");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateUserModel model)
        {
            var user = await _db.Users.FindAsync(model.Id);

            if(user != null) 
            {
               
                user.Name = model.Name;
                user.Email = model.Email;
                user.UserName= model.Username;
               
                await _db.SaveChangesAsync();

                return RedirectToAction("ViewUser");
             }
            return RedirectToAction("ViewUser");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                
            }
            else
            {
                
                TempData["Message"] = "User not found or already deleted.";
            }
            return RedirectToAction("ViewUser");
        }

    }
}
