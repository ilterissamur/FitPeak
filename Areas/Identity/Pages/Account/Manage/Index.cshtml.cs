// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitPeak.Models;
using FitPeak.Data;

namespace FitPeak.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; } = new BufferedSingleFileUploadDb();

        public byte[] Picture { get; set; }
        public string Biography { get; set; }
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Biography")]
            public string Biography { get; set; }
        }

        public class BufferedSingleFileUploadDb
        {
            [Display(Name = "Profile Picture")]
            public IFormFile FormFile { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Detached;

            user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == user.Id);

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var biography = user.Biography;

            Username = userName;
            Biography = biography;

            if (user != null)
            {
                if (user.ProfilePicture != null)
                {
                    Picture = user.ProfilePicture ?? Array.Empty<byte>();
                }
                else
                {
                    string picturePath = "./wwwroot/images/default_profile_picture.jpeg";
                    using var stream = System.IO.File.OpenRead(picturePath);
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    Picture = memoryStream.ToArray();

                    user.ProfilePicture = Picture;

                    _context.Attach(user);
                    _context.Entry(user).Property(u => u.ProfilePicture).IsModified = true;

                    await _context.SaveChangesAsync();
                }
            }

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Biography = biography
            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var profileDetail = await _context.ApplicationUsers.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (profileDetail != null)
            {
                if (FileUpload.FormFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await FileUpload.FormFile.CopyToAsync(memoryStream);
                        profileDetail.ProfilePicture = memoryStream.ToArray();
                    }
                }

                profileDetail.Biography = Input.Biography;

                _context.ApplicationUsers.Update(profileDetail);
                await _context.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
