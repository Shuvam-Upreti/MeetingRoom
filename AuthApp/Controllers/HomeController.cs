using AuthApp.Models;
using MeetingRoom.Data;
using MeetingRoom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _context.RoomModels.ToListAsync();
            return View(rooms);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> All()
        {
            var rooms = await _context.RoomModels.ToListAsync();
            return View(rooms);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create(RoomModel roomModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var path = Path.Combine(wwwrootPath, @"images\rooms");
                    var extension = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }

                    roomModel.ImageUrl = @"\images\rooms\" + fileName + extension;
                }

                await _context.RoomModels.AddAsync(roomModel);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(roomModel);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(Guid id, IFormFile? file)
        {
            var obj = await _context.RoomModels.FindAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            string wwwrootPath = _hostEnvironment.WebRootPath;

            if (obj.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(wwwrootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(RoomModel obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var path = Path.Combine(wwwrootPath, @"images\rooms");
                    var extension = Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(path, fileName + extension), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }

                    obj.ImageUrl = @"\images\rooms\" + fileName + extension;
                }

                _context.RoomModels.Update(obj);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(obj);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var obj = await _context.RoomModels.FindAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoomModel obj)
        {
            if (!ModelState.IsValid)
            {
                _context.RoomModels.Remove(obj);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
