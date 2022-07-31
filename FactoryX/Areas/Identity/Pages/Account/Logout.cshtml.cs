using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FactoryX.Controllers;
using FactoryX.Data;
using FactoryX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FactoryX.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ApplicationDbContext _context;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var userId = User.getUserId();

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            var lo = _context.LogOn.Where(w => w.UserId == userId).Count();

            if (lo > 0)
            {
                //Borro el registro del login
                LogOn l = _context.LogOn.Where(w => w.UserId == userId).FirstOrDefault();
                _context.LogOn.Remove(l);
                _context.SaveChanges();
            }           

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
    }
}