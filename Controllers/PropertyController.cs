using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstateApp.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        private readonly ApplicationDbContext _context;
  
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(ApplicationDbContext context, ILogger<PropertyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Create(Property model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user's email from claims
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                // Find the user in the database
                var user = _context.Users.SingleOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    // Set the SellerId
                    model.SellerId = user.Id;

                    // Add the property to the context and save changes
                    _context.Properties.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Log the error if the user is not found
                    _logger.LogError($"User with email {userEmail} not found.");
                    ModelState.AddModelError("", "An error occurred. Please try again.");
                }
            }
            else
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Property model)
        {
            if (ModelState.IsValid)
            {
                _context.Properties.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                property.Likes++;
                await _context.SaveChangesAsync();
                return Json(new { likes = property.Likes });
            }
            return Json(new { likes = 0 });
        }
    }
}


