using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public HomeController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _context.Properties.Include(p => p.Seller).ToListAsync();
            return View(properties);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ShowInterest(int id)
        {
            var property = await _context.Properties.Include(p => p.Seller).FirstOrDefaultAsync(p => p.Id == id);
            if (property != null)
            {
                var seller = property.Seller;
                var buyerEmail = User.Identity.Name;
                var buyer = await _context.Users.SingleOrDefaultAsync(u => u.Email == buyerEmail);

                if (buyer != null)
                {
                    // Send email to buyer
                    var buyerMessage = $"Seller Email: {seller.Email}, Seller Phone: {seller.PhoneNumber}";
                    await _emailService.SendEmailAsync(buyerEmail, "Property Interest", buyerMessage);

                    // Send email to seller
                    var sellerMessage = $"Buyer Email: {buyer.Email}, Buyer Phone: {buyer.PhoneNumber}";
                    await _emailService.SendEmailAsync(seller.Email, "New Interest in Your Property", sellerMessage);

                    return Json(new { sellerEmail = seller.Email, sellerPhone = seller.PhoneNumber });
                }
            }
            return Json(new { sellerEmail = "", sellerPhone = "" });
        }
    }
}